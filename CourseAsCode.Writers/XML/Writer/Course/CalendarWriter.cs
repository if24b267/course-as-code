using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class CalendarWriter : XmlWriterBase
{
	public void WriteCalendar(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/calendar.xml"));
		
		using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
		{
			writer.WriteStartDocument();
			
			writer.WriteStartElement("events");
			
			// Add whitespace to prevent self-closing tag
			writer.WriteString("\n");
			
			writer.WriteEndElement(); // Close <events>
			
			writer.WriteEndDocument();
		}
	}
}