using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlQuestionAnswer
    {
        public string AnswerText { get; set; }
        public string? Feedback { get; set; }
        public float AnswerGrade { get; set; }
        public FormattingFormat AnswerTextFormat { get; set; } = FormattingFormat.PlainText;
        public FormattingFormat FeedbackFormat { get; set; } = FormattingFormat.PlainText;
    }
}
