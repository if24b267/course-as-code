using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlLesson : YamlActivity
    {
        public string ModuleName { get; set; } = "lesson";
        public Context Context { get; set; } = Context.Course;
        public string Intro { get; set; } = LessonDefaults.DEFAULT_INTRO;
        public FormattingFormat IntroFormat { get; set; } = LessonDefaults.DEFAULT_INTRO_FORMAT;
        public int Grade { get; set; } = LessonDefaults.DEFAULT_GRADE;
        public bool Practice { get; set; } = LessonDefaults.DEFAULT_PRACTICE;
        public int MaxAttempts { get; set; } = LessonDefaults.DEFAULT_MAX_ATTEMPTS;
        public bool Review { get; set; } = LessonDefaults.DEFAULT_REVIEW;
        public bool Retake { get; set; } = LessonDefaults.DEFAULT_RETAKE;
        public bool ProgressBar { get; set; } = LessonDefaults.DEFAULT_PROGRESS_BAR;
        public bool DisplayLeft { get; set; } = LessonDefaults.DEFAULT_DISPLAY_LEFT;
        public bool UsePassword { get; set; } = LessonDefaults.DEFAULT_USE_PASSWORD;
        public string Password { get; set; } = LessonDefaults.DEFAULT_PASSWORD;
        public int TimeLimit { get; set; } = LessonDefaults.DEFAULT_TIME_LIMIT;
        public DateTime? Available { get; set; } = LessonDefaults.DEFAULT_AVAILABLE;
        public DateTime? Deadline { get; set; } = LessonDefaults.DEFAULT_DEADLINE;
        public YamlLessonCondition Conditions { get; set; } = new();
        public List<YamlLessonPage> Pages { get; set; } = new();
    }
}
