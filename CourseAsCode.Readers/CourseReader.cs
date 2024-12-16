using System.Globalization;
using System.Text.Json;
using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NodeDeserializers;
using CourseFormat = CourseAsCode.Readers.Enums.CourseFormat;

namespace CourseAsCode.Readers;

public class CourseReader : ICourseReader
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	private static readonly string[] DATE_FORMATS = ["dd-MM-yyyy HH:mm"];
	private Course? _course;

	public Course? Read(string path, CourseFormat format)
	{
		_logger.Debug($"Starting Reading - Path: {path}, Format: {format}");
		var coursePath = Path.GetFullPath(path);
		if (Directory.Exists(coursePath))
		{
			switch (format)
			{
				case CourseFormat.YAML:
					return ReadYaml(coursePath);
				default:
					_logger.Error($"The format {format} is not supported.");
					return null;
			}
		}

		_logger.Error($"The directory {coursePath} does not exist.");

		return null;
	}

	public Course? ReadYaml(string coursePath)
	{
		_logger.Debug($"Reading YAML course from {coursePath}");

		// TODO: make directory reading better - maybe modify file node
		var rootNode = DirectoryReader.ReadDirectory(coursePath);
		var yamlFile = rootNode.Children.FirstOrDefault(c => !c.IsDirectory && c.Name.EndsWith(".yaml"));
		var fileContent = File.ReadAllText(Path.Combine(coursePath, yamlFile.Name));
		var deserializer = new DeserializerBuilder()
			.WithTypeConverter(new DateTimeConverter(
				provider: CultureInfo.InvariantCulture,
				formats: DATE_FORMATS))
			.IgnoreUnmatchedProperties()
			.WithNodeDeserializer(i => new DeserializerValidation(i),
				s => s.InsteadOf<ObjectNodeDeserializer>())
			.Build();
		YAML.Model.Course yamlCourse = null;
		try
		{
			_logger.Debug($"Deserializing {yamlFile}");
			yamlCourse = deserializer.Deserialize<YAML.Model.Course>(fileContent);
		}
		catch (YamlException e)
		{
			_logger.Error($"Error deserializing {yamlFile}: {e.Message}");
			return null;
		}
		

		yamlCourse.ShortName ??= GenerateShortName(yamlCourse.CourseName);

		// TODO: is this really needed - should be optional?
		yamlCourse.ForcedLanguage = yamlCourse.ForcedLanguage switch
		{
			"en" => "English",
			"de" => "German",
			_ => "English"
		};

		Enum.TryParse(yamlCourse.ForcedLanguage, true, out Language courseLanguage);

		_course = new Course
		{
			FullName = yamlCourse.CourseName,
			ShortName = yamlCourse.ShortName,
			StartDate = yamlCourse.StartDate,
			EndDate = yamlCourse.EndDate,
			Summary = yamlCourse.Summary,
			GroupMode = yamlCourse.GroupMode,
			// TODO: maybe add a helper class where you can get the default SensibleDefaults class
			SensibleDefaults = new Course.SensibleDefaultsCourse
			{
				CourseIdNumber = yamlCourse.IdNumber,
				VisibleToStudents = yamlCourse.Visible,
				CategoryName = yamlCourse.Category,
				CategoryDescription = yamlCourse.CategoryDescription,
				ShowGrades = yamlCourse.ShowGrades,
				ForceGroupMode = yamlCourse.ForceGroupMode,
				EnableCompletionTracking = yamlCourse.EnableCompletionTracking,
				NumberOfAnnouncementsToDisplay = yamlCourse.NumberOfAnnouncements,
				ShowActivityDates = yamlCourse.ShowActivityDates,
				ShowActivityCompletionConditions = yamlCourse.ShowCompletionConditions
			},
			SystemDefaults = new Course.SystemDefaultsCourse
			{
				CourseLanguage = courseLanguage
			}
		};

		_logger.Debug($"Final course object: {JsonSerializer.Serialize(_course)}");
		return _course;
	}

	private static string GenerateShortName(string fullName)
	{
		_logger.Debug($"Shortname not set - generating one...");
		var objRandom = new Random();
		var intValue = objRandom.Next(10000, 99999);
		var uppercaseLetters = new string(fullName.Where(char.IsUpper).ToArray());
		var shortName = $"{uppercaseLetters}{intValue}";
		_logger.Debug($"Generated Shortname: {shortName}");
		return shortName;
	}
}