using CourseAsCode.Model;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers
{
    public class Writer : IWriter
    {
        // -- General Writers -- //
        private readonly FilesWriter _filesWriter;
        private readonly GradehistoryWriter _gradehistoryWriter;
        private readonly GradebookWriter _gradebookWriter;
        private readonly GroupsWriter _groupsWriter;
        private readonly MoodleBackupWriter _moodleBackupWriter;
        private readonly OutcomesWriter _outcomesWriter;
        private readonly QuestionsWriter _questionsWriter;
        private readonly RolesWriter _rolesWriter;
        private readonly ScalesWriter _scalesWriter;
        private readonly UsersWriter _usersWriter;
        private readonly BadgesWriter _badgesWriter;
        private readonly CompletionWriter _completionWriter;

        // -- Additional Dependencies -- //
        private readonly ILogger<Writer> _logger;

        // -- Services for writing the various aspects of the Moodle Course -- //
        private readonly IActivitiesWriterService _activitiesWriterService;
        private readonly ISectionFilesWriterService _sectionFilesWriterService;
        private readonly ICourseFilesWriterService _courseFilesWriterService;

        public Writer(
            FilesWriter filesWriter,
            GradehistoryWriter gradehistoryWriter,
            GradebookWriter gradebookWriter,
            GroupsWriter groupsWriter,
            MoodleBackupWriter moodleBackupWriter,
            OutcomesWriter outcomesWriter,
            QuestionsWriter questionsWriter,
            RolesWriter rolesWriter,
            ScalesWriter scalesWriter,
            UsersWriter usersWriter,
            BadgesWriter badgesWriter,
            CompletionWriter completionWriter,
            IActivitiesWriterService activitiesWriterService,
            ISectionFilesWriterService sectionFilesWriterService,
            ICourseFilesWriterService courseFilesWriterService,
            ILogger<Writer> logger)
        {
            // -- General Writers Assignment -- //
            _filesWriter = filesWriter ?? throw new ArgumentNullException(nameof(filesWriter));
            _gradehistoryWriter = gradehistoryWriter ?? throw new ArgumentNullException(nameof(gradehistoryWriter));
            _gradebookWriter = gradebookWriter ?? throw new ArgumentNullException(nameof(gradebookWriter));
            _groupsWriter = groupsWriter ?? throw new ArgumentNullException(nameof(groupsWriter));
            _moodleBackupWriter = moodleBackupWriter ?? throw new ArgumentNullException(nameof(moodleBackupWriter));
            _outcomesWriter = outcomesWriter ?? throw new ArgumentNullException(nameof(outcomesWriter));
            _questionsWriter = questionsWriter ?? throw new ArgumentNullException(nameof(questionsWriter));
            _rolesWriter = rolesWriter ?? throw new ArgumentNullException(nameof(rolesWriter));
            _scalesWriter = scalesWriter ?? throw new ArgumentNullException(nameof(scalesWriter));
            _usersWriter = usersWriter ?? throw new ArgumentNullException(nameof(usersWriter));
            _badgesWriter = badgesWriter ?? throw new ArgumentNullException(nameof(badgesWriter));
            _completionWriter = completionWriter ?? throw new ArgumentNullException(nameof(completionWriter));

            // -- Additional Dependencies Assignment -- //
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Services
            _activitiesWriterService = activitiesWriterService ?? throw new ArgumentNullException(nameof(activitiesWriterService));
            _sectionFilesWriterService = sectionFilesWriterService ?? throw new ArgumentNullException(nameof(sectionFilesWriterService));
            _courseFilesWriterService = courseFilesWriterService ?? throw new ArgumentNullException(nameof(courseFilesWriterService));
        }


        public bool WriteCourse(Course course, string outputPath)
        {
            var user = new User { };
            course.EnrolledUsers = new List<User> { user };


            _logger.LogDebug($"Writer started for course '{course.FullName}' with ID {course.SystemDefaults.Id}");
            _logger.LogInformation($"The course will be saved at {outputPath}");

            // Ensure sections list is not empty
            if (course.Sections.Count == 0)
            {
                _logger.LogInformation("No sections specified by user, adding default section");
                course.Sections = new List<Section> { SectionDefaults.GetDefaultSection() };
            }

            // -- Write course files -- //

            // Create course directory if it does not exist
            if (!SafeCreateDirectory(Path.Combine(outputPath, "course")))
            {
                Environment.Exit(1);
            }

            // Root directory
            _badgesWriter.WriteBadges(outputPath);
            _completionWriter.WriteCompletion(course, outputPath);
            _filesWriter.WriteFiles(course, outputPath);
            _gradebookWriter.WriteGradebook(course, outputPath);
            _gradehistoryWriter.WriteGradehistory(course, outputPath);
            _groupsWriter.WriteGroups(course, outputPath);
            _moodleBackupWriter.WriteMoodleBackup(course, outputPath);
            _outcomesWriter.WriteOutcomes(course, outputPath);
            _questionsWriter.WriteQuestions(course, outputPath);
            _rolesWriter.WriteRoles(course, outputPath);
            _scalesWriter.WriteScales(course, outputPath);
            _usersWriter.WriteUsers(course, outputPath);

            // -- Write /course directory files -- //
            _courseFilesWriterService.WriteCourseFiles(course, outputPath);

            // -- Write section files -- //
            _sectionFilesWriterService.WriteSectionFiles(course, outputPath);

            // -- Write activity files -- //
            _activitiesWriterService.WriteActivities(course, outputPath);

            return true;
        }


        private bool SafeCreateDirectory(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Could not create directory {path}: {e.Message}");
                return false;
            }
        }
    }
}
