using System.Xml;

namespace CourseAsCode.Writers.XML;

public class BadgesWriter
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = false
	};

	
	public void WriteBadges(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "badges.xml"));
		
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			
			writer.WriteStartElement("badges");

			// Add whitespace to prevent a self-closing tag
			writer.WriteWhitespace("\n");
			
			writer.WriteEndElement(); // Close <badges>

			writer.WriteEndDocument();
		}
	}
}