using System.Text.Json;

using CourseAsCode.Model;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Model.Defaults.Grading;
using CourseAsCode.Model.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using CourseAsCode.Readers.YAML.Readers;
using Microsoft.Extensions.DependencyInjection;

using CourseFormat = CourseAsCode.Readers.Enums.CourseFormat;
using IQuizReader = CourseAsCode.Readers.Interfaces.IQuizReader;

namespace CourseAsCode.Readers
{
    // TODO: rework the way we read the files - especially with activities where we have things like descriptions which *can* be set but should use the default otherwise
    public class CourseReader : ICourseReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private Course? _course;
        private readonly ISectionReader _sectionReader;
        private readonly IServiceProvider _serviceProvider;
        private readonly IYamlFileReaderHelper _yamlReader;
        private readonly IQuestionReader _questionReader;
        private readonly IGradeCategoriesReader _gradeCategoriesReader;

        public CourseReader(ISectionReader sectionReader, IServiceProvider serviceProvider, IYamlFileReaderHelper yamlReader, IQuestionReader questionReader, IGradeCategoriesReader iGradeCategoriesReader)
        {
            _sectionReader = sectionReader;
            _serviceProvider = serviceProvider;
            _yamlReader = yamlReader;
            _questionReader = questionReader;
            _gradeCategoriesReader = iGradeCategoriesReader;
        }

        // For backward compatibility
        public CourseReader(ISectionReader sectionReader, IServiceProvider serviceProvider)
            : this(sectionReader, serviceProvider, new YamlFileReaderHelper(), new YamlQuestionReader(new YamlFileReaderHelper()), new YamlIGradeCategoriesReader())
        {
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

            if (courseSectionsYamlFile == null || courseConfigYamlFile == null)
            {
                _logger.Error($"Course is missing a config.yaml or sections.yaml file.");
                throw new FileNotFoundException("Course is missing a config.yaml or sections.yaml file.");
            }


            var yamlYamlCourse = _yamlReader.DeserializeFromFile<YamlCourse>(Path.Combine(coursePath, courseConfigYamlFile.Name));
            var yamlSectionsDefinition = _yamlReader.DeserializeFromFile<YamlSectionsDefinition>(Path.Combine(coursePath, courseSectionsYamlFile.Name));

            var gradeCategories = _gradeCategoriesReader.Read(Path.Combine(coursePath, yamlYamlCourse.GradeGatoriesPath));

            List<Section> sections = [];
            ValueKeeper.Questions = [];
            ValueKeeper.CurrentActivityId = 1;
            ValueKeeper.CurrentSectionId = 1;

            if (yamlYamlCourse.QuestionListPaths != null)
            {
                foreach (var questionListPath in yamlYamlCourse.QuestionListPaths)
                {
                    var questionPath = Path.GetFullPath(Path.Combine(coursePath, questionListPath));
                    var moodleQuestionList = _questionReader.Read(questionPath);
                    ValueKeeper.Questions.AddRange(moodleQuestionList);
                }
            }

            foreach (var section in yamlSectionsDefinition.Sections)
            {
                var sectionRead = _sectionReader.Read(Path.GetFullPath(Path.Combine(coursePath, section.SectionPath, "section.yaml")));

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
                _ => "English",
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
                Questions = ValueKeeper.Questions,
                GradeCategories = gradeCategories ?? [],

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
                    ShowActivityCompletionConditions = yamlYamlCourse.ShowCompletionConditions,
                },
                SystemDefaults = new Course.SystemDefaultsCourse
                {
                },
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

        public static void ConfigureYamlServices(IServiceCollection services)
        {
            // Register helpers
            services.AddSingleton<IYamlFileReaderHelper, YamlFileReaderHelper>();
            services.AddSingleton<DirectoryReader>();

            // Register readers with interfaces
            services.AddSingleton<IFileReader, YamlFileReader>();
            services.AddSingleton<IQuizReader, YamlQuizReader>();
            services.AddSingleton<ITextAndMediaAreaReader, YamlTextMediaAreaReader>();
            services.AddSingleton<IUrlReader, YamlUrlReader>();
            services.AddSingleton<IActivityReader, YamlActivityReader>();
            services.AddSingleton<ISectionReader, YamlSectionReader>();
            services.AddSingleton<IQuestionReader, YamlQuestionReader>();
            services.AddSingleton<IAssignmentReader, YamlAssignmentReader>();
            services.AddSingleton<ICourseReader, CourseReader>();
            services.AddSingleton<IWikiReader, YamlWikiReader>();
            services.AddSingleton<IWikiPageReader, YamlWikiPageReader>();
            services.AddSingleton<IGradeCategoriesReader, YamlIGradeCategoriesReader>();
        }
    }
}
