using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types;

namespace CourseAsCode.Writers.XML.Helper;

public class EnumMappers
{
    public static string MapTimeExpiryHandle(TimeExpiryHandle h) => h switch
    {
        TimeExpiryHandle.AutoSubmit => "autosubmit",
        TimeExpiryHandle.GracePeriod => "graceperiod",
        TimeExpiryHandle.DoNotCount => "autoabandon",
        _ => "autosubmit"
    };

    
    public static string MapQuestionBehaviour(QuestionBehaviour b) => b switch
    {
        QuestionBehaviour.Adaptive => "adaptive",
        QuestionBehaviour.AdaptiveNoPenalties => "adaptivenopenalty",
        QuestionBehaviour.Deferred => "deferredfeedback",
        QuestionBehaviour.DeferredCBM => "deferredcbm",
        QuestionBehaviour.Immediate => "immediatefeedback",
        QuestionBehaviour.ImmediateCBM => "immediatecbm",
        QuestionBehaviour.InteractiveMultipleTries => "interactive",
        _ => "deferredfeedback"
    };
    
    
    public static string MapQuestionType(Question question)
    {
        if (question is SingleChoiceQuestion or MultipleChoiceQuestion)
        {
            return "multichoice";
        }
        
        // Implement other question types here
        
        return "multichoice";
    }


    public static string MapNumberingFormat(NumberingFormat f) => f switch
    {
        NumberingFormat.None => "none",
        NumberingFormat.AlphabeticLower => "abc",
        NumberingFormat.AlphabeticUpper => "ABCD",
        NumberingFormat.Numeric => "123",
        NumberingFormat.RomanLower => "iii",
        NumberingFormat.RomanUpper => "IIII",
        _ => "none"
    };
}