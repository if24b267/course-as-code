using System.Globalization;
using CourseAsCode.Model;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using YamlDotNet.Serialization;

namespace CourseAsCode.Readers;

public class CourseReader : ICourseReader
{
	private string _dateFormat = "dd-MM-yyyy HH:mm";

	public Course Read(string path, CourseFormat format)
	{
		var coursePath = Path.GetFullPath(path);
		if (Directory.Exists(coursePath))
		{
			switch (format)
			{
				case CourseFormat.YAML:
					return ReadYaml(coursePath);
				default:
					throw new NotSupportedException($"The format {format} is not supported.");
			}
		}
		else
		{
			throw new DirectoryNotFoundException($"The directory {coursePath} does not exist.");
		}

		return null;
	}

	public Course ReadYaml(string coursePath)
	{
		var rootNode = DirectoryReader.ReadDirectory(coursePath);
		Console.WriteLine("test");
		var yamlFile = rootNode.Children.FirstOrDefault(c => !c.IsDirectory && c.Name.EndsWith(".yaml"));
		var fileContent = File.ReadAllText(Path.Combine(coursePath, yamlFile.Name));
		var deserializer = new Deserializer();
		var x = (Dictionary<object, object>)deserializer.Deserialize(fileContent)!;

		x.TryGetValue("courseName", out var courseName);
		x.TryGetValue("courseCategory", out var courseCategory);
		x.TryGetValue("courseSummary", out var courseSummary);
		x.TryGetValue("startDate", out var startDate);
		x.TryGetValue("endDate", out var endDate);

		DateTime.TryParseExact(startDate as string, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartTime);
		DateTime.TryParseExact(endDate as string, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndTime);

		var course = new Course
		{
			FullName = courseName as string,
			ShortName = null,
			BackupMetadata = null,
			Summary = courseSummary as string,
			StartDate = parsedStartTime,
			EndDate = parsedEndTime
		};

		return null;
	}
}