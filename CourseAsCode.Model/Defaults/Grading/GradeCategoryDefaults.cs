using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Model.Grading;

namespace CourseAsCode.Model.Defaults.Grading
{
    public class GradeCategoryDefaults
    {
        public const int DEFAULT_GRADE_CATEGORY_ID = 1;
        public const GradeAggregation DEFAULT_GRADE_AGGREGATION = GradeAggregation.Natural;
        public const GradeCategory? DEFAULT_PARENT_CATEGORY = null;
        public static readonly int? DEFAULT_DROP_LOWEST_GRADES = null;
        public static readonly DateTime DEFAULT_CREATED = DateTime.Now;
        public static readonly DateTime DEFAULT_MODIFIED = DateTime.Now;
        public static readonly float? DEFAULT_MINIMUM_GRADE = null;
        public const float DEFAULT_MAXIMUM_GRADE = 100f;
        public static readonly int? DEFAULT_GRADING_WEIGHT = null;

        // ────────────────────────────────────────────────────────────────────────────
        // IGradeable
        // ────────────────────────────────────────────────────────────────────────────
        public const string? DEFAULT_GRADE_NAME = null;
        public const string? DEFAULT_GRADE_INFO = null;
        public const float DEFAULT_GRADE_TO_PASS = 0.0f;
        public const GradeType DEFAULT_GRADE_TYPE = GradeType.Value;
        public static readonly int? DEFAULT_GRADE_DECIMAL_POINTS = null;
        public static readonly float? DEFAULT_GRADE_MULTIPLICATOR = null;
        public static readonly float? DEFAULT_GRADE_OFFSET = null;
        public const bool DEFAULT_HIDE_GRADE = false;
        public static readonly DateTime? DEFAULT_GRADE_HIDDEN_UNTIL = null;
        public const bool DEFAULT_LOCKED_GRADE = false;
        public static readonly DateTime? DEFAULT_GRADE_LOCKED_UNTIL = null;
    }
}
