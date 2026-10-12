namespace CourseAsCode.Uploader;

public class MoodleUploaderOptions
{
    public required Uri BaseUrl { get; set; }

    public bool Headless { get; set; } = true;

    public float TimeoutMs { get; set; } = 30000;
}
