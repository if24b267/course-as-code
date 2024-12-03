namespace CourseAsCode.Model.Activities.Forum;

public class ForumPost
{
	// TODO: don't we need a title here too?
	public string Author { get; set; }
	public DateTime PostDate { get; set; }
	public string Content { get; set; }
	public List<ForumComment> Comments { get; set; }
}