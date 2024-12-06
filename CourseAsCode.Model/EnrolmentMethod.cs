using System.ComponentModel;
using CourseAsCode.Model.Enrolments;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model;

/// <summary>
/// the process of adding students to courses is called Enrolment
/// </summary>
public abstract class EnrolmentMethod
{
    [Description("whether the enrolment method is enabled")]
    public bool Enabled { get; set; }
    [Description("the default role that is assigned to students when they are enrolled via this method")]
    public required Role DefaultRole { get; set; }

    public class SystemDef
    {
        [Description("unique identifier for the enrolment method, used for referencing")]
        public int EnrolmentId { get; set; }
        
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
    }
}