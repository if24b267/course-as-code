using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.CalculatedSimple
{
    public class SimpleCalculatedAnswer
    {
        [Description("Answer expression using dataset variables, e.g. {b}*{h}")]
        public required string AnswerText { get; init; }

        [Description("Format of the answer text")]
        public FormattingFormat AnswerTextFormat { get; init; } = FormattingFormat.PlainText;

        [Description("Grade (fraction) assigned to this answer (0.0..1.0)")]
        public float Fraction { get; init; } = 1.0f;

        [Description("Feedback when this answer is matched")]
        public string? Feedback { get; init; }

        [Description("Format in which feedback is provided")]
        public FormattingFormat FeedbackFormat { get; init; } = FormattingFormat.PlainText;
    }
}
