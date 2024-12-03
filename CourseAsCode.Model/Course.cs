namespace CourseAsCode.Model
{
	public class Course
	{
		public int Id { get; set; }
		public string ShortName { get; set; }
		public string FullName { get; set; }
		public List<Section> Sections { get; set; }
	}
}
