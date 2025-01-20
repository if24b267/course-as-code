using System.ComponentModel;

namespace CourseAsCode.Model.Activities.Resources;

public class URL : Activity
{
	[Description("the url itself")]
	public string ExternalUrl { get; set; }

	public SensibleDefaultsURL SensibleDefaults { get; set; } = new SensibleDefaultsURL();
	public SystemDefaultsURL SystemDefaults { get; set; } = new SystemDefaultsURL();

	public class SensibleDefaultsURL
	{
		// TODO
	}


	public class SystemDefaultsURL
	{
		public int ModuleId { get; set; } = 2;
		public string ModuleName { get; set; } = "url";
	}
}