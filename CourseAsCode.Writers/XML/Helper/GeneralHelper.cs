using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using NLog;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;

namespace CourseAsCode.Writers.XML.Helper;

public class GeneralHelper : IGeneralHelper
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	
	#region Singleton
	private static GeneralHelper? _instance;

	public static GeneralHelper Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GeneralHelper();
			}

			return _instance;
		}
	}
	#endregion

	/// <summary>
	/// creates the name of the backup (file) for the moodle backup according to moodle naming conventions
	/// </summary>
	/// <param name="course">the initialized moodle course</param>
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
		Logger.Trace($"Generated backup name: {backupName}");

		return backupName;
	}


	/// <summary>
	/// converts a C# DateTime object to a Unix Timestamp, considering timezones
	/// </summary>
	/// <returns>String representing the Unix Timestamp, e.g. 1735122151</returns>
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
		
		Logger.Trace($"Converted {dateTime} to Unix Timestamp: {unixTimestamp}");

		return unixTimestamp.ToString();
	}


	/// <summary>
	/// converts the integer values of an enum to a string, e.g. 1 -> "1"
	/// </summary>
	/// <param name="enumValue"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public string EnumIntegerToString<T>(T enumValue) where T : Enum
	{
		return ((int)(object)enumValue).ToString();
	}


	/// <summary>
	/// maps the Language enum to their corresponding shorts that are accepted by Moodle
	/// </summary>
	/// <param name="language"></param>
	/// <returns></returns>
	public string GetLanguageFromEnum(Language? language)
	{
		return language switch
		{
			Language.English => "en",
			Language.German => "de",
			_ => ""
		};
	}


	/// <summary>
	/// maps the TimeZone enum to their corresponding shorts that are accepted by Moodle
	/// </summary>
	/// <param name="timeZone"></param>
	/// <returns></returns>
	public string GetTimeZoneFromEnum(TimeZone timeZone)
	{
		return timeZone switch
		{
			TimeZone.EuropeBerlin => "Europe/Berlin",
			TimeZone.EuropeVienna => "Europe/Vienna",
			TimeZone.EuropeZurich => "Europe/Zurich",
			_ => "99"
		};
	}
}