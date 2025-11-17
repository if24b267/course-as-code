using System;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities.Resources
{
    public static class FolderDefault
    {
        public const string DEFAULT_INTRO = "";
        public const FormattingFormat DEFAULT_INTRO_FORMAT = FormattingFormat.HTML;

        public const int DEFAULT_REVISION = 1;
        public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;

        public const int DEFAULT_DISPLAY = 0;
        public const bool DEFAULT_SHOW_EXPANDED = false;
        public const bool DEFAULT_SHOW_DOWNLOAD_FOLDER = false;
        public const bool DEFAULT_FORCE_DOWNLOAD = false;
    }
}
