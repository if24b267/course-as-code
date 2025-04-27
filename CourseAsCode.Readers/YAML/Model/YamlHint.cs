using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlHint
    {
        public string HintText { get; set; }
        public FormattingFormat HintTextFormat { get; set; } = FormattingFormat.PlainText;
        public bool ClearIncorrectResponses { get; set; } = false;
        public bool ShowNumberOfCorrectAnswers { get; set; } = false;
    }
}
