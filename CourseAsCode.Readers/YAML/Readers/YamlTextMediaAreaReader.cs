using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers;

public class YamlTextMediaAreaReader : ITextAndMediaAreaReader
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	private readonly IYamlFileReaderHelper _yamlFileReaderHelper;
	
	public YamlTextMediaAreaReader(IYamlFileReaderHelper yamlFileReaderHelper)
	{
		_yamlFileReaderHelper = yamlFileReaderHelper;
	}
	
	// For backward compatibility
	public YamlTextMediaAreaReader() : this(new YamlFileReaderHelper())
	{
	}

	public TextAndMediaArea? Read(string modelObjectPath)
	{
		if (!File.Exists(modelObjectPath))
		{
			_logger.Error($"File {modelObjectPath} does not exist.");
			return null;
		}

		var yamlFile = _yamlFileReaderHelper.DeserializeFromFile<YamlTextMediaArea>(modelObjectPath);

		var finalContent = "";

		foreach (var content in yamlFile.Content)
		{
			// TODO: implement more types
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
		var format = yamlTextMediaArea.Format switch
		{
			TextMediaFieldFormat.AUTO => FormattingFormat.MoodleAutoFormat,
			TextMediaFieldFormat.HTML => FormattingFormat.HTML,
			TextMediaFieldFormat.PLAIN => FormattingFormat.PlainText,
			TextMediaFieldFormat.MARKDOWN => FormattingFormat.Markdown,
			_ => FormattingFormat.MoodleAutoFormat
		};

		return new TextAndMediaArea()
		{
			FormattingFormat = format,
			ActivityId = ValueKeeper.CurrentActivityId,
			Name = yamlTextMediaArea.Name,
			Content = content
		};
	}

	private string? ReadMarkdownFile(string markdownFilePath)
	{
		_logger.Debug($"Reading markdown file {markdownFilePath}");
		if (!File.Exists(markdownFilePath))
		{
			_logger.Error($"File {markdownFilePath} does not exist.");
			return null;
		}

		return File.ReadAllText(markdownFilePath);
	}
}