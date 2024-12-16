using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.Helper;

public static class Helper
{
	public static string GetCourseFormatString(CourseFormat format)
	{
		return format switch
		{
			CourseFormat.CustomSections => "topics",
			CourseFormat.WeeklySections => "weeks",
			CourseFormat.SingleActivity => "singleactivity",
			CourseFormat.Social => "social",
			_ => "topics"
		};
	}

	public static string GetMoodleVersionFromEnum(MoodleVersion version)
	{
		return version switch
		{
			MoodleVersion.Moodle440 => "2024042200",
			MoodleVersion.Moodle441 => "2024042201",
			MoodleVersion.Moodle442 => "2024042202",
			MoodleVersion.Moodle443 => "2024042203",
			MoodleVersion.Moodle444 => "2024042204",
			MoodleVersion.Moodle445 => "2024042205",
			MoodleVersion.Moodle450 => "2024100700",
			MoodleVersion.Moodle451 => "2024100701",
			_ => "2024100700"
		};
	}

	public static string EnumIntegerToString<T>(T enumValue) where T : Enum
	{
		return ((int)(object)enumValue).ToString();
	}

	public static long ConvertToUnixTimestamp(DateTime dateTime)
	{
		// Ensure the DateTime is in UTC
		DateTime utcDateTime = dateTime.ToUniversalTime();

		// Define the Unix epoch (January 1, 1970, 00:00:00 UTC)
		DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Calculate the difference in seconds
		long unixTimestamp = (long)(utcDateTime - unixEpoch).TotalSeconds;

		return unixTimestamp;
	}
}