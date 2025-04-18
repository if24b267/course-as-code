using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum GradingMethod
{
    [Description("the highest grade of all attempts")]
    HighestGrade,
    [Description("the average (mean) grade of all attempts")]
    AverageGrade,
    [Description("the first attempt (all other attempts are ignored)")]
    FirstAttempt,
    [Description("the last attempt (all other attempts are ignored)")]
    LastAttempt
}