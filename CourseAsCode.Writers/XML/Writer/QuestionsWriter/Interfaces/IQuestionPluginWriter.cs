using System.Xml;
using CourseAsCode.Model.Questions;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces
{
    /// <summary>
    /// Defines the contract for writing the plugin-specific XML data for a question type
    /// </summary>
    public interface IQuestionPluginWriter
    {
        /// <summary>
        /// Writes the specific XML elements for the question type's plugin block
        /// </summary>
        /// <param name="writer">The XmlWriter to write to</param>
        /// <param name="question">The question data (needs casting to the specific type)</param>
        /// <param name="generalHelper">>Instance of a general helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">The ID generator for generating unique IDs</param>
        void WritePluginData(XmlWriter writer, Question question, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator);

        /// <summary>
        /// Gets the internal Moodle string identifier for this question type (e.g., "multichoice", "match").
        /// </summary>
        /// <returns>The Moodle question type string.</returns>
        string GetQuestionTypeString();
    }
}
