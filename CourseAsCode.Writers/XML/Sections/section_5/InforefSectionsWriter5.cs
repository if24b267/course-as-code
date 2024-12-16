using System.Xml;

namespace CourseAsCode.Writers.XML.Sections.section_5;

public class InforefSectionsWriter5
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = false // Removes the XML declaration
	};

	public void WriteInforefSections5(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "sections/section_5/inforef.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();

			// <inforef> starts here
			writer.WriteStartElement("inforef");

			// </inforef> ends here
			writer.WriteEndElement();

			writer.WriteEndDocument();
		}
	}
}