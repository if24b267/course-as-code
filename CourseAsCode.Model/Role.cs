using System.ComponentModel;

using CourseAsCode.Model.Defaults.Roles;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model;

public class Role
{
    [Description("optional for standard roles, required for custom roles")]
    public string? Name { get; set; } = DefaultStudent.DEFAULT_NAME;
    [Description("low level role identifier in which only ASCII alphanumeric characters are allowed")]
    public required string ShortName { get; set; } = DefaultStudent.DEFAULT_SHORT_NAME;
    [Description("can be used to override the default name of the role for a specific course only")]
    public string? LocalName { get; set; } = DefaultStudent.DEFAULT_LOCAL_NAME;
    [Description("description of the role; automatically generated for standard roles if not provided")]
    public string? Description { get; set; } = DefaultStudent.DEFAULT_DESCRIPTION;
    [Description("Blueprint for the role, defines a standard set of permissions")]
    public Archetype Archetype { get; set; } = DefaultStudent.DEFAULT_ARCHETYPE;

    public SensibleDefaultsRole SensibleDefaults { get; set; } = new SensibleDefaultsRole();
    public SystemDefaultsRole SystemDefaults { get; set; } = new SystemDefaultsRole();

    // Defaults
    public class SensibleDefaultsRole
    {
    }

    public class SystemDefaultsRole
    {
        public int RoleId { get; set; } = DefaultStudent.DEFAULT_ID;
    }
}
