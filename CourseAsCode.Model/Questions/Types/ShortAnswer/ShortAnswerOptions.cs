using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.ShortAnswer
{
    public class ShortAnswerOptions
    {
        [Description("Shows the text of the correct answer")]
        public required string AnswerText { get; init; }

        [Description("Format in which AnswerText is provided")]
        public FormattingFormat AnswerTextFormat { get; init; } = FormattingFormat.PlainText;

        [Description("Grade contribution: 1.0 = 100% of question mark, negative values allowed")]
        public float AnswerGrade { get; init; }

        [Description("Feedback shown if the learner selects this answer (optional)")]
        public string? AnswerFeedback { get; init; }

        public FormattingFormat AnswerFeedbackFormat { get; init; } = FormattingFormat.PlainText;
    }
}
