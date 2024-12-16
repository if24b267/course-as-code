using CourseAsCode.Readers;
using CourseAsCode.Readers.Enums;

namespace CourseAsCode
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var courseReader = new CourseReader();
			var course = courseReader.Read(Path.Combine(Environment.CurrentDirectory, "../../../../Documentation/exampleCourse"), CourseFormat.YAML);
			Console.WriteLine("done");
		}
	}
}