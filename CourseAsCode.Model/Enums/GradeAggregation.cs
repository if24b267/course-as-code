using System.ComponentModel;

namespace CourseAsCode.Model.Enums
{
    /// <summary>
    /// The aggregation determines how grades in a category are combined
    /// </summary>
    public enum GradeAggregation
    {
        [Description("The sum of all grades divided by the total number of grades")]
        MeanOfGrades,
        [Description("The middle grade when grades are arranged in order of size")]
        MedianOfGrades,
        [Description("The lowest grade of any activity in the category")]
        LowestGrade,
        [Description("The highest grade of any activity in the category")]
        HighestGrade,
        [Description("The grade that occurs most frequently")]
        ModeOfGrades,
        [Description("The sum of all grade values scaled by weight")]
        Natural = 13,
    }
}
