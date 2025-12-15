using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities.Resources
{
    public static class LessonDefaults
    {
        // General
        public const string DEFAULT_INTRO = "";
        public const FormattingFormat DEFAULT_INTRO_FORMAT = FormattingFormat.HTML;
        public const int DEFAULT_GRADE = 100;
        public const bool DEFAULT_CUSTOM = true; // custom scoring enabled

        // Access and conditions
        public const bool DEFAULT_USE_PASSWORD = false;
        public const string DEFAULT_PASSWORD = "";
        public const int DEFAULT_DEPENDENCY = 0;
        public const string DEFAULT_CONDITIONS = "O:8:\"stdClass\":3:{s:9:\"timespent\";i:0;s:9:\"completed\";i:0;s:15:\"gradebetterthan\";i:0;}";

        // Attempts and behavior
        public const bool DEFAULT_PRACTICE = false;
        public const bool DEFAULT_MOD_ATTEMPTS = false;
        public const int DEFAULT_MAX_ANSWERS = 5;
        public const int DEFAULT_MAX_ATTEMPTS = 1;
        public const bool DEFAULT_REVIEW = false;
        public const int DEFAULT_NEXT_PAGE_DEFAULT = 0;
        public const int DEFAULT_FEEDBACK = 0;
        public const int DEFAULT_MIN_QUESTIONS = 0;
        public const int DEFAULT_MAX_PAGES = 1;
        public const int DEFAULT_TIME_LIMIT = 0;
        public const bool DEFAULT_RETAKE = false;
        public const int DEFAULT_ACTIVITY_LINK = 0;

        // Media
        public const string DEFAULT_MEDIA_FILE = "";
        public const int DEFAULT_MEDIA_HEIGHT = 480;
        public const int DEFAULT_MEDIA_WIDTH = 640;
        public const bool DEFAULT_MEDIA_CLOSE = false;

        // Slideshow
        public const bool DEFAULT_SLIDESHOW = false;
        public const int DEFAULT_WIDTH = 640;
        public const int DEFAULT_HEIGHT = 480;
        public const string DEFAULT_BG_COLOR = "#FFFFFF";

        // Navigation/Display
        public const bool DEFAULT_DISPLAY_LEFT = false;
        public const bool DEFAULT_DISPLAY_LEFT_IF = false;
        public const bool DEFAULT_PROGRESS_BAR = false;

        // Availability
        public static readonly DateTime? DEFAULT_AVAILABLE = null; // 0 in XML
        public static readonly DateTime? DEFAULT_DEADLINE = null;  // 0 in XML

        // Completion
        public const bool DEFAULT_COMPLETION_END_REACHED = false;
        public const int DEFAULT_COMPLETION_TIME_SPENT = 0;

        // Mobile
        public const bool DEFAULT_ALLOW_OFFLINE_ATTEMPTS = false;
    }
}
