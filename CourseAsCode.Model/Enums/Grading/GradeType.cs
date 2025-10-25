using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Grading
{
    public enum GradeType
    {
        [Description("No grading possible")]
        None = 0,
        [Description("A numerical value with a maximum and minimum")]
        Value = 1,
        [Description("An item in a list")]
        Scale = 2,
        [Description("Feedback only")]
        Text = 3,
    }
}
