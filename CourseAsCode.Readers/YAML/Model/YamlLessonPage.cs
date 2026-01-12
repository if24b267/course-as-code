using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlLessonPage
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public FormattingFormat ContentFormat { get; set; } = FormattingFormat.HTML;
    }
}
