using CourseAsCode.Model;
using CourseAsCode.Writers.Static;
using CourseAsCode.Writers.XML;
using CourseAsCode.Writers.XML.Course;
using CourseAsCode.Writers.XML.Sections.section_1;
using CourseAsCode.Writers.XML.Sections.section_2;
using CourseAsCode.Writers.XML.Sections.section_3;
using CourseAsCode.Writers.XML.Sections.section_4;
using CourseAsCode.Writers.XML.Sections.section_5;
using CourseAsCode.Writers.XML.Files.E4;

namespace CourseAsCode.Writers;

public class Writer
{

	public bool WriteXML(Course course, string outputPath)
	{
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

		GradehistoryWriter gradehistoryWriter = new GradehistoryWriter();
		gradehistoryWriter.WriteGradehistory(outputPath);

		GradebookWriter gradebookWriter = new GradebookWriter();
		gradebookWriter.WriteGradebook(outputPath);

		GroupsWriter groupsWriter = new GroupsWriter();
		groupsWriter.WriteGroups(outputPath);

		MoodleBackupWriter moodleBackupWriter = new MoodleBackupWriter();
		moodleBackupWriter.WriteMoodleBackup(course.BackupMetadata, outputPath);

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
		enrolmentsWriter.WriteEnrolment(outputPath);

		FiltersWriter filtersWriter = new FiltersWriter();
		filtersWriter.WriteFilters(outputPath);

		InforefWriter inforefWriter = new InforefWriter();
		inforefWriter.WriteInforef(course.EnrolledUsers, course.Roles, outputPath);

		RolesCourseWriter rolesCourseWriter = new RolesCourseWriter();
		rolesCourseWriter.WriteRolesCourse(course.EnrolledUsers, outputPath);

		BadgesWriter badgesWriter = new BadgesWriter();
		badgesWriter.WriteBadges(outputPath);

		CompletionWriter completionWriter = new CompletionWriter();
		completionWriter.WriteCompletion(outputPath);


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
		sectionWriter1.WriteSection1();

		InforefSectionsWriter1 inforefSectionsWriter1 = new InforefSectionsWriter1();
		inforefSectionsWriter1.WriteInforefSections1();

		// Section 2
		SectionWriter2 sectionWriter2 = new SectionWriter2();
		sectionWriter2.WriteSection2();

		InforefSectionsWriter2 inforefSectionsWriter2 = new InforefSectionsWriter2();
		inforefSectionsWriter2.WriteInforefSections2();

		// Section 3
		SectionWriter3 sectionWriter3 = new SectionWriter3();
		sectionWriter3.WriteSection3();

		InforefSectionsWriter3 inforefSectionsWriter3 = new InforefSectionsWriter3();
		inforefSectionsWriter3.WriteInforefSections3();

		// Section 4
		SectionWriter4 sectionWriter4 = new SectionWriter4();
		sectionWriter4.WriteSection4();

		InforefSectionsWriter4 inforefSectionsWriter4 = new InforefSectionsWriter4();
		inforefSectionsWriter4.WriteInforefSections4();

		// Section 5
		SectionWriter5 sectionWriter5 = new SectionWriter5();
		sectionWriter5.WriteSection5();

		InforefSectionsWriter5 inforefSectionsWriter5 = new InforefSectionsWriter5();
		inforefSectionsWriter5.WriteInforefSections5();


		// Files
		// Create files directory if it does not exist
		System.IO.Directory.CreateDirectory(Path.Combine(outputPath,"files"));

		// Create subfolder in files
		System.IO.Directory.CreateDirectory(Path.Combine(outputPath, Path.Combine("files", "4e")));

		FileWriter fileWriter = new FileWriter();
		fileWriter.WriteFile();

		return true;
	}
}