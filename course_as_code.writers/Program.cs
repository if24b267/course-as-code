using course_as_code.writers.XML.course;

namespace course_as_code.writers;

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
    }
}