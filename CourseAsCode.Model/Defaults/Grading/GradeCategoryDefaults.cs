using CourseAsCode.Model.Enums.Grading;

namespace CourseAsCode.Model.Defaults.Grading
{
    public class GradeCategoryDefaults
    {
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
