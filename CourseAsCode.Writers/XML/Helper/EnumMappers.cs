using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;

namespace CourseAsCode.Writers.XML.Helper
{
    public class EnumMappers
    {
        public static string MapTimeExpiryHandle(TimeExpiryHandle handle)
        {
            return handle switch
            {
                TimeExpiryHandle.AutoSubmit => "autosubmit",
                TimeExpiryHandle.GracePeriod => "graceperiod",
                TimeExpiryHandle.DoNotCount => "autoabandon",
                _ => "autosubmit",
            };
        }

        public static string MapQuestionBehaviour(QuestionBehaviour behaviour)
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

        public static string MapQuestionType(Question question)
        {
            return question switch
            {
                SingleChoiceQuestion or MultipleChoiceQuestion => "multichoice",
                DropdownQuestion => "match",
                DragAndDropTextQuestion => "ddwtos",
                CodeRunnerQuestion => "coderunner",

                // Implement other question types here
                _ => "multichoice",
            };
        }


        public static string MapNumberingFormat(NumberingFormat format)
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