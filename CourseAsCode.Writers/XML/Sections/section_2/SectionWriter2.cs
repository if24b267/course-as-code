using System.Xml;

namespace CourseAsCode.Writers.XML.Sections.section_2;

public class SectionWriter2
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = true // Removes the XML declaration
	};

	public void WriteSection2(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "sections/section_2/section.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();

			// <section> starts here with attributes
			writer.WriteStartElement("section");
			writer.WriteAttributeString("id", "2");

			// child elements of <section>
			writer.WriteElementString("number", "1");
			writer.WriteElementString("name", "$@NULL@$");
			writer.WriteElementString("summary", "");
			writer.WriteElementString("summaryformat", "1");
			writer.WriteElementString("sequence", "");
			writer.WriteElementString("visible", "1");
			writer.WriteElementString("availabilityjson", "$@NULL@$");
			writer.WriteElementString("component", "$@NULL@$");
			writer.WriteElementString("itemid", "$@NULL@$");
			writer.WriteElementString("timemodified", "1731001588");

			// </section> ends here
			writer.WriteEndElement();

			writer.WriteEndDocument();
		}
	}
}