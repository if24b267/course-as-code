using System.Xml;

namespace CourseAsCode.Writers.XML;

public class ScalesWriter : XmlWriterBase
{
	public void WriteScales(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "scales.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("scales_definition");
			writer.WriteString(" ");
			writer.WriteEndElement(); // Close <scales_definition>
			writer.WriteEndDocument();
		}
	}
}