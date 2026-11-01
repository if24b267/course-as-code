namespace CourseAsCode.Uploader;

public enum MoodleExecutionMode
{
    Docker,
    LocalProcess
}

public class MoodlePhpCliOptions
{
    /// <summary>
    /// Gets or sets whether to execute restore via Docker or a local PHP binary.
    /// Default is Docker.
    /// </summary>
    public MoodleExecutionMode ExecutionMode { get; set; } = MoodleExecutionMode.Docker;

    /// <summary>
    /// Gets or sets the name or container ID of the Docker container running Moodle.
    /// Default is "moodle".
    /// </summary>
    public string ContainerName { get; set; } = "moodle";

    /// <summary>
    /// Gets or sets the path to Moodle's restore_backup.php CLI script.
    /// Default inside Bitnami container: "/opt/bitnami/moodle/admin/cli/restore_backup.php".
    /// </summary>
    public string MoodleCliScriptPath { get; set; } = "/opt/bitnami/moodle/admin/cli/restore_backup.php";

    /// <summary>
    /// Gets or sets the PHP executable name/path when running locally (default: "php").
    /// </summary>
    public string PhpExecutable { get; set; } = "php";

    /// <summary>
    /// Gets or sets the Docker binary name/path (default: "docker").
    /// </summary>
    public string DockerExecutable { get; set; } = "docker";

    /// <summary>
    /// Gets or sets the default category ID to restore into (default: 1 for Miscellaneous).
    /// </summary>
    public int DefaultCategoryId { get; set; } = 1;

    /// <summary>
    /// Gets or sets the process execution timeout in seconds (default: 300).
    /// </summary>
    public int TimeoutSeconds { get; set; } = 300;

    /// <summary>
    /// Gets or sets whether to enable Moodle developer debugging (--showdebugging).
    /// </summary>
    public bool ShowDebugging { get; set; } = false;
}
