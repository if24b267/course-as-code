namespace CourseAsCode.Readers.YAML.Model;

public class Course
{
	public string CourseName { get; set; }
	public string? ShortName { get; set; }
	public string Summary { get; set; }
	public string Description { get; set; }
	public bool Visible { get; set; }
	public string Category { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public int IdNumber { get; set; }

}