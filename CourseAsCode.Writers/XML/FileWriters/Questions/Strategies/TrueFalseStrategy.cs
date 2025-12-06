using System.Globalization;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.TrueFalse;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class TrueFalseStrategy : IQuestionStrategy
    {
        private readonly ILogger<TrueFalseStrategy> _logger;
        private readonly IGeneralHelper _generalHelper;

        /// <summary>
        /// Creates a new instance of the strategy for writing true or false questions
        /// </summary>
        /// <param name="logger">Logger instance specific to TrueFalseStrategy</param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        public TrueFalseStrategy(ILogger<TrueFalseStrategy> logger, IGeneralHelper generalHelper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }


        /// <summary>
        /// Writes the `plugin_qtype_truefalse_question` element of questions.xml that is used for true / false questions
        /// </summary>
        /// <param name="xmlService">The XML Service to use to write XML elements</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not TrueFalseQuestion trueFalseQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(TrueFalseStrategy)}. Expected {nameof(TrueFalseQuestion)} but got {question.GetType().Name}.");
            }

            xmlService.WriteElement("plugin_qtype_truefalse_question");

            // Answers ------------------------------------------------------------
            xmlService.WriteElement("answers");

            if (trueFalseQuestion.Answers.Count != 2)
            {
                _logger.LogWarning("True/False question with ID {QuestionId} does not have exactly 2 answers. It has {AnswerCount} answers. Course As Code will try to recover from this, but expect undefined behavior!", question.QuestionId, trueFalseQuestion.Answers.Count);
            }

            List<int> answerIds = new List<int> { 1, 2, 3 };

            for (int i = 0; i < 2; i++)
            {
                xmlService.WriteElementWithAttributes("answer", new Dictionary<string, string>
                {
                    { "id", answerIds.ElementAtOrDefault(i).ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "answertext", trueFalseQuestion.Answers.ElementAtOrDefault(i)?.AnswerText ?? (i == 0 ? "True" : "False") },
                    { "answerformat", _generalHelper.EnumIntegerToString(trueFalseQuestion.Answers.ElementAtOrDefault(i)?.AnswerTextFormat ?? FormattingFormat.MoodleAutoFormat) },
                    { "fraction", trueFalseQuestion.Answers.ElementAtOrDefault(i)?.AnswerGrade.ToString("0.0000000", CultureInfo.InvariantCulture) ?? (i == 0 && trueFalseQuestion.CorrectAnswer ? "1.0000000" : i == 1 && !trueFalseQuestion.CorrectAnswer ? "1.0000000" : "0.0000000") },
                    { "feedback", trueFalseQuestion.Answers.ElementAtOrDefault(i)?.AnswerFeedback ?? string.Empty },
                    { "feedbackformat", _generalHelper.EnumIntegerToString(trueFalseQuestion.Answers.ElementAtOrDefault(i)?.AnswerFeedbackFormat ?? FormattingFormat.HTML) },
                });

                xmlService.CloseElement(); // </answer>
            }
            xmlService.CloseElement(); // </answers>

            // truefalse ------------------------------------------------------------
            int trueFalseId = answerIds.ElementAtOrDefault(2);
            xmlService.WriteElementWithAttributes("truefalse", new Dictionary<string, string>
            {
                { "id", trueFalseId.ToString() },
            });

            var trueFalseElements = new Dictionary<string, string>
            {
                { "trueanswer", answerIds.ElementAtOrDefault(0).ToString() },
                { "falseanswer", answerIds.ElementAtOrDefault(1).ToString() },
                { "showstandardinstruction", trueFalseQuestion.ShowStandardInstructions ? "1" : "0" },
            };
            xmlService.WriteElements(trueFalseElements);

            xmlService.CloseElement(); // </truefalse>
            xmlService.CloseElement(); // </plugin_qtype_truefalse_question>
        }


        public string GetQuestionTypeString()
        {
            return "truefalse";
        }
    }
}
