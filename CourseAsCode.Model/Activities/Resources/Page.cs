using System;
using System.Collections.Generic;
using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources
{
    public class Page : CourseAsCode.Model.Activities.Activity
    {
        private static readonly Random _rnd = new();

        [Description("intro text for the page")]
        public string Intro { get; set; } = PageDefault.DEFAULT_INTRO;

        [Description("format of the intro/description")]
        public FormattingFormat IntroFormat { get; set; } = PageDefault.DEFAULT_INTRO_FORMAT;

        [Description("full HTML/Markdown content of the page")]
        public string Content { get; set; } = PageDefault.DEFAULT_CONTENT;

        [Description("format of the content field")]
        public FormattingFormat ContentFormat { get; set; } = PageDefault.DEFAULT_CONTENT_FORMAT;

        [Description("legacy files flag")]
        public int LegacyFiles { get; set; } = PageDefault.DEFAULT_LEGACY_FILES;

        [Description("legacy files last marker")]
        public string LegacyFilesLast { get; set; } = PageDefault.DEFAULT_LEGACY_FILES_LAST;

        [Description("display mode")]
        public int Display { get; set; } = PageDefault.DEFAULT_DISPLAY;

        [Description("display options (serialized)")]
        public string DisplayOptions { get; set; } = PageDefault.DEFAULT_DISPLAY_OPTIONS;

        [Description("revision number")]
        public int Revision { get; set; } = PageDefault.DEFAULT_REVISION;

        [Description("time the page was last modified")]
        public DateTime TimeModified { get; set; } = PageDefault.DEFAULT_TIME_MODIFIED;

        public SensibleDefaultsPage SensibleDefaults { get; set; } = new SensibleDefaultsPage();
        public SystemDefaultsPage SystemDefaults { get; set; } = new SystemDefaultsPage();

        public class SensibleDefaultsPage
        {
            // placeholder for future sensible defaults
        }

        public class SystemDefaultsPage
        {
            public int ModuleId { get; set; } = _rnd.Next(1, 1000);
            public string ModuleName { get; set; } = "page";
        }
    }
}
