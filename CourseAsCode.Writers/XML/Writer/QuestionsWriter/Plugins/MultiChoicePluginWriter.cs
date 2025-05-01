using System.Globalization;
using System.Xml;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.QuestionsWriter.Plugins
{
    public class MultiChoicePluginWriter : XmlWriterBase, IQuestionPluginWriter
    {
        /// <summary>
        /// Creates a new instance of the MultiChoicePluginWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to MultiChoicePluginWriter</param>
        public MultiChoicePluginWriter(IXmlWriterFactory xmlWriterFactory, ILogger<MultiChoicePluginWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the `plugin_qtype_multichoice_question` element of questions.xml that is used for multiple choice questions
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="question">the question to save</param>
        /// <param name="generalHelper">>Instance of a general helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Instance of a sequential ID generator for creating unique identifiers</param>
        public void WritePluginData(XmlWriter writer, Question question, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator)
        {
            if (question is not MultipleChoiceQuestion multiChoiceQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(MultiChoicePluginWriter)}. Expected {nameof(MultipleChoiceQuestion)} but got {question.GetType().Name}.");
            }

            writer.WriteStartElement("plugin_qtype_multichoice_question");

            // Answers ------------------------------------------------------------
            writer.WriteStartElement("answers");
            foreach (var answer in multiChoiceQuestion.Answers)
            {
                int answerId = sequentialIdGenerator.GetNextAnswerId();
                writer.WriteStartElement("answer");
                writer.WriteAttributeString("id", answerId.ToString());

                WriteElements(writer, new Dictionary<string, string>
                {
                    { "answertext", answer.AnswerText },
                    { "answerformat", generalHelper.EnumIntegerToString(answer.AnswerTextFormat) },
                    { "fraction", answer.AnswerGrade.ToString("0.0000000", CultureInfo.InvariantCulture) },
                    { "feedback", answer.AnswerFeedback ?? string.Empty },
                    { "feedbackformat", generalHelper.EnumIntegerToString(answer.AnswerFeedbackFormat) },
                });

                writer.WriteEndElement(); // </answer>
            }

            writer.WriteEndElement(); // </answers>

            // Multichoice settings -------------------------------------------------
            writer.WriteStartElement("multichoice");
            int multichoiceId = sequentialIdGenerator.GetNextAnswerId(); // Reuse answer ID counter for simplicity (avoids creating a new counter for just this element)
            writer.WriteAttributeString("id", multichoiceId.ToString());

            var multichoiceElements = new Dictionary<string, string>
            {
                { "layout", "0" },
                { "single", multiChoiceQuestion.SingleChoice ? "1" : "0" },
                { "shuffleanswers", multiChoiceQuestion.ShuffleAnswers ? "1" : "0" },
                { "correctfeedback", multiChoiceQuestion.CorrectResponseFeedback ?? string.Empty },
                { "correctfeedbackformat", generalHelper.EnumIntegerToString(multiChoiceQuestion.CorrectResponseFeedbackFormat) },
                { "partiallycorrectfeedback", multiChoiceQuestion.PartiallyCorrectResponseFeedback ?? string.Empty },
                { "partiallycorrectfeedbackformat", generalHelper.EnumIntegerToString(multiChoiceQuestion.PartiallyCorrectResponseFeedbackFormat) },
                { "incorrectfeedback", multiChoiceQuestion.IncorrectResponseFeedback ?? string.Empty },
                { "incorrectfeedbackformat", generalHelper.EnumIntegerToString(multiChoiceQuestion.IncorrectResponseFeedbackFormat) },
                { "answernumbering", EnumMappers.MapNumberingFormat(multiChoiceQuestion.ChoiceNumbering) },
                { "shownumcorrect", multiChoiceQuestion.Hints != null && multiChoiceQuestion.Hints.Any(h => h.ShowNumberOfCorrectAnswers) ? "1" : "0" },
                { "showstandardinstruction", multiChoiceQuestion.ShowStandardInstructions ? "1" : "0" },
            };
            WriteElements(writer, multichoiceElements);

            writer.WriteEndElement(); // </multichoice>
            writer.WriteEndElement(); // </plugin_qtype_multichoice_question>
        }


        public string GetQuestionTypeString()
        {
            return "multichoice";
        }
    }
}
