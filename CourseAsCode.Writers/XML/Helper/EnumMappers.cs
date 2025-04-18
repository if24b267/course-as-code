using CourseAsCode.Model.Enums.Quizzes;

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
}