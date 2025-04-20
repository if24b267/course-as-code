using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Helper;

public class RolesHelper : IRolesHelper
{
    public string GetArchetypeFromEnum(Archetype archetype)
    {
        return archetype switch
        {
            Archetype.Manager => "manager",
            Archetype.CourseCreator => "coursecreator",
            Archetype.EditingTeacher => "editingteacher",
            Archetype.NonEditingTeacher => "teacher",
            Archetype.Student => "student",
            Archetype.Guest => "guest",
            Archetype.AuthenticatedUser => "user",
            Archetype.AuthenticatedUserOnFrontpage => "frontpage",
            _ => "user",
        };
    }
}
