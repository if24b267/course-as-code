using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Readers.AbstractClasses;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers;

public class YAMLSectionReader : SectionReader<Section>
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	// TODO: dependency injection
	private YamlActivityReader _activityReader = new YamlActivityReader();

	public override Section? Read(string modelObjectPath)
	{
		if (!File.Exists(modelObjectPath))
		{
			_logger.Error($"File {modelObjectPath} does not exist.");
			return null;
		}

		// TODO: this might error
		var yamlSection = YamlFileReaderHelper.DeserializeFromFile<YamlSection>(modelObjectPath);
		List<Activity> activities = [];

		if (yamlSection.Activities != null && yamlSection.Activities.Any())
		{
			foreach (var yamlSectionActivity in yamlSection.Activities)
			{
				var tempPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(modelObjectPath), yamlSectionActivity.ActivityFilePath));
				var activity = _activityReader.Read(tempPath);
				activities.Add(activity);
				ValueKeeper.CurrentActivityId++;
			}
		}

		var modelSection = ConvertYamlSectionToSection(yamlSection, activities);
		return modelSection;
	}

	private Section ConvertYamlSectionToSection(YamlSection yamlSection, List<Activity> activities = null)
	{
		var courseSection = new Section
		{
			Name = yamlSection.SectionName,
			Summary = yamlSection.Summary,
			VisibleToStudents = yamlSection.VisibleToStudents,
			Activities = activities
		};
		courseSection.SystemDefaults.SectionId = ValueKeeper.CurrentSectionId;
		courseSection.SystemDefaults.PathToSection = $"sections/section_{ValueKeeper.CurrentSectionId}";
		return courseSection;
	}
}