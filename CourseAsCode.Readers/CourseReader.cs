using System.Globalization;
using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;

namespace CourseAsCode.Readers;

public class CourseReader : ICourseReader
{
	private static readonly string[] DATE_FORMATS = ["dd-MM-yyyy HH:mm"];

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
		var yamlFile = rootNode.Children.FirstOrDefault(c => !c.IsDirectory && c.Name.EndsWith(".yaml"));
		var fileContent = File.ReadAllText(Path.Combine(coursePath, yamlFile.Name));
		var deserializer = new DeserializerBuilder()
			.WithTypeConverter(new DateTimeConverter(
				provider: CultureInfo.InvariantCulture,
				formats: DATE_FORMATS))
			.Build();
		var yamlCourse = deserializer.Deserialize<YAML.Model.Course>(fileContent);

		yamlCourse.ShortName ??= GenerateShortName(yamlCourse.CourseName);

		//DateTime.TryParseExact(startDate as string, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartTime);
		//DateTime.TryParseExact(endDate as string, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndTime);

		return null;
	}

	private static string GenerateShortName(string fullName)
	{
		var objRandom = new Random();
		var intValue = objRandom.Next(10000, 99999);
		var uppercaseLetters = new string(fullName.Where(char.IsUpper).ToArray());
		var shortName = $"{uppercaseLetters}{intValue}";
		return shortName;
	}
}