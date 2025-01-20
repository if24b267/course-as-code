using CourseAsCode.Model;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.Activities;
using CourseAsCode.Writers.XML.Writer.Activities.Labels;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Writers.XML.Writer.Sections;

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

    private readonly LabelWriter _labelWriter = new LabelWriter();
    private readonly ModuleWriterLabel _moduleWriterLabel = new ModuleWriterLabel();
    private readonly GradesWriterLabel _gradesWriterLabel = new GradesWriterLabel();
    private readonly GradehistoryWriterLabel _gradehistoryWriterLabel = new GradehistoryWriterLabel();
    private readonly InforefWriterLabel _inforefWriterLabel = new InforefWriterLabel();
    private readonly RolesWriterLabel _rolesWriterLabel = new RolesWriterLabel();
    private readonly XapistateWriterLabel _xapistateWriterLabel = new XapistateWriterLabel();
    private readonly CalendarWriterLabel _calendarWriterLabel = new CalendarWriterLabel();
    private readonly CommentsWriterLabel _commentsWriterLabel = new CommentsWriterLabel();
    private readonly CompetenciesWriterLabel _competenciesWriterLabel = new CompetenciesWriterLabel();
    private readonly CompletionWriterLabel _completionWriterLabel = new CompletionWriterLabel();
    private readonly FiltersWriterLabel _filtersWriterLabel = new FiltersWriterLabel();
    
    private readonly UrlWriter _urlWriter = new UrlWriter();
    private readonly GradesWriterUrl _gradesWriterUrl = new GradesWriterUrl();
    private readonly GradehistoryWriterUrl _gradehistoryWriterUrl = new GradehistoryWriterUrl();
    private readonly CommentsWriterUrl _commentsWriterUrl = new CommentsWriterUrl();
    private readonly CalendarWriterUrl _calendarWriterUrl = new CalendarWriterUrl();
    private readonly InforefWriterUrl _inforefWriterUrl = new InforefWriterUrl();
    private readonly RolesWriterUrl _rolesWriterUrl = new RolesWriterUrl();
    private readonly XapistateWriterUrl _xapistateWriterUrl = new XapistateWriterUrl();
    private readonly ModuleWriterUrl _moduleWriterUrl = new ModuleWriterUrl();
    private readonly CompletionWriterUrl _completionWriterUrl = new CompletionWriterUrl();
    private readonly FiltersWriterUrl _filtersWriterUrl = new FiltersWriterUrl();
    private readonly CompetenciesWriterUrl _competenciesWriterUrl = new CompetenciesWriterUrl();
    
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



        Directory.CreateDirectory(Path.Combine(outputPath, "activities"));
        foreach (var section in course.Sections)
        {
            foreach (var activity in section.Activities)
            {
                if (activity is TextAndMediaArea textAndMediaArea)
                {
                    string activityPath = Path.Combine(outputPath, "activities", $"label_{activity.ActivityId}");
                    Directory.CreateDirectory(activityPath);
                    _labelWriter.WriteLabel(activityPath, textAndMediaArea);
                    _calendarWriterLabel.WriteCalendar(activityPath);
                    _commentsWriterLabel.WriteComments(activityPath);
                    _competenciesWriterLabel.WriteCompetencies(activityPath);
                    _completionWriterLabel.WriteCompletion(activityPath);
                    _filtersWriterLabel.WriteFilters(activityPath);
                    _gradesWriterLabel.WriteGrade(activityPath);
                    _gradehistoryWriterLabel.WriteGradehistory(course, activityPath);
                    _inforefWriterLabel.WriteInforefLabel(activityPath);
                    _rolesWriterLabel.WriteRoles(activityPath);
                    _xapistateWriterLabel.WriteState(activityPath);
                    _moduleWriterLabel.WriteModule(activityPath, section);
                }

                if (activity is URL urlObject)
                {
                    string activityPath = Path.Combine(outputPath, "activities", $"url_{activity.ActivityId}");
                    Directory.CreateDirectory(activityPath);
                    _urlWriter.WriteLabel(activityPath, urlObject);
                    _gradesWriterUrl.WriteGrade(activityPath);
                    _gradehistoryWriterUrl.WriteGradehistory(course, activityPath);
                    _commentsWriterUrl.WriteComments(activityPath);
                    _calendarWriterUrl.WriteCalendar(activityPath);
                    _inforefWriterUrl.WriteInforefLabel(activityPath);
                    _rolesWriterUrl.WriteRoles(activityPath);
                    _xapistateWriterUrl.WriteState(activityPath);
                    _moduleWriterUrl.WriteModule(activityPath, section);
                    _completionWriterUrl.WriteCompletion(activityPath);
                    _filtersWriterUrl.WriteFilters(activityPath);
                    _competenciesWriterUrl.WriteCompetencies(activityPath);
                }
            }
        }

            return true;
    }
}
