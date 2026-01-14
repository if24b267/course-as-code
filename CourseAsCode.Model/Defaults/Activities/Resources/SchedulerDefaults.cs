using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities.Resources
{
    public static class SchedulerDefaults
    {
        public const string DEFAULT_SCHEDULER_MODE = "oneonly";
        public const int DEFAULT_MAXBOOKINGS = 1;
        public const int DEFAULT_GUARDTIME = 0;
        public const int DEFAULT_DEFAULTSLOTDURATION = 20;
        public const int DEFAULT_ALLOWNOTIFICATIONS = 0;
        public const string DEFAULT_STAFFROLENAME = "";
        public const int DEFAULT_SCALE = 0;
        public const int DEFAULT_GRADINGSTRATEGY = 0;
        public const int DEFAULT_BOOKINGROUPING = -1;
        public const int DEFAULT_USENOTES = 1;
        public const int DEFAULT_USEBOOKINGFORM = 0;
        public const string DEFAULT_BOOKINGINSTRUCTIONS = "";
        public const FormattingFormat DEFAULT_BOOKINGINSTRUCTIONSFORMAT = FormattingFormat.HTML;
        public const int DEFAULT_USESTUDENTNOTES = 0;
        public const int DEFAULT_REQUIREUPLOAD = 0;
        public const int DEFAULT_UPLOADMAXFILES = 0;
        public const int DEFAULT_UPLOADMAXSIZE = 0;
        public const int DEFAULT_USECAPTCHA = 0;

        public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;
    }
}
