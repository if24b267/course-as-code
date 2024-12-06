using System.ComponentModel;

namespace CourseAsCode.Model.Enrolments;

/// <summary>
/// the administrator or course teacher adds users manually
/// </summary>
public class ManualEnrolment : EnrolmentMethod
{
    [Description("default length of the time that the enrolment is valid starting from the users' enrolment date, blank for unlimited")]
    public int EnrolmentDuration { get; set; }
    [Description("if enabled, the person who enrolled the user is notified when the user's enrollment in the course is about to expire")]
    public bool NotifyEnroller { get; set; }
    [Description("if enabled, the user is notified when their enrollment in the course is about to expire")]
    public bool NotifyStudent { get; set; }
    [Description("if enabled, students receive a welcome email when they are enrolled in the course")]
    public bool SendWelcomeMessage { get; set; }
    [Description("the message that is sent to the student when they are enrolled in the course")]
    public string? WelcomeMessage { get; set; }
    
    public class SensibleDefaults
    {
        [Description("the threshold in days before the enrolment expires when the notifications are sent")]
        public int NotifyThreshold { get; set; }
    }

    public class SystemDefaults
    {
        
    }
}