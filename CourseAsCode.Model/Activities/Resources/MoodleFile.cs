using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;
using Component = CourseAsCode.Model.Enums.Component;
using License = CourseAsCode.Model.Enums.License;

namespace CourseAsCode.Model.Activities.Resources;


public class MoodleFile : Activity
{
	[Description("name of the file that is to be included in the backup")]
	public string FileName { get; set; } = MoodleFileDefaults.DEFAULT_FILE_NAME;
	[Description("path to the file that is to be included in the backup")]
	public string FilePath { get; set; } = MoodleFileDefaults.DEFAULT_FILE_PATH;

	public SensibleDefaultsFile SensibleDefaults { get; set; } = new SensibleDefaultsFile();
	public SystemDefaultsFile SystemDefaults { get; set; } = new SystemDefaultsFile();

	
	// Defaults
	public class SensibleDefaultsFile
	{
		[Description("user who uploaded the file")]
		public User? Author { get; set; } = MoodleFileDefaults.DEFAULT_AUTHOR;
		[Description("the license under which the file is distributed")]
		public License? License { get; set; } = MoodleFileDefaults.DEFAULT_LICENSE;
	}

	
	public class SystemDefaultsFile
	{
		[Description("SHA1 Hash of the file content")]
		public string? ContentHash { get; set; } = MoodleFileDefaults.DEFAULT_CONTENT_HASH;
		[Description("the date and time that the file was created")]
		public DateTime TimeCreated { get; set; } = MoodleFileDefaults.DEFAULT_TIME_CREATED;
		[Description("the date and time that the file was last modified")]
		public DateTime TimeModified { get; set; } = MoodleFileDefaults.DEFAULT_TIME_MODIFIED;
		[Description("refers to the subsystem or plugin responsible for the file")]
		public Component? Component { get; set; } = MoodleFileDefaults.DEFAULT_COMPONENT;
		[Description("Unknown Purpose (possibly WHERE the file is created: user's private files, course files, etc.)")]
		public FileArea? FileArea { get; set; } = MoodleFileDefaults.DEFAULT_FILE_AREA;
		[Description("the itemid is needed if the file area in question is not already uniquely identified by the contextid + component + filearea")]
		public int ItemId { get; set; } = MoodleFileDefaults.DEFAULT_ITEM_ID;
	}
}