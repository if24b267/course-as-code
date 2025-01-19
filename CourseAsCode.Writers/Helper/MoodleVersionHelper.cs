using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.Helper;

public class MoodleVersionHelper
{
	// See https://moodledev.io/general/releases

	#region Singleton
	private static MoodleVersionHelper? _instance;

	public static MoodleVersionHelper Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MoodleVersionHelper();
			}

			return _instance;
		}
	}
	#endregion


	/// <summary>
	/// maps the MoodleVersion enum to the Moodle Version String
	/// </summary>
	/// <returns>Moodle Version String, e.g. 2024100700</returns>
	public string GetMoodleVersionStringFromEnum(MoodleVersion version)
	{
		return version switch
		{
			MoodleVersion.Moodle451 => "2024100701",
			MoodleVersion.Moodle450 => "2024100700",
			MoodleVersion.Moodle445 => "2024042205",
			MoodleVersion.Moodle444 => "2024042204",
			MoodleVersion.Moodle443 => "2024042203",
			MoodleVersion.Moodle442 => "2024042202",
			MoodleVersion.Moodle441 => "2024042201",
			MoodleVersion.Moodle440 => "2024042200",
			_ => "0000000000"
		};
	}


	/// <summary>
	/// maps the MoodleVersion enum to the Moodle Release String
	/// </summary>
	/// <returns>Moodle Release String, e.g. 4.5 (Build: 20241007)</returns>
	public string GetMoodleReleaseStringFromEnum(MoodleVersion version)
	{
		return version switch
		{
			MoodleVersion.Moodle451 => $"4.5.1 (Build: {GetMoodleVersionStringFromEnum(version)})",
			MoodleVersion.Moodle450 => $"4.5 (Build: {GetMoodleVersionStringFromEnum(version)})",
			MoodleVersion.Moodle445 => $"4.4.5 (Build: {GetMoodleVersionStringFromEnum(version)})",
			MoodleVersion.Moodle444 => $"4.4.4 (Build: {GetMoodleVersionStringFromEnum(version)})",
			MoodleVersion.Moodle443 => $"4.4.3 (Build: {GetMoodleVersionStringFromEnum(version)})",
			MoodleVersion.Moodle442 => $"4.4.2 (Build: {GetMoodleVersionStringFromEnum(version)})",
			MoodleVersion.Moodle441 => $"4.4.1 (Build: {GetMoodleVersionStringFromEnum(version)})",
			MoodleVersion.Moodle440 => $"4.4 (Build: {GetMoodleVersionStringFromEnum(version)})",
			_ => $"1.0 (Build: {GetMoodleVersionStringFromEnum(version)})",
		};
	}
}