using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources
{
    public class Folder : CourseAsCode.Model.Activities.Activity
    {
        private static readonly Random _rnd = new();

        [Description("intro text for the folder")]
        public string Intro { get; set; } = FolderDefault.DEFAULT_INTRO;

        [Description("format of the intro/description")]
        public FormattingFormat IntroFormat { get; set; } = FolderDefault.DEFAULT_INTRO_FORMAT;

        [Description("revision number")]
        public int Revision { get; set; } = FolderDefault.DEFAULT_REVISION;

        [Description("time the folder was last modified")]
        public DateTime TimeModified { get; set; } = FolderDefault.DEFAULT_TIME_MODIFIED;

        [Description("display mode")]
        public int Display { get; set; } = FolderDefault.DEFAULT_DISPLAY;

        [Description("whether the folder is shown expanded on the course page")]
        public bool ShowExpanded { get; set; } = FolderDefault.DEFAULT_SHOW_EXPANDED;

        [Description("show a download folder link")]
        public bool ShowDownloadFolder { get; set; } = FolderDefault.DEFAULT_SHOW_DOWNLOAD_FOLDER;

        [Description("force download of contained files")]
        public bool ForceDownload { get; set; } = FolderDefault.DEFAULT_FORCE_DOWNLOAD;

        [Description("files contained in this folder activity")]
        public List<MoodleFile> Files { get; set; } = new();
        public SensibleDefaultsFolder SensibleDefaults { get; set; } = new SensibleDefaultsFolder();
        public SystemDefaultsFolder SystemDefaults { get; set; } = new SystemDefaultsFolder();

        public class SensibleDefaultsFolder
        {
            // placeholder for future sensible defaults
        }

        public class SystemDefaultsFolder
        {
            public int ModuleId { get; set; } = _rnd.Next(1, 1000);
            public string ModuleName { get; set; } = "folder";
        }
    }
}
