using CourseAsCode.Model.Defaults.Roles;

namespace CourseAsCode.Model.Defaults.Enrolments;

public class ManualEnrolmentDefaults
{
    public static readonly Role DEFAULT_DEFAULT_ROLE = DefaultStudent.GetDefaultStudent();

    public static readonly int? DEFAULT_NOTIFY_THRESHOLD = null;
}