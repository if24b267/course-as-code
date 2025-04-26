using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum TimeExpiryHandle
{
    [Description("open attempts are submitted automatically")]
    AutoSubmit,
    [Description("there is a grace period when open attempts can be submitted, but no more questions answered")]
    GracePeriod,
    [Description("attempts must be submitted before time expires, or they are not counted")]
    DoNotCount
}