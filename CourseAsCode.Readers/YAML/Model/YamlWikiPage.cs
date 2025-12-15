using CourseAsCode.Model.Defaults.Activities.Resources;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlWikiPage
    {
        public string Title { get; set; } = string.Empty;
        public string CachedContent { get; set; } = WikiDefaults.DEFAULT_CACHED_CONTENT;
        public bool ReadOnly { get; set; } = WikiDefaults.DEFAULT_READ_ONLY;
    }
}
