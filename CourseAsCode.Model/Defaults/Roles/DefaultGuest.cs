using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Roles;

public static class DefaultGuest
{
    public const string DEFAULT_NAME = "Guest";
    public const string DEFAULT_SHORT_NAME = "guest";
    public const string? DEFAULT_LOCAL_NAME = null;
    public const string DEFAULT_DESCRIPTION = " ";
    public const Archetype DEFAULT_ARCHETYPE = Enums.Archetype.Guest;
    public const int DEFAULT_ID = 0;
    
    
    /// <summary>
    /// Returns a role object with all properties set to the default values for a guest
    /// </summary>
    public static Role GetDefaultGuest()
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