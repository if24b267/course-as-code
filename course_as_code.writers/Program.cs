using course_as_code.writers.XML.course;

namespace course_as_code.writers;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        CalendarWriter calendarWriter = new CalendarWriter();
        calendarWriter.WriteCalendar();
    }
}