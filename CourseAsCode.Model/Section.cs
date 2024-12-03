namespace CourseAsCode.Model;

public class Section
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public List<Resource> Resources
	{
		get; set;
	}
}