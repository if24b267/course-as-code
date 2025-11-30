using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources
{
    /// <summary>
    /// Repräsentiert eine Forum-Aktivität im Kurs.
    /// Erbt von der abstrakten Basisklasse Activity.
    /// </summary>
    public class MoodleForum : Activity
    {
        // ────────────────────────────────────────────────────────────────────────────
        // Forum Specific Settings
        // ────────────────────────────────────────────────────────────────────────────

        [Description("The type of forum (e.g., standard, blog)")]
        public string Type { get; set; } = string.Empty; // Entspricht <type>

        [Description("Introduction text for the forum")]
        public string Intro { get; set; } = string.Empty; // Entspricht <intro>

        [Description("Format of the introduction text")]
        public FormattingFormat IntroFormat { get; set; } // Entspricht <introformat> (muss auf Enum gemappt werden)

        [Description("The due date for the forum activity")]
        public int DueDate { get; set; } // Entspricht <duedate> (wahrscheinlich ein Unix-Timestamp, wird hier als int beibehalten)

        [Description("The cutoff date for the forum activity")]
        public int CutoffDate { get; set; } // Entspricht <cutoffdate> (wahrscheinlich ein Unix-Timestamp)

        [Description("Whether the forum is assessed")]
        public bool Assessed { get; set; } // Entspricht <assessed> (0/1 zu bool)

        [Description("Assessment start time")]
        public int AssessTimeStart { get; set; } // Entspricht <assesstimestart>

        [Description("Assessment finish time")]
        public int AssessTimeFinish { get; set; } // Entspricht <assesstimefinish>

        [Description("The maximum scale/points for the forum")]
        public int Scale { get; set; } // Entspricht <scale>

        [Description("Maximum number of bytes for attachments")]
        public int MaxBytes { get; set; } // Entspricht <maxbytes>

        [Description("Maximum number of attachments")]
        public int MaxAttachments { get; set; } // Entspricht <maxattachments>

        [Description("Whether all participants are forced to subscribe")]
        public bool ForceSubscribe { get; set; } // Entspricht <forcesubscribe>

        [Description("Tracking type for reading posts")]
        public int TrackingType { get; set; } // Entspricht <trackingtype>

        [Description("RSS feed type")]
        public int RssType { get; set; } // Entspricht <rsstype>

        [Description("Number of RSS articles")]
        public int RssArticles { get; set; } // Entspricht <rssarticles>

        [Description("Time of last modification")]
        public DateTime TimeModified { get; set; } // Entspricht <timemodified> (Unix-Timestamp)

        [Description("Warn users after this period of inactivity")]
        public int WarnAfter { get; set; } // Entspricht <warnafter>

        [Description("Block users after this period of inactivity")]
        public int BlockAfter { get; set; } // Entspricht <blockafter>

        [Description("Blocking period in seconds")]
        public int BlockPeriod { get; set; } // Entspricht <blockperiod>

        [Description("Required number of discussions for completion")]
        public int CompletionDiscussions { get; set; } // Entspricht <completiondiscussions>

        [Description("Required number of replies for completion")]
        public int CompletionReplies { get; set; } // Entspricht <completionreplies>

        [Description("Required number of posts for completion")]
        public int CompletionPosts { get; set; } // Entspricht <completionposts>

        [Description("Whether to display word count")]
        public bool DisplayWordCount { get; set; } // Entspricht <displaywordcount>

        [Description("Lock discussion after this period of inactivity")]
        public int LockDiscussionAfter { get; set; } // Entspricht <lockdiscussionafter>

        [Description("Whether the forum is graded")]
        public bool GradeForum { get; set; } // Entspricht <grade_forum>

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
