using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities.Resources;

public class MoodleFileDefaults
{
	// General defaults
	public const string DEFAULT_FILE_NAME = "$@NULL@$";
	public const string DEFAULT_FILE_PATH = "/";

	// Sensible defaults
	public static User? DEFAULT_AUTHOR = null;
	public static License? DEFAULT_LICENSE = null;

	// System defaults
	public const string DEFAULT_CONTENT_HASH = "d7ecb4a8bc606fbc2b104e0f94d6476246b024bf";
	public static Component? DEFAULT_COMPONENT = Component.Course;
	public static FileArea? DEFAULT_FILE_AREA = FileArea.CourseContent;
	public const int DEFAULT_ITEM_ID = 0;


	/// <summary>
	/// Returns a MoodleFile object with all properties set to the default values
	/// </summary>
	public MoodleFile GetDefaultMoodleFile()
	{
		return new MoodleFile()
		{
			Name = ActivityDefaults.DEFAULT_NAME,
			Description = ActivityDefaults.DEFAULT_DESCRIPTION,
			GroupMode = ActivityDefaults.DEFAULT_GROUP_MODE,
			Tags = ActivityDefaults.DEFAULT_TAGS,
			VisibleToStudents = ActivityDefaults.DEFAULT_VISIBLE_TO_STUDENTS,
			DisplayDescription = ActivityDefaults.DEFAULT_DISPLAY_DESCRIPTION,
			ActivityId = ActivityDefaults.DEFAULT_ACTIVITY_ID,
			CustomId = ActivityDefaults.DEFAULT_CUSTOM_ID,
			Language = ActivityDefaults.DEFAULT_LANGUAGE,

			FileName = DEFAULT_FILE_NAME,
			FilePath = DEFAULT_FILE_PATH,

			SensibleDefaults = new MoodleFile.SensibleDefaultsFile()
			{
				Author = DEFAULT_AUTHOR,
				License = DEFAULT_LICENSE
			},
			SystemDefaults = new MoodleFile.SystemDefaultsFile()
			{
				ContentHash = DEFAULT_CONTENT_HASH,
				Component = DEFAULT_COMPONENT,
				FileArea = DEFAULT_FILE_AREA,
				ItemId = DEFAULT_ITEM_ID
			}
		};
	}
}