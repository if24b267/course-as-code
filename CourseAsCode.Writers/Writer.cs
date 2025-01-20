using CourseAsCode.Model;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Writer.Activities.File;
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
    
    // -- Activity Writers -- //
    // Common Activity Writers
    private readonly ActivityGradesWriter _activityGradesWriter = new ActivityGradesWriter();
    private readonly ActivityGradehistoryWriter _activityGradehistoryWriter = new ActivityGradehistoryWriter();
    private readonly ActivityInforefWriter _activityInforefWriter = new ActivityInforefWriter();
    private readonly ActivityRolesWriter _activityRolesWriter = new ActivityRolesWriter();
    private readonly ActivityXapistateWriter _activityXapistateWriter = new ActivityXapistateWriter();
    private readonly ActivityCalendarWriter _activityCalendarWriter = new ActivityCalendarWriter();
    private readonly ActivityCommentsWriter _activityCommentsWriter = new ActivityCommentsWriter();
    private readonly ActivityCompetenciesWriter _activityCompetenciesWriter = new ActivityCompetenciesWriter();
    private readonly ActivityCompletionWriter _activityCompletionWriter = new ActivityCompletionWriter();
    private readonly ActivityFiltersWriter _activityFiltersWriter = new ActivityFiltersWriter();
    
    // Text and Media Area (Label) Activity Writers
    private readonly LabelWriter _labelWriter = new LabelWriter();
    private readonly ModuleWriterLabel _moduleWriterLabel = new ModuleWriterLabel();
    
    // URL Activity Writers
    private readonly UrlWriter _urlWriter = new UrlWriter();
    private readonly ModuleWriterUrl _moduleWriterUrl = new ModuleWriterUrl();
    
    // File Activity Writers
    private readonly FileWriter _fileWriter = new FileWriter();
    private readonly ModuleWriterFile _moduleWriterFile = new ModuleWriterFile();
    
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

        // -- Write course files -- //
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

        // -- Write section files -- //
        Directory.CreateDirectory(Path.Combine(outputPath, "sections"));

        // Write sections using SectionWriter
        _logger.Debug("Writing sections...");
        _sectionWriter.WriteSections(outputPath, course.Sections);

        // Write inforef sections using InforefSectionsWriter
        InforefSectionsWriter inforefSectionsWriter = new InforefSectionsWriter();
        inforefSectionsWriter.WriteInforefSections(outputPath, course.Sections);

        // -- Write activity files -- //
        Directory.CreateDirectory(Path.Combine(outputPath, "files"));
        Directory.CreateDirectory(Path.Combine(outputPath, "activities"));
        
        // Iterate through all sections of the course
        foreach (var section in course.Sections)
        {
            // Iterate through all activities of that section
            foreach (var activity in section.Activities)
            {
                string activityPath = string.Empty;
                
                if (activity is TextAndMediaArea textAndMediaArea)
                {
                    // Text and Media Area (Label)-specific XML files
                    activityPath = Path.Combine(outputPath, "activities", $"label_{activity.ActivityId}");
                    Directory.CreateDirectory(activityPath);
                    _labelWriter.WriteLabel(activityPath, textAndMediaArea);
                    _moduleWriterLabel.WriteModule(activityPath, section);
                }

                if (activity is URL urlObject)
                {
                    // URL-specific XML files
                    activityPath = Path.Combine(outputPath, "activities", $"url_{activity.ActivityId}");
                    Directory.CreateDirectory(activityPath);
                    _urlWriter.WriteUrl(activityPath, urlObject);
                    _moduleWriterUrl.WriteModule(activityPath, course, section, urlObject);
                }

                if (activity is MoodleFile moodleFile)
                {
                    // File-specific XML files
                    activityPath = Path.Combine(outputPath, "activities", $"resource_{activity.ActivityId}");
                    Directory.CreateDirectory(activityPath);
                    _fileWriter.WriteFile(activityPath, moodleFile);
                    _moduleWriterFile.WriteModule(activityPath, course, section, moodleFile);
                }
                
                // General activity XML files
                _activityCalendarWriter.WriteCalendar(activityPath);
                _activityCommentsWriter.WriteComments(activityPath);
                _activityCompetenciesWriter.WriteCompetencies(activityPath);
                _activityCompletionWriter.WriteCompletion(activityPath);
                _activityFiltersWriter.WriteFilters(activityPath);
                _activityGradesWriter.WriteGrade(activityPath);
                _activityGradehistoryWriter.WriteGradehistory(course, activityPath);
                _activityInforefWriter.WriteInforefLabel(activityPath);
                _activityRolesWriter.WriteRoles(activityPath);
                _activityXapistateWriter.WriteState(activityPath);
            }
        }
        return true;
    }
}
