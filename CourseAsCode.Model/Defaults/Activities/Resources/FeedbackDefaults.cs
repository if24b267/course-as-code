using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities.Resources;

public static class FeedbackDefaults
{
    public const FormattingFormat DEFAULT_INTRO_FORMAT = FormattingFormat.HTML;

    public const int DEFAULT_ANONYMOUS = 2;
    public const int DEFAULT_EMAIL_NOTIFICATION = 0;
    public const int DEFAULT_MULTIPLE_SUBMIT = 0;
    public const int DEFAULT_AUTONUMBERING = 0;
    public const string? DEFAULT_SITE_AFTER_SUBMIT = null;
    public const string? DEFAULT_PAGE_AFTER_SUBMIT = null;
    public const FormattingFormat DEFAULT_PAGE_AFTER_SUBMIT_FORMAT = FormattingFormat.HTML;
    public const int DEFAULT_PUBLISH_STATS = 1;

    public static readonly DateTime? DEFAULT_TIMEOPEN = null;
    public static readonly DateTime? DEFAULT_TIMECLOSE = null;
    public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;

    public const bool DEFAULT_COMPLETION_SUBMIT = false;
}
