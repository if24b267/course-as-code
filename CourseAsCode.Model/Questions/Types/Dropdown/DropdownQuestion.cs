using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions.Types.Dropdown
{
    /// <summary>
    /// <para>Model for the Moodle Dropdown question type</para>
    /// <para>Notes: Internal name in Moodle is: `Matching Question`</para>
    /// </summary>
    public class DropdownQuestion : Question
    {
        [Description("Each dropdown question consists of multiple sub-questions, for each of which an answer is to be selected")]
        public List<DropdownSubQuestion> SubQuestions { get; set; } = new List<DropdownSubQuestion>();

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
    }
}
