using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class CompletiondefaultsWriter : XmlWriterBase
{
	public void WriteCompletionDefaults(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/completiondefaults.xml"));
		
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			
			writer.WriteStartElement("course_completion_defaults");
			
			writer.WriteString("\n");
			
			writer.WriteEndElement(); // Close </course_completion_defaults>
			
			writer.WriteEndDocument();
		}
	}
}