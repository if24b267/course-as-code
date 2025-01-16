using CourseAsCode.Model.Defaults.Activities.Resources;
using System.ComponentModel;

namespace CourseAsCode.Model.Activities.Resources;

public class URL : Activity
{
	[Description("the url itself")]
	public string ExternalUrl { get; set; }
}