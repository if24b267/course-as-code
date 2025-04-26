using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Quizzes;

public enum GradingMethod
{
    [Description("the highest grade of all attempts")]
    HighestGrade = 1,
    [Description("the average (mean) grade of all attempts")]
    AverageGrade = 2,
    [Description("the first attempt (all other attempts are ignored)")]
    FirstAttempt = 3,
    [Description("the last attempt (all other attempts are ignored)")]
    LastAttempt = 4
}