using System.ComponentModel.DataAnnotations;
using CourseAsCode.Readers.Enums;

namespace CourseAsCode.Readers.YAML.Model;

public class YamlActivity
{
	[Required]
	public ActivityType Type { get; set; }
	[Required]
	public string Name { get; set; }
	public string Description { get; set; }
	public string URL { get; set; }
}