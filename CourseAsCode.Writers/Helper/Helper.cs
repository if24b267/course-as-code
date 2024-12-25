using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.Helper;

public class Helper
{
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