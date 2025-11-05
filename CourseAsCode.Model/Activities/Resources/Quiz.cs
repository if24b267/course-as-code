using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Grading;
using CourseAsCode.Model.Grading.Interfaces;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Activities.Resources
{
    public class Quiz : Activity, IGradeable
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
        // IGradeable
        // ────────────────────────────────────────────────────────────────────────────
        [Description("the name with which the quiz will be displayed in the gradebook")]
        public string? GradeName { get; set; } = QuizDefaults.DEFAULT_GRADE_NAME;

        [Description("general information about the quiz grade item; optional and will NOT be displayed in the gradebook")]
        public string? GradeInfo { get; set; } = QuizDefaults.DEFAULT_GRADE_INFO;

        [Description("The GradeCategory this quiz belongs to")]
        public GradeCategory GradeCategory { get; set; } = QuizDefaults.DEFAULT_GRADE_CATEGORY;

        [Description("Minimum required grade (0‑10) to pass the quiz")]
        public float? GradeToPass { get; set; } = QuizDefaults.DEFAULT_GRADE_TO_PASS;

        [Description("how the quiz is graded")]
        public GradeType GradeType { get; set; } = QuizDefaults.DEFAULT_GRADE_TYPE;

        [Description("The weight of this quiz in the grade calculation of its GradeCategory in % of 100, null for default weight")]
        public int? GradingWeight { get; set; } = QuizDefaults.DEFAULT_GRADE_WEIGHT;

        [Description("specifies the number of decimal points to display for the grade | has no effect on grade calculations, which are made with an accuracy of 5 decimal places")]
        public int? GradeDecimalPoints { get; set; } = QuizDefaults.DEFAULT_GRADE_DECIMAL_POINTS;

        [Description("Factor by which the grade for this activity will be multiplied, with a maximum value of the maximum grade | For example, if the multiplicator is 2 and the maximum grade is 100, then all grades less than 50 are doubled, and all grades 50 and above are changed to 100")]
        public float? GradeMultiplicator { get; set; } = QuizDefaults.DEFAULT_GRADE_MULTIPLICATOR;

        [Description("Number that will be added to every grade for this activity, after the Multiplicator is applied. (can be negative)")]
        public float? GradeOffset { get; set; } = QuizDefaults.DEFAULT_GRADE_OFFSET;

        [Description("whether this activity should be hidden from students in the gradebook")]
        public bool GradeHidden { get; set; } = QuizDefaults.DEFAULT_HIDE_GRADE;

        [Description("An optional date after which the activity will no longer be hidden in the gradebook (i.e. a grades release date)")]
        public DateTime? GradeHiddenUntil { get; set; } = QuizDefaults.DEFAULT_GRADE_HIDDEN_UNTIL;

        [Description("Whether or not to lock the grading for this activity. A locked grade item doesn't accept changes to its settings or grades from its related module")]
        public bool GradingLocked { get; set; } = QuizDefaults.DEFAULT_LOCKED_GRADE;

        [Description("A date after which the locking will be effective, usually after the grades release date.")]
        public DateTime? GradingLockedAfter { get; set; } = QuizDefaults.DEFAULT_GRADE_LOCKED_UNTIL;

        // ────────────────────────────────────────────────────────────────────────────
        // Attempts
        // ────────────────────────────────────────────────────────────────────────────
        [Description("Number of allowed attempts – 0 means unlimited")]
        public int AllowedAttempts { get; set; } = QuizDefaults.DEFAULT_ALLOWED_ATTEMPTS;

        [Description("Method used when several attempts exist")]
        public GradingMethod GradingMethod { get; set; } = QuizDefaults.DEFAULT_GRADING_METHOD;

        [Description("How many points this quiz is worth")]
        public float MaxGrade { get; set; } = QuizDefaults.DEFAULT_MAX_GRADE;

        [Description("Automatically insert page breaks after this many questions – 0 = all questions on one single page")]
        public int NewPageThreshold { get; set; } = QuizDefaults.DEFAULT_NEW_PAGE_THRESHOLD;

        [Description("Shuffle the order of questions in the quiz")]
        public bool ShuffleQuestions { get; set; } = QuizDefaults.DEFAULT_SHUFFLE_QUESTIONS;

        [Description("Shuffle the parts making up questions (must also be enabled per‑question)")]
        public bool ShuffleWithinQuestions { get; set; } = QuizDefaults.DEFAULT_SHUFFLE_WITHIN_QUESTIONS;

        [Description("Question behaviour defining how students interact with questions")]
        public QuestionBehaviour QuestionBehaviour { get; set; } = QuizDefaults.DEFAULT_QUESTION_BEHAVIOUR;

        [Description("Allow students to redo questions within an attempt (behaviour‑specific)")]
        public bool AllowRedosDuringAttempt { get; set; } = QuizDefaults.DEFAULT_ALLOW_REDOS_DURING_ATTEMPT;

        [Description("Each new attempt starts with the previous attempt’s answers pre‑filled")]
        public bool BuildOnPreviousAttempts { get; set; } = QuizDefaults.DEFAULT_BUILD_ON_PREVIOUS_ATTEMPTS;

        [Description("Review options available to the student during the attempt; only applicable to some question behaviours like InteractiveMultipleTries")]
        public ReviewOptionsQuiz ReviewOptionsDuringAttempt { get; set; } = new ReviewOptionsQuiz();

        [Description("Review options available to the student immediately after they clicked on 'Submit all and finish'")]
        public ReviewOptionsQuiz ReviewOptionsImmediatelyAfterAttempt { get; set; } = new ReviewOptionsQuiz();

        [Description("Review options available to the student two minutes after they clicked on 'Submit all and finish'")]
        public ReviewOptionsQuiz ReviewOptionsAfterAttempt{ get; set; } = new ReviewOptionsQuiz();

        [Description("Review options available to the student after the quiz was closed | If the quiz doesn't have a close date, this state is never reached")]
        public ReviewOptionsQuiz ReviewOptionsQuizClosed { get; set; } = new ReviewOptionsQuiz();

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
        }


        /// <summary>
        /// saves which review-options are enabled
        /// </summary>
        public class ReviewOptionsQuiz
        {
            [Description("whether the student can review the attempt at all")]
            public bool AllowReviewAttempt { get; set; } = QuizDefaults.DEFAULT_ALLOW_REVIEW_ATTEMPT;
            [Description("whether the student can see if he answered the question \"correct\", \"partially correct\" or \"incorrect\"")]
            public bool AllowReviewWhetherCorrect { get; set; } = QuizDefaults.DEFAULT_ALLOW_REVIEW_WHETHER_CORRECT;
            [Description("whether the student can see the maximum mark he could have achieved for each question")]
            public bool AllowReviewMaximumMarks { get; set; } = QuizDefaults.DEFAULT_ALLOW_REVIEW_MAX_MARKS;
            [Description("whether the student can see the mark obtained for each question and the overall attempt score - only available if MaximumMarks is true")]
            public bool AllowReviewMarks { get; set; } = QuizDefaults.DEFAULT_ALLOW_REVIEW_MARKS;
            [Description("whether the student can see feedback that depends on what response the student gave")]
            public bool AllowReviewSpecificFeedback { get; set; } = QuizDefaults.DEFAULT_ALLOW_REVIEW_SPECIFIC_FEEDBACK;
            [Description("whether the student can see general feedback, displayed to all students regardless of their answer. E.g. a fully worked answer and a link to more information")]
            public bool AllowReviewGeneralFeedback { get; set; } = QuizDefaults.DEFAULT_ALLOW_REVIEW_GENERAL_FEEDBACK;
            [Description(" whether the student can see what would have been the correct answer(s)")]
            public bool AllowReviewRightAnswer { get; set; } = QuizDefaults.DEFAULT_ALLOW_REVIEW_RIGHT_ANSWER;
            [Description("whether the student can see the feedback given at the end of the attempt, depending on the student's total mark")]
            public bool AllowReviewOverallFeedback { get; set; } = QuizDefaults.DEFAULT_ALLOW_REVIEW_OVERALL_FEEDBACK;
        }
    }
}
