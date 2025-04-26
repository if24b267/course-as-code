using CourseAsCode.Model.Enums;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;

namespace CourseAsCode.Writers.XML.Helper.Interfaces;

public interface IGeneralHelper
{
    /// <summary>
    /// creates the name of the backup (file) for the moodle backup according to moodle naming conventions.
    /// </summary>
    /// <param name="course">the initialized moodle course.</param>
    string GenerateBackupName(Model.Course course);

    /// <summary>
    /// converts a C# DateTime object to a Unix Timestamp, considering timezones.
    /// </summary>
    /// <returns>String representing the Unix Timestamp, e.g. 1735122151.</returns>
    string DateTimeToUnixTimestamp(DateTime dateTime);

    /// <summary>
    /// converts the integer values of an enum to a string, e.g. 1 -> "1".
    /// </summary>
    /// <param name="enumValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    string EnumIntegerToString<T>(T enumValue)
        where T : Enum;

    /// <summary>
    /// maps the Language enum to their corresponding shorts that are accepted by Moodle.
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    string GetLanguageFromEnum(Language? language);

    /// <summary>
    /// maps the TimeZone enum to their corresponding shorts that are accepted by Moodle.
    /// </summary>
    /// <param name="timeZone"></param>
    /// <returns></returns>
    string GetTimeZoneFromEnum(TimeZone timeZone);
}
