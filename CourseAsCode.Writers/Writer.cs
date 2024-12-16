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
		System.IO.Directory.CreateDirectory("course");

		CalendarWriter calendarWriter = new CalendarWriter();
		calendarWriter.WriteCalendar();

		CommentsWriter commentsWriter = new CommentsWriter();
		commentsWriter.WriteComments();

		CompetenciesWriter competenciesWriter = new CompetenciesWriter();
		competenciesWriter.WriteCompetencies();

		CompletiondefaultsWriter completiondefaultsWriter = new CompletiondefaultsWriter();
		completiondefaultsWriter.WriteCompletionDefaults();

		ContentbankWriter contentbankWriter = new ContentbankWriter();
		contentbankWriter.WriteContentbank();

		CourseWriter courseWriter = new CourseWriter();
		courseWriter.WriteCourse(course);

		GradehistoryWriter gradehistoryWriter = new GradehistoryWriter();
		gradehistoryWriter.WriteGradehistory();

		GradebookWriter gradebookWriter = new GradebookWriter();
		gradebookWriter.WriteGradebook();

		GroupsWriter groupsWriter = new GroupsWriter();
		groupsWriter.WriteGroups();

		MoodleBackupWriter moodleBackupWriter = new MoodleBackupWriter();
		moodleBackupWriter.WriteMoodleBackup(course.BackupMetadata);

		OutcomesWriter outcomesWriter = new OutcomesWriter();
		outcomesWriter.WriteOutcomes();

		QuestionsWriter questionsWriter = new QuestionsWriter();
		questionsWriter.WriteQuestions();

		RolesWriter rolesWriter = new RolesWriter();
		rolesWriter.WriteRoles(course.Roles);

		ScalesWriter scalesWriter = new ScalesWriter();
		scalesWriter.WriteScales();

		UsersWriter usersWriter = new UsersWriter();
		usersWriter.WriteUsers(course.EnrolledUsers);

		EnrolmentsWriter enrolmentsWriter = new EnrolmentsWriter();
		enrolmentsWriter.WriteEnrolment();

		FiltersWriter filtersWriter = new FiltersWriter();
		filtersWriter.WriteFilters();

		InforefWriter inforefWriter = new InforefWriter();
		inforefWriter.WriteInforef(course.EnrolledUsers, course.Roles);

		RolesCourseWriter rolesCourseWriter = new RolesCourseWriter();
		rolesCourseWriter.WriteRolesCourse(course.EnrolledUsers);

		BadgesWriter badgesWriter = new BadgesWriter();
		badgesWriter.WriteBadges();

		CompletionWriter completionWriter = new CompletionWriter();
		completionWriter.WriteCompletion();


		// Sections
		// Create sections directory if it does not exist
		System.IO.Directory.CreateDirectory("sections");

		// Name of the subfolders, for the hardcoded sections 
		string[] subfolders = { "section_1", "section_2", "section_3", "section_4", "section_5", };

		// Create subfolders in "sections"
		foreach (string subfolder in subfolders)
		{
			System.IO.Directory.CreateDirectory(Path.Combine("sections", subfolder));
		}

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
		System.IO.Directory.CreateDirectory("files");

		// Create subfolder in files
		System.IO.Directory.CreateDirectory(Path.Combine("files", "4e"));

		FileWriter fileWriter = new FileWriter();
		fileWriter.WriteFile();

		return true;
	}
}