using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class ScalesWriter : XmlWriterBase
{
	public void WriteScales(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "scales.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, Settings);
		
		writer.WriteStartDocument();
		
		writer.WriteStartElement("scales_definition");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <scales_definition>
		
		writer.WriteEndDocument();
	}
}