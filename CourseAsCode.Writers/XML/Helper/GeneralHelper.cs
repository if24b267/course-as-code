using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;
using NLog;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// Provides various helper methods that don't fit into other helper classes
    /// </summary>
    public class GeneralHelper : IGeneralHelper
    {
        private readonly ILogger<GeneralHelper> _logger;

        /// <summary>
        /// Creates a new instance of the GeneralHelper class
        /// </summary>
        /// <param name="logger">Logger instance specific to TagsWriter</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public GeneralHelper(ILogger<GeneralHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Creates the name of the backup file for a Moodle course following Moodle's naming conventions
        /// </summary>
        /// <param name="course">The initialized Moodle course containing all necessary information</param>
        public string GenerateBackupName(Model.Course course)
        {
            // Moodle Backup Naming Convention:
            // e.g. backup-moodle2-course-2-empco-20241225-0920.mbz
            // -> backup-moodle2-course-<courseid>-<shortname>-<date>-<time>.mbz
            var courseId = course.SystemDefaults.Id;

            var shortName = course.ShortName;

            var date = DateTime.Now.ToString("yyyyMMdd");
            var time = DateTime.Now.ToString("HHmm");

            string backupName = $"backup-moodle2-course-{courseId}-{shortName}-{date}-{time}.mbz";
            _logger.LogTrace($"Generated backup name: {backupName}");

            return backupName;
        }


        /// <summary>
        /// Converts a C# <see cref="DateTime"/> object to a Unix timestamp, considering time zones.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to be converted.</param>
        /// <returns>A string representing the Unix timestamp, e.g. "1735122151".</returns>
        public string DateTimeToUnixTimestamp(DateTime dateTime)
        {
            // Ensure the DateTime is in UTC
            DateTime viennaTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            TimeZoneInfo viennaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(viennaTime, viennaTimeZone);

            // Define the Unix epoch (January 1, 1970, 00:00:00 UTC)
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Calculate the difference in seconds
            long unixTimestamp = (long)(utcDateTime - unixEpoch).TotalSeconds;

            _logger.LogTrace($"Converted {dateTime} to Unix Timestamp: {unixTimestamp}");

            return unixTimestamp.ToString();
        }


        /// <summary>
        /// Converts the integer value of an enum to its string representation
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="enumValue">The enum value to be converted.</param>
        /// <returns>A string representation of the underlying integer value of the enum</returns>
        public string EnumIntegerToString<T>(T enumValue)
            where T : Enum
        {
            return ((int)(object)enumValue).ToString();
        }


        /// <summary>
        /// Maps a given <see cref="Language"/> enum value to its corresponding language code accepted by Moodle.
        /// </summary>
        /// <param name="language">The <see cref="Language"/> enum value to be mapped.</param>
        /// <returns>A string representing the language code if the mapping is successful; otherwise, an empty string.</returns>
        public string GetLanguageFromEnum(Language? language)
        {
            return language switch
            {
                Language.English => "en",
                Language.German => "de",
                _ => "",
            };
        }


        /// <summary>
        /// Maps the provided <see cref="TimeZone"/> enum value to its corresponding short representation that is accepted by Moodle.
        /// </summary>
        /// <param name="timeZone">The <see cref="TimeZone"/> enum value to be mapped.</param>
        /// <returns>A string representation of the time zone corresponding to the provided enum value.</returns>
        public string GetTimeZoneFromEnum(TimeZone timeZone)
        {
            return timeZone switch
            {
                TimeZone.EuropeBerlin => "Europe/Berlin",
                TimeZone.EuropeVienna => "Europe/Vienna",
                TimeZone.EuropeZurich => "Europe/Zurich",
                _ => "99",
            };
        }
    }
}
