using System.Globalization;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.MultipleChoice;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class MultiChoiceStrategy : IQuestionStrategy
    {
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;
        private readonly IQuizHelper _quizHelper;

        /// <summary>
        /// Creates a new instance of the strategy for writing multiple choice questions
        /// </summary>
        /// <param name="logger">Logger instance specific to MultiChoicePluginWriter</param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Sequential ID generator for generating unique, ascending IDs</param>
        /// <param name="quizHelper">Quiz Helper providing utility methods for quizzes</param>
        public MultiChoiceStrategy(ILogger<MultiChoiceStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator, IQuizHelper quizHelper)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
            _quizHelper = quizHelper ?? throw new ArgumentNullException(nameof(quizHelper));
        }


        /// <summary>
        /// Writes the `plugin_qtype_multichoice_question` element of questions.xml that is used for multiple choice questions
        /// </summary>
        /// <param name="xmlService">The XML Service to use to write XML elements</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not MultipleChoiceQuestion multiChoiceQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(MultiChoiceStrategy)}. Expected {nameof(MultipleChoiceQuestion)} but got {question.GetType().Name}.");
            }

            xmlService.WriteElement("plugin_qtype_multichoice_question");

            // Answers ------------------------------------------------------------
            xmlService.WriteElement("answers");
            foreach (var answer in multiChoiceQuestion.Answers)
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

            // Multichoice settings -------------------------------------------------
            int multichoiceId = _sequentialIdGenerator.GetNextAnswerId(); // Reuse answer ID counter for simplicity (avoids creating a new counter for just this element)
            xmlService.WriteElementWithAttributes("multichoice", new Dictionary<string, string>
            {
                { "id", multichoiceId.ToString() },
            });

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
            xmlService.WriteElements(multichoiceElements);

            xmlService.CloseElement(); // </multichoice>
            xmlService.CloseElement(); // </plugin_qtype_multichoice_question>
        }


        public string GetQuestionTypeString()
        {
            return "multichoice";
        }
    }
}
