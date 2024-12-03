using CourseAsCode.Writers.XML;
using CourseAsCode.Writers.XML.Course;

namespace CourseAsCode.Writers;

class Program
{
    static void Main(string[] args)
    {
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
        courseWriter.WriteCourse();

        GradehistoryWriter gradehistoryWriter = new GradehistoryWriter();
        gradehistoryWriter.WriteGradehistory();

        GradebookWriter gradebookWriter = new GradebookWriter();
        gradebookWriter.WriteGradebook();

        GroupsWriter groupsWriter = new GroupsWriter();
        groupsWriter.WriteGroups();

        MoodleBackupWriter moodleBackupWriter = new MoodleBackupWriter();
        moodleBackupWriter.WriteMoodleBackup();

        OutcomesWriter outcomesWriter = new OutcomesWriter();
        outcomesWriter.WriteOutcomes();

        QuestionsWriter questionsWriter = new QuestionsWriter();
        questionsWriter.WriteQuestions();

        RolesWriter rolesWriter = new RolesWriter();
        rolesWriter.WriteRoles();

        ScalesWriter scalesWriter = new ScalesWriter();
        scalesWriter.WriteScales();

        UsersWriter usersWriter = new UsersWriter();    
        usersWriter.WriteUsers();
    }
}