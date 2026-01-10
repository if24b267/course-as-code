using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlLesson : YamlActivity
    {
        public string Intro { get; set; } = LessonDefaults.DEFAULT_INTRO;
        public FormattingFormat IntroFormat { get; set; } = LessonDefaults.DEFAULT_INTRO_FORMAT;
        public int Grade { get; set; } = LessonDefaults.DEFAULT_GRADE;
        public bool Custom { get; set; } = LessonDefaults.DEFAULT_CUSTOM;
        public bool UsePassword { get; set; } = LessonDefaults.DEFAULT_USE_PASSWORD;
        public string Password { get; set; } = LessonDefaults.DEFAULT_PASSWORD;
        // TODO: instead of id here, maybe build in reader so you can link another lesson activity file and get the id based on that?
        public int Dependency { get; set; } = LessonDefaults.DEFAULT_DEPENDENCY;
        // TODO: this should be a subclass (in the model too first!) so it is human readable in YAML
        public string Conditions { get; set; } = LessonDefaults.DEFAULT_CONDITIONS;
        public bool Practice { get; set; } = LessonDefaults.DEFAULT_PRACTICE;
        public bool ModAttempts { get; set; } = LessonDefaults.DEFAULT_MOD_ATTEMPTS;
        public int MaxAnswers { get; set; } = LessonDefaults.DEFAULT_MAX_ANSWERS;
        public int MaxAttempts  { get; set; } = LessonDefaults.DEFAULT_MAX_ATTEMPTS;
        public bool Review { get; set; } = LessonDefaults.DEFAULT_REVIEW;


    }
}
