using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions.Types;

public class MultipleChoiceQuestion : Question
{
    [Description("Shuffle the order of answer choices per attempt")]
    public bool ShuffleAnswers { get; set; } = false;

    [Description("Defines the numbering/lettering style for the choices")]
    public NumberingFormat ChoiceNumbering { get; set; } = NumberingFormat.None;

    [Description("Show Moodle’s standard instructions (e.g. ‘Select one or more:’)")]
    public bool ShowStandardInstructions { get; set; } = true;

    [Description("If true only a single answer may be selected; otherwise multiple")]
    public bool SingleChoice { get; set; }

    [Description("List of possible answer choices")]
    public List<MultiChoiceAnswer> Answers { get; set; } = new();

    // Specific feedbacks
    public string? CorrectResponseFeedback { get; set; }
    public FormattingFormat CorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

    public string? PartiallyCorrectResponseFeedback { get; set; }
    public FormattingFormat PartiallyCorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

    public string? IncorrectResponseFeedback { get; set; }
    public FormattingFormat IncorrectResponseFeedbackFormat { get; set; } = FormattingFormat.PlainText;

    [Description("Hints offered when using interactive/adaptive behaviour")]
    public List<Hint>? Hints { get; set; }
}
