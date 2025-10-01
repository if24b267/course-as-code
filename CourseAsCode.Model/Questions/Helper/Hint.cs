using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Helper
{
    public class Hint
    {
        [Description("Hint text presented to the learner")]
        public required string HintText { get; init; }

        [Description("Format in which HintText is provided")]
        public FormattingFormat HintTextFormat { get; init; } = FormattingFormat.PlainText;

        [Description("Clear incorrect responses when the hint is shown")]
        public bool ClearIncorrectResponses { get; init; } = false;

        [Description("Show the number of correct answers when the hint is shown")]
        public bool ShowNumberOfCorrectAnswers { get; init; } = false;
    }
}
