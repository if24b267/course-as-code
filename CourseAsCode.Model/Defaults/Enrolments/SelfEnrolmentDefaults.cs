using CourseAsCode.Model.Defaults.Roles;
using CourseAsCode.Model.Enrolments;

namespace CourseAsCode.Model.Defaults.Enrolments;

public class SelfEnrolmentDefaults
{
	public const bool DEFAULT_ENABLED = true;
	public static readonly Role DEFAULT_DEFAULT_ROLE = DefaultStudent.GetDefaultStudent();
	public const string? DEFAULT_CUSTOM_NAME = null;
	public const string? DEFAULT_PASSWORD = null;
	public static readonly long? DEFAULT_ENROLMENT_DURATION = null;
	public const bool DEFAULT_NOTIFY_ENROLLER = false;
	public const bool DEFAULT_NOTIFY_STUDENT = false;
	public static readonly long? DEFAULT_NOTIFY_THRESHOLD = null;
	public const bool DEFAULT_SEND_WELCOME_MESSAGE = false;
	public const string? DEFAULT_WELCOME_MESSAGE = null;

	public static readonly DateTime? EnrolmentStartDate = DateTime.Now;
	public static readonly DateTime? EnrolmentEndDate = null;
	public static readonly long? UnenrolInactiveAfter = null;
	public static readonly int? MaximumNumberOfEnrolments = null;


	/// <summary>
	/// Returns a self enrolment object with all properties set to the default values
	/// </summary>
	public static SelfEnrolment GetDefaultSelfEnrolment()
	{
		return new SelfEnrolment()
		{
			Enabled = DEFAULT_ENABLED,
			DefaultRole = DEFAULT_DEFAULT_ROLE,
			EnrolmentDuration = DEFAULT_ENROLMENT_DURATION,
			NotifyEnroller = DEFAULT_NOTIFY_ENROLLER,
			NotifyStudent = DEFAULT_NOTIFY_STUDENT,
			NotifyThreshold = DEFAULT_NOTIFY_THRESHOLD,
			SendWelcomeMessage = DEFAULT_SEND_WELCOME_MESSAGE,
			WelcomeMessage = DEFAULT_WELCOME_MESSAGE,
			EnrolmentStartDate = EnrolmentStartDate,
			EnrolmentEndDate = EnrolmentEndDate,
			UnenrolInactiveAfter = UnenrolInactiveAfter,
			MaximumNumberOfEnrolments = MaximumNumberOfEnrolments,
			SystemDefaults = new EnrolmentMethod.SystemDefaultsEnrolmentMethod()
			{
				EnrolmentId = EnrolmentDefaults.DEFAULT_ENROLMENT_ID,
				TimeCreated = EnrolmentDefaults.DEFAULT_TIME_CREATED,
				TimeModified = EnrolmentDefaults.DEFAULT_TIME_MODIFIED
			}
		};
	}
}