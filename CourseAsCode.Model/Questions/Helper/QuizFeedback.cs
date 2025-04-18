using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Helper;

public class QuizFeedback
{
    [Description("Minimum grade boundary (percentage 0‑100) for which this feedback applies")]
    public float GradeBoundary { get; init; }

    [Description("Feedback body shown once the attempt is submitted")]
    public required string FeedbackText { get; init; }

    public FormattingFormat OverallFeedbackFormat { get; init; } = FormattingFormat.PlainText;
}