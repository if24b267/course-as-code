using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class CommentsWriter : XmlWriterBase
{
	public void WriteComments(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/comments.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("comments");
			writer.WriteString(" ");
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}
	}
}