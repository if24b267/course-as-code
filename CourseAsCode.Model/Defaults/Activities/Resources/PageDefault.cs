using System;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities.Resources
{
    public static class PageDefault
    {
        public const string DEFAULT_INTRO = "";
        public const FormattingFormat DEFAULT_INTRO_FORMAT = FormattingFormat.HTML;

        public const string DEFAULT_CONTENT = "";
        public const FormattingFormat DEFAULT_CONTENT_FORMAT = FormattingFormat.HTML;

        public const int DEFAULT_LEGACY_FILES = 0;
        public const string DEFAULT_LEGACY_FILES_LAST = "$@NULL@$";

        public const int DEFAULT_DISPLAY = 0;
        public const string DEFAULT_DISPLAY_OPTIONS = "";

        public const int DEFAULT_REVISION = 1;
        public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;
    }
}
