using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.AbstractClasses;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers;

public class YamlFileReader : FileReader<MoodleFile>
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

	public override MoodleFile? Read(string modelObjectPath)
	{
		if (!File.Exists(modelObjectPath))
		{
			_logger.Error($"File {modelObjectPath} does not exist.");
			return null;
		}


		var yamlFile = YamlFileReaderHelper.DeserializeFromFile<YamlFile>(modelObjectPath);



		var fullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(modelObjectPath), yamlFile.FilePath));
		yamlFile.FilePath = fullPath;

		if (!File.Exists(yamlFile.FilePath))
		{
			_logger.Error($"File {yamlFile.FilePath} does not exist.");
			return null;
		}

		var moodleFile = ConvertYamlFileToMoodleFile(yamlFile);
		return moodleFile;
	}

	private MoodleFile ConvertYamlFileToMoodleFile(YamlFile yamlFile)
	{
		return new MoodleFile()
		{
			ActivityId = ValueKeeper.CurrentActivityId,
			Name = yamlFile.Name,
			Description = yamlFile.Description,
			FilePath = yamlFile.FilePath,
			FileName = Path.GetFileName(yamlFile.FilePath)
		};
	}
}