namespace CourseAsCode.Model.Resources;

public class File : Resource
{
	public int Size { get; set; }
	public string Path { get; set; }
	// maybe change this to file extension?
	public string Type { get; set; }
}