using CourseAsCode.Model;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Model.Grading;
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

                // --- Uncomment the following lines to test the grading functionality | Only works with exampleCourse --- //
                AddGradingCategories(course);
                ModifyQuizzesWithGradingInfo(course);
                ModifyAssignmentWithGradingInfo(course);

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
        }


        /// <summary>
        /// Temporary method to add grading categories to the course | can be removed once grading is implemented in the Readers
        /// </summary>
        /// <param name="course">The course to which grading categories will be added</param>
        private void AddGradingCategories(Course course)
        {
            course.GradeCategories.Add(new GradeCategory() {
                GradeCategoryId = 1,
                GradeName = "Course Overall",
                AggregationType = GradeAggregation.Natural,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                MaxGrade = 30,
                GradeToPass = 15,
            });

            course.GradeCategories.Add(new GradeCategory()
            {
                GradeCategoryId = 2,
                GradeName = "Theoretical Part",
                Parent = course.GradeCategories[0],
                AggregationType = GradeAggregation.MeanOfGrades,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                MaxGrade = 10,
                GradeToPass = 5,
                GradingWeight = 30,
            });

            course.GradeCategories.Add(new GradeCategory()
            {
                GradeCategoryId = 3,
                GradeName = "Practical Part",
                Parent = course.GradeCategories[0],
                AggregationType = GradeAggregation.Natural,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                MaxGrade = 70,
                GradeToPass = 15,
                DropLowestGrades = 1,
                GradingWeight = 70,
                GradeHidden = true,
                GradingLocked = true,
            });
        }


        /// <summary>
        /// Temporary method to modify the grading information of the quizzes in the course | can be removed once grading is implemented in the Readers
        /// </summary>
        /// <param name="course">The course containing the quizzes to be modified</param>
        private void ModifyQuizzesWithGradingInfo(Course course)
        {
            // Get activity with ID 14
            var quiz1 = course.Sections[2].Activities
                .OfType<Quiz>()
                .FirstOrDefault(a => a.ActivityId == 14);

            // Get activity with ID 16
            var quiz2 = course.Sections[3].Activities
                .OfType<Quiz>()
                .FirstOrDefault(a => a.ActivityId == 16);

            // Get activity with ID 18
            var quiz3 = course.Sections[3].Activities
                .OfType<Quiz>()
                .FirstOrDefault(a => a.ActivityId == 18);

            quiz1.GradeCategory = course.GradeCategories[1];
            quiz1.GradingLocked = true;
            quiz1.GradeName = "Different from the Activities name";
            quiz1.GradeMultiplicator = 2.0f;

            quiz2.GradeCategory = course.GradeCategories[2];
            quiz2.GradingWeight = 15;

            quiz3.GradeCategory = course.GradeCategories[2];
            quiz3.GradingWeight = 35;
            quiz3.GradeToPass = 10;
            quiz3.GradeHiddenUntil = DateTime.Now.AddDays(10);
        }


        /// <summary>
        /// Temporary method to modify the grading information of the assignments in the course | can be removed once grading is implemented in the Readers
        /// </summary>
        /// <param name="course">The course containing the assignments to be modified</param>
        private void ModifyAssignmentWithGradingInfo(Course course)
        {
            // Get activity with ID 10
            var assignment1 = course.Sections[1].Activities
                .OfType<Assignment>()
                .FirstOrDefault(a => a.ActivityId == 10);

            assignment1.GradeCategory = course.GradeCategories[2];
            assignment1.GradingWeight = 50;
            assignment1.GradingLocked = true;
            assignment1.GradeName = "Documentation Report";
            assignment1.GradeOffset = 1.0f;
            assignment1.BlindMarking = true;
            assignment1.HideGrader = true;
        }
    }
}
