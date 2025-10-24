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
    }
}
