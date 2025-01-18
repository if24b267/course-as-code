using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.AbstractClasses;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers;

public class YamlActivityReader : ActivityReader<Activity>
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	private YamlFileReader _fileReader = new YamlFileReader();
	private YamlTextMediaAreaReader _textMediaAreaReader = new YamlTextMediaAreaReader();
	public override Activity? Read(string modelObjectPath)
	{
		if (!File.Exists(modelObjectPath))
		{
			_logger.Error($"File {modelObjectPath} does not exist.");
			return null;
		}

		// TODO: this might error
		var yamlActivity = YamlFileReaderHelper.DeserializeFromFile<YamlActivity>(modelObjectPath);

		if (yamlActivity == null)
		{
			_logger.Error($"Could not deserialize the file {modelObjectPath}.");
			return null;
		}

		switch (yamlActivity.Type)
		{
			// TODO: do we maybe want this in a different reader? -> problem: we dont follow the interface structure since we dont want to read the file twice
			case ActivityType.URL:
				return ConvertYamlActivityToMoodleUrl(yamlActivity);
			case ActivityType.FILE:
				return _fileReader.Read(modelObjectPath);
			case ActivityType.LABEL:
				return _textMediaAreaReader.Read(modelObjectPath);
		}


		return null;
	}

	private URL? ConvertYamlActivityToMoodleUrl(YamlActivity yamlActivity)
	{
		// TODO: improve activity reading -> every activity could for example have visible to students set
		// maybe a different class that reads the non specific parts of the activity
		return new URL()
		{
			Name = yamlActivity.Name,
			Description = yamlActivity.Description,
			ExternalUrl = yamlActivity.URL
		};
	}
}