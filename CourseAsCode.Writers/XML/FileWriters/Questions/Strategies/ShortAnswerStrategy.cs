using System.Globalization;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.ShortAnswer;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class ShortAnswerStrategy : IQuestionStrategy
    {
        private readonly ILogger<ShortAnswerStrategy> _logger;
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;

        /// <summary>
        /// Creates a new instance of the strategy for writing dropdown (matching) questions
        /// </summary>
        /// <param name="logger">Logger instance specific to <see cref="ShortAnswerStrategy"/></param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Sequential ID generator for generating unique, ascending IDs</param>
        public ShortAnswerStrategy(ILogger<ShortAnswerStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
        }


        /// <summary>
        /// Writes the `plugin_qtype_multichoice_question` element of questions.xml that is used for multiple choice questions
        /// </summary>
        /// <param name="xmlService">The XML Service to use to write XML elements</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not ShortAnswerQuestion shortAnswerQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(ShortAnswerStrategy)}. Expected {nameof(ShortAnswerQuestion)} but got {question.GetType().Name}.");
            }

            xmlService.WriteElement("plugin_qtype_shortanswer_question");

            // Answers ------------------------------------------------------------
            xmlService.WriteElement("answers");
            foreach (var answer in shortAnswerQuestion.Answers)
            {
                int answerId = _sequentialIdGenerator.GetNextAnswerId();
                xmlService.WriteElementWithAttributes("answer", new Dictionary<string, string>
                {
                    { "id", answerId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "answertext", answer.AnswerText },
                    { "answerformat", _generalHelper.EnumIntegerToString(answer.AnswerTextFormat) },
                    { "fraction", answer.AnswerGrade.ToString("0.0000000", CultureInfo.InvariantCulture) },
                    { "feedback", answer.AnswerFeedback ?? string.Empty },
                    { "feedbackformat", _generalHelper.EnumIntegerToString(answer.AnswerFeedbackFormat) },
                });

                xmlService.CloseElement(); // </answer>
            }

            xmlService.CloseElement(); // </answers>

            // Short Answer settings -------------------------------------------------
            int shortanswerId = _sequentialIdGenerator.GetNextAnswerId(); // Reuse answer ID counter for simplicity (avoids creating a new counter for just this element)
            xmlService.WriteElementWithAttributes("shortanswer", new Dictionary<string, string>
            {
                { "id", shortanswerId.ToString() },
            });

            var shortanswerElements = new Dictionary<string, string>
            {
                { "usecase", shortAnswerQuestion.UseCase ? "1" : "0" },
            };
            xmlService.WriteElements(shortanswerElements);

            xmlService.CloseElement(); // </shortanswer>
            xmlService.CloseElement(); // </plugin_qtype_shortanswer_question>
        }


        public string GetQuestionTypeString()
        {
            return "shortanswer";
        }
    }
}
