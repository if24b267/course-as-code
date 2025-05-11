using CourseAsCode.Readers.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlTextMediaArea : YamlActivity
    {
        public TextMediaFieldFormat Format { get; set; }
        public List<YamlTextMediaAreaContent> Content { get; set; }
    }
}
