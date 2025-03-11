using CourseAsCode.Model.Defaults.Enrolments;
using System.ComponentModel;

namespace CourseAsCode.Model.Enrolments;

/// <summary>
/// a user can choose to enrol him/herself into a course
/// </summary>
public class SelfEnrolment() : EnrolmentMethod(defaultRole: SelfEnrolmentDefaults.DEFAULT_DEFAULT_ROLE)
{
	[Description("optional custom name for the enrolment method")]
	public string? CustomName { get; set; } = SelfEnrolmentDefaults.DEFAULT_CUSTOM_NAME;
	[Description("if set, users must provide a password to enrol in the course")]
	public string? Password { get; set; } = SelfEnrolmentDefaults.DEFAULT_PASSWORD;
	[Description("default length of the time (in seconds) that the enrolment is valid starting from the users' enrolment date, blank for unlimited")]
	public long? EnrolmentDuration { get; set; } = SelfEnrolmentDefaults.DEFAULT_ENROLMENT_DURATION;
	[Description("if enabled, the person who enrolled the user is notified when the user's enrollment in the course is about to expire")]
	public bool NotifyEnroller { get; set; } = SelfEnrolmentDefaults.DEFAULT_NOTIFY_ENROLLER;
	[Description("if enabled, the user is notified when their enrollment in the course is about to expire")]
	public bool NotifyStudent { get; set; } = SelfEnrolmentDefaults.DEFAULT_NOTIFY_STUDENT;
	[Description("the threshold (in seconds) before the enrolment expires when the notifications are sent")]
	public long? NotifyThreshold { get; set; } = SelfEnrolmentDefaults.DEFAULT_NOTIFY_THRESHOLD;
	[Description("if enabled, students receive a welcome email when they are enrolled in the course")]
	public bool SendWelcomeMessage { get; set; } = SelfEnrolmentDefaults.DEFAULT_SEND_WELCOME_MESSAGE;
	[Description("the message that is sent to the student when they are enrolled in the course")]
	public string? WelcomeMessage { get; set; } = SelfEnrolmentDefaults.DEFAULT_WELCOME_MESSAGE;
	[Description("if set, enrolment is only possible starting from this date")]
	public DateTime? EnrolmentStartDate { get; set; } = SelfEnrolmentDefaults.EnrolmentStartDate;
	[Description("if set, enrolment is only possible until this date")]
	public DateTime? EnrolmentEndDate { get; set; } = SelfEnrolmentDefaults.EnrolmentEndDate;
	[Description("if set, inactive users are unenrolled after this many seconds")]
	public long? UnenrolInactiveAfter { get; set; } = SelfEnrolmentDefaults.UnenrolInactiveAfter;
	[Description("if set, the maximum number of users that can self enrol is limited")]
	public int? MaximumNumberOfEnrolments { get; set; } = SelfEnrolmentDefaults.MaximumNumberOfEnrolments;
}