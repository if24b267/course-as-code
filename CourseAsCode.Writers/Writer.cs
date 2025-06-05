using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.GeneralWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.Activities.Assignment;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using CourseAsCode.Writers.XML.Writer.Activities.Labels;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter;
using CourseAsCode.Writers.XML.Writer.Sections;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers
{
    public class Writer : IWriter
    {
        // -- General Writers -- //
        private readonly CalendarWriter _calendarWriter;
        private readonly CommentsWriter _commentsWriter;
        private readonly CompetenciesWriter _competenciesWriter;
        private readonly CompletiondefaultsWriter _completiondefaultsWriter;
        private readonly ContentbankWriter _contentbankWriter;
        private readonly CourseWriter _courseWriter;
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
        private readonly EnrolmentsWriter _enrolmentsWriter;
        private readonly FiltersWriter _filtersWriter;
        private readonly InforefWriter _inforefWriter;
        private readonly RoleAssignmentsWriter _roleAssignmentsWriter;
        private readonly BadgesWriter _badgesWriter;
        private readonly CompletionWriter _completionWriter;
        private readonly SectionWriter _sectionWriter;
        private readonly InforefSectionsWriter _inforefSectionsWriter;

        // -- Additional Dependencies -- //
        private readonly ILogger<Writer> _logger;

        // Services
        private IActivitiesWriterService _activitiesWriterService;

        public Writer(
            CalendarWriter calendarWriter,
            CommentsWriter commentsWriter,
            CompetenciesWriter competenciesWriter,
            CompletiondefaultsWriter completiondefaultsWriter,
            ContentbankWriter contentbankWriter,
            CourseWriter courseWriter,
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
            EnrolmentsWriter enrolmentsWriter,
            FiltersWriter filtersWriter,
            InforefWriter inforefWriter,
            RoleAssignmentsWriter roleAssignmentsWriter,
            BadgesWriter badgesWriter,
            CompletionWriter completionWriter,
            SectionWriter sectionWriter,
            InforefSectionsWriter inforefSectionsWriter,
            IActivitiesWriterService activitiesWriterService,
            ILogger<Writer> logger)
        {
            // -- General Writers Assignment -- //
            _calendarWriter = calendarWriter ?? throw new ArgumentNullException(nameof(calendarWriter));
            _commentsWriter = commentsWriter ?? throw new ArgumentNullException(nameof(commentsWriter));
            _competenciesWriter = competenciesWriter ?? throw new ArgumentNullException(nameof(competenciesWriter));
            _completiondefaultsWriter = completiondefaultsWriter ?? throw new ArgumentNullException(nameof(completiondefaultsWriter));
            _contentbankWriter = contentbankWriter ?? throw new ArgumentNullException(nameof(contentbankWriter));
            _courseWriter = courseWriter ?? throw new ArgumentNullException(nameof(courseWriter));
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
            _enrolmentsWriter = enrolmentsWriter ?? throw new ArgumentNullException(nameof(enrolmentsWriter));
            _filtersWriter = filtersWriter ?? throw new ArgumentNullException(nameof(filtersWriter));
            _inforefWriter = inforefWriter ?? throw new ArgumentNullException(nameof(inforefWriter));
            _roleAssignmentsWriter = roleAssignmentsWriter ?? throw new ArgumentNullException(nameof(roleAssignmentsWriter));
            _badgesWriter = badgesWriter ?? throw new ArgumentNullException(nameof(badgesWriter));
            _completionWriter = completionWriter ?? throw new ArgumentNullException(nameof(completionWriter));
            _sectionWriter = sectionWriter ?? throw new ArgumentNullException(nameof(sectionWriter));
            _inforefSectionsWriter = inforefSectionsWriter ?? throw new ArgumentNullException(nameof(inforefSectionsWriter));

            // -- Additional Dependencies Assignment -- //
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Services
            _activitiesWriterService = activitiesWriterService ?? throw new ArgumentNullException(nameof(activitiesWriterService));
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

            // /course directory
            _calendarWriter.WriteCalendar(course, outputPath);
            _commentsWriter.WriteComments(course, outputPath);
            _competenciesWriter.WriteCompetencies(course, outputPath);
            _completiondefaultsWriter.WriteCompletionDefaults(course, outputPath);
            _contentbankWriter.WriteContentbank(course, outputPath);
            _courseWriter.WriteCourse(course, outputPath);
            _enrolmentsWriter.WriteEnrolment(course, outputPath);
            _filtersWriter.WriteFilters(course, outputPath);
            _inforefWriter.WriteInforef(course, outputPath);
            _roleAssignmentsWriter.WriteRoleAssignments(course, outputPath);


            // -- Write section files -- //

            // Create sections directory if it does not exist
            if (!SafeCreateDirectory(Path.Combine(outputPath, "sections")))
            {
                Environment.Exit(1);
            }

            // Create section.xml for each section
            _sectionWriter.WriteSections(outputPath, course.Sections);

            // Create inforef.xml for each section
            _inforefSectionsWriter.WriteInforefSections(outputPath, course.Sections);

            // -- Write activity files -- //
            _activitiesWriterService.WriteActivities(course, outputPath);

            return true;
        }


        /// <summary>
        /// Creates an activity directory using the specified prefix and processes it via the given action.
        /// </summary>
        /// <param name="outputPath">The base output directory.</param>
        /// <param name="activity">The activity instance.</param>
        /// <param name="prefix">prefix that distinguishes the activity type (e.g., "resource", "url", "label").</param>
        /// <param name="processActivity">A lambda that performs the writing logic for the activity.</param>
        /// <returns>The path to the created directory, or null if directory creation fails.</returns>
        private string? CreateActivity(string outputPath, Activity activity, string prefix, Action<string> processActivity)
        {
            string activityPath = Path.Combine(outputPath, "activities", $"{prefix}_{activity.ActivityId}");

            if (!SafeCreateDirectory(activityPath))
            {
                return null;
            }

            processActivity(activityPath);
            return activityPath;
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
