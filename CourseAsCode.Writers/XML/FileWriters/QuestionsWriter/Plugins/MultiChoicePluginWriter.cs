using System.Globalization;
using System.Xml;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Plugins
{
    public class MultiChoicePluginWriter : XmlWriterBase, IQuestionPluginWriter
    {
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;
        private readonly IQuizHelper _quizHelper;

        /// <summary>
        /// Creates a new instance of the MultiChoicePluginWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to MultiChoicePluginWriter</param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Sequential ID generator for generating unique IDs</param>
        /// <param name="quizHelper">Quiz Helper providing utility methods for quizzes</param>
        public MultiChoicePluginWriter(IXmlWriterFactory xmlWriterFactory, ILogger<MultiChoicePluginWriter> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator, IQuizHelper quizHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
            _quizHelper = quizHelper ?? throw new ArgumentNullException(nameof(quizHelper));
        }


        /// <summary>
        /// Writes the `plugin_qtype_multichoice_question` element of questions.xml that is used for multiple choice questions
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(XmlWriter writer, Question question)
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
                int answerId = _sequentialIdGenerator.GetNextAnswerId();
                writer.WriteStartElement("answer");
                writer.WriteAttributeString("id", answerId.ToString());

                WriteElements(writer, new Dictionary<string, string>
                {
                    { "answertext", answer.AnswerText },
                    { "answerformat", _generalHelper.EnumIntegerToString(answer.AnswerTextFormat) },
                    { "fraction", answer.AnswerGrade.ToString("0.0000000", CultureInfo.InvariantCulture) },
                    { "feedback", answer.AnswerFeedback ?? string.Empty },
                    { "feedbackformat", _generalHelper.EnumIntegerToString(answer.AnswerFeedbackFormat) },
                });

                writer.WriteEndElement(); // </answer>
            }

            writer.WriteEndElement(); // </answers>

            // Multichoice settings -------------------------------------------------
            writer.WriteStartElement("multichoice");
            int multichoiceId = _sequentialIdGenerator.GetNextAnswerId(); // Reuse answer ID counter for simplicity (avoids creating a new counter for just this element)
            writer.WriteAttributeString("id", multichoiceId.ToString());

            var multichoiceElements = new Dictionary<string, string>
            {
                { "layout", "0" },
                { "single", multiChoiceQuestion.SingleChoice ? "1" : "0" },
                { "shuffleanswers", multiChoiceQuestion.ShuffleAnswers ? "1" : "0" },
                { "correctfeedback", multiChoiceQuestion.CorrectResponseFeedback ?? string.Empty },
                { "correctfeedbackformat", _generalHelper.EnumIntegerToString(multiChoiceQuestion.CorrectResponseFeedbackFormat) },
                { "partiallycorrectfeedback", multiChoiceQuestion.PartiallyCorrectResponseFeedback ?? string.Empty },
                { "partiallycorrectfeedbackformat", _generalHelper.EnumIntegerToString(multiChoiceQuestion.PartiallyCorrectResponseFeedbackFormat) },
                { "incorrectfeedback", multiChoiceQuestion.IncorrectResponseFeedback ?? string.Empty },
                { "incorrectfeedbackformat", _generalHelper.EnumIntegerToString(multiChoiceQuestion.IncorrectResponseFeedbackFormat) },
                { "answernumbering", _quizHelper.MapNumberingFormat(multiChoiceQuestion.ChoiceNumbering) },
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
