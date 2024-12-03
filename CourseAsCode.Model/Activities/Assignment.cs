namespace CourseAsCode.Model.Activities;

public class Assignment : Activity
{
	public string OpenDate { get; set; }
	public string DueDate { get; set; }
	// maybe this could also be an enum?
	public string SubmissionType { get; set; }
	public int Grade { get; set; }
}