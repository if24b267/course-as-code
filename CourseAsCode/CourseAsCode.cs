using System.IO.Compression;
using CourseAsCode.Readers;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Uploader;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace CourseAsCode
{
    internal class CourseAsCode
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string?[] args)
        {
            _logger.Debug("Starting CourseAsCode...");
            string? inputPath = null;
            string? outputPath = null;
            var courseFormat = CourseFormat.YAML; // default
            var showHelp = false;
            DeployConfig? deployConfig = null; // null = no deployment

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
                    case "--deploy":
                        if (i + 1 < args.Length)
                        {
                            _logger.Debug($"Parsing value for --deploy: {args[i + 1]}");
                            deployConfig = new DeployConfig { Target = args[++i] };
                        }
                        else
                        {
                            _logger.Debug("Error: Missing value for --deploy (Moodle Docker container)!");
                            Console.WriteLine("Error: Missing value for --deploy (Moodle Docker container)!");
                            return;
                        }

                        break;
                    case "-u":
                    case "--upload":
                        // Shorthand for --deploy with automatic container detection
                        _logger.Debug("Upload to Moodle requested");
                        deployConfig = new DeployConfig();
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

            inputPath = ResolvePath(inputPath);
            outputPath = ResolvePath(outputPath);

            if (inputPath == null || outputPath == null)
            {
                return;
            }

            _logger.Debug($"Resolved input path: {inputPath}");
            _logger.Debug($"Resolved output path: {outputPath}");
            _logger.Debug($"Resolved format: {courseFormat}");

            // Setup dependency injection
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog();
            });

            CourseReader.ConfigureYamlServices(serviceCollection);
            Writers.XML.Logic.DependencyService.ConfigureXmlServices(serviceCollection);
            serviceCollection.AddSingleton<IDeployer, MoodleDeployer>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var courseReader = serviceProvider.GetRequiredService<ICourseReader>();
            var course = courseReader.Read(inputPath, courseFormat);

            var courseWriter = serviceProvider.GetRequiredService<IWriter>();
            var courseOutputPath = Path.GetFullPath(Path.Combine(outputPath, "moodleCourse"));

            if (Directory.Exists(courseOutputPath))
            {
                _logger.Debug($"Wiping existing moodleCourse directory: {courseOutputPath}");
                Directory.Delete(courseOutputPath, true);
            }

            var success = courseWriter.WriteCourse(course, courseOutputPath);

            if (!success)
            {
                Console.WriteLine("Error: Writing course failed.");
                return;
            }

            IGeneralHelper _writerGeneralHelper = serviceProvider.GetRequiredService<IGeneralHelper>();
            var mbzFilePath = Path.GetFullPath(Path.Combine(outputPath, _writerGeneralHelper.GenerateBackupName(course)));

            if (File.Exists(mbzFilePath))
            {
                _logger.Debug($"Deleting existing MBZ file: {mbzFilePath}");
                File.Delete(mbzFilePath);
            }

            _logger.Debug($"Creating MBZ file: {mbzFilePath}");
            ZipFile.CreateFromDirectory(courseOutputPath, mbzFilePath);
            Console.WriteLine("DONE!");

            if (deployConfig != null)
            {
                Console.WriteLine($"Restoring MBZ file to Moodle: {mbzFilePath}");
                var deployer = serviceProvider.GetRequiredService<IDeployer>();
                var restoreResult = deployer.DeployAsync(mbzFilePath, deployConfig).GetAwaiter().GetResult();
                if (restoreResult.Success)
                {
                    Console.WriteLine($"Moodle course restore succeeded! Restored Course ID: {restoreResult.CourseId?.ToString() ?? "Unknown"}");
                }
                else
                {
                    Console.WriteLine($"Error: Moodle course restore failed: {restoreResult.ErrorMessage}");
                }
            }
        }

        private static string ResolvePath(string path)
        {
            var absolutePath = Path.GetFullPath(path);
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }

            return absolutePath;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  -i <Input Path>       - Required: The input directory path (relative or absolute).");
            Console.WriteLine("  -o <Output Path>      - Required: The output folder path (relative or absolute).");
            Console.WriteLine("  -f <Format>           - Optional: The input format (currently only YAML is supported).");
            Console.WriteLine("  --deploy <Container>  - Optional: Restores the generated .mbz into the given Moodle Docker container.");
            Console.WriteLine("  -u or --upload        - Optional: Like --deploy, but detects the Moodle container automatically.");
            Console.WriteLine("  -h or --help          - Optional: Displays this help message.");
        }
    }
}
