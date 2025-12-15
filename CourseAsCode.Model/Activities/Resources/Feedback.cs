using System;
using System.Collections.Generic;
using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources;

public class Feedback : Activity
{
    private static Random _rnd = new Random();

    // General
    [Description("format of the intro/description")]
    public FormattingFormat IntroFormat { get; set; } = FeedbackDefaults.DEFAULT_INTRO_FORMAT;

    [Description("anonymous setting for feedback (0/1/2 etc.)")]
    public int Anonymous { get; set; } = FeedbackDefaults.DEFAULT_ANONYMOUS;

    [Description("email notifications enabled")]
    public int EmailNotification { get; set; } = FeedbackDefaults.DEFAULT_EMAIL_NOTIFICATION;

    [Description("allow multiple submissions")]
    public int MultipleSubmit { get; set; } = FeedbackDefaults.DEFAULT_MULTIPLE_SUBMIT;

    [Description("autonumbering setting")]
    public int Autonumbering { get; set; } = FeedbackDefaults.DEFAULT_AUTONUMBERING;

    [Description("URL to show after submit")]
    public string? SiteAfterSubmit { get; set; } = FeedbackDefaults.DEFAULT_SITE_AFTER_SUBMIT;

    [Description("page to show after submit")]
    public string? PageAfterSubmit { get; set; } = FeedbackDefaults.DEFAULT_PAGE_AFTER_SUBMIT;

    [Description("format for page after submit")]
    public FormattingFormat PageAfterSubmitFormat { get; set; } = FeedbackDefaults.DEFAULT_PAGE_AFTER_SUBMIT_FORMAT;

    [Description("publish statistics")]
    public int PublishStats { get; set; } = FeedbackDefaults.DEFAULT_PUBLISH_STATS;

    [Description("time the feedback opens")]
    public DateTime? TimeOpen { get; set; } = FeedbackDefaults.DEFAULT_TIMEOPEN;

    [Description("time the feedback closes")]
    public DateTime? TimeClose { get; set; } = FeedbackDefaults.DEFAULT_TIMECLOSE;

    [Description("time the feedback was last modified")]
    public DateTime TimeModified { get; set; } = FeedbackDefaults.DEFAULT_TIME_MODIFIED;

    [Description("whether completion requires a submission")]
    public bool CompletionSubmit { get; set; } = FeedbackDefaults.DEFAULT_COMPLETION_SUBMIT;

    // Items and completeds
    [Description("list of items/questions in the feedback")]
    public List<FeedbackItem> Items { get; set; } = new();

    [Description("completed submissions placeholder")]
    public List<object> Completeds { get; set; } = new();

    // Defaults containers
    public SensibleDefaultsFeedback SensibleDefaults { get; set; } = new SensibleDefaultsFeedback();
    public SystemDefaultsFeedback SystemDefaults { get; set; } = new SystemDefaultsFeedback();

    public class SensibleDefaultsFeedback
    {
        // placeholder for future sensible defaults
    }

    public class SystemDefaultsFeedback
    {
        public int ModuleId { get; set; } = _rnd.Next(1, 1000);
        public string ModuleName { get; set; } = "feedback";
    }

    public class FeedbackItem
    {
        public int Id { get; set; }
        public int Template { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string? Label { get; set; } = string.Empty;
        public string? Presentation { get; set; } = string.Empty;
        public string Typ { get; set; } = string.Empty;
        public int HasValue { get; set; } = 0;
        public int Position { get; set; } = 0;
        public int Required { get; set; } = 0;
        public int DependItem { get; set; } = 0;
        public string? DependValue { get; set; } = string.Empty;
        public string? Options { get; set; } = string.Empty;
    }
}
