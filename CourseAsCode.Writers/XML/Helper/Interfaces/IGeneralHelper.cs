using CourseAsCode.Model.Enums;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;

namespace CourseAsCode.Writers.XML.Helper.Interfaces;

public interface IGeneralHelper
{
    /// <summary>
    /// Creates the name of the backup file for a Moodle course following Moodle's naming conventions
    /// </summary>
    /// <param name="course">The initialized Moodle course containing all necessary information</param>
    string GenerateBackupName(Model.Course course);

    /// <summary>
    /// Converts a C# <see cref="DateTime"/> object to a Unix timestamp, considering time zones.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to be converted.</param>
    /// <returns>A string representing the Unix timestamp, e.g. "1735122151".</returns>
    string DateTimeToUnixTimestamp(DateTime dateTime);

    /// <summary>
    /// Converts the integer value of an enum to its string representation
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="enumValue">The enum value to be converted.</param>
    /// <returns>A string representation of the underlying integer value of the enum</returns>
    string EnumIntegerToString<T>(T enumValue)
        where T : Enum;

    /// <summary>
    /// Maps a given <see cref="Language"/> enum value to its corresponding language code accepted by Moodle.
    /// </summary>
    /// <param name="language">The <see cref="Language"/> enum value to be mapped.</param>
    /// <returns>A string representing the language code if the mapping is successful; otherwise, an empty string.</returns>
    string GetLanguageFromEnum(Language? language);

    /// <summary>
    /// Maps the provided <see cref="TimeZone"/> enum value to its corresponding short representation that is accepted by Moodle.
    /// </summary>
    /// <param name="timeZone">The <see cref="TimeZone"/> enum value to be mapped.</param>
    /// <returns>A string representation of the time zone corresponding to the provided enum value.</returns>
    string GetTimeZoneFromEnum(TimeZone timeZone);
}
