using System.Threading;
using System.Threading.Tasks;

namespace CourseAsCode.Uploader;

/// <summary>
/// Deploys a generated .mbz backup into a Moodle instance.
/// The chosen approach is Moodle's CLI restore script (admin/cli/restore_backup.php) executed via Docker,
/// see Documentation/MkDocs/docs/Usage/Importing_A_Course.md.
/// </summary>
public interface IDeployer
{
    /// <summary>
    /// Restores the given .mbz file as a new course in the Moodle instance described by <paramref name="config"/>.
    /// </summary>
    /// <param name="mbzFilePath">Path to the generated .mbz file.</param>
    /// <param name="config">Deployment target (Docker container, course category).</param>
    /// <param name="cancellationToken">Token to cancel the deployment.</param>
    /// <returns>The restore result; <see cref="MoodleRestoreResult.Success"/> is false on error.</returns>
    Task<MoodleRestoreResult> DeployAsync(
        string mbzFilePath,
        DeployConfig config,
        CancellationToken cancellationToken = default);
}
