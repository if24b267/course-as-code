using CourseAsCode.Model.Defaults.Activities.Resources;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlLessonCondition
    {
        public int TimeSpentSeconds { get; set; } = LessonDefaults.DEFAULT_CONDITIONS_TIMESPENT_SECONDS;
        public int Completed { get; set; } = LessonDefaults.DEFAULT_CONDITIONS_COMPLETED;
        public int GradeBetterThan { get; set; } = LessonDefaults.DEFAULT_CONDITIONS_GRADEBETTERTHAN;
    }
}
