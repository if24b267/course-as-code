using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.Calculated
{
    public class CalculatedAnswer
    {
        [Description("Text shown for the answer (formula using dataset variables, e.g. {b} * {h})")]
        public required string AnswerText { get; init; }

        [Description("Format in which AnswerText is provided")]
        public FormattingFormat AnswerTextFormat { get; init; } = FormattingFormat.PlainText;

        [Description("Grade contribution: 1.0 = 100% of question mark, negative values allowed")]
        public float AnswerGrade { get; init; } = 1.0f;

        [Description("Feedback shown when this answer is matched")]
        public string? AnswerFeedback { get; init; }

        [Description("Format in which AnswerFeedback is provided")]
        public FormattingFormat AnswerFeedbackFormat { get; init; } = FormattingFormat.PlainText;
    }
}
