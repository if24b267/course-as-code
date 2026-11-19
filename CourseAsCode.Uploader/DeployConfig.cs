namespace CourseAsCode.Uploader;

/// <summary>
/// Target configuration for <see cref="IDeployer"/>.
/// </summary>
public class DeployConfig
{
    /// <summary>
    /// Gets or sets the name or ID of the Docker container running Moodle.
    /// If null or empty, a running container whose name contains "moodle" is detected automatically.
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets the ID of the course category the course is restored into (default: 1 for Miscellaneous).
    /// </summary>
    public int CategoryId { get; set; } = 1;
}
