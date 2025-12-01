using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Wikis;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlWiki : YamlActivity
    {
        public string Intro { get; set; } = WikiDefaults.DEFAULT_INTRO;
        public FormattingFormat IntroFormat { get; set; } = WikiDefaults.DEFAULT_INTRO_FORMAT;
        public string FirstPageTitle { get; set; } = WikiDefaults.DEFAULT_FIRST_PAGE_TITLE;
        public WikiMode WikiMode { get; set; } = WikiDefaults.DEFAULT_WIKI_MODE;
        public WikiFormat DefaultFormat { get; set; } = WikiDefaults.DEFAULT_WIKI_FORMAT;
        public bool ForceFormat { get; set; } = WikiDefaults.DEFAULT_FORCE_FORMAT;
        public long EditBegin { get; set; } = WikiDefaults.DEFAULT_EDIT_BEGIN;
        public long EditEnd { get; set; } = WikiDefaults.DEFAULT_EDIT_END;
        public List<YamlWikiSubwiki> Subwikis { get; set; } = [];
    }
}
