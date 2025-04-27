using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources;

public class Assignment : Activity
{
    // ────────────────────────────────────────────────────────────────────────────
    // General Settings
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Introduction text for the assignment")]
    public string Intro { get; set; } = AssignmentDefaults.DEFAULT_INTRO;

    [Description("Format of the introduction text (e.g., HTML, PlainText)")]
    public FormattingFormat FormattingFormat { get; set; } = AssignmentDefaults.DEFAULT_FORMATTING_FORMAT;

    [Description("Whether to always show the description")]
    public bool AlwaysShowDescription { get; set; } = AssignmentDefaults.DEFAULT_ALWAYS_SHOW_DESCRIPTION;

    // ────────────────────────────────────────────────────────────────────────────
    // Submission Settings
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Whether submission drafts are allowed")]
    public bool SubmissionDrafts { get; set; } = AssignmentDefaults.DEFAULT_SUBMISSION_DRAFTS;

    [Description("Whether to send notifications to teachers")]
    public bool SendNotifications { get; set; } = AssignmentDefaults.DEFAULT_SEND_NOTIFICATIONS;

    [Description("Whether to send late submission notifications")]
    public bool SendLateNotifications { get; set; } = AssignmentDefaults.DEFAULT_SEND_LATE_NOTIFICATIONS;

    [Description("Whether to send notifications to students")]
    public bool SendStudentNotifications { get; set; } = AssignmentDefaults.DEFAULT_SEND_STUDENT_NOTIFICATIONS;

    [Description("The due date for the assignment")]
    public DateTime? DueDate { get; set; } = AssignmentDefaults.DEFAULT_DUE_DATE;

    [Description("The cutoff date for the assignment")]
    public DateTime? CutoffDate { get; set; } = AssignmentDefaults.DEFAULT_CUTOFF_DATE;

    [Description("The grading due date for the assignment")]
    public DateTime? GradingDueDate { get; set; } = AssignmentDefaults.DEFAULT_GRADING_DUE_DATE;

    [Description("The date from which submissions are allowed")]
    public DateTime? AllowSubmissionsFromDate { get; set; } = AssignmentDefaults.DEFAULT_ALLOW_SUBMISSIONS_FROM_DATE;

    // ────────────────────────────────────────────────────────────────────────────
    // Grading Settings
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Maximum grade for the assignment")]
    public int Grade { get; set; } = AssignmentDefaults.DEFAULT_GRADE;

    [Description("Whether completion requires submission")]
    public bool CompletionSubmit { get; set; } = AssignmentDefaults.DEFAULT_COMPLETION_SUBMIT;

    [Description("Whether a submission statement is required")]
    public bool RequireSubmissionStatement { get; set; } = AssignmentDefaults.DEFAULT_REQUIRE_SUBMISSION_STATEMENT;

    // ────────────────────────────────────────────────────────────────────────────
    // Team Submission Settings
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Whether team submissions are allowed")]
    public bool TeamSubmission { get; set; } = AssignmentDefaults.DEFAULT_TEAM_SUBMISSION;

    [Description("Whether all team members must submit")]
    public bool RequireAllTeamMembersSubmit { get; set; } = AssignmentDefaults.DEFAULT_REQUIRE_ALL_TEAM_MEMBERS_SUBMIT;

    [Description("Grouping ID for team submissions")]
    public int TeamSubmissionGroupingId { get; set; } = AssignmentDefaults.DEFAULT_TEAM_SUBMISSION_GROUPING_ID;

    // ────────────────────────────────────────────────────────────────────────────
    // Marking Settings
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Whether blind marking is enabled")]
    public bool BlindMarking { get; set; } = AssignmentDefaults.DEFAULT_BLIND_MARKING;

    [Description("Whether the grader is hidden")]
    public bool HideGrader { get; set; } = AssignmentDefaults.DEFAULT_HIDE_GRADER;

    [Description("Whether identities are revealed after grading")]
    public bool RevealIdentities { get; set; } = AssignmentDefaults.DEFAULT_REVEAL_IDENTITIES;

    [Description("Method for reopening attempts")]
    public string AttemptReopenMethod { get; set; } = AssignmentDefaults.DEFAULT_ATTEMPT_REOPEN_METHOD;

    [Description("Maximum number of attempts allowed")]
    public int MaxAttempts { get; set; } = AssignmentDefaults.DEFAULT_MAX_ATTEMPTS;

    [Description("Whether marking workflow is enabled")]
    public bool MarkingWorkflow { get; set; } = AssignmentDefaults.DEFAULT_MARKING_WORKFLOW;

    [Description("Whether marking allocation is enabled")]
    public bool MarkingAllocation { get; set; } = AssignmentDefaults.DEFAULT_MARKING_ALLOCATION;

    [Description("Whether marking is anonymous")]
    public bool MarkingAnonymous { get; set; } = AssignmentDefaults.DEFAULT_MARKING_ANONYMOUS;

    [Description("Whether submissions not in a group are prevented")]
    public bool PreventSubmissionNotInGroup { get; set; } = AssignmentDefaults.DEFAULT_PREVENT_SUBMISSION_NOT_IN_GROUP;

    // ────────────────────────────────────────────────────────────────────────────
    // Nested Structures
    // ────────────────────────────────────────────────────────────────────────────
    [Description("List of submissions for the assignment")]
    public List<AssignmentSubmission> Submissions { get; set; } = new();

    [Description("List of plugin configurations for the assignment")]
    public List<PluginConfig> PluginConfigs { get; set; } = new();

    [Description("List of overrides for the assignment")]
    public List<AssignmentOverride> Overrides { get; set; } = new();

    public SensibleDefaultsAssignment SensibleDefaults { get; set; } = new SensibleDefaultsAssignment();
    public SystemDefaultsAssignment SystemDefaults { get; set; } = new SystemDefaultsAssignment();


    public class SensibleDefaultsAssignment
    {
        // TODO
    }


    public class SystemDefaultsAssignment
    {
        public int ModuleId { get; set; } = 13; 
        public string ModuleName { get; set; } = "assign";
    }
}



/// <summary>
/// Represents an individual submission record for an assignment.
/// </summary>
public class AssignmentSubmission
{
    public int SubmissionId { get; set; }
    public int UserId { get; set; }
    public DateTime TimeCreated { get; set; }
    public DateTime? TimeModified { get; set; } // Nullable, falls es keinen Wert gibt
    public string? TimeStarted { get; set; } // Optional, falls es keinen Wert gibt
    public string Status { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public int AttemptNumber { get; set; }
    public bool IsLatest { get; set; }
}


/// <summary>
/// Represents a custom override for an assignment (e.g., extended due dates).
/// </summary>
public class AssignmentOverride
{
        public int OverrideId { get; set; }
        public int UserId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CutoffDate { get; set; }
        // Add additional override-related properties as needed.
}

/// <summary>
/// Represents a generic plugin configuration for an assignment.
/// </summary>
public class PluginConfig
{
    public int ConfigId { get; set; }
    public string PluginName { get; set; } = string.Empty;

    // Add the missing Subtype property:
    public string Subtype { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
