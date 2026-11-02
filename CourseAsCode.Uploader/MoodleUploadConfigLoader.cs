using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseAsCode.Uploader;

/// <summary>
/// Result of loading and validating the Moodle upload configuration.
/// </summary>
public class MoodleUploadConfigResult
{
    public MoodlePhpCliOptions Options { get; init; } = new();

    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

    public bool IsValid => Errors.Count == 0;
}

/// <summary>
/// Loads <see cref="MoodlePhpCliOptions"/> from defaults, an optional JSON file and environment variables
/// (in this order, later sources win) and validates the result.
/// The PHP-CLI restore needs no credentials; never put passwords or tokens into the config file in the repository.
/// </summary>
public static class MoodleUploadConfigLoader
{
    public const string DefaultConfigFileName = "moodle-upload.json";
    public const string EnvPrefix = "COURSEASCODE_MOODLE_";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public static MoodleUploadConfigResult Load(
        string? configFilePath = null,
        Func<string, string?>? getEnv = null,
        string? baseDirectory = null)
    {
        getEnv ??= name => Environment.GetEnvironmentVariable(name);
        var errors = new List<string>();
        var options = new MoodlePhpCliOptions();

        var explicitFile = !string.IsNullOrWhiteSpace(configFilePath);
        var filePath = explicitFile
            ? configFilePath!
            : Path.Combine(baseDirectory ?? Directory.GetCurrentDirectory(), DefaultConfigFileName);

        if (File.Exists(filePath))
        {
            try
            {
                var fromFile = JsonSerializer.Deserialize<MoodlePhpCliOptions>(File.ReadAllText(filePath), JsonOptions);
                if (fromFile != null)
                {
                    options = fromFile;
                }
            }
            catch (JsonException ex)
            {
                errors.Add($"Config file '{filePath}' is not valid JSON: {ex.Message}");
            }
        }
        else if (explicitFile)
        {
            errors.Add($"Config file '{filePath}' was not found.");
        }

        ApplyEnvironment(options, getEnv, errors);
        Validate(options, errors);

        return new MoodleUploadConfigResult { Options = options, Errors = errors };
    }

    private static void ApplyEnvironment(MoodlePhpCliOptions options, Func<string, string?> getEnv, List<string> errors)
    {
        string? Read(string key) => getEnv(EnvPrefix + key) is { Length: > 0 } value ? value : null;

        if (Read("MODE") is { } mode)
        {
            if (Enum.TryParse<MoodleExecutionMode>(mode, true, out var parsedMode))
            {
                options.ExecutionMode = parsedMode;
            }
            else
            {
                errors.Add($"{EnvPrefix}MODE must be 'Docker' or 'LocalProcess' (was '{mode}').");
            }
        }

        if (Read("CONTAINER") is { } container)
        {
            options.ContainerName = container;
        }

        if (Read("CLI_SCRIPT") is { } script)
        {
            options.MoodleCliScriptPath = script;
        }

        if (Read("PHP") is { } php)
        {
            options.PhpExecutable = php;
        }

        if (Read("DOCKER") is { } docker)
        {
            options.DockerExecutable = docker;
        }

        if (Read("CATEGORY_ID") is { } category)
        {
            if (int.TryParse(category, out var parsedCategory))
            {
                options.DefaultCategoryId = parsedCategory;
            }
            else
            {
                errors.Add($"{EnvPrefix}CATEGORY_ID must be a number (was '{category}').");
            }
        }

        if (Read("TIMEOUT_SECONDS") is { } timeout)
        {
            if (int.TryParse(timeout, out var parsedTimeout))
            {
                options.TimeoutSeconds = parsedTimeout;
            }
            else
            {
                errors.Add($"{EnvPrefix}TIMEOUT_SECONDS must be a number (was '{timeout}').");
            }
        }

        if (Read("SHOW_DEBUGGING") is { } debugging)
        {
            if (bool.TryParse(debugging, out var parsedDebugging))
            {
                options.ShowDebugging = parsedDebugging;
            }
            else
            {
                errors.Add($"{EnvPrefix}SHOW_DEBUGGING must be 'true' or 'false' (was '{debugging}').");
            }
        }
    }

    private static void Validate(MoodlePhpCliOptions options, List<string> errors)
    {
        if (!Enum.IsDefined(options.ExecutionMode))
        {
            errors.Add("ExecutionMode must be 'Docker' or 'LocalProcess'.");
        }

        if (options.ExecutionMode == MoodleExecutionMode.Docker && string.IsNullOrWhiteSpace(options.ContainerName))
        {
            errors.Add("ContainerName must not be empty in Docker mode.");
        }

        if (string.IsNullOrWhiteSpace(options.MoodleCliScriptPath))
        {
            errors.Add("MoodleCliScriptPath must not be empty.");
        }

        if (options.DefaultCategoryId < 1)
        {
            errors.Add("DefaultCategoryId must be 1 or higher.");
        }

        if (options.TimeoutSeconds is < 1 or > 3600)
        {
            errors.Add("TimeoutSeconds must be between 1 and 3600.");
        }
    }
}
