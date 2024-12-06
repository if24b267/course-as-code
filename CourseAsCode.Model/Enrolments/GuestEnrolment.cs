using System.ComponentModel;

namespace CourseAsCode.Model.Enrolments;

/// <summary>
/// users can view course materials but not participate
/// </summary>
public class GuestEnrolment : EnrolmentMethod
{
    [Description("if set, guest access to the course is restricted to only those who know the password")]
    public string? Password { get; set; }
}