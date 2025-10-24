using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Grading
{
    public class GradeCategory
    {
        [Description("Unique identifier for the grade category, used to reference the category in files")]
        public int GradeCategoryId { get; set; }

        [Description("The name of the grade category")]
        public string Name { get; set; }

        [Description("how grades within the category are aggregated")]
        public GradeAggregation AggregationType { get; set; }

        [Description("The parent grade category, if any")]
        public GradeCategory? Parent { get; set; }

        [Description("enables a specified number of the lowest grades to be excluded from the aggregation")]
        public int? DropLowestGrades { get; set; }

        [Description("when the grade category was created")]
        public DateTime Created { get; set; }

        [Description("when the grade category was last modified")]
        public DateTime Modified { get; set; }

        [Description("the maximum amount of points that can be earned in this grade category")]
        public int MaxGrade { get; set; }

        [Description("the minimum amount of points that has to be earned in this grade category to pass, null for no passing grade in this category")]
        public int? PassingGrade { get; set; }

        [Description("the weight of this grade category in the grade calculation of its parent grade category in % of 100, null for default weight")]
        public int? GradingWeight { get; set; }
    }
}
