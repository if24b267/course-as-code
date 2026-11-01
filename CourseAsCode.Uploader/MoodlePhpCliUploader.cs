using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Uploader;

public partial class MoodlePhpCliUploader : IMoodleCourseRestorer
{
    private readonly MoodlePhpCliOptions _options;
    private readonly IProcessRunner _processRunner;
    private readonly ILogger<MoodlePhpCliUploader>? _logger;

    public MoodlePhpCliUploader(
        MoodlePhpCliOptions options,
        IProcessRunner? processRunner = null,
        ILogger<MoodlePhpCliUploader>? logger = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _processRunner = processRunner ?? new ProcessRunner();
        _logger = logger;
    }

    public async Task<MoodleRestoreResult> RestoreCourseAsync(
        string mbzFilePath,
        int? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(mbzFilePath))
        {
            throw new ArgumentException("MBZ file path cannot be null or empty.", nameof(mbzFilePath));
        }

        var fullMbzPath = Path.GetFullPath(mbzFilePath);
        if (!File.Exists(fullMbzPath))
        {
            throw new FileNotFoundException($"The specified MBZ file was not found: {fullMbzPath}", fullMbzPath);
        }

        var targetCategory = categoryId ?? _options.DefaultCategoryId;
        _logger?.LogInformation("Starting course restore for '{File}' into category {CategoryId} (Mode: {Mode})...",
            fullMbzPath, targetCategory, _options.ExecutionMode);

