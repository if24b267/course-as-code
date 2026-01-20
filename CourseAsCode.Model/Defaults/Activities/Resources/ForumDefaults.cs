using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Forums;

namespace CourseAsCode.Model.Defaults.Activities.Resources;

public static class ForumDefaults
{
    // General Settings
    public const ForumType DEFAULT_FORUM_TYPE = ForumType.General;
    public const string DEFAULT_INTRO = "";
    public const FormattingFormat DEFAULT_INTRO_FORMAT = FormattingFormat.HTML;

    // Date Settings (Unix timestamps - 0 means not set)
    public const int DEFAULT_DUE_DATE = 0;
    public const int DEFAULT_CUTOFF_DATE = 0;

    // Assessment Settings
    public const bool DEFAULT_ASSESSED = false;
    public const int DEFAULT_ASSESS_TIME_START = 0;
    public const int DEFAULT_ASSESS_TIME_FINISH = 0;
    public const int DEFAULT_SCALE = 0;

    // Attachment Settings
    public const int DEFAULT_MAX_BYTES = 512000;
    public const int DEFAULT_MAX_ATTACHMENTS = 1;

    // Subscription Settings
    public const SubscriptionMode DEFAULT_SUBSCRIPTION_MODE = SubscriptionMode.Optional;

    // Tracking Settings
    public const ForumTrackingType DEFAULT_TRACKING_TYPE = ForumTrackingType.Optional;

    // RSS Settings
    public const ForumRssType DEFAULT_RSS_TYPE = ForumRssType.None;
    public const int DEFAULT_RSS_ARTICLES = 0;

    // Time Settings
    public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;

    // Blocking Settings
    public const int DEFAULT_WARN_AFTER = 0;
    public const int DEFAULT_BLOCK_AFTER = 0;
    public const int DEFAULT_BLOCK_PERIOD = 0;

    // Completion Settings
    public const int DEFAULT_COMPLETION_DISCUSSIONS = 0;
    public const int DEFAULT_COMPLETION_REPLIES = 0;
    public const int DEFAULT_COMPLETION_POSTS = 0;

    // Display Settings
    public const bool DEFAULT_DISPLAY_WORD_COUNT = false;

    // Lock Settings
    public const int DEFAULT_LOCK_DISCUSSION_AFTER = 0;

    // Grading Settings
    public const bool DEFAULT_GRADE_FORUM = false;
}
