using System.IO.Compression;
using CourseAsCode.Readers;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Writers;

namespace CourseAsCode
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var courseReader = new CourseReader();
			var course = courseReader.Read(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Documentation\exampleCourse")), CourseFormat.YAML);
			var writer = new Writer();
			var outputPath =
				Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Documentation\out\moodleCourseOut"));
			var success = writer.WriteXML(course, outputPath);
			var zipPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Documentation\out"));
			ZipFile.CreateFromDirectory(outputPath, Path.Combine(zipPath, "backup.mbz"));
			Console.WriteLine("done");
		}
	}
}