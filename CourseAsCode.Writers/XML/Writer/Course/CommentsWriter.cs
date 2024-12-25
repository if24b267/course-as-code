using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class CommentsWriter : XmlWriterBase
{
	public void WriteComments(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/comments.xml"));
		
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			
			writer.WriteStartElement("comments");
			
			// Add whitespace to prevent self-closing tag
			writer.WriteString("\n");
			
			writer.WriteEndElement(); // Close <comments>
			
			writer.WriteEndDocument();
		}
	}
}