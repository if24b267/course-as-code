using CourseAsCode.Model.Questions;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Interfaces
{
    /// <summary>
    /// Defines the contract for writing the plugin-specific XML data for a question type
    /// </summary>
    public interface IQuestionStrategy
    {
        /// <summary>
        /// Writes the specific XML elements for the question type's plugin block
        /// </summary>
        /// <param name="xmlService">The XML service to use for writing XML elements</param>
        /// <param name="question">The question data (needs casting to the specific type)</param>
        void WritePluginData(IXmlService xmlService, Question question);

        /// <summary>
        /// Gets the internal Moodle string identifier for this question type (e.g., "multichoice", "match").
        /// </summary>
        /// <returns>The Moodle question type string.</returns>
        string GetQuestionTypeString();
    }
}
