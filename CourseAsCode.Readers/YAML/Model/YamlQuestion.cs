using CourseAsCode.Model.Enums;
using CourseAsCode.Readers.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public string QuestionText { get; set; }
        public string Category { get; set; }
        public bool ShuffleAnswers { get; set; } = false;
        public FormattingFormat QuestionTextFormat { get; set; } = FormattingFormat.PlainText;
        public bool QuestionReady { get; set; } = true;
        public float DefaultMark { get; set; } = 1.0f;
        public QuestionType QuestionType { get; set; }
        public List<YamlQuestionAnswer> Answers { get; set; }
        public float HintPenalty { get; set; } = 0.1f;
        public List<YamlHint>? Hints { get; set; }
        public List<YamlSubQuestion>? SubQuestions { get; set; }
        public string? CorrectResponseFeedback { get; set; }
        public FormattingFormat CorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;
        public string? PartiallyCorrectResponseFeedback { get; set; }
        public FormattingFormat PartiallyCorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;
        public string? IncorrectResponseFeedback { get; set; }
        public FormattingFormat IncorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;
        public List<YamlDragDropAnswerText>? DragDropAnswers { get; set; }
    }
}
