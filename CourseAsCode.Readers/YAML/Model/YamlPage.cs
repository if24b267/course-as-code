using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlPage : YamlActivity
    {
        public Context Context { get; set; } = Context.Course;
        public string Intro { get; set; } = PageDefault.DEFAULT_INTRO;
        public FormattingFormat IntroFormat { get; set; } = PageDefault.DEFAULT_INTRO_FORMAT;
        public string Content { get; set; } = PageDefault.DEFAULT_CONTENT;
        public FormattingFormat ContentFormat { get; set; } = PageDefault.DEFAULT_CONTENT_FORMAT;
        public string ModuleName { get; set; } = "page";
    }
}
