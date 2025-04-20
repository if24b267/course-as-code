using CourseAsCode.Model.Enums.Quizzes;

namespace CourseAsCode.Model.Defaults.Activities.Resources;

public class QuizDefaults
{
    // Timing
    public static readonly DateTime? DEFAULT_OPEN_TIME = null;
    public static readonly DateTime? DEFAULT_CLOSE_TIME = null;
    public const int DEFAULT_TIME_LIMIT = 0;
    public const TimeExpiryHandle DEFAULT_TIME_EXPIRY_HANDLING = TimeExpiryHandle.AutoSubmit;
    public const int DEFAULT_GRACE_PERIOD = 0;

    // Grading / attempts
    public const float DEFAULT_GRADE_TO_PASS = 0.0f;
    public const int DEFAULT_ALLOWED_ATTEMPTS = 0;
    public const GradingMethod DEFAULT_GRADING_METHOD = GradingMethod.HighestGrade;
    public const float DEFAULT_MAX_GRADE = 10.0f;
    public const int DEFAULT_NEW_PAGE_THRESHOLD = 0;
    public const bool DEFAULT_SHUFFLE_WITHIN_QUESTIONS = false;
    public const QuestionBehaviour DEFAULT_QUESTION_BEHAVIOUR = QuestionBehaviour.Deferred;
    public const bool DEFAULT_ALLOW_REDOS_DURING_ATTEMPT = false;
    public const bool DEFAULT_BUILD_ON_PREVIOUS_ATTEMPTS = false;
    public const int DEFAULT_REVIEW_OPTIONS = 0;

    // Presentation
    public const PictureOptions DEFAULT_SHOW_PICTURE = PictureOptions.NoImage;
    public const int DEFAULT_DECIMALS_DISPLAY_GRADES = 2;
    public const int DEFAULT_DECIMALS_DISPLAY_MARKS = -1;
}