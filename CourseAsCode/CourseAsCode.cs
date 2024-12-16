using System.IO.Compression;
using CourseAsCode.Readers;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Writers;

namespace CourseAsCode
{
	internal class CourseAsCode
	{
		static void Main(string?[] args)
		{
			string? inputPath = null;
			string? outputPath = null;
			var courseFormat = CourseFormat.YAML; // default
			var showHelp = false;

			

			for (var i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "-i":
						if (i + 1 < args.Length)
						{
							inputPath = args[++i];
						}
						else
						{
							Console.WriteLine("Error: Missing value for -i (input folder)!");
							return;
						}
						break;
					case "-o":
						if (i + 1 < args.Length)
						{
							outputPath = args[++i];
						}
						else
						{
							Console.WriteLine("Error: Missing value for -o (output folder)!");
							return;
						}
						break;
					case "-f":
						if (i + 1 < args.Length)
						{
							var formatArg = args[++i].ToUpper();
							if (Enum.TryParse(formatArg, out CourseFormat parsedCourseFormat))
							{
								courseFormat = parsedCourseFormat;
							}
							else
							{
								Console.WriteLine("Error: Invalid value for -f (Format). Supported formats: YAML.");
								return;
							}
						}
						break;
					case "-h":
					case "--help":
						showHelp = true;
						break;
					default:
						Console.WriteLine($"Error: Unknown argument: {args[i]}. Use -h for usage information.");
						return;
				}
			}

			if (showHelp || string.IsNullOrEmpty(inputPath) || string.IsNullOrEmpty(outputPath))
			{
				PrintHelp();
				return;
			}

			inputPath = ResolvePath(inputPath, "input");
			outputPath = ResolvePath(outputPath, "output");

			if (inputPath == null || outputPath == null)
			{
				return;
			}

			Console.WriteLine($"Input Path: {inputPath}");
			Console.WriteLine($"Output Path: {outputPath}");
			Console.WriteLine($"Format: {courseFormat}");

			var courseReader = new CourseReader();
			var course = courseReader.Read(inputPath, courseFormat);
			var writer = new Writer();
			var courseOutputPath = Path.GetFullPath(Path.Combine(outputPath, "moodleCourse"));
			var success = writer.WriteXML(course, courseOutputPath);
			var mbzFilePath = Path.GetFullPath(Path.Combine(outputPath, "backup.mbz"));
			ZipFile.CreateFromDirectory(outputPath, Path.Combine(mbzFilePath, "backup.mbz"));
			//Console.WriteLine("done");
		}

		private static string? ResolvePath(string path, string argName)
		{
			var absolutePath = Path.GetFullPath(path);
			if (Directory.Exists(absolutePath)) return absolutePath;
			Console.WriteLine($"Error: Invalid {argName} path '{path}'. Path does not exist.");
			return null;
		}

		private static void PrintHelp()
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("  -i <Input Path>     - Required: The input directory path (relative or absolute).");
			Console.WriteLine("  -o <Output Path>    - Required: The output folder path (relative or absolute).");
			Console.WriteLine("  -f <Format>         - Optional: The input format (currently only YAML is supported).");
			Console.WriteLine("  -h or --help        - Optional: Displays this help message.");
		}
	}
}