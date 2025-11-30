using CourseAsCode.Model.Defaults.Activities.Resources;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlWikiSubwiki
    {
        public int GroupId { get; set; } = WikiDefaults.DEFAULT_GROUP_ID;
        public int UserId { get; set; } = WikiDefaults.DEFAULT_USER_ID;
        public List<YamlWikiPageFilePath> Pages { get; set; } = [];
    }
}
