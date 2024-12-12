using CourseAsCode.Model;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;

namespace CourseAsCode.Readers;

public class CourseReader : ICourseReader
{
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
		return null;
	}
}