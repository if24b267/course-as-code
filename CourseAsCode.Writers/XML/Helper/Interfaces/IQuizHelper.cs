using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;

namespace CourseAsCode.Writers.XML.Helper
{
    public interface IQuizHelper
    {
        /// <summary>
        /// Maps a TimeExpiryHandle enum value to its corresponding string representation as used in Moodle XML
        /// </summary>
        /// <param name="handle">The TimeExpiryHandle value to map.</param>
        /// <returns>A string representation of the specified TimeExpiryHandle enum value. Defaults to "autosubmit" if the value is not recognized.</returns>
        string MapTimeExpiryHandle(TimeExpiryHandle handle);

        /// <summary>
        /// Maps the provided QuestionBehaviour enum to its corresponding string value as used in Moodle XML
        /// </summary>
        /// <param name="behaviour">The QuestionBehaviour enum value representing the question's behavior type.</param>
        /// <returns>A string representation of the given QuestionBehaviour value. Defaults to "deferredfeedback" if the value is not recognized.</returns>
        string MapQuestionBehaviour(QuestionBehaviour behaviour);

        /// <summary>
        /// Maps a given question instance to the corresponding question type string as used in Moodle XML.
        /// </summary>
        /// <param name="question">The question instance to map to a question type.</param>
        /// <returns>A string representing the mapped question type. Defaults to "multichoice" if the question type is unrecognized.</returns>
        string MapQuestionType(Question question);

        /// <summary>
        /// Maps a NumberingFormat enum value to its corresponding string representation as used in Moodle XML.
        /// </summary>
        /// <param name="format">The numbering format to map.</param>
        /// <returns>A string representing the specified numbering format. Returns "none" for unrecognized formats.</returns>
        string MapNumberingFormat(NumberingFormat format);
    }
}
