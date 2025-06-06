using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface IMoodleVersionHelper
    {
        /// <summary>
        /// Maps the MoodleVersion enum to the corresponding Moodle Version String
        /// </summary>
        /// <param name="version">The MoodleVersion enum value to be mapped</param>
        /// <returns>A string representing the Moodle Version, e.g., 2024100700</returns>
        string GetMoodleVersionStringFromEnum(MoodleVersion version);

        /// <summary>
        /// Maps the MoodleVersion enum to the corresponding Moodle Release String.
        /// </summary>
        /// <param name="version">The MoodleVersion enum value to be converted to a Moodle Release String.</param>
        /// <returns>A string representing the Moodle Release, e.g., "4.5 (Build: 20241007)".</returns>
        string GetMoodleReleaseStringFromEnum(MoodleVersion version);
    }
}
