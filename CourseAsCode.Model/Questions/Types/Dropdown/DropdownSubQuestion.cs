using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.Dropdown
{
    /// <summary>
    /// Each dropdown question consists of multiple sub-questions, for each of which an answer is to be selected.
    /// Each sub-question must have one correct answer and can have additional wrong answers
    /// </summary>
    public class DropdownSubQuestion
    {
        [Description("Text shown besides the Dropdown menu, i.e. the question text")]
        public required string QuestionText { get; init; }

        [Description("Format in which QuestionText is provided")]
        public FormattingFormat QuestionTextFormat { get; init; } = FormattingFormat.PlainText;

        [Description("Correct answer to the question")]
        public required string CorrectAnswer { get; init; }

        [Description("List of incorrect answers to the question")]
        public List<string> WrongAnswers { get; init; } = new List<string>();

        /* Note: Due to the way Matching Questions work in Moodle, any Wrong and Correct Answer will also be displayed as a selectable option for all other sub-questions:
         *
         * Example:
         * SubQuestion1: CorrectAnswer1, WrongAnswer1, WrongAnswer2
         * SubQuestion2: CorrectAnswer2, WrongAnswer1, WrongAnswer3
         *
         * Dropdown Menu for SubQuestion1: CorrectAnswer1, CorrectAnswer2, WrongAnswer1, WrongAnswer2, WrongAnswer3
         * Dropdown Menu for SubQuestion2: CorrectAnswer1, CorrectAnswer2, WrongAnswer1, WrongAnswer2, WrongAnswer3
         */
    }
}
