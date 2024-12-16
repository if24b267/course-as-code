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
			var success = writer.WriteXML(course, Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\..\Documentation\out")));
			Console.WriteLine("done");
		}
	}
}