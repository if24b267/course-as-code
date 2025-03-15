using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper;

public interface IMoodleVersionHelper
{
    /// <summary>
    /// maps the MoodleVersion enum to the Moodle Version String
    /// </summary>
    /// <returns>Moodle Version String, e.g. 2024100700</returns>
    string GetMoodleVersionStringFromEnum(MoodleVersion version);

    /// <summary>
    /// maps the MoodleVersion enum to the Moodle Release String
    /// </summary>
    /// <returns>Moodle Release String, e.g. 4.5 (Build: 20241007)</returns>
    string GetMoodleReleaseStringFromEnum(MoodleVersion version);
}