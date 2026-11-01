using System.Threading;
using System.Threading.Tasks;

namespace CourseAsCode.Uploader;

public record ProcessExecutionResult(int ExitCode, string StandardOutput, string StandardError);

public interface IProcessRunner
{
    Task<ProcessExecutionResult> ExecuteAsync(
        string executable,
        string arguments,
        int timeoutSeconds = 300,
        CancellationToken cancellationToken = default);
}
