using System.Xml;

namespace CourseAsCode.Writers.XML.Sections.section_1;

public class InforefSectionsWriter1
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = true // Removes the XML declaration
    };

    public void WriteInforefSections1(string filePath = "sections/section_1/inforef.xml")
    {
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