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
        public bool CorrectAnswer { get; set; } = true;
        public bool ShowStandardInstructions { get; set; } = true;

        // CodeRunner Question specific properties
        public string CodeRunnerType { get; set; }
        public bool AllOrNothing { get; set; } = true;
        public int Precheck { get; set; } = 0;
        public bool ShowSource { get; set; } = false;
        public bool HideCheck { get; set; } = false;
        public int AnswerBoxLines { get; set; } = 18;
        public int AnswerBoxColumns { get; set; } = 100;
        public string AnswerPreload { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public bool ValidateOnSave { get; set; } = true;
        public bool DisplayFeedback { get; set; } = true;
        public bool GiveUpAllowed { get; set; } = true;
        public List<YamlCodeRunnerTestCase> CodeRunnerTestCases { get; set; }

        // Short Answer specific property
        public bool UseCase { get; set; } = false;
    }
}
