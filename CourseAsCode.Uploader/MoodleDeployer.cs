using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Uploader;

/// <summary>
/// <see cref="IDeployer"/> implementation that restores the course via <see cref="MoodlePhpCliUploader"/> in a Docker container.
/// </summary>
public class MoodleDeployer : IDeployer
{
    private readonly ILoggerFactory? _loggerFactory;

    public MoodleDeployer(ILoggerFactory? loggerFactory = null)
    {
        _loggerFactory = loggerFactory;
    }

    public Task<MoodleRestoreResult> DeployAsync(
        string mbzFilePath,
        DeployConfig config,
        CancellationToken cancellationToken = default)
    {
        var options = new MoodlePhpCliOptions
        {
            ExecutionMode = MoodleExecutionMode.Docker,
            DefaultCategoryId = config.CategoryId,
        };

        if (!string.IsNullOrWhiteSpace(config.Target))
        {
            options.ContainerName = config.Target;
        }

        var restorer = new MoodlePhpCliUploader(options, logger: _loggerFactory?.CreateLogger<MoodlePhpCliUploader>());
        return restorer.RestoreCourseAsync(mbzFilePath, config.CategoryId, cancellationToken);
    }
}
