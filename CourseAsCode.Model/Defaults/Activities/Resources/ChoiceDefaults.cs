using System;

namespace CourseAsCode.Model.Defaults.Activities
{
    public static class ChoiceDefaults
    {
        public const bool DEFAULT_PUBLISH = false;
        public const bool DEFAULT_SHOW_RESULTS = false;
        public const int DEFAULT_DISPLAY = 0;

        public const bool DEFAULT_ALLOW_UPDATE = false;
        public const bool DEFAULT_ALLOW_MULTIPLE = false;
        public const bool DEFAULT_SHOW_UNANSWERED = false;
        public const bool DEFAULT_LIMIT_ANSWERS = false;

        public static readonly DateTime? DEFAULT_TIMEOPEN = null;
        public static readonly DateTime? DEFAULT_TIMECLOSE = null;
        public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;

        public const bool DEFAULT_COMPLETION_SUBMIT = false;
        public const bool DEFAULT_SHOW_PREVIEW = false;
        public const bool DEFAULT_INCLUDE_INACTIVE = false;
        public const bool DEFAULT_SHOW_AVAILABLE = false;

        public const int DEFAULT_OPTION_MAX_ANSWERS = 0; 
    }
}
