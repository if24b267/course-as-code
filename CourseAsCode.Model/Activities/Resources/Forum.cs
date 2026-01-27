using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Forums;

namespace CourseAsCode.Model.Activities.Resources
{
    /// <summary>
    /// Repräsentiert eine Forum-Aktivität im Kurs.
    /// Erbt von der abstrakten Basisklasse Activity.
    /// </summary>
    public class Forum : Activity
    {
        // ────────────────────────────────────────────────────────────────────────────
        // Forum Specific Settings
        // ────────────────────────────────────────────────────────────────────────────

        [Description("The type of forum (General, Blog, Single, EachUser, QAndA)")]
        public ForumType ForumType { get; set; } = ForumDefaults.DEFAULT_FORUM_TYPE;

        [Description("Introduction text for the forum")]
        public string Intro { get; set; } = ForumDefaults.DEFAULT_INTRO;

        [Description("Format of the introduction text")]
        public FormattingFormat IntroFormat { get; set; } = ForumDefaults.DEFAULT_INTRO_FORMAT;

        [Description("The due date for the forum activity")]
        public int DueDate { get; set; } = ForumDefaults.DEFAULT_DUE_DATE;

        [Description("The cutoff date for the forum activity")]
        public int CutoffDate { get; set; } = ForumDefaults.DEFAULT_CUTOFF_DATE;

        [Description("Whether the forum is assessed")]
        public bool Assessed { get; set; } = ForumDefaults.DEFAULT_ASSESSED;

        [Description("Assessment start time")]
        public int AssessTimeStart { get; set; } = ForumDefaults.DEFAULT_ASSESS_TIME_START;

        [Description("Assessment finish time")]
        public int AssessTimeFinish { get; set; } = ForumDefaults.DEFAULT_ASSESS_TIME_FINISH;

        [Description("The maximum scale/points for the forum")]
        public int Scale { get; set; } = ForumDefaults.DEFAULT_SCALE;

        [Description("Maximum number of bytes for attachments")]
        public int MaxBytes { get; set; } = ForumDefaults.DEFAULT_MAX_BYTES;

        [Description("Maximum number of attachments")]
        public int MaxAttachments { get; set; } = ForumDefaults.DEFAULT_MAX_ATTACHMENTS;

        [Description("Subscription mode (Optional, Forced, Auto, Disabled)")]
        public SubscriptionMode SubscriptionMode { get; set; } = ForumDefaults.DEFAULT_SUBSCRIPTION_MODE;

        [Description("Tracking type for reading posts (Off, Optional, Forced)")]
        public ForumTrackingType TrackingType { get; set; } = ForumDefaults.DEFAULT_TRACKING_TYPE;

        [Description("RSS feed type (None, Discussions, Posts)")]
        public ForumRssType RssType { get; set; } = ForumDefaults.DEFAULT_RSS_TYPE;

        [Description("Number of RSS articles")]
        public int RssArticles { get; set; } = ForumDefaults.DEFAULT_RSS_ARTICLES;

        [Description("Time of last modification")]
        public DateTime TimeModified { get; set; } = ForumDefaults.DEFAULT_TIME_MODIFIED;

        [Description("Warn users after this period of inactivity")]
        public int WarnAfter { get; set; } = ForumDefaults.DEFAULT_WARN_AFTER;

        [Description("Block users after this period of inactivity")]
        public int BlockAfter { get; set; } = ForumDefaults.DEFAULT_BLOCK_AFTER;

        [Description("Blocking period in seconds")]
        public int BlockPeriod { get; set; } = ForumDefaults.DEFAULT_BLOCK_PERIOD;

        [Description("Required number of discussions for completion")]
        public int CompletionDiscussions { get; set; } = ForumDefaults.DEFAULT_COMPLETION_DISCUSSIONS;

        [Description("Required number of replies for completion")]
        public int CompletionReplies { get; set; } = ForumDefaults.DEFAULT_COMPLETION_REPLIES;

        [Description("Required number of posts for completion")]
        public int CompletionPosts { get; set; } = ForumDefaults.DEFAULT_COMPLETION_POSTS;

        [Description("Whether to display word count")]
        public bool DisplayWordCount { get; set; } = ForumDefaults.DEFAULT_DISPLAY_WORD_COUNT;

        [Description("Lock discussion after this period of inactivity")]
        public int LockDiscussionAfter { get; set; } = ForumDefaults.DEFAULT_LOCK_DISCUSSION_AFTER;

        [Description("Whether the forum is graded")]
        public bool GradeForum { get; set; } = ForumDefaults.DEFAULT_GRADE_FORUM;

        // ────────────────────────────────────────────────────────────────────────────
        // Nested Structures
        // ────────────────────────────────────────────────────────────────────────────

        [Description("List of discussions in the forum")]
        public List<ForumDiscussion> Discussions { get; set; } = new(); // Entspricht <discussions>

        // Leere Listen aus der XML zur Vollständigkeit
        public List<ForumSubscription> Subscriptions { get; set; } = new();
        public List<ForumDigest> Digests { get; set; } = new();
        public List<ForumReadPost> ReadPosts { get; set; } = new();
        public List<ForumTrackedPref> TrackedPrefs { get; set; } = new();
        public List<ForumPostTag> PostsTags { get; set; } = new();
        public List<ForumGrade> Grades { get; set; } = new();
    }

    /// <summary>
    /// Repräsentiert eine Diskussion innerhalb eines Forums.
    /// </summary>
    public class ForumDiscussion
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int FirstPost { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public bool Assessed { get; set; }
        public int TimeModified { get; set; }
        public int UserModified { get; set; }
        public int TimeStart { get; set; }
        public int TimeEnd { get; set; }
        public bool Pinned { get; set; }
        public bool TimeLocked { get; set; }
        public List<ForumPost> Posts { get; set; } = new();
        public List<DiscussionSubscription> DiscussionSubscriptions { get; set; } = new();
    }

    /// <summary>
    /// Repräsentiert einen Beitrag (Post) innerhalb einer Diskussion.
    /// </summary>
    public class ForumPost
    {
        public int Id { get; set; }
        public int Parent { get; set; }
        public int UserId { get; set; }
        public int Created { get; set; }
        public int Modified { get; set; }
        public bool Mailed { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public FormattingFormat MessageFormat { get; set; } // Entspricht <messageformat>
        public int MessageTrust { get; set; }
        public string Attachment { get; set; } = string.Empty;
        public int TotalScore { get; set; }
        public bool MailNow { get; set; }
        public int PrivateReplyTo { get; set; }
        public List<PostRating> Ratings { get; set; } = new();
    }

    /// <summary>
    /// Repräsentiert ein Abonnement für eine einzelne Diskussion.
    /// </summary>
    public class DiscussionSubscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Preference { get; set; }
    }

    // Platzhalter für weitere Nested Structures, die in der XML leer sind,
    // aber für die Vollständigkeit des Datenmodells relevant sein könnten.

    public class ForumSubscription { /* ... */ }
    public class ForumDigest { /* ... */ }
    public class ForumReadPost { /* ... */ }
    public class ForumTrackedPref { /* ... */ }
    public class ForumPostTag { /* ... */ }
    public class ForumGrade { /* ... */ }
    public class PostRating { /* ... */ }
}
