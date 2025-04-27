using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities.Resources;

public class AssignmentDefaults
{
    // General Settings
    public const string DEFAULT_INTRO = "";
    public const FormattingFormat DEFAULT_FORMATTING_FORMAT = FormattingFormat.HTML;
    public const bool DEFAULT_ALWAYS_SHOW_DESCRIPTION = true;

    // Submission Settings
    public const bool DEFAULT_SUBMISSION_DRAFTS = false;
    public const bool DEFAULT_SEND_NOTIFICATIONS = true;
    public const bool DEFAULT_SEND_LATE_NOTIFICATIONS = false;
    public const bool DEFAULT_SEND_STUDENT_NOTIFICATIONS = true;
    public static readonly DateTime? DEFAULT_DUE_DATE = null;
    public static readonly DateTime? DEFAULT_CUTOFF_DATE = null;
    public static readonly DateTime? DEFAULT_GRADING_DUE_DATE = null;
    public static readonly DateTime? DEFAULT_ALLOW_SUBMISSIONS_FROM_DATE = null;

    // Grading Settings
    public const int DEFAULT_GRADE = 100;
    public const bool DEFAULT_COMPLETION_SUBMIT = true;
    public const bool DEFAULT_REQUIRE_SUBMISSION_STATEMENT = false;

    // Team Submission Settings
    public const bool DEFAULT_TEAM_SUBMISSION = false;
    public const bool DEFAULT_REQUIRE_ALL_TEAM_MEMBERS_SUBMIT = false;
    public const int DEFAULT_TEAM_SUBMISSION_GROUPING_ID = 0;

    // Marking Settings
    public const bool DEFAULT_BLIND_MARKING = false;
    public const bool DEFAULT_HIDE_GRADER = false;
    public const bool DEFAULT_REVEAL_IDENTITIES = false;
    public const string DEFAULT_ATTEMPT_REOPEN_METHOD = "none";
    public const int DEFAULT_MAX_ATTEMPTS = 1;
    public const bool DEFAULT_MARKING_WORKFLOW = false;
    public const bool DEFAULT_MARKING_ALLOCATION = false;
    public const bool DEFAULT_MARKING_ANONYMOUS = false;
    public const bool DEFAULT_PREVENT_SUBMISSION_NOT_IN_GROUP = false;
}
