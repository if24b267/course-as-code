using CourseAsCode.Model.Defaults.Roles;

namespace CourseAsCode.Model.Defaults.Enrolments;

public class GuestEnrolmentDefaults
{
    public static readonly Role DEFAULT_DEFAULT_ROLE = DefaultGuest.GetDefaultGuest();
}