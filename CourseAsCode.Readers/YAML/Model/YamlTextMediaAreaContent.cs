using CourseAsCode.Readers.Enums;

namespace CourseAsCode.Readers.YAML.Model;

public class YamlTextMediaAreaContent
{
	public TextMediaContentType Type { get; set; }
	public string ContentFilePath { get; set; }
}