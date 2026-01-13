using CourseAsCode.Model.Defaults.Activities;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlChoice : YamlActivity
    {
        public Context Context { get; set; } = Context.Course;
        public bool Publish { get; set; } = ChoiceDefaults.DEFAULT_PUBLISH;
        public bool ShowResults { get; set; } = ChoiceDefaults.DEFAULT_SHOW_RESULTS;
        public int Display { get; set; } = ChoiceDefaults.DEFAULT_DISPLAY;
        public bool AllowUpdate { get; set; } = ChoiceDefaults.DEFAULT_ALLOW_UPDATE;
        public bool AllowMultiple { get; set; } = ChoiceDefaults.DEFAULT_ALLOW_MULTIPLE;
        public bool ShowUnanswered { get; set; } = ChoiceDefaults.DEFAULT_SHOW_UNANSWERED;
        public bool LimitAnswers { get; set; } = ChoiceDefaults.DEFAULT_LIMIT_ANSWERS;
        public DateTime? TimeOpen { get; set; } = ChoiceDefaults.DEFAULT_TIMEOPEN;
        public DateTime? TimeClose { get; set; } = ChoiceDefaults.DEFAULT_TIMECLOSE;
        public bool CompletionSubmit { get; set; } = ChoiceDefaults.DEFAULT_COMPLETION_SUBMIT;
        public bool ShowPreview { get; set; } = ChoiceDefaults.DEFAULT_SHOW_PREVIEW;
        public bool IncludeInteractive { get; set; } = ChoiceDefaults.DEFAULT_INCLUDE_INACTIVE;
        public bool ShowAvailable { get; set; } = ChoiceDefaults.DEFAULT_SHOW_AVAILABLE;
        public List<YamlChoiceOption> Options { get; set; } = [];
    }
}
