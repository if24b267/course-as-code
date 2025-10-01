using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface IFileHelper
    {
        /// <summary>
        /// Retrieves the string representation of a FileArea enum value
        /// </summary>
        /// <param name="fileArea">The enum value of type <see cref="FileArea"/> to be converted to its corresponding file area string.</param>
        /// <returns>The string representation of the provided <paramref name="fileArea"/>, such as "overviewfiles" or "content". For unknown values, defaults to "content".</returns>
        string GetFileAreaFromEnum(FileArea fileArea);

        /// <summary>
        /// Returns the corresponding string representation of the given Component enum value
        /// </summary>
        /// <param name="component">The Component enum value to convert</param>
        /// <returns>A string representing the given Component value. For unknown values, defaults to "course".</returns>
        string GetComponentFromEnum(Component component);

        /// <summary>
        /// Converts a given <see cref="License"/> enum value to its corresponding string representation as used in Moodle
        /// </summary>
        /// <param name="license">The license enum value to be converted.</param>
        /// <returns>A string representation of the provided license value. If the license is not recognized, returns "unknown".</returns>
        string GetLicenseFromEnum(License license);
    }
}
