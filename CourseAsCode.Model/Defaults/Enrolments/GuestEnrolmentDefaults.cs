using CourseAsCode.Model.Defaults.Roles;
using CourseAsCode.Model.Enrolments;

namespace CourseAsCode.Model.Defaults.Enrolments;

public class GuestEnrolmentDefaults
{
    public const bool DEFAULT_ENABLED = false;
    public static readonly Role DEFAULT_DEFAULT_ROLE = DefaultGuest.GetDefaultGuest();
    public const string? DEFAULT_PASSWORD = null;

    /// <summary>
    /// Returns a guest enrolment object with all properties set to the default values
    /// </summary>
    public static GuestEnrolment GetDefaultGuestEnrolment()
    {
        return new GuestEnrolment()
        {
            Enabled = DEFAULT_ENABLED,
            DefaultRole = DEFAULT_DEFAULT_ROLE,
            SystemDefaults = new EnrolmentMethod.SystemDefaultsEnrolmentMethod()
            {
                EnrolmentId = EnrolmentDefaults.DEFAULT_ENROLMENT_ID,
                TimeCreated = EnrolmentDefaults.DEFAULT_TIME_CREATED,
                TimeModified = EnrolmentDefaults.DEFAULT_TIME_MODIFIED,
            },
            Password = DEFAULT_PASSWORD,
        };
    }
}
