using System.ComponentModel;

using CourseAsCode.Model.Defaults.Enrolments;

namespace CourseAsCode.Model.Enrolments;

/// <summary>
/// the administrator or course teacher adds users manually
/// </summary>
public class ManualEnrolment() : EnrolmentMethod(defaultRole: ManualEnrolmentDefaults.DEFAULT_DEFAULT_ROLE)
{
    [Description("default length of the time (in seconds) that the enrolment is valid starting from the users' enrolment date, blank for unlimited")]
    public long? EnrolmentDuration { get; set; } = ManualEnrolmentDefaults.DEFAULT_ENROLMENT_DURATION;
    [Description("if enabled, the person who enrolled the user is notified when the user's enrollment in the course is about to expire")]
    public bool NotifyEnroller { get; set; } = ManualEnrolmentDefaults.DEFAULT_NOTIFY_ENROLLER;
    [Description("if enabled, the user is notified when their enrollment in the course is about to expire")]
    public bool NotifyStudent { get; set; } = ManualEnrolmentDefaults.DEFAULT_NOTIFY_STUDENT;
    [Description("the threshold (in seconds) before the enrolment expires when the notifications are sent, blank if not applicable")]
    public long? NotifyThreshold { get; set; } = ManualEnrolmentDefaults.DEFAULT_NOTIFY_THRESHOLD;
    [Description("if enabled, students receive a welcome email when they are enrolled in the course")]
    public bool SendWelcomeMessage { get; set; } = ManualEnrolmentDefaults.DEFAULT_SEND_WELCOME_MESSAGE;
    [Description("the message that is sent to the student when they are enrolled in the course")]
    public string? WelcomeMessage { get; set; } = ManualEnrolmentDefaults.DEFAULT_WELCOME_MESSAGE;
}
