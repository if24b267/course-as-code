using CourseAsCode.Model.Defaults.Roles;

namespace CourseAsCode.Model.Defaults.Enrolments;

public class SelfEnrolmentDefaults
{
    public static readonly Role DEFAULT_DEFAULT_ROLE = DefaultStudent.GetDefaultStudent();
}