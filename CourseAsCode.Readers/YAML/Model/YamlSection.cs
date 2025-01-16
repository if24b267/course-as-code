using CourseAsCode.Model.Defaults;
using System.ComponentModel.DataAnnotations;

namespace CourseAsCode.Readers.YAML.Model;

public class YamlSection
{
	[Required]
	public string SectionName { get; set; }

	public bool VisibleToStudents { get; set; } = SectionDefaults.DEFAULT_VISIBLE_TO_STUDENTS;

	public string Summary { get; set; }

	List<YamlActivity> Activities { get; set; }
}