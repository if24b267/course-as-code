using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using CourseAsCode.Readers.YAML.Readers;
using System.Text.Json;
using CourseFormat = CourseAsCode.Readers.Enums.CourseFormat;

namespace CourseAsCode.Readers;

// TODO: rework the way we read the files - especially with activities where we have things like descriptions which *can* be set but should use the default otherwise
public class CourseReader : ICourseReader
{
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	private Course? _course;
	private YAMLSectionReader _yamlSectionReader;

	// TODO: dependency injection with services
	public CourseReader(YAMLSectionReader yamlSectionReader)
	{
		_yamlSectionReader = yamlSectionReader;
	}

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
		ValueKeeper.CoursePath = coursePath;

		// TODO: make directory reading better - maybe modify file node
		var rootNode = DirectoryReader.ReadDirectory(coursePath);
		var courseConfigYamlFile = rootNode.Children.FirstOrDefault(c => c is { IsDirectory: false, Name: "config.yaml", Depth: 1 });
		var courseSectionsYamlFile = rootNode.Children.FirstOrDefault(c => c is { IsDirectory: false, Name: "sections.yaml", Depth: 1 });

		var yamlYamlCourse = YamlFileReaderHelper.DeserializeFromFile<YamlCourse>(Path.Combine(coursePath, courseConfigYamlFile.Name));
		var yamlSectionsDefinition = YamlFileReaderHelper.DeserializeFromFile<YamlSectionsDefinition>(Path.Combine(coursePath, courseSectionsYamlFile.Name));

		List<Section> sections = [];
		ValueKeeper.CurrentActivityId = 1;
		ValueKeeper.CurrentSectionId = 1;

		foreach (var section in yamlSectionsDefinition.Sections)
		{
			var sectionRead = _yamlSectionReader.Read(Path.GetFullPath(Path.Combine(coursePath, section.SectionPath, "section.yaml")));

			if (sectionRead != null)
			{
				sections.Add(sectionRead);
				ValueKeeper.CurrentSectionId++;
			}
		}

		yamlYamlCourse.ShortName ??= GenerateShortName(yamlYamlCourse.CourseName);

		// TODO: is this really needed - should be optional?
		yamlYamlCourse.ForcedLanguage = yamlYamlCourse.ForcedLanguage switch
		{
			"en" => "English",
			"de" => "German",
			_ => "English"
		};

		Enum.TryParse(yamlYamlCourse.ForcedLanguage, true, out Language courseLanguage);

		_course = new Course
		{
			FullName = yamlYamlCourse.CourseName,
			ShortName = yamlYamlCourse.ShortName,
			StartDate = yamlYamlCourse.StartDate,
			EndDate = yamlYamlCourse.EndDate,
			Summary = yamlYamlCourse.Summary,
			GroupMode = yamlYamlCourse.GroupMode,
			CourseLanguage = courseLanguage,
			Sections = sections,
			// TODO: maybe add a helper class where you can get the default SensibleDefaults class
			SensibleDefaults = new Course.SensibleDefaultsCourse
			{
				CourseIdNumber = yamlYamlCourse.IdNumber,
				VisibleToStudents = yamlYamlCourse.Visible,
				CategoryName = yamlYamlCourse.Category,
				CategoryDescription = yamlYamlCourse.CategoryDescription,
				ShowGrades = yamlYamlCourse.ShowGrades,
				ForceGroupMode = yamlYamlCourse.ForceGroupMode,
				EnableCompletionTracking = yamlYamlCourse.EnableCompletionTracking,
				NumberOfAnnouncementsToDisplay = yamlYamlCourse.NumberOfAnnouncements,
				ShowActivityDates = yamlYamlCourse.ShowActivityDates,
				ShowActivityCompletionConditions = yamlYamlCourse.ShowCompletionConditions
			},
			SystemDefaults = new Course.SystemDefaultsCourse
			{
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