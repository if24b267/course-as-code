using System.ComponentModel.DataAnnotations;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model;

public class YamlCourse
{
	[Required]
	public string CourseName { get; set; }
	public string? ShortName { get; set; }
	public string Summary { get; set; }
	public string Description { get; set; }
	public bool Visible { get; set; }
	public string Category { get; set; }
	public int CategoryId { get; set; }
	public string CategoryDescription { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public int IdNumber { get; set; }
	public bool ShowGrades { get; set; }
	public GroupMode GroupMode { get; set; }
	public bool ForceGroupMode { get; set; }
	public bool EnableCompletionTracking { get; set; }
	public int NumberOfAnnouncements { get; set; }
	public string ForcedLanguage { get; set; }
	public bool ShowActivityDates { get; set; }
	public bool ShowCompletionConditions { get; set; }

}