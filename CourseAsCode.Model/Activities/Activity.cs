using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities;

public abstract class Activity
{
	[Description("unique identifier for the activity (quiz, file, etc.) - used for referencing")]
	public int Id { get; set; }
	[Description("name of the activity (quiz, file, etc.) as it will be displayed to the user")]
	public required string Name { get; set; }
	public string? Description { get; set; }
	[Description("which group mode to use for the activity")]
	public GroupMode GroupMode { get; set; }
	
	[Description("list of tags that are used to describe the activity")]
	public List<string>? Tags { get; set; }
	public SensibleDefaultsActivity SensibleDefaults { get; set; }
	public SystemDefaultsActivity SystemDefaults { get; set; }

	// Defaults
	public class SensibleDefaultsActivity
	{
		[Description("whether the activity is visible to students or teachers only")]
		public bool VisibleToStudents { get; set; }
		[Description("whether the activity description is displayed on the course page")]
		public bool DisplayDescription { get; set; }
	}
	
	public class SystemDefaultsActivity
	{
		[Description("optional identifier for the activity in the course, used for grade calculation and custom reports")]
		public int? CustomId { get; set; }
		[Description("if set, the activity will be displayed in this language despite the user's preferences")]
		public Language Language { get; set; }
	}
}