using CourseAsCode.Model;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.Sections;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.Course;

namespace CourseAsCode.Writers;

public class Writer : IWriter
{
    // Writer
    private readonly CalendarWriter _calendarWriter = new CalendarWriter();
    private readonly CommentsWriter _commentsWriter = new CommentsWriter();
    private readonly CompetenciesWriter _competenciesWriter = new CompetenciesWriter();
    private readonly CompletiondefaultsWriter _completiondefaultsWriter = new CompletiondefaultsWriter();
    private readonly ContentbankWriter _contentbankWriter = new ContentbankWriter();
    private readonly CourseWriter _courseWriter = new CourseWriter();
    private readonly FilesWriter _filesWriter = new FilesWriter();
    private readonly GradehistoryWriter _gradehistoryWriter = new GradehistoryWriter();
    private readonly GradebookWriter _gradebookWriter = new GradebookWriter();
    private readonly GroupsWriter _groupsWriter = new GroupsWriter();
    private readonly MoodleBackupWriter _moodleBackupWriter = new MoodleBackupWriter();
    private readonly OutcomesWriter _outcomesWriter = new OutcomesWriter();
    private readonly QuestionsWriter _questionsWriter = new QuestionsWriter();
    private readonly RolesWriter _rolesWriter = new RolesWriter();
    private readonly ScalesWriter _scalesWriter = new ScalesWriter();
    private readonly UsersWriter _usersWriter = new UsersWriter();
    private readonly EnrolmentsWriter _enrolmentsWriter = new EnrolmentsWriter();
    private readonly FiltersWriter _filtersWriter = new FiltersWriter();
    private readonly InforefWriter _inforefWriter = new InforefWriter();
    private readonly RoleAssignmentsWriter _roleAssignmentsWriter = new RoleAssignmentsWriter();
    private readonly BadgesWriter _badgesWriter = new BadgesWriter();
    private readonly CompletionWriter _completionWriter = new CompletionWriter();
    private readonly SectionWriter _sectionWriter = new SectionWriter();

    private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    public bool WriteCourse(Course course, string outputPath)
    {
        _logger.Debug($"Writing XML files to {outputPath}");
        course.BackupMetadata ??= new BackupMetadataDefaults().GetDefaultBackupMetadata();

        // Ensure sections list is not empty
        if (course.Sections == null || course.Sections.Count == 0)
        {
            _logger.Debug("No sections found, adding default section.");
            course.Sections = new List<Section> { SectionDefaults.GetDefaultSection() };
        }

        // Course
        // Create course directory if it does not exist
        Directory.CreateDirectory(Path.Combine(outputPath, "course"));

        _calendarWriter.WriteCalendar(course, outputPath);
        _commentsWriter.WriteComments(course, outputPath);
        _competenciesWriter.WriteCompetencies(course, outputPath);
        _completiondefaultsWriter.WriteCompletionDefaults(course, outputPath);
        _contentbankWriter.WriteContentbank(course, outputPath);
        _courseWriter.WriteCourse(course, outputPath);
        _filesWriter.WriteFiles(course, outputPath);
        _gradehistoryWriter.WriteGradehistory(course, outputPath);
        _gradebookWriter.WriteGradebook(course, outputPath);
        _groupsWriter.WriteGroups(course, outputPath);
        _moodleBackupWriter.WriteMoodleBackup(course, outputPath);
        _outcomesWriter.WriteOutcomes(course, outputPath);
        _questionsWriter.WriteQuestions(course, outputPath);
        _rolesWriter.WriteRoles(course, outputPath);
        _scalesWriter.WriteScales(course, outputPath);
        _usersWriter.WriteUsers(course, outputPath);
        _enrolmentsWriter.WriteEnrolment(course, outputPath);
        _filtersWriter.WriteFilters(course, outputPath);
        _inforefWriter.WriteInforef(course, outputPath);
        _roleAssignmentsWriter.WriteRoleAssignments(course, outputPath);
        _badgesWriter.WriteBadges(outputPath);
        _completionWriter.WriteCompletion(course, outputPath);

        // Sections
        Directory.CreateDirectory(Path.Combine(outputPath, "sections"));

        // Write sections using SectionWriter
        _logger.Debug("Writing sections...");
        _sectionWriter.WriteSections(outputPath, course.Sections);

        // Write inforef sections using InforefSectionsWriter
        InforefSectionsWriter inforefSectionsWriter = new InforefSectionsWriter();
        inforefSectionsWriter.WriteInforefSections(outputPath, course.Sections);

        // Files
        // Create files directory if it does not exist
        Directory.CreateDirectory(Path.Combine(outputPath, "files"));

        // Create subfolder in files
        //Directory.CreateDirectory(Path.Combine(outputPath, Path.Combine("files", "4e")));

        return true;
    }
}
