using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface IRolesHelper
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
        string GetArchetypeFromEnum(Archetype archetype);
    }
}
