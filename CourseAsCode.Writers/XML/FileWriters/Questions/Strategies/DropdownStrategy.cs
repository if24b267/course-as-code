using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class DropdownStrategy : IQuestionStrategy
    {
        private readonly ILogger<DropdownStrategy> _logger;
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;

        /// <summary>
        /// Creates a new instance of the strategy for writing dropdown (matching) questions
        /// </summary>
        /// <param name="logger">Logger instance specific to <see cref="DropdownStrategy"/></param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Sequential ID generator for generating unique, ascending IDs</param>
        public DropdownStrategy(ILogger<DropdownStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
        }


        /// <summary>
        /// Writes the `plugin_qtype_match_question` element of questions.xml for dropdown (matching) questions
        /// </summary>
        /// <param name="xmlService">The XML Service to use to write XML elements</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not DropdownQuestion dropdownQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(DropdownStrategy)}. Expected {nameof(DropdownQuestion)} but got {question.GetType().Name}.");
            }

            xmlService.WriteElement("plugin_qtype_match_question");

            // Match Options ------------------------------------------------------
            int matchOptionsId = _sequentialIdGenerator.GetNextAnswerId(); // Reuse answer ID counter for simplicity (avoids creating a new counter for just this element)
            xmlService.WriteElementWithAttributes("matchoptions", new Dictionary<string, string>
            {
                { "id", matchOptionsId.ToString() },
            });

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
            xmlService.WriteElements(matchOptionsElements);
            xmlService.CloseElement(); // </matchoptions>


            // Matches (Sub-questions and Answer Pool) ---------------------------
            xmlService.WriteElement("matches");

            // Use HashSet to efficiently collect all additional answer options (the wrong answers)
            var additionalAnswerOptions = new HashSet<string>();

            // First pass: Write the actual sub-questions and store additional (wrong) answer options in the HashSet
            foreach (var subQuestion in dropdownQuestion.SubQuestions)
            {
                int matchId = _sequentialIdGenerator.GetNextAnswerId();
                xmlService.WriteElementWithAttributes("match", new Dictionary<string, string>
                {
                    { "id", matchId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "questiontext", subQuestion.QuestionText },
                    { "questiontextformat", _generalHelper.EnumIntegerToString(subQuestion.QuestionTextFormat) },
                    { "answertext", subQuestion.CorrectAnswer },
                });
                xmlService.CloseElement(); // </match>

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
                xmlService.WriteElementWithAttributes("match", new Dictionary<string, string>
                {
                    { "id", poolMatchId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "questiontext", string.Empty }, // Empty question text signifies this is just an available answer choice in the dropdown and not a sub-question
                    { "questiontextformat", "1" },
                    { "answertext", answerOption },
                });
                xmlService.CloseElement(); // </match>
            }

            xmlService.CloseElement(); // </matches>
            xmlService.CloseElement(); // </plugin_qtype_match_question>
        }


        public string GetQuestionTypeString()
        {
            return "match";
        }
    }
}
