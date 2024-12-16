using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model;

public class Role
{
	[Description("optional for standard roles, required for custom roles")]
	public string? Name { get; set; }

	[Description("low level role identifier in which only ASCII alphanumeric characters are allowed")]
	public required string ShortName { get; set; }

	[Description("can be used to override the default name of the role for a specific course only")]
	public string? LocalName { get; set; }

	[Description("description of the role; automatically generated for standard roles if not provided")]
	public string? Description { get; set; }

	[Description("Blueprint for the role, defines a standard set of permissions")]
	public Archetype Archetype { get; set; }

	public SensibleDefaultsRole SensibleDefaults { get; set; }
	public SystemDefaultsRole SystemDefaults { get; set; }

	// Defaults
	public class SensibleDefaultsRole
	{

	}

	public class SystemDefaultsRole
	{
		public int Id { get; set; }
	}
}