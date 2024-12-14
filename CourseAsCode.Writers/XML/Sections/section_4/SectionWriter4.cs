using System.Xml;

namespace CourseAsCode.Writers.XML.Sections.section_4;

public class SectionWriter4
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = true // Removes the XML declaration
    };

    public void WriteSection4(string filePath = "sections/section_4/section.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            // <section> starts here with attributes
            writer.WriteStartElement("section");
            writer.WriteAttributeString("id", "4");

            // child elements of <section>
            writer.WriteElementString("number", "3");
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