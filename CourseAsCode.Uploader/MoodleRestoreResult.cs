namespace CourseAsCode.Uploader;

public record MoodleRestoreResult(
    bool Success,
    int? CourseId,
    int ExitCode,
    string StandardOutput,
    string StandardError,
    string? ErrorMessage = null);
