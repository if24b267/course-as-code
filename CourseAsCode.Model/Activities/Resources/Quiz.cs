using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Activities.Resources;

public class Quiz : Activity
{
    // ────────────────────────────────────────────────────────────────────────────
    // Timing
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Students can only start the quiz after this timestamp (null = no restriction)")]
    public DateTime? OpenTime { get; set; } = QuizDefaults.DEFAULT_OPEN_TIME;

    [Description("Students must finish the quiz before this timestamp (null = no restriction)")]
    public DateTime? CloseTime { get; set; } = QuizDefaults.DEFAULT_CLOSE_TIME;

    [Description("Overall time limit in seconds (0 = no time‑limit)")]
    public int TimeLimit { get; set; } = QuizDefaults.DEFAULT_TIME_LIMIT;

    [Description("Controls what happens to attempts that run out of time")]
    public TimeExpiryHandle TimeExpiryHandling { get; set; } = QuizDefaults.DEFAULT_TIME_EXPIRY_HANDLING;

    [Description("Grace period in seconds when TimeExpiryHandling = GracePeriod")]
    public int GracePeriod { get; set; } = QuizDefaults.DEFAULT_GRACE_PERIOD;

    // ────────────────────────────────────────────────────────────────────────────
    // Grading / attempts
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Minimum required grade (0‑10) to pass the quiz")]
    public float GradeToPass { get; set; } = QuizDefaults.DEFAULT_GRADE_TO_PASS;

    [Description("Number of allowed attempts – 0 means unlimited")]
    public int AllowedAttempts { get; set; } = QuizDefaults.DEFAULT_ALLOWED_ATTEMPTS;

    [Description("Method used when several attempts exist")]
    public GradingMethod GradingMethod { get; set; } = QuizDefaults.DEFAULT_GRADING_METHOD;

    [Description("How many points this quiz is worth")]
    public float MaxGrade { get; set; } = QuizDefaults.DEFAULT_MAX_GRADE;

    [Description("Automatically insert page breaks after this many questions – 0 = all questions on one single page")]
    public int NewPageThreshold { get; set; } = QuizDefaults.DEFAULT_NEW_PAGE_THRESHOLD;

    [Description("Shuffle the parts making up questions (must also be enabled per‑question)")]
    public bool ShuffleWithinQuestions { get; set; } = QuizDefaults.DEFAULT_SHUFFLE_WITHIN_QUESTIONS;

    [Description("Question behaviour defining how students interact with questions")]
    public QuestionBehaviour QuestionBehaviour { get; set; } = QuizDefaults.DEFAULT_QUESTION_BEHAVIOUR;

    [Description("Allow students to redo questions within an attempt (behaviour‑specific)")]
    public bool AllowRedosDuringAttempt { get; set; } = QuizDefaults.DEFAULT_ALLOW_REDOS_DURING_ATTEMPT;

    [Description("Each new attempt starts with the previous attempt’s answers pre‑filled")]
    public bool BuildOnPreviousAttempts { get; set; } = QuizDefaults.DEFAULT_BUILD_ON_PREVIOUS_ATTEMPTS;
    
    [Description("Raw review options bit‑mask as used by Moodle (see QuizzesAnalysis.md)")]
    public int ReviewOptions { get; set; } = QuizDefaults.DEFAULT_REVIEW_OPTIONS;

    // ────────────────────────────────────────────────────────────────────────────
    // Presentation
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Whether to show the student’s profile picture during the attempt/review")]
    public PictureOptions ShowPicture { get; set; } = QuizDefaults.DEFAULT_SHOW_PICTURE;

    [Description("Decimal places to show for overall grades (0‑5)")]
    public int DecimalsToDisplayGrades { get; set; } = QuizDefaults.DEFAULT_DECIMALS_DISPLAY_GRADES;

    [Description("Decimal places to show for the marks of an individual question (‑1 = ‘same as grades’)")]
    public int DecimalsToDisplayMarks { get; set; } = QuizDefaults.DEFAULT_DECIMALS_DISPLAY_MARKS;

    // ────────────────────────────────────────────────────────────────────────────
    // Safe‑Exam‑Browser
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Safe‑Exam‑Browser configuration")]
    public SafeExamBrowserSettings SafeExamBrowser { get; init; } = new();

    // ────────────────────────────────────────────────────────────────────────────
    // Misc
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Password that must be entered before a quiz attempt can start (plain‑text)")]
    public string? Password { get; set; }

    [Description("Overall feedback entries shown once an attempt is complete")]
    public List<QuizFeedback>? Feedback { get; set; }

    [Description("The pool of questions that constitute this quiz")]
    public List<Question> Questions { get; init; } = new();
    
    
    public SensibleDefaultsQuiz SensibleDefaults { get; set; } = new SensibleDefaultsQuiz();
    public SystemDefaultsQuiz SystemDefaults { get; set; } = new SystemDefaultsQuiz();

	
    public class SensibleDefaultsQuiz
    {
        // TODO
    }

    
    public class SystemDefaultsQuiz
    {
        public int ModuleId { get; set; } = 227; // TODO: Check if this is the same for all quizzes or a unique value per quiz | Seems to be the same for all quizzes
        public string ModuleName { get; set; } = "quiz";
    }
}