using CourseAsCode.Model;
using CourseAsCode.Writers.Static;
using CourseAsCode.Writers.XML.Sections.section_1;
using CourseAsCode.Writers.XML.Sections.section_2;
using CourseAsCode.Writers.XML.Sections.section_3;
using CourseAsCode.Writers.XML.Sections.section_4;
using CourseAsCode.Writers.XML.Sections.section_5;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.XML.Writer.Course;

namespace CourseAsCode.Writers;

public class Writer
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
	
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	
	public bool WriteXML(Course course, string outputPath)
	{
		_logger.Debug($"Writing XML files to {outputPath}");
		course.BackupMetadata ??= new BackupMetadataDefaults().GetDefaultBackupMetadata();

		// Course
		// Create course directory if it does not exist
		Directory.CreateDirectory(Path.Combine(outputPath,"course"));

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
		// Create sections directory if it does not exist
		Directory.CreateDirectory(Path.Combine(outputPath,"sections"));

		// Name of the subfolders, for the hardcoded sections 
		string[] subfolders = { "section_1", "section_2", "section_3", "section_4", "section_5", };

		// Create subfolders in "sections"
		foreach (string subfolder in subfolders)
		{
			Directory.CreateDirectory(Path.GetFullPath(Path.Combine(outputPath, Path.Combine("sections", subfolder))));
		}
		// TODO: Refactor this to not be a hardcoded mess
		// Section 1
		SectionWriter1 sectionWriter1 = new SectionWriter1();
		sectionWriter1.WriteSection1(outputPath);

		InforefSectionsWriter1 inforefSectionsWriter1 = new InforefSectionsWriter1();
		inforefSectionsWriter1.WriteInforefSections1(outputPath);

		// Section 2
		SectionWriter2 sectionWriter2 = new SectionWriter2();
		sectionWriter2.WriteSection2(outputPath);

		InforefSectionsWriter2 inforefSectionsWriter2 = new InforefSectionsWriter2();
		inforefSectionsWriter2.WriteInforefSections2(outputPath);

		// Section 3
		SectionWriter3 sectionWriter3 = new SectionWriter3();
		sectionWriter3.WriteSection3(outputPath);

		InforefSectionsWriter3 inforefSectionsWriter3 = new InforefSectionsWriter3();
		inforefSectionsWriter3.WriteInforefSections3(outputPath);

		// Section 4
		SectionWriter4 sectionWriter4 = new SectionWriter4();
		sectionWriter4.WriteSection4(outputPath);

		InforefSectionsWriter4 inforefSectionsWriter4 = new InforefSectionsWriter4();
		inforefSectionsWriter4.WriteInforefSections4(outputPath);

		// Section 5
		SectionWriter5 sectionWriter5 = new SectionWriter5();
		sectionWriter5.WriteSection5(outputPath);

		InforefSectionsWriter5 inforefSectionsWriter5 = new InforefSectionsWriter5();
		inforefSectionsWriter5.WriteInforefSections5(outputPath);


		// Files
		// Create files directory if it does not exist
		Directory.CreateDirectory(Path.Combine(outputPath,"files"));

		// Create subfolder in files
		//Directory.CreateDirectory(Path.Combine(outputPath, Path.Combine("files", "4e")));

		return true;
	}
}