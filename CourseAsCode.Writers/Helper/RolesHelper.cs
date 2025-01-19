using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.Helper;

public class RolesHelper
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
			_ => "user"
		};
	}
}