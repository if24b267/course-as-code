using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class QuestionsWriter : XmlWriterBase
{
	public void WriteQuestions(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "questions.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, _settings);
		
		writer.WriteStartDocument();
		
		writer.WriteStartElement("question_categories");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <question_categories>
		
		writer.WriteEndDocument();
	}
}