using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;

public class ActivityCalendarWriter : XmlWriterBase
{
    /// <summary>
    /// writes the calendar.xml file required for an activity
    /// </summary>
    /// <param name="outputPath"></param>
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