using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CourseAsCode.Uploader;

public class ProcessRunner : IProcessRunner
{
    public async Task<ProcessExecutionResult> ExecuteAsync(
        string executable,
        string arguments,
        int timeoutSeconds = 300,
        CancellationToken cancellationToken = default)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = executable,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        using var outputWaitHandle = new AutoResetEvent(false);
        using var errorWaitHandle = new AutoResetEvent(false);

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data == null)
            {
                outputWaitHandle.Set();
            }
            else
            {
                outputBuilder.AppendLine(e.Data);
            }
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data == null)
            {
                errorWaitHandle.Set();
            }
            else
            {
                errorBuilder.AppendLine(e.Data);
            }
        };

        try
        {
            if (!process.Start())
            {
                return new ProcessExecutionResult(-1, string.Empty, $"Failed to start process: {executable}");
            }
        }
        catch (Exception ex)
        {
            return new ProcessExecutionResult(-1, string.Empty, $"Failed to start process '{executable}': {ex.Message}");
        }

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

        try
        {
            await process.WaitForExitAsync(cts.Token).ConfigureAwait(false);
            outputWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            errorWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
        }
        catch (OperationCanceledException)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(true);
                }
            }
            catch
            {
                // Ignore kill errors
            }

            if (cancellationToken.IsCancellationRequested)
            {
                throw;
            }

            return new ProcessExecutionResult(-1, outputBuilder.ToString(), $"Process timed out after {timeoutSeconds} seconds.");
        }

        return new ProcessExecutionResult(process.ExitCode, outputBuilder.ToString(), errorBuilder.ToString());
    }
}
