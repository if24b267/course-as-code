using System.ComponentModel;

namespace CourseAsCode.Model.Activities.Resources;

public class URL : Activity
{
	private static Random _rnd = new Random();
	
	[Description("the url itself")]
	public string ExternalUrl { get; set; }

	public SensibleDefaultsUrl SensibleDefaults { get; set; } = new SensibleDefaultsUrl();
	public SystemDefaultsUrl SystemDefaults { get; set; } = new SystemDefaultsUrl();

	
	public class SensibleDefaultsUrl
	{
		// TODO
	}


	public class SystemDefaultsUrl
	{
		public int ModuleId { get; set; } = _rnd.Next(1, 100);
		public string ModuleName { get; set; } = "url";
	}
}