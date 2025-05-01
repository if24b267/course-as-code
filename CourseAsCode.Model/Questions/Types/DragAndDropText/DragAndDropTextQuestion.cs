using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions.Types.DragAndDropText
{
    public class DragAndDropTextQuestion : Question
    {
        [Description("list of the possible choices for the cloze text")]
        public required List<TextChoice> Answers { get; set; } = new();

        [Description("whether the possible choices for the cloze text are shuffled")]
        public bool ShuffleAnswers { get; set; }

        // Specific feedbacks
        [Description("Feedback shown after the question was answered correctly")]
        public string? CorrectResponseFeedback { get; set; }
        public FormattingFormat CorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

        [Description("Feedback shown after the question was answered partially correct")]
        public string? PartiallyCorrectResponseFeedback { get; set; }
        public FormattingFormat PartiallyCorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

        [Description("Feedback shown after the question was answered incorrectly")]
        public string? IncorrectResponseFeedback { get; set; }
        public FormattingFormat IncorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

        [Description("Hints offered when using interactive/adaptive behaviour")]
        public List<Hint>? Hints { get; set; }
    }
}
