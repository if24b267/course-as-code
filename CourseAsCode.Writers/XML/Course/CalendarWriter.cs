using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class CalendarWriter : XmlWriterBase
{
    public void WriteCalendar(string filePath = "course/calendar.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("events");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <events>
            writer.WriteEndDocument();
        }
    }
}
