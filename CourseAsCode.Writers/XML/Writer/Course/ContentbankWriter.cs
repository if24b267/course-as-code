using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class ContentbankWriter : XmlWriterBase
{
	public void WriteContentbank(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/contentbank.xml"));
		
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			
			writer.WriteStartElement("contents");
			
			writer.WriteString("\n");
			
			writer.WriteEndElement(); // Close </contents>
			
			writer.WriteEndDocument();
		}
	}
}