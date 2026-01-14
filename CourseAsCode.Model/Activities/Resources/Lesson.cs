using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources
{
    public class Lesson : Activity
    {
        // ────────────────────────────────────────────────────────────────────────────
        // General
        // ────────────────────────────────────────────────────────────────────────────
        [Description("Course id this lesson belongs to")]
        public int CourseId { get; set; }

        [Description("Lesson intro text")]
        public string Intro { get; set; } = LessonDefaults.DEFAULT_INTRO;

        [Description("Format of the intro text")]
        public FormattingFormat IntroFormat { get; set; } = LessonDefaults.DEFAULT_INTRO_FORMAT;

        [Description("Maximum grade for the lesson")]
        public int Grade { get; set; } = LessonDefaults.DEFAULT_GRADE;

        [Description("Use custom scoring")]
        public bool Custom { get; set; } = LessonDefaults.DEFAULT_CUSTOM;

        // ────────────────────────────────────────────────────────────────────────────
        // Access and conditions
        // ────────────────────────────────────────────────────────────────────────────
        [Description("Whether password is required")]
        public bool UsePassword { get; set; } = LessonDefaults.DEFAULT_USE_PASSWORD;

        [Description("Lesson password if required")]
        public string Password { get; set; } = LessonDefaults.DEFAULT_PASSWORD;

        [Description("Dependency lesson id (0 for none)")]
        public int Dependency { get; set; } = LessonDefaults.DEFAULT_DEPENDENCY;

        [Description("Lesson access/condition rules")]
        public LessonConditions Conditions { get; set; } = new();

        // ────────────────────────────────────────────────────────────────────────────
        // Attempts and behavior
        // ────────────────────────────────────────────────────────────────────────────
        [Description("Practice lesson (no grades in gradebook)")]
        public bool Practice { get; set; } = LessonDefaults.DEFAULT_PRACTICE;

        [Description("Allow multiple attempts per question")]
        public bool ModAttempts { get; set; } = LessonDefaults.DEFAULT_MOD_ATTEMPTS;

        [Description("Maximum answers per page")]
        public int MaxAnswers { get; set; } = LessonDefaults.DEFAULT_MAX_ANSWERS;

        [Description("Maximum attempts per question")]
        public int MaxAttempts { get; set; } = LessonDefaults.DEFAULT_MAX_ATTEMPTS;

        [Description("Allow review")]
        public bool Review { get; set; } = LessonDefaults.DEFAULT_REVIEW;

        [Description("Default next page behavior")]
        public int NextPageDefault { get; set; } = LessonDefaults.DEFAULT_NEXT_PAGE_DEFAULT;

        [Description("Feedback mode")]
        public int Feedback { get; set; } = LessonDefaults.DEFAULT_FEEDBACK;

        [Description("Minimum number of questions")]
        public int MinQuestions { get; set; } = LessonDefaults.DEFAULT_MIN_QUESTIONS;

        [Description("Maximum number of pages")]
        public int MaxPages { get; set; } = LessonDefaults.DEFAULT_MAX_PAGES;

        [Description("Time limit in seconds (0 for none)")]
        public int TimeLimit { get; set; } = LessonDefaults.DEFAULT_TIME_LIMIT;

        [Description("Allow retake")]
        public bool Retake { get; set; } = LessonDefaults.DEFAULT_RETAKE;

        [Description("Activity link id (0 for none)")]
        public int ActivityLink { get; set; } = LessonDefaults.DEFAULT_ACTIVITY_LINK;

        // ────────────────────────────────────────────────────────────────────────────
        // Media
        // ────────────────────────────────────────────────────────────────────────────
        [Description("Media file path or URL")]
        public string MediaFile { get; set; } = LessonDefaults.DEFAULT_MEDIA_FILE;

        public int MediaHeight { get; set; } = LessonDefaults.DEFAULT_MEDIA_HEIGHT;
        public int MediaWidth { get; set; } = LessonDefaults.DEFAULT_MEDIA_WIDTH;
        public bool MediaClose { get; set; } = LessonDefaults.DEFAULT_MEDIA_CLOSE;

        // ────────────────────────────────────────────────────────────────────────────
        // Slideshow
        // ────────────────────────────────────────────────────────────────────────────
        public bool Slideshow { get; set; } = LessonDefaults.DEFAULT_SLIDESHOW;
        public int Width { get; set; } = LessonDefaults.DEFAULT_WIDTH;
        public int Height { get; set; } = LessonDefaults.DEFAULT_HEIGHT;
        public string BgColor { get; set; } = LessonDefaults.DEFAULT_BG_COLOR;

        // ────────────────────────────────────────────────────────────────────────────
        // Navigation/Display
        // ────────────────────────────────────────────────────────────────────────────
        public bool DisplayLeft { get; set; } = LessonDefaults.DEFAULT_DISPLAY_LEFT;
        public bool DisplayLeftIf { get; set; } = LessonDefaults.DEFAULT_DISPLAY_LEFT_IF;
        public bool ProgressBar { get; set; } = LessonDefaults.DEFAULT_PROGRESS_BAR;

        // ────────────────────────────────────────────────────────────────────────────
        // Availability
        // ────────────────────────────────────────────────────────────────────────────
        public DateTime? Available { get; set; } = LessonDefaults.DEFAULT_AVAILABLE;
        public DateTime? Deadline { get; set; } = LessonDefaults.DEFAULT_DEADLINE;

        // ────────────────────────────────────────────────────────────────────────────
        // Completion
        // ────────────────────────────────────────────────────────────────────────────
        public bool CompletionEndReached { get; set; } = LessonDefaults.DEFAULT_COMPLETION_END_REACHED;
        public int CompletionTimeSpent { get; set; } = LessonDefaults.DEFAULT_COMPLETION_TIME_SPENT;

        // ────────────────────────────────────────────────────────────────────────────
        // Mobile
        // ────────────────────────────────────────────────────────────────────────────
        public bool AllowOfflineAttempts { get; set; } = LessonDefaults.DEFAULT_ALLOW_OFFLINE_ATTEMPTS;

        public List<LessonPage> Pages { get; set; } = new();
        public List<object> Grades { get; set; } = new();
        public List<object> Timers { get; set; } = new();
        public List<object> Overrides { get; set; } = new();

        public class LessonPage
        {
            [Description("unique page id within the lesson")]
            public int PageId { get; set; }

            [Description("page title")]
            public string Title { get; set; } = string.Empty;

            [Description("page contents (HTML)")]
            public string Content { get; set; } = string.Empty;

            [Description("format of page content")]
            public FormattingFormat ContentFormat { get; set; } = FormattingFormat.HTML;

            [Description("page time modified")]
            public DateTime TimeModified { get; set; } = DateTime.UtcNow;

            // ────────────────────────────────────────────────────────────────────────
            // Moodle lesson backup fields 
            // ────────────────────────────────────────────────────────────────────────
            public int QType { get; set; } = LessonDefaults.DEFAULT_PAGE_QTYPE;
            public int QOption { get; set; } = LessonDefaults.DEFAULT_PAGE_QOPTION;
            public int Layout { get; set; } = LessonDefaults.DEFAULT_PAGE_LAYOUT;
            public int Display { get; set; } = LessonDefaults.DEFAULT_PAGE_DISPLAY;

            // Default single "Continue" answer
            public string AnswerText { get; set; } = LessonDefaults.DEFAULT_ANSWER_TEXT;
        }
        public sealed class LessonConditions
        {
            [Description("Minimum time spent (seconds) before access is granted")]
            public int TimeSpentSeconds { get; set; } = LessonDefaults.DEFAULT_CONDITIONS_TIMESPENT_SECONDS;

            [Description("Whether completion is required (0/1)")]
            public int Completed { get; set; } = LessonDefaults.DEFAULT_CONDITIONS_COMPLETED;

            [Description("Minimum grade required (0+). 0 disables this check.")]
            public int GradeBetterThan { get; set; } = LessonDefaults.DEFAULT_CONDITIONS_GRADEBETTERTHAN;
        }
    }
}
