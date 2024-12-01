using System.Xml;

namespace CourseAsCode.Writers.XML;

public class GroupsWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteGroups(string filePath = "groups.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("groups"); // Root element
            writer.WriteStartElement("groupcustomfields");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close </groupcustomfields>
            writer.WriteStartElement("groupings");
            writer.WriteStartElement("groupingcustomfields");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close </groupingcustomfields>
            writer.WriteEndElement(); // Close </groupings>
            writer.WriteEndElement(); // Close </groups>

            writer.WriteEndDocument();
        }
    }
}