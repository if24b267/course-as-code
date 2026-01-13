using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Choice;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Model.Grading;
using CourseAsCode.Model.Questions.Types.Description;
using CourseAsCode.Model.Questions.Types.MultipleChoice;
using CourseAsCode.Model.Questions.Types.ShortAnswer;
using CourseAsCode.Model.Questions.Types.TrueFalse;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers
{
    public class Writer : IWriter
    {
        // -- Services for writing the various aspects of the Moodle Course -- //
        private readonly IRootFilesWriterService _rootFilesWriterService;
        private readonly ICourseFilesWriterService _courseFilesWriterService;
        private readonly ISectionFilesWriterService _sectionFilesWriterService;
        private readonly IActivitiesWriterService _activitiesWriterService;

        // -- Additional Dependencies -- //
        private readonly ILogger<Writer> _logger;
        private readonly IFileService _fileService;

        public Writer(
            ILogger<Writer> logger,
            IRootFilesWriterService rootFilesWriterService,
            ICourseFilesWriterService courseFilesWriterService,
            ISectionFilesWriterService sectionFilesWriterService,
            IActivitiesWriterService activitiesWriterService,
            IFileService fileService)
        {
            // -- Services Assignment -- //
            _rootFilesWriterService = rootFilesWriterService ?? throw new ArgumentNullException(nameof(rootFilesWriterService));
            _courseFilesWriterService = courseFilesWriterService ?? throw new ArgumentNullException(nameof(courseFilesWriterService));
            _sectionFilesWriterService = sectionFilesWriterService ?? throw new ArgumentNullException(nameof(sectionFilesWriterService));
            _activitiesWriterService = activitiesWriterService ?? throw new ArgumentNullException(nameof(activitiesWriterService));

            // -- Additional Dependencies Assignment -- //
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        /// <summary>
        /// Writes the specified course to disk, creating the necessary folder structure and files.
        /// </summary>
        /// <param name="course">The course object containing data to be written.</param>
        /// <param name="outputPath">The directory path where the course data will be saved.</param>
        /// <returns>True if the course was successfully written; false in case of an error.</returns>
        public bool WriteCourse(Course course, string outputPath)
        {
            _logger.LogDebug($"Writer started for course '{course.FullName}' with ID {course.SystemDefaults.Id}");
            _logger.LogInformation($"The course will be saved at {outputPath}");

            PrepareCourseDefaults(course);

            try
            {
                EnsureFolderStructureExists(outputPath);

                _rootFilesWriterService.WriteRootFiles(course, outputPath);
                _courseFilesWriterService.WriteCourseFiles(course, outputPath);
                _sectionFilesWriterService.WriteSectionFiles(course, outputPath);
                _activitiesWriterService.WriteActivities(course, outputPath);

                _logger.LogInformation($"Course '{course.FullName}' successfully written to {outputPath}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Unexpected error while writing course '{course.FullName}'");
                return false;
            }
        }

        /// <summary>
        /// Ensures the required folder structure for the Moodle Backup exists by creating necessary directories under the specified output path.
        /// </summary>
        /// <param name="outputPath">The root directory where the folder structure will be created.</param>
        private void EnsureFolderStructureExists(string outputPath)
        {
            _fileService.CreateDirectory(outputPath);
            _fileService.CreateDirectory(Path.Combine(outputPath, "course"));
            _fileService.CreateDirectory(Path.Combine(outputPath, "sections"));
            _fileService.CreateDirectory(Path.Combine(outputPath, "activities"));
            _fileService.CreateDirectory(Path.Combine(outputPath, "files"));
        }

        /// <summary>
        /// Equip the course with default values for enrolled users and sections if they are not defined
        /// + injects 2 example Lessons (hardcoded) if none exist yet.
        /// </summary>
        /// <param name="course">The course object to populate with default values</param>
        private void PrepareCourseDefaults(Course course)
        {
            if (course.EnrolledUsers.Count == 0)
            {
                _logger.LogDebug("No enrolled users found, adding a default user.");
                course.EnrolledUsers = new List<User> { UserDefaults.GetDefaultUser() };
            }

            if (course.Sections.Count == 0)
            {
                _logger.LogInformation("No sections specified by user, adding default section");
                course.Sections = new List<Section> { SectionDefaults.GetDefaultSection() };
            }

            // --- Dummy Data LESSONS ---
            /*
            var section = course.Sections[0];
            section.Activities ??= new List<Activity>();

            if (section.Activities.Any(a => a is Lesson))
            {
                return;
            }

            section.Activities.Add(new Lesson
            {
                Name = "Example Lesson 1",
                ActivityId = 9001,
                ModuleId = 50,
                ModuleName = "lesson",
                CourseId = course.SystemDefaults.Id,
                Context = Context.Course,

                Intro = "<p>Example Lesson 1 Intro</p>",
                IntroFormat = FormattingFormat.HTML,
                Grade = 100,
                Practice = false,
                MaxAttempts = 3,
                Review = true,
                Retake = true,
                ProgressBar = true,
                DisplayLeft = true,

                Conditions = new Lesson.LessonConditions
                {
                    TimeSpentSeconds = 0,
                    Completed = 0,
                    GradeBetterThan = 0
                },

                Pages = new List<Lesson.LessonPage>
                {
                    new Lesson.LessonPage { PageId = 1, Title = "Page 1", Content = "<p>Welcome</p>" },
                    new Lesson.LessonPage { PageId = 2, Title = "Page 2", Content = "<p>Concepts</p>" },
                    new Lesson.LessonPage { PageId = 3, Title = "Page 3", Content = "<p>Example</p>" },
                    new Lesson.LessonPage { PageId = 4, Title = "Page 4", Content = "<p>Mini quiz placeholder</p>" },
                    new Lesson.LessonPage { PageId = 5, Title = "Page 5", Content = "<p>Summary</p>" },
                    new Lesson.LessonPage { PageId = 6, Title = "Page 6", Content = "<p>End</p>" },
                },
            });

            section.Activities.Add(new Lesson
            {
                Name = "Example Lesson 2",
                ActivityId = 9002,
                ModuleId = 51,
                ModuleName = "lesson",
                CourseId = course.SystemDefaults.Id,
                Context = Context.Course,

                Intro = "<p>Example Lesson 2 Intro</p>",
                IntroFormat = FormattingFormat.HTML,
                Grade = 50,
                Practice = false,
                MaxAttempts = 1,
                Review = false,
                Retake = false,
                ProgressBar = false,
                DisplayLeft = false,
                UsePassword = true,
                Password = "letmein",
                TimeLimit = 15 * 60,
                Available = DateTime.UtcNow.AddDays(-1),
                Deadline = DateTime.UtcNow.AddDays(7),

                Pages = new List<Lesson.LessonPage>
                {
                    new Lesson.LessonPage { PageId = 10, Title = "Page 1", Content = "<p>Instructions</p>" },
                    new Lesson.LessonPage { PageId = 11, Title = "Page 2", Content = "<p>Task 1</p>" },
                    new Lesson.LessonPage { PageId = 12, Title = "Page 3", Content = "<p>Task 2</p>" },
                    new Lesson.LessonPage { PageId = 13, Title = "Page 4", Content = "<p>Finish</p>" },
                },
            });

            // --- DUMMY PAGE DATA ---
            section.Activities ??= new List<Activity>();

            if (!section.Activities.Any(a => a is Page))
            {
                section.Activities.Add(new Page
                {
                    Name = "Example Page: Welcome",
                    ActivityId = 8001,
                    Context = Context.Course,

                    Intro = "<p>This is a short intro shown on the course page.</p>",
                    IntroFormat = FormattingFormat.HTML,

                    Content = "<h2>Welcome</h2><p>This is an example HTML page.</p>",
                    ContentFormat = FormattingFormat.HTML,

                    TimeModified = DateTime.UtcNow,

                    ModuleId = 200,
                    ModuleName = "page",
                });

                section.Activities.Add(new Page
                {
                    Name = "Example Page: Markdown Content",
                    ActivityId = 8002,
                    Context = Context.Course,

                    Intro = "This page uses Markdown.",
                    IntroFormat = FormattingFormat.PlainText,

                    Content = "## Heading\n- Item A\n- Item B\n",
                    ContentFormat = FormattingFormat.Markdown,

                    TimeModified = DateTime.UtcNow,

                    ModuleId = 201,
                    ModuleName = "page",
                });
            }

            // --- DUMMY CHOICE DATA ---
            if (!section.Activities.Any(a => a is Choice))
            {
                var now = DateTime.UtcNow;

                section.Activities.Add(new Choice
                {
                    Name = "Example Choice",
                    ActivityId = 8201,
                    Context = Context.Course,
                    TimeCreated = now.AddMinutes(-5),
                    TimeModified = now,

                    Description = "<p>Please pick one topic.</p>",
                    DescriptionFormat = FormattingFormat.HTML,

                    Publish = false,
                    ShowResults = false,
                    Display = 0,
                    AllowUpdate = false,
                    AllowMultiple = false,
                    ShowUnanswered = false,
                    LimitAnswers = false,
                    TimeOpen = null,
                    TimeClose = null,
                    CompletionSubmit = false,
                    ShowPreview = false,
                    IncludeInactive = false,
                    ShowAvailable = false,
                    Options = new List<Choice.ChoiceOption>
                    {
                        new Choice.ChoiceOption { Id = 1, Text = "Topic A", MaxAnswers = 0, TimeModified = now },
                        new Choice.ChoiceOption { Id = 2, Text = "Topic B", MaxAnswers = 0, TimeModified = now },
                        new Choice.ChoiceOption { Id = 3, Text = "Topic C", MaxAnswers = 0, TimeModified = now },

                    },
                });
            }
            // --- DUMMY FEEDBACK DATA ---
            if (!section.Activities.Any(a => a is Feedback))
            {
                var now = DateTime.UtcNow;

                section.Activities.Add(new Feedback
                {
                    Name = "Example Feedback",
                    ActivityId = 8301,
                    Context = Context.Course,
                    TimeCreated = now.AddMinutes(-10),
                    TimeModified = now,

                    Description = "<p>Please fill out this self-assessment.</p>",
                    DescriptionFormat = FormattingFormat.HTML,

                    Anonymous = 2,
                    EmailNotification = 0,
                    MultipleSubmit = 0,
                    Autonumbering = 0,
                    PublishStats = 1,
                    CompletionSubmit = false,

                    // items
                    Items = new List<Feedback.FeedbackItem>
                    {
                        new Feedback.FeedbackItem
                        {
                            Id = 1,
                            Template = 0,
                            Name = "label",
                            Label = "",
                            Type = "label",
                            Presentation = "<p>Legend: (1) low … (4) high</p>",
                            HasValue = 0,
                            Position = 1,
                            Required = 0,
                            DependItem = 0,
                            DependValue = "",
                            Options = ""
                        },

                        new Feedback.FeedbackItem
                        {
                            Id = 2,
                            Template = 0,
                            Name = "I can describe the basics",
                            Label = "",
                            Type = "multichoice",
                            Presentation = "r>>>>>1|2|3|4<<<<<1",
                            HasValue = 1,
                            Position = 2,
                            Required = 0,
                            DependItem = 0,
                            DependValue = "",
                            Options = "h"
                        },
                    },
                });
            }

            // --- DUMMY FOLDER DATA ---
            if (!section.Activities.Any(a => a is Folder))
            {
                var now = DateTime.UtcNow;

                var folder = new Folder
                {
                    Name = "Example Folder",
                    ActivityId = 8401,
                    Context = Context.Course,

                    TimeCreated = now,
                    TimeModified = now,
                    VisibleToStudents = true,
                    DisplayDescription = false,
                };

                folder.Files.Add(new MoodleFile
                {
                    ActivityId = 8501,
                    Name = "test.txt",
                    FileName = "test.txt",

                    //Pfusch lösung ich weiß
                    FilePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Documentation", "SampleFiles", "test.txt")),

                    TimeCreated = now,
                    TimeModified = now,

                    SystemDefaults =
                    {
                        Component = Component.Folder,
                        FileArea = FileArea.CourseContent,
                        ItemId = 0
                    },
                });

                section.Activities.Add(folder);
            }*/
        }
    }
}
