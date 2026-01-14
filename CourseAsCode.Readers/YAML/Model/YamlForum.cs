namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlForum : YamlActivity
    {
        public string? ForumType { get; set; }
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
        public bool? ForceSubscribe { get; set; }
        public int? TrackingType { get; set; }
        public int? RssType { get; set; }
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
