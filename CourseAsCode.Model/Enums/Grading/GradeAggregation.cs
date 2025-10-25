using System.ComponentModel;

namespace CourseAsCode.Model.Enums.Grading
{
    /// <summary>
    /// The aggregation determines how grades in a category are combined. See <see href="https://docs.moodle.org/500/en/Grade_aggregation"/> for more information.
    /// </summary>
    public enum GradeAggregation
    {
        [Description("The sum of all grades divided by the total number of grades")]
        MeanOfGrades = 0,
        [Description("The middle grade when grades are arranged in order of size")]
        MedianOfGrades = 2,
        [Description("The lowest grade of any activity in the category")]
        LowestGrade = 4,
        [Description("The highest grade of any activity in the category")]
        HighestGrade = 6,
        [Description("The grade that occurs most frequently")]
        ModeOfGrades = 8,
        [Description("The sum of all grade values scaled by weight")]
        Natural = 13,
        [Description("Same as `MeanOfGrades`, but each grade item can be given a weight to change its importance in the overall mean")]
        WeightedMeanOfGrades = 10,
        [Description("Same as `MeanOfGrades`, but allows some GradeItems to be weighted more heavily | Deprecated, only used for backwards compatibility")]
        MeanOfGradesExtraCredit = 12,
        [Description("Same as `WeightedMeanOfGrades`, but the weight of each item is its Maximum grade")]
        SimpleWeightedMeanOfGrades = 11,
    }
}
