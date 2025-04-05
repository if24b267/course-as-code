using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.AbstractClasses;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers;

public class YAMLUrlReader : IUrlReader
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

	public URL? Read(string modelObjectPath)
	{
		// TODO: maybe make this into a middleware
		if (!File.Exists(modelObjectPath))
		{
			_logger.Error($"File {modelObjectPath} does not exist.");
			return null;
		}

		// TODO: this might error
		var yamlUrl = YamlFileReaderHelper.DeserializeFromFile<YamlUrl>(modelObjectPath);
		var modelUrl = ConvertYamlUrlToModelUrl(yamlUrl);
		return modelUrl;
	}

	private URL? ConvertYamlUrlToModelUrl(YamlUrl yamlUrl)
	{
		// TODO: default values?
		return new URL()
		{
			ActivityId = ValueKeeper.CurrentActivityId,
			Name = yamlUrl.Name,
			ExternalUrl = yamlUrl.URL,
			Description = yamlUrl.Description
		};
	}
}