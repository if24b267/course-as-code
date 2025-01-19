using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class BadgesWriter : XmlWriterBase
{
	public void WriteBadges(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "badges.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, Settings);

		writer.WriteStartDocument();

		writer.WriteStartElement("badges");

		// Add whitespace to prevent a self-closing tag
		writer.WriteWhitespace("\n");

		writer.WriteEndElement(); // Close <badges>

		writer.WriteEndDocument();
	}
}