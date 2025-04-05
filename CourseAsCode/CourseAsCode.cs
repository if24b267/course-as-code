using CourseAsCode.Readers;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Writers;
using NLog;
using System.IO.Compression;
using CourseAsCode.Writers.XML.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace CourseAsCode
{
	internal class CourseAsCode
	{
		private static readonly GeneralHelper WriterGeneralHelper = GeneralHelper.Instance;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		static void Main(string?[] args)
		{
			_logger.Debug("Starting CourseAsCode...");
			string? inputPath = null;
			string? outputPath = null;
			var courseFormat = CourseFormat.YAML; // default
			var showHelp = false;

			for (var i = 0; i < args.Length; i++)
			{
				_logger.Debug($"Parsing argument number {i} - {args[i]}");
				switch (args[i])
				{
					case "-i":
						if (i + 1 < args.Length)
						{
							_logger.Debug($"Parsing value for -i: {args[i + 1]}");
							inputPath = args[++i];
						}
						else
						{
							_logger.Debug("Error: Missing value for -i (input folder)!");
							Console.WriteLine("Error: Missing value for -i (input folder)!");
							return;
						}
						break;
					case "-o":
						if (i + 1 < args.Length)
						{
							_logger.Debug($"Parsing value for -o: {args[i + 1]}");
							outputPath = args[++i];
						}
						else
						{
							_logger.Debug("Error: Missing value for -o (output folder)!");
							Console.WriteLine("Error: Missing value for -o (output folder)!");
							return;
						}
						break;
					case "-f":
						if (i + 1 < args.Length)
						{
							var formatArg = args[++i]?.ToUpper();
							if (Enum.TryParse(formatArg, out CourseFormat parsedCourseFormat))
							{
								_logger.Debug($"Parsed the format {parsedCourseFormat}");
								courseFormat = parsedCourseFormat;
							}
							else
							{
								_logger.Debug($"Error: Invalid value for -f (Format). Supported formats: YAML.");
								Console.WriteLine("Error: Invalid value for -f (Format). Supported formats: YAML.");
								return;
							}
						}
						break;
					case "-h":
					case "--help":
						_logger.Debug("Help was requested");
						showHelp = true;
						break;
					default:
						_logger.Debug($"Error: Unknown argument: {args[i]}. Use -h for usage information.");
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

			_logger.Debug($"Resolved input path: {inputPath}");
			_logger.Debug($"Resolved output path: {outputPath}");
			_logger.Debug($"Resolved format: {courseFormat}");

			// Setup dependency injection
			var serviceCollection = new ServiceCollection();
			CourseReader.ConfigureYamlServices(serviceCollection);
			var serviceProvider = serviceCollection.BuildServiceProvider();

			var courseReader = serviceProvider.GetRequiredService<ICourseReader>();
			var course = courseReader.Read(inputPath, courseFormat);
			var writer = new Writer();
			var courseOutputPath = Path.GetFullPath(Path.Combine(outputPath, "moodleCourse"));
			var success = writer.WriteCourse(course, courseOutputPath);

			if (!success)
			{
				Console.WriteLine("Error: Writing course failed.");
				return;
			}

			var mbzFilePath = Path.GetFullPath(Path.Combine(outputPath, WriterGeneralHelper.GenerateBackupName(course)));

			if (File.Exists(mbzFilePath))
			{
				_logger.Debug($"Deleting existing MBZ file: {mbzFilePath}");
				File.Delete(mbzFilePath);
			}

			_logger.Debug($"Creating MBZ file: {mbzFilePath}");
			ZipFile.CreateFromDirectory(courseOutputPath, mbzFilePath);
			Console.WriteLine("DONE!");
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