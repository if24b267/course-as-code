namespace CourseAsCode.Model.Activities.Forum;

public class Forum : Activity
{
	// enum?
	public string Type { get; set; }
	public string OpenDate { get; set; }
	public string CloseDate { get; set; }
	public List<ForumPost> Posts { get; set; }
}