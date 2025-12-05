using System.Xml;
using CourseAsCode.Model.Questions;

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
        /// <param name="writer">The XmlWriter to write to</param>
        /// <param name="question">The question data (needs casting to the specific type)</param>
        void WritePluginData(XmlWriter writer, Question question);

        /// <summary>
        /// Gets the internal Moodle string identifier for this question type (e.g., "multichoice", "match").
        /// </summary>
        /// <returns>The Moodle question type string.</returns>
        string GetQuestionTypeString();
    }
}
