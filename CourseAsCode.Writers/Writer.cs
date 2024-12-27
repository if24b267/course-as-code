using CourseAsCode.Model;
using CourseAsCode.Writers.Static;
using CourseAsCode.Writers.XML;
using CourseAsCode.Writers.XML.Files.E4;
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
	private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
	public bool WriteXML(Course course, string outputPath)
	{
		_logger.Debug($"Writing XML files to {outputPath}");
		course.BackupMetadata ??= new BackupMetadataDefaults().GetDefaultBackupMetadata();

		// Course
		// Create course directory if it does not exist
		Directory.CreateDirectory(Path.Combine(outputPath,"course"));

		CalendarWriter calendarWriter = new CalendarWriter();
		calendarWriter.WriteCalendar(outputPath);

		CommentsWriter commentsWriter = new CommentsWriter();
		commentsWriter.WriteComments(outputPath);

		CompetenciesWriter competenciesWriter = new CompetenciesWriter();
		competenciesWriter.WriteCompetencies(outputPath);

		CompletiondefaultsWriter completiondefaultsWriter = new CompletiondefaultsWriter();
		completiondefaultsWriter.WriteCompletionDefaults(outputPath);

		ContentbankWriter contentbankWriter = new ContentbankWriter();
		contentbankWriter.WriteContentbank(outputPath);

		CourseWriter courseWriter = new CourseWriter();
		courseWriter.WriteCourse(course, outputPath);

		FilesWriter fileWriter = new FilesWriter();
		fileWriter.WriteFiles(course, outputPath);

		GradehistoryWriter gradehistoryWriter = new GradehistoryWriter();
		gradehistoryWriter.WriteGradehistory(course, outputPath);

		GradebookWriter gradebookWriter = new GradebookWriter();
		gradebookWriter.WriteGradebook(course, outputPath);

		GroupsWriter groupsWriter = new GroupsWriter();
		groupsWriter.WriteGroups(course, outputPath);

		MoodleBackupWriter moodleBackupWriter = new MoodleBackupWriter();
		moodleBackupWriter.WriteMoodleBackup(course.BackupMetadata, course, outputPath);

		OutcomesWriter outcomesWriter = new OutcomesWriter();
		outcomesWriter.WriteOutcomes(outputPath);

		QuestionsWriter questionsWriter = new QuestionsWriter();
		questionsWriter.WriteQuestions(outputPath);

		RolesWriter rolesWriter = new RolesWriter();
		rolesWriter.WriteRoles(course.Roles, outputPath);

		ScalesWriter scalesWriter = new ScalesWriter();
		scalesWriter.WriteScales(outputPath);

		UsersWriter usersWriter = new UsersWriter();
		usersWriter.WriteUsers(course.EnrolledUsers, outputPath);

		EnrolmentsWriter enrolmentsWriter = new EnrolmentsWriter();
		enrolmentsWriter.WriteEnrolment(course, outputPath);

		FiltersWriter filtersWriter = new FiltersWriter();
		filtersWriter.WriteFilters(course, outputPath);

		InforefWriter inforefWriter = new InforefWriter();
		inforefWriter.WriteInforef(course, outputPath);

		RoleAssignmentsWriter roleAssignmentsWriter = new RoleAssignmentsWriter();
		roleAssignmentsWriter.WriteRoleAssignments(course, outputPath);

		BadgesWriter badgesWriter = new BadgesWriter();
		badgesWriter.WriteBadges(outputPath);

		CompletionWriter completionWriter = new CompletionWriter();
		completionWriter.WriteCompletion(course, outputPath);


		// Sections
		// Create sections directory if it does not exist
		System.IO.Directory.CreateDirectory("sections");

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