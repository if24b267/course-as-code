using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class OutcomesWriter : XmlWriterBase
{
	public void WriteOutcomes(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "outcomes.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, Settings);
		
		writer.WriteStartDocument();
		
		writer.WriteStartElement("outcomes_definition");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <outcomes_definition>
		
		writer.WriteEndDocument();
	}
}