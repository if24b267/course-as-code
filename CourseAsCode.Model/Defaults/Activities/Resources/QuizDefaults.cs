using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Grading;

namespace CourseAsCode.Model.Defaults.Activities.Resources
{
    public class QuizDefaults
    {
        // Timing
        public static readonly DateTime? DEFAULT_OPEN_TIME = null;
        public static readonly DateTime? DEFAULT_CLOSE_TIME = null;
        public const int DEFAULT_TIME_LIMIT = 0;
        public const TimeExpiryHandle DEFAULT_TIME_EXPIRY_HANDLING = TimeExpiryHandle.AutoSubmit;
        public const int DEFAULT_GRACE_PERIOD = 0;

        // ────────────────────────────────────────────────────────────────────────────
        // IGradeable
        // ────────────────────────────────────────────────────────────────────────────
        public const string? DEFAULT_GRADE_NAME = null;
        public const string? DEFAULT_GRADE_INFO = null;
        public static readonly GradeCategory DEFAULT_GRADE_CATEGORY = new GradeCategory()
        {
            GradeCategoryId = 1,
            GradeName = "Course Overall",
            AggregationType = GradeAggregation.Natural,
            Created = DateTime.Now,
            Modified = DateTime.Now,
        };
        public const float DEFAULT_GRADE_TO_PASS = 0.0f;
        public static readonly int? DEFAULT_GRADE_WEIGHT = null;
        public const GradeType DEFAULT_GRADE_TYPE = GradeType.Value;
        public static readonly int? DEFAULT_GRADE_DECIMAL_POINTS = null;
        public static readonly float? DEFAULT_GRADE_MULTIPLICATOR = null;
        public static readonly float? DEFAULT_GRADE_OFFSET = null;
        public const bool DEFAULT_HIDE_GRADE = false;
        public static readonly DateTime? DEFAULT_GRADE_HIDDEN_UNTIL = null;
        public const bool DEFAULT_LOCKED_GRADE = false;
        public static readonly DateTime? DEFAULT_GRADE_LOCKED_UNTIL = null;

        // Grading / attempts
        public const int DEFAULT_ALLOWED_ATTEMPTS = 0;
        public const GradingMethod DEFAULT_GRADING_METHOD = GradingMethod.HighestGrade;

        public static readonly float? DEFAULT_MIN_GRADE = null;
        public const float DEFAULT_MAX_GRADE = 10.0f;
        public const int DEFAULT_NEW_PAGE_THRESHOLD = 0;
        public const bool DEFAULT_SHUFFLE_QUESTIONS = false;
        public const bool DEFAULT_SHUFFLE_WITHIN_QUESTIONS = false;
        public const QuestionBehaviour DEFAULT_QUESTION_BEHAVIOUR = QuestionBehaviour.Deferred;
        public const bool DEFAULT_ALLOW_REDOS_DURING_ATTEMPT = false;
        public const bool DEFAULT_BUILD_ON_PREVIOUS_ATTEMPTS = false;

        // Review options
        public const bool DEFAULT_ALLOW_REVIEW_ATTEMPT = true;
        public const bool DEFAULT_ALLOW_REVIEW_WHETHER_CORRECT = true;
        public const bool DEFAULT_ALLOW_REVIEW_MAX_MARKS = true;
        public const bool DEFAULT_ALLOW_REVIEW_MARKS = true;
        public const bool DEFAULT_ALLOW_REVIEW_SPECIFIC_FEEDBACK = true;
        public const bool DEFAULT_ALLOW_REVIEW_GENERAL_FEEDBACK = true;
        public const bool DEFAULT_ALLOW_REVIEW_RIGHT_ANSWER = true;
        public const bool DEFAULT_ALLOW_REVIEW_OVERALL_FEEDBACK = true;

        // Presentation
        public const PictureOptions DEFAULT_SHOW_PICTURE = PictureOptions.NoImage;
        public const int DEFAULT_DECIMALS_DISPLAY_GRADES = 2;
        public const int DEFAULT_DECIMALS_DISPLAY_MARKS = -1;
    }
}
