using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.AbstractClasses;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers;

public class YamlTextMediaAreaReader : TextMediaAreaReader<TextAndMediaArea>
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

	public override TextAndMediaArea? Read(string modelObjectPath)
	{
		if (!File.Exists(modelObjectPath))
		{
			_logger.Error($"File {modelObjectPath} does not exist.");
			return null;
		}

		var yamlFile = YamlFileReaderHelper.DeserializeFromFile<YamlTextMediaArea>(modelObjectPath);

		var finalContent = "";

		foreach (var content in yamlFile.Content)
		{
			switch (content.Type)
			{
				case TextMediaContentType.MARKDOWN:
					finalContent += ReadMarkdownFile(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(modelObjectPath), content.ContentFilePath)));
					break;
			}
		}

		return ConvertYamlTextMediaAreaAndContentToMoodleTextMediaArea(yamlFile, finalContent);
	}

	private TextAndMediaArea? ConvertYamlTextMediaAreaAndContentToMoodleTextMediaArea(YamlTextMediaArea yamlTextMediaArea, string content)
	{
		return new TextAndMediaArea()
		{
			Name = yamlTextMediaArea.Name,
			Content = content
		};
	}

	private string? ReadMarkdownFile(string markdownFilePath)
	{
		if (!File.Exists(markdownFilePath))
		{
			_logger.Error($"File {markdownFilePath} does not exist.");
			return null;
		}

		return File.ReadAllText(markdownFilePath);
	}
}