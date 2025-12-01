using System.ComponentModel.DataAnnotations;
using CourseAsCode.Model.Defaults.Grading;
using CourseAsCode.Model.Enums.Grading;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlGradeCategory
    {
        [Required]
        public int GradeCategoryId { get; set; }
        public GradeAggregation AggregationType { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_AGGREGATION;
        public int? ParentGradeCategoryId { get; set; }
        public int? DropLowestGrades { get; set; } = GradeCategoryDefaults.DEFAULT_DROP_LOWEST_GRADES;
        public string? GradeName { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_NAME;
        public float MaxGrade { get; set; } = GradeCategoryDefaults.DEFAULT_MAXIMUM_GRADE;
        public float GradeToPass { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_TO_PASS;
        public int? GradingWeight { get; set; } = GradeCategoryDefaults.DEFAULT_GRADING_WEIGHT;
        public bool GradeHidden { get; set; } = GradeCategoryDefaults.DEFAULT_HIDE_GRADE;
        public bool GradingLocked { get; set; } = GradeCategoryDefaults.DEFAULT_LOCKED_GRADE;
    }
}
