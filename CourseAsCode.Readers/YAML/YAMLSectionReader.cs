using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Readers.AbstractClasses;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML;

public class YAMLSectionReader : SectionReader<Section>
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

	public override Section? Read(string modelObjectPath)
	{
		if (!File.Exists(modelObjectPath))
		{
			_logger.Error($"File {modelObjectPath} does not exist.");
			return null;
		}

		// TODO: this might error
		var yamlSection = YamlFileReaderHelper.DeserializeFromFile<YamlSection>(modelObjectPath);
		var modelSection = ConvertYamlSectionToSection(yamlSection);
		return modelSection;
	}

	private Section ConvertYamlSectionToSection(YamlSection yamlSection)
	{
		var courseSection = new Section
		{
			Name = yamlSection.SectionName,
			Summary = yamlSection.Summary,
			VisibleToStudents = yamlSection.VisibleToStudents
		};
		return courseSection;
	}
}