using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// Provides helper methods for working with roles in Moodle
    /// </summary>
    public class RolesHelper : IRolesHelper
    {
        /// <summary>
        /// Converts the provided <see cref="Archetype"/> value to its corresponding string representation as used in Moodle.
        /// </summary>
        /// <param name="archetype">The archetype value to be converted.</param>
        /// <returns>
        /// <para>A string representation of the specified archetype. Returns "manager", "coursecreator", "editingteacher",
        /// "teacher", "student", "guest", "user", or "frontpage" based on the input value. </para>
        /// <para>If the value does not match any known <see cref="Archetype"/>, "user" is returned as the default.</para>
        /// </returns>
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
}
