using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class CompletiondefaultsWriter : XmlWriterBase
{
	public void WriteCompletionDefaults(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/completion_defaults.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("course_completion_defaults");
			writer.WriteString(" ");
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}
	}
}