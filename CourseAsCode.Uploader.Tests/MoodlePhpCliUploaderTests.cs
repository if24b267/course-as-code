using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CourseAsCode.Uploader.Tests;

[TestFixture]
public class MoodlePhpCliUploaderTests
{
    private string _tempTestDir = null!;
    private string _tempMbzFile = null!;

    [SetUp]
    public void SetUp()
    {
        _tempTestDir = Path.Combine(Path.GetTempPath(), "CourseAsCodeTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempTestDir);
        _tempMbzFile = Path.Combine(_tempTestDir, "test_course.mbz");
        File.WriteAllText(_tempMbzFile, "dummy mbz content");
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_tempTestDir))
        {
            Directory.Delete(_tempTestDir, true);
        }
    }

    [Test]
    public void Constructor_ThrowsArgumentNullException_WhenOptionsAreNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MoodlePhpCliUploader(null!));
    }

    [Test]
    public void RestoreCourseAsync_ThrowsArgumentException_WhenPathIsNullOrEmpty()
    {
        var uploader = new MoodlePhpCliUploader(new MoodlePhpCliOptions());
        Assert.ThrowsAsync<ArgumentException>(async () => await uploader.RestoreCourseAsync(string.Empty));
    }

    [Test]
    public void RestoreCourseAsync_ThrowsFileNotFoundException_WhenFileDoesNotExist()
    {
        var uploader = new MoodlePhpCliUploader(new MoodlePhpCliOptions());
        var nonExistentPath = Path.Combine(_tempTestDir, "missing.mbz");
        Assert.ThrowsAsync<FileNotFoundException>(async () => await uploader.RestoreCourseAsync(nonExistentPath));
    }

    [Test]
    public async Task RestoreCourseAsync_DockerMode_CopiesFileExecutesCliAndCleansUp()
    {
        var mockRunner = new MockProcessRunner();
        mockRunner.RegisterResponse("docker", "inspect", new ProcessExecutionResult(0, "true", string.Empty));
        mockRunner.RegisterResponse("docker", "cp ", new ProcessExecutionResult(0, string.Empty, string.Empty));
        mockRunner.RegisterResponse("docker", "exec moodle php", new ProcessExecutionResult(0, "-->Course id 12 restored successfully\n== Restoring finished ==", string.Empty));
        mockRunner.RegisterResponse("docker", "rm -f", new ProcessExecutionResult(0, string.Empty, string.Empty));

        var options = new MoodlePhpCliOptions
        {
            ExecutionMode = MoodleExecutionMode.Docker,
            ContainerName = "moodle",
            DefaultCategoryId = 2,
        };

        var uploader = new MoodlePhpCliUploader(options, mockRunner);
        var result = await uploader.RestoreCourseAsync(_tempMbzFile);

        Assert.That(result.Success, Is.True);
        Assert.That(result.CourseId, Is.EqualTo(12));
        Assert.That(mockRunner.ExecutedCommands.Any(c => c.Arguments.StartsWith("cp ")), Is.True);
        Assert.That(mockRunner.ExecutedCommands.Any(c => c.Arguments.Contains("exec moodle php") && c.Arguments.Contains("--categoryid=2")), Is.True);
        Assert.That(mockRunner.ExecutedCommands.Any(c => c.Arguments.Contains("rm -f")), Is.True);
    }

    [Test]
    public async Task RestoreCourseAsync_DockerMode_FailsWhenCopyFails()
    {
        var mockRunner = new MockProcessRunner();
        mockRunner.RegisterResponse("docker", "inspect", new ProcessExecutionResult(0, "true", string.Empty));
        mockRunner.RegisterResponse("docker", "cp ", new ProcessExecutionResult(1, string.Empty, "Error: No such container: moodle"));

        var options = new MoodlePhpCliOptions
        {
            ExecutionMode = MoodleExecutionMode.Docker,
            ContainerName = "moodle",
        };

        var uploader = new MoodlePhpCliUploader(options, mockRunner);
        var result = await uploader.RestoreCourseAsync(_tempMbzFile);

        Assert.That(result.Success, Is.False);
        Assert.That(result.CourseId, Is.Null);
        Assert.That(result.ErrorMessage, Does.Contain("No such container"));
        Assert.That(mockRunner.ExecutedCommands.Any(c => c.Arguments.StartsWith("cp ")), Is.True);
    }

    [Test]
    public async Task RestoreCourseAsync_LocalProcessMode_ExecutesPhpAndParsesOutput()
    {
        var mockRunner = new MockProcessRunner();
        mockRunner.RegisterResponse("php", "restore_backup.php", new ProcessExecutionResult(0, "Restoring course...\nCourse id: 99\nDone.", string.Empty));

        var options = new MoodlePhpCliOptions
        {
            ExecutionMode = MoodleExecutionMode.LocalProcess,
            MoodleCliScriptPath = "/var/www/moodle/admin/cli/restore_backup.php",
            DefaultCategoryId = 1,
        };

        var uploader = new MoodlePhpCliUploader(options, mockRunner);
        var result = await uploader.RestoreCourseAsync(_tempMbzFile, categoryId: 5);

        Assert.That(result.Success, Is.True);
        Assert.That(result.CourseId, Is.EqualTo(99));
        Assert.That(mockRunner.ExecutedCommands.Count, Is.EqualTo(1));
        Assert.That(mockRunner.ExecutedCommands[0].Arguments, Does.Contain("--categoryid=5"));
        Assert.That(mockRunner.ExecutedCommands[0].Arguments, Does.Contain(_tempMbzFile));
    }

    [Test]
    public async Task RestoreCourseAsync_ReturnsFailure_WhenProcessFails()
    {
        var mockRunner = new MockProcessRunner();
        mockRunner.RegisterResponse("php", "restore_backup.php", new ProcessExecutionResult(2, string.Empty, "Category ID 999 does not exist!"));

        var options = new MoodlePhpCliOptions
        {
            ExecutionMode = MoodleExecutionMode.LocalProcess,
            MoodleCliScriptPath = "admin/cli/restore_backup.php",
        };

        var uploader = new MoodlePhpCliUploader(options, mockRunner);
        var result = await uploader.RestoreCourseAsync(_tempMbzFile);

        Assert.That(result.Success, Is.False);
        Assert.That(result.CourseId, Is.Null);
        Assert.That(result.ExitCode, Is.EqualTo(2));
        Assert.That(result.ErrorMessage, Does.Contain("Category ID 999 does not exist!"));
    }

    private sealed class MockProcessRunner : IProcessRunner
    {
        public sealed record ExecutedCommand(string Executable, string Arguments);

        private readonly List<(string Executable, string ArgSubstring, ProcessExecutionResult Result)> _responses = new();
        public List<ExecutedCommand> ExecutedCommands { get; } = new();

        public void RegisterResponse(string executable, string argSubstring, ProcessExecutionResult result)
        {
            _responses.Add((executable, argSubstring, result));
        }

        public Task<ProcessExecutionResult> ExecuteAsync(
            string executable,
            string arguments,
            int timeoutSeconds = 300,
            CancellationToken cancellationToken = default)
        {
            ExecutedCommands.Add(new ExecutedCommand(executable, arguments));

            foreach (var (expectedExe, argSubstring, result) in _responses)
            {
                if (executable.Equals(expectedExe, StringComparison.OrdinalIgnoreCase) &&
                    arguments.Contains(argSubstring, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(result);
                }
            }

            return Task.FromResult(new ProcessExecutionResult(0, string.Empty, string.Empty));
        }
    }
}
