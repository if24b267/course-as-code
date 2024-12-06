using System.ComponentModel;

namespace CourseAsCode.Model.Enrolments;

/// <summary>
/// a user can choose to enrol him/herself into a course
/// </summary>
public class SelfEnrolment : EnrolmentMethod
{
    [Description("optional custom name for the enrolment method")]
    public string? CustomName { get; set; }
    [Description("if set, users must provide a password to enrol in the course")]
    public string? Password { get; set; }
    
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
    
    // Defaults
    public class SensibleDefaults
    {
        [Description("if set, enrolment is only possible starting from this date")]
        public DateTime? EnrolmentStartDate { get; set; }
        [Description("if set, enrolment is only possible until this date")]
        public DateTime? EnrolmentEndDate { get; set; }
        [Description("if set, inactive users are unenrolled after this many days")]
        public int? UnenrolInactiveAfter { get; set; }
        [Description("if set, the maximum number of users that can self enrol is limited")]
        public int? MaximumNumberOfEnrolments { get; set; }
        [Description("the threshold in days before the enrolment expires when the notifications are sent")]
        public int NotifyThreshold { get; set; }
    }

    public class SystemDefaults
    {
        
    }
}