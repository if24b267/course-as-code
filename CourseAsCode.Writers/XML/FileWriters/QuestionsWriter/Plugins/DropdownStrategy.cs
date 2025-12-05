using System.Xml;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Plugins
{
    public class DropdownStrategy : XmlWriterBase, IQuestionStrategy
    {
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;

        /// <summary>
        /// Creates a new instance of the <see cref="DropdownStrategy"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="DropdownStrategy"/></param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Sequential ID generator for generating unique IDs</param>
        public DropdownStrategy(IXmlWriterFactory xmlWriterFactory, ILogger<DropdownStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
        }


        /// <summary>
        /// Writes the `plugin_qtype_match_question` element of questions.xml for dropdown (matching) questions
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(XmlWriter writer, Question question)
        {
            if (question is not DropdownQuestion dropdownQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(DropdownStrategy)}. Expected {nameof(DropdownQuestion)} but got {question.GetType().Name}.");
            }

            writer.WriteStartElement("plugin_qtype_match_question");

            // Match Options ------------------------------------------------------
            writer.WriteStartElement("matchoptions");
            int matchOptionsId = _sequentialIdGenerator.GetNextAnswerId(); // Reuse answer ID counter for simplicity (avoids creating a new counter for just this element)
            writer.WriteAttributeString("id", matchOptionsId.ToString());

            var matchOptionsElements = new Dictionary<string, string>
            {
                { "shuffleanswers", "1" }, // The answer choices will ALWAYS be shuffled, this cannot be changed
                { "correctfeedback", dropdownQuestion.CorrectResponseFeedback ?? string.Empty },
                { "correctfeedbackformat", _generalHelper.EnumIntegerToString(dropdownQuestion.CorrectResponseFeedbackFormat) },
                { "partiallycorrectfeedback", dropdownQuestion.PartiallyCorrectResponseFeedback ?? string.Empty },
                { "partiallycorrectfeedbackformat", _generalHelper.EnumIntegerToString(dropdownQuestion.PartiallyCorrectResponseFeedbackFormat) },
                { "incorrectfeedback", dropdownQuestion.IncorrectResponseFeedback ?? string.Empty },
                { "incorrectfeedbackformat", _generalHelper.EnumIntegerToString(dropdownQuestion.IncorrectResponseFeedbackFormat) },
                { "shownumcorrect", (dropdownQuestion.Hints?.Any(h => h.ShowNumberOfCorrectAnswers) ?? false) ? "1" : "0" }, // Determine if hints require showing the number of correct answers | TODO: Check if this is correct
            };
            WriteElements(writer, matchOptionsElements);
            writer.WriteEndElement(); // </matchoptions>


            // Matches (Sub-questions and Answer Pool) ---------------------------
            writer.WriteStartElement("matches");

            // Use HashSet to efficiently collect all additional answer options (the wrong answers)
            var additionalAnswerOptions = new HashSet<string>();

            // First pass: Write the actual sub-questions and store additional (wrong) answer options in the HashSet
            foreach (var subQuestion in dropdownQuestion.SubQuestions)
            {
                int matchId = _sequentialIdGenerator.GetNextAnswerId();
                writer.WriteStartElement("match");
                writer.WriteAttributeString("id", matchId.ToString());

                WriteElements(writer, new Dictionary<string, string>
                {
                    { "questiontext", subQuestion.QuestionText },
                    { "questiontextformat", _generalHelper.EnumIntegerToString(subQuestion.QuestionTextFormat) },
                    { "answertext", subQuestion.CorrectAnswer },
                });
                writer.WriteEndElement(); // </match>

                // Add all wrong answers for this sub-question to the pool
                foreach (var wrongAnswer in subQuestion.WrongAnswers)
                {
                    additionalAnswerOptions.Add(wrongAnswer);
                }
            }

            // Second pass: Write the additional answer options (the wrong answers) to the XML
            // In Moodle, these are stored as matching options that have no <questiontext> element but do have an <answertext> element
            foreach (var answerOption in additionalAnswerOptions)
            {
                int poolMatchId = _sequentialIdGenerator.GetNextAnswerId();
                writer.WriteStartElement("match");
                writer.WriteAttributeString("id", poolMatchId.ToString());

                WriteElements(writer, new Dictionary<string, string>
                {
                    { "questiontext", string.Empty }, // Empty question text signifies this is just an available answer choice in the dropdown and not a sub-question
                    { "questiontextformat", "1" },
                    { "answertext", answerOption },
                });
                writer.WriteEndElement(); // </match>
            }

            writer.WriteEndElement(); // </matches>
            writer.WriteEndElement(); // </plugin_qtype_match_question>
        }


        public string GetQuestionTypeString()
        {
            return "match";
        }
    }
}
