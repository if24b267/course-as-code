using System.ComponentModel;
using CourseAsCode.Model.Defaults.Enrolments;

namespace CourseAsCode.Model;

/// <summary>
/// the process of adding students to courses is called Enrolment
/// </summary>
public abstract class EnrolmentMethod(Role defaultRole)
{
	[Description("whether the enrolment method is enabled")]
	public bool Enabled { get; set; } = EnrolmentDefaults.DEFAULT_ENABLED;
	[Description("the default role that is assigned to students when they are enrolled via this method")]
	public Role DefaultRole { get; set; } = defaultRole;

	public SystemDefaultsEnrolmentMethod SystemDefaults { get; set; } = new SystemDefaultsEnrolmentMethod();

	
	public class SystemDefaultsEnrolmentMethod
	{
		[Description("unique identifier for the enrolment method, used for referencing")]
		public int EnrolmentId { get; set; } = EnrolmentDefaults.DEFAULT_ENROLMENT_ID;
		public DateTime TimeCreated { get; set; } = EnrolmentDefaults.DEFAULT_TIME_CREATED;
		public DateTime TimeModified { get; set; } = EnrolmentDefaults.DEFAULT_TIME_MODIFIED;
	}
}