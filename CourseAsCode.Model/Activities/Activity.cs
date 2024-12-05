using System.ComponentModel;

namespace CourseAsCode.Model.Activities;

public abstract class Activity
{
	[Description("unique identifier for the activity (quiz, file, etc.) - used for referencing")]
	public int Id { get; set; }
	[Description("name of the activity (quiz, file, etc.) as it will be displayed to the user")]
	public string Name { get; set; }
	public string Description { get; set; }
}