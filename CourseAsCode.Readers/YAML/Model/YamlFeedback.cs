using CourseAsCode.Model.Defaults.Activities.Resources;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlFeedback : YamlActivity
    {
        public int Anonymous { get; set; } = FeedbackDefaults.DEFAULT_ANONYMOUS;
        public int EmailNotification { get; set; } = FeedbackDefaults.DEFAULT_EMAIL_NOTIFICATION;
        public int MultipleSubmit { get; set; } = FeedbackDefaults.DEFAULT_MULTIPLE_SUBMIT;
        public int Autonumbering { get; set; } = FeedbackDefaults.DEFAULT_AUTONUMBERING;
        public int PublishStats { get; set; } = FeedbackDefaults.DEFAULT_PUBLISH_STATS;
        public bool CompletionSubmit { get; set; } = FeedbackDefaults.DEFAULT_COMPLETION_SUBMIT;
        public List<YamlFeedbackItem> Items { get; set; } = [];
    }
}
