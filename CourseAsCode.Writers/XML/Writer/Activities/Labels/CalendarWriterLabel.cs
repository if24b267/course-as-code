using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class CalendarWriterLabel : XmlWriterBase
{
    public void WriteCalendar(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "calendar.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("events");

        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");

        writer.WriteEndElement(); // Close <events>

        writer.WriteEndDocument();
    }
}