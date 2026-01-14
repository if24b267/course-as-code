using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.Description;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Model.Questions.Types.MultipleChoice;
using CourseAsCode.Model.Questions.Types.TrueFalse;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// Provides utility methods to handle mappings and operations related to quizzes
    /// </summary>
    public class QuizHelper : IQuizHelper
    {
        private readonly ILogger<QuizHelper> _logger;

        /// <summary>
        /// Creates a new instance of the QuizHelper class
        /// </summary>
        /// <param name="logger">Logger instance specific to QuizHelper</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public QuizHelper(ILogger<QuizHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public string MapTimeExpiryHandle(TimeExpiryHandle handle)
        {
            return handle switch
            {
                TimeExpiryHandle.AutoSubmit => "autosubmit",
                TimeExpiryHandle.GracePeriod => "graceperiod",
                TimeExpiryHandle.DoNotCount => "autoabandon",

                _ => "autosubmit",
            };
        }


        public string MapQuestionBehaviour(QuestionBehaviour behaviour)
        {
            return behaviour switch
            {
                QuestionBehaviour.Adaptive => "adaptive",
                QuestionBehaviour.AdaptiveNoPenalties => "adaptivenopenalty",
                QuestionBehaviour.Deferred => "deferredfeedback",
                QuestionBehaviour.DeferredCBM => "deferredcbm",
                QuestionBehaviour.Immediate => "immediatefeedback",
                QuestionBehaviour.ImmediateCBM => "immediatecbm",
                QuestionBehaviour.InteractiveMultipleTries => "interactive",

                _ => "deferredfeedback",
            };
        }


        public string MapQuestionType(Question question)
        {
            return question switch
            {
                SingleChoiceQuestion or MultipleChoiceQuestion => "multichoice",
                DropdownQuestion => "match",
                DragAndDropTextQuestion => "ddwtos",
                CodeRunnerQuestion => "coderunner",
                DescriptionQuestion => "description",
                TrueFalseQuestion => "truefalse",

                // Implement other question types here
                _ => "multichoice",
            };
        }


        public string MapNumberingFormat(NumberingFormat format)
        {
            return format switch
            {
                NumberingFormat.None => "none",
                NumberingFormat.AlphabeticLower => "abc",
                NumberingFormat.AlphabeticUpper => "ABCD",
                NumberingFormat.Numeric => "123",
                NumberingFormat.RomanLower => "iii",
                NumberingFormat.RomanUpper => "IIII",

                _ => "none",
            };
        }
    }
}
