using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.CalculatedMulti
{
    public class CalculatedMultiAnswer
    {
        [Description("Answer text (may contain calculated expressions like {={A}*{B}})")]
        public required string AnswerText { get; init; }

        [Description("Format in which AnswerText is provided")]
        public FormattingFormat AnswerTextFormat { get; init; } = FormattingFormat.PlainText;

        [Description("Grade contribution for this choice (0.0..1.0)")]
        public float Fraction { get; init; } = 0.0f;

        [Description("Feedback when this answer is selected")]
        public string? Feedback { get; init; }

        [Description("Format in which Feedback is provided")]
        public FormattingFormat FeedbackFormat { get; init; } = FormattingFormat.PlainText;
    }
}
