using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
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

                _rootFilesWriterService.WriteRootFiles(course, outputPath); // Files in /
                _courseFilesWriterService.WriteCourseFiles(course, outputPath); // Files in /course
                _sectionFilesWriterService.WriteSectionFiles(course, outputPath); // Files in /sections
                _activitiesWriterService.WriteActivities(course, outputPath); // Files in /activities

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

            // --- ONLY EXAMPLE LESSONS ---
            var section = course.Sections[0];
            section.Activities ??= new List<Activity>();

            if (section.Activities.Any(a => a is Lesson))
            {
                return;
            }

            section.Activities.Add(new Lesson
            {
                // Required for folder naming & references
                Name = "Example Lesson 1",
                ActivityId = 9001,
                ModuleId = 50,
                ModuleName = "lesson",
                CourseId = course.SystemDefaults.Id,
                Context = Context.Course,

                // User relevant settings
                Intro = "<p>Example Lesson 1 Intro</p>",
                IntroFormat = FormattingFormat.HTML,
                Grade = 100,
                Practice = false,
                MaxAttempts = 3,
                Review = true,
                Retake = true,
                ProgressBar = true,
                DisplayLeft = true,

               
                Pages = new List<object>
                {
                    new { PageId = 1, Title = "Page 1", Content = "<p>Welcome</p>" },
                    new { PageId = 2, Title = "Page 2", Content = "<p>Concepts</p>" },
                    new { PageId = 3, Title = "Page 3", Content = "<p>Example</p>" },
                    new { PageId = 4, Title = "Page 4", Content = "<p>Mini quiz placeholder</p>" },
                    new { PageId = 5, Title = "Page 5", Content = "<p>Summary</p>" },
                    new { PageId = 6, Title = "Page 6", Content = "<p>End</p>" },
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

                Pages = new List<object>
                {
                    new { PageId = 10, Title = "Page 1", Content = "<p>Instructions</p>" },
                    new { PageId = 11, Title = "Page 2", Content = "<p>Task 1</p>" },
                    new { PageId = 12, Title = "Page 3", Content = "<p>Task 2</p>" },
                    new { PageId = 13, Title = "Page 4", Content = "<p>Finish</p>" },
                },
            });
        }
    }
}