        return _options.ExecutionMode switch
        {
            MoodleExecutionMode.Docker => await RestoreViaDockerAsync(fullMbzPath, targetCategory, cancellationToken).ConfigureAwait(false),
            MoodleExecutionMode.LocalProcess => await RestoreViaLocalProcessAsync(fullMbzPath, targetCategory, cancellationToken).ConfigureAwait(false),
            _ => throw new NotSupportedException($"Execution mode '{_options.ExecutionMode}' is not supported.")
        };
    }

    private async Task<MoodleRestoreResult> RestoreViaDockerAsync(
        string fullMbzPath,
        int categoryId,
        CancellationToken cancellationToken)
    {
        var containerName = await ResolveContainerNameAsync(_options.ContainerName, cancellationToken).ConfigureAwait(false);
        var tempContainerFileName = $"/tmp/moodle_restore_{Guid.NewGuid():N}.mbz";
        _logger?.LogInformation("Copying MBZ file into Docker container '{Container}' as '{Dest}'...",
            containerName, tempContainerFileName);

        // 1. Copy MBZ file into Docker container
        var copyArgs = $"cp \"{fullMbzPath}\" \"{containerName}:{tempContainerFileName}\"";
        var copyResult = await _processRunner.ExecuteAsync(
            _options.DockerExecutable,
            copyArgs,
            _options.TimeoutSeconds,
            cancellationToken).ConfigureAwait(false);

        if (copyResult.ExitCode != 0)
        {
            var copyError = string.IsNullOrWhiteSpace(copyResult.StandardError) ? copyResult.StandardOutput : copyResult.StandardError;
            _logger?.LogError("Failed to copy MBZ file to Docker container: {Error}", copyError);
            return new MoodleRestoreResult(
                Success: false,
                CourseId: null,
                ExitCode: copyResult.ExitCode,
                StandardOutput: copyResult.StandardOutput,
                StandardError: copyResult.StandardError,
                ErrorMessage: $"Failed to copy MBZ file into Docker container '{containerName}': {copyError.Trim()}");
        }

        try
        {
            // 2. Execute restore_backup.php inside Docker container
            var debugFlag = _options.ShowDebugging ? " --showdebugging" : string.Empty;
            var execArgs = $"exec {containerName} php {_options.MoodleCliScriptPath} --file={tempContainerFileName} --categoryid={categoryId}{debugFlag}";

            _logger?.LogInformation("Executing Moodle CLI restore inside container: docker {Args}", execArgs);
            var restoreResult = await _processRunner.ExecuteAsync(
                _options.DockerExecutable,
                execArgs,
                _options.TimeoutSeconds,
                cancellationToken).ConfigureAwait(false);

            return ParseRestoreOutput(restoreResult);
        }
        finally
        {
            // 3. Remove temp file from container
            var cleanupArgs = $"exec {containerName} rm -f {tempContainerFileName}";
            _ = await _processRunner.ExecuteAsync(
                _options.DockerExecutable,
                cleanupArgs,
                30,
                cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<MoodleRestoreResult> RestoreViaLocalProcessAsync(
        string fullMbzPath,
        int categoryId,
        CancellationToken cancellationToken)
    {
        var debugFlag = _options.ShowDebugging ? " --showdebugging" : string.Empty;
        var arguments = $"\"{_options.MoodleCliScriptPath}\" --file=\"{fullMbzPath}\" --categoryid={categoryId}{debugFlag}";

        _logger?.LogInformation("Executing local PHP restore: {Php} {Args}", _options.PhpExecutable, arguments);
        var result = await _processRunner.ExecuteAsync(
            _options.PhpExecutable,
            arguments,
            _options.TimeoutSeconds,
            cancellationToken).ConfigureAwait(false);

        return ParseRestoreOutput(result);
    }

    private MoodleRestoreResult ParseRestoreOutput(ProcessExecutionResult result)
    {
        var output = result.StandardOutput;
        var error = result.StandardError;

        if (result.ExitCode != 0)
        {
            var msg = !string.IsNullOrWhiteSpace(error) ? error : output;
            _logger?.LogError("Moodle restore CLI process failed with exit code {Code}: {Msg}", result.ExitCode, msg);
            return new MoodleRestoreResult(
                Success: false,
                CourseId: null,
                ExitCode: result.ExitCode,
                StandardOutput: output,
                StandardError: error,
                ErrorMessage: $"Moodle CLI restore failed with exit code {result.ExitCode}: {msg.Trim()}");
        }

        int? courseId = null;
        var match = CourseIdRegex().Match(output);
        if (match.Success && int.TryParse(match.Groups[1].Value, out var parsedId))
        {
            courseId = parsedId;
        }

        _logger?.LogInformation("Moodle restore completed successfully. Course ID: {CourseId}", courseId);
        return new MoodleRestoreResult(
            Success: true,
            CourseId: courseId,
            ExitCode: result.ExitCode,
            StandardOutput: output,
            StandardError: error);
    }

    private async Task<string> ResolveContainerNameAsync(string configuredName, CancellationToken cancellationToken)
    {
        var inspectResult = await _processRunner.ExecuteAsync(
            _options.DockerExecutable,
            $"inspect -f \"{{{{.State.Running}}}}\" {configuredName}",
            5,
            cancellationToken).ConfigureAwait(false);

        if (inspectResult.ExitCode == 0 && inspectResult.StandardOutput.Trim().Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            return configuredName;
        }

        var findResult = await _processRunner.ExecuteAsync(
            _options.DockerExecutable,
            "ps --filter \"name=moodle\" --format \"{{.Names}}\"",
            5,
            cancellationToken).ConfigureAwait(false);

        if (findResult.ExitCode == 0 && !string.IsNullOrWhiteSpace(findResult.StandardOutput))
        {
            var lines = findResult.StandardOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var candidate = line.Trim();
                if (!string.IsNullOrWhiteSpace(candidate) && candidate.Contains("moodle", StringComparison.OrdinalIgnoreCase) && !candidate.Contains("mariadb", StringComparison.OrdinalIgnoreCase))
                {
                    _logger?.LogInformation("Resolved Moodle Docker container to: '{Container}'", candidate);
                    return candidate;
                }
            }
        }

        return configuredName;
    }

    [GeneratedRegex(@"(?:course\s+id|course\s+restored\s+with\s+id|id)\s*[:=]?\s*(\d+)", RegexOptions.IgnoreCase)]
    private static partial Regex CourseIdRegex();
}

