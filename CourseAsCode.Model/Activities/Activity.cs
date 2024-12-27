using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities;

public abstract class Activity
{
	[Description("name of the activity (quiz, file, etc.) as it will be displayed to the user")]
	public required string Name { get; set; } = ActivityDefaults.DEFAULT_NAME;
	public string? Description { get; set; } = ActivityDefaults.DEFAULT_DESCRIPTION;
	[Description("which group mode to use for the activity")]
	public GroupMode GroupMode { get; set; } = ActivityDefaults.DEFAULT_GROUP_MODE;
	[Description("list of tags that are used to describe the activity")]
	public List<string>? Tags { get; set; } = ActivityDefaults.DEFAULT_TAGS;
	
	public SensibleDefaultsActivity SensibleDefaults { get; set; } = new SensibleDefaultsActivity();
	public SystemDefaultsActivity SystemDefaults { get; set; } = new SystemDefaultsActivity();

	
	// Defaults
	public class SensibleDefaultsActivity
	{
		[Description("whether the activity is visible to students or teachers only")]
		public bool VisibleToStudents { get; set; } = ActivityDefaults.DEFAULT_VISIBLE_TO_STUDENTS;
		[Description("whether the activity description is displayed on the course page")]
		public bool DisplayDescription { get; set; } = ActivityDefaults.DEFAULT_DISPLAY_DESCRIPTION;
	}
	
	
	public class SystemDefaultsActivity
	{
		[Description("unique identifier for the activity (quiz, file, etc.) - used for referencing")]
		public int ActivityId { get; set; } = ActivityDefaults.DEFAULT_ACTIVITY_ID;
		[Description("optional identifier for the activity in the course, used for grade calculation and custom reports")]
		public int? CustomId { get; set; } = ActivityDefaults.DEFAULT_CUSTOM_ID;
		[Description("if set, the activity will be displayed in this language despite the user's preferences")]
		public Language? Language { get; set; } = ActivityDefaults.DEFAULT_LANGUAGE;
	}
}