using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Roles;

public static class DefaultStudent
{
    public const string DEFAULT_NAME = "Student";
    public const string DEFAULT_SHORT_NAME = "student";
    public const string? DEFAULT_LOCAL_NAME = null;
    public const string DEFAULT_DESCRIPTION = "Students generally have fewer privileges within a course.";
    public const Archetype DEFAULT_ARCHETYPE = Enums.Archetype.Student;
    public const int DEFAULT_ID = 5;
    
    
    public static Role GetDefaultStudent()
    {
        return new Role()
        {
            Name = DEFAULT_NAME,
            ShortName = DEFAULT_SHORT_NAME,
            LocalName = DEFAULT_LOCAL_NAME,
            Description = DEFAULT_DESCRIPTION,
            Archetype = DEFAULT_ARCHETYPE,
            SensibleDefaults = new Role.SensibleDefaultsRole(),
            SystemDefaults = new Role.SystemDefaultsRole()
            {
                RoleId = DEFAULT_ID
            }
        };
    }
}