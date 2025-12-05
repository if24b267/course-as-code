using System.Xml;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Plugins
{
    public class DragDropTextStrategy : XmlWriterBase, IQuestionStrategy
    {
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;

        /// <summary>
        /// Creates a new instance of the <see cref="DragDropTextStrategy"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="DragDropTextStrategy"/></param>
        public DragDropTextStrategy(IXmlWriterFactory xmlWriterFactory, ILogger<DragDropTextStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
        }


        /// <summary>
        /// Writes the `plugin_qtype_ddwtos_question` element for Drag and Drop onto Text questions
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(XmlWriter writer, Question question)
        {
            if (question is not DragAndDropTextQuestion dragDropQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(DragDropTextStrategy)}. Expected {nameof(DragAndDropTextQuestion)} but got {question.GetType().Name}.");
            }

            writer.WriteStartElement("plugin_qtype_ddwtos_question");

            // Answers (Draggable Choices) ---------------------------------------
            writer.WriteStartElement("answers");

            // The categories are represented as numbers in the XML -> map the strings to unique IDs
            Dictionary<string, int> categoryToGroupIdMap = MapCategoryToGroupId(dragDropQuestion);

            foreach (var choice in dragDropQuestion.Answers)
            {
                int answerId = _sequentialIdGenerator.GetNextAnswerId();
                writer.WriteStartElement("answer");
                writer.WriteAttributeString("id", answerId.ToString());

                // The <feedback> tag contains a PHP serialized object that is responsible for mapping the answers to a specific category (drag group)

                // --- Determine Drag Group ID ---
                string choiceCategory = choice.Category ?? string.Empty;
                string dragGroupId = categoryToGroupIdMap.TryGetValue(choiceCategory, out int mappedId) ? mappedId.ToString() : "1";

                // --- Construct PHP Serialized Feedback ---
                // Example: O:8:"stdClass":2:{s:9:"draggroup";s:1:"1";s:8:"infinite";b:0;}
                string phpFeedback = $"O:8:\"stdClass\":2:{{s:9:\"draggroup\";s:{dragGroupId.Length}:\"{dragGroupId}\";s:8:\"infinite\";b:0;}}";

                WriteElements(writer, new Dictionary<string, string>
                {
                    { "answertext", choice.Value },
                    { "answerformat", _generalHelper.EnumIntegerToString(choice.ValueFormat) },
                    { "fraction", "0.0000000" }, // Always 0 for this question type
                    { "feedback", phpFeedback },
                    { "feedbackformat", "0" }, // Always 0 for this question type
                });

                writer.WriteEndElement(); // </answer>
            }

            writer.WriteEndElement(); // </answers>

            // DDWTOS Options -----------------------------------------------------
            writer.WriteStartElement("ddwtos");
            int ddwtosId = _sequentialIdGenerator.GetNextAnswerId(); // Reuse answer ID counter for simplicity (avoids creating a new counter for just this element)
            writer.WriteAttributeString("id", ddwtosId.ToString());

            var ddwtosElements = new Dictionary<string, string>
            {
                { "shuffleanswers", dragDropQuestion.ShuffleAnswers ? "1" : "0" },
                { "correctfeedback", dragDropQuestion.CorrectResponseFeedback },
                { "correctfeedbackformat", _generalHelper.EnumIntegerToString(dragDropQuestion.CorrectResponseFeedbackFormat) },
                { "partiallycorrectfeedback", dragDropQuestion.PartiallyCorrectResponseFeedback },
                { "partiallycorrectfeedbackformat", _generalHelper.EnumIntegerToString(dragDropQuestion.PartiallyCorrectResponseFeedbackFormat) },
                { "incorrectfeedback", dragDropQuestion.IncorrectResponseFeedback },
                { "incorrectfeedbackformat", _generalHelper.EnumIntegerToString(dragDropQuestion.IncorrectResponseFeedbackFormat) },
                { "shownumcorrect", dragDropQuestion.Hints?.Any(h => h.ShowNumberOfCorrectAnswers) ?? false ? "1" : "0" },
            };
            WriteElements(writer, ddwtosElements);

            writer.WriteEndElement(); // </ddwtos>
            writer.WriteEndElement(); // </plugin_qtype_ddwtos_question>
        }


        public string GetQuestionTypeString()
        {
            return "ddwtos";
        }


        /// <summary>
        /// Creates a mapping from the string Category (e.g., "A", "B") to a 1-based numeric ID
        /// </summary>
        /// <param name="dragDropQuestion">The <see cref="DragAndDropTextQuestion"/> containing answers belonging to certain categories</param>
        /// <returns>A dictionary mapping category strings to unique group IDs</returns>
        private Dictionary<string, int> MapCategoryToGroupId(DragAndDropTextQuestion dragDropQuestion)
        {
            var categoryToGroupIdMap = new Dictionary<string, int>();
            int nextGroupId = 1;

            var distinctCategories = dragDropQuestion.Answers
                .Select(c => c.Category ?? string.Empty) // There may be answers with no category
                .Distinct()
                .OrderBy(cat => cat); // Sort alphabetically for consistent mapping

            foreach (var category in distinctCategories)
            {
                if (!categoryToGroupIdMap.ContainsKey(category))
                {
                    categoryToGroupIdMap[category] = nextGroupId++;
                }
            }

            // Default to 1 for answers not belonging to any category
            categoryToGroupIdMap[string.Empty] = 1;

            return categoryToGroupIdMap;
        }
    }
}
