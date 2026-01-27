using CourseAsCode.Model.Enums.Forums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlForum : YamlActivity
    {
        public ForumType? ForumType { get; set; }
        public string? Intro { get; set; }
        public int? IntroFormat { get; set; }
        public int? DueDate { get; set; }
        public int? CutoffDate { get; set; }
        public bool? Assessed { get; set; }
        public int? AssessTimeStart { get; set; }
        public int? AssessTimeFinish { get; set; }
        public int? Scale { get; set; }
        public int? MaxBytes { get; set; }
        public int? MaxAttachments { get; set; }
        public SubscriptionMode? SubscriptionMode { get; set; }
        public ForumTrackingType? TrackingType { get; set; }
        public ForumRssType? RssType { get; set; }
        public int? RssArticles { get; set; }
        public int? WarnAfter { get; set; }
        public int? BlockAfter { get; set; }
        public int? BlockPeriod { get; set; }
        public int? CompletionDiscussions { get; set; }
        public int? CompletionReplies { get; set; }
        public int? CompletionPosts { get; set; }
        public bool? DisplayWordCount { get; set; }
        public int? LockDiscussionAfter { get; set; }
        public bool? GradeForum { get; set; }
    }
}
