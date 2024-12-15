using System.Xml;

namespace CourseAsCode.Writers.XML;

public class GroupsWriter : XmlWriterBase
{
    public void WriteGroups(string filePath = "groups.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("groups");

            WriteGroupCustomFields(writer);
            WriteGroupings(writer);

            writer.WriteEndElement(); // Close <groups>
            writer.WriteEndDocument();
        }
    }

    private void WriteGroupCustomFields(XmlWriter writer)
    {
        writer.WriteStartElement("groupcustomfields");
        writer.WriteString(" ");
        writer.WriteEndElement(); // Close <groupcustomfields>
    }

    private void WriteGroupings(XmlWriter writer)
    {
        writer.WriteStartElement("groupings");

        writer.WriteStartElement("groupingcustomfields");
        writer.WriteString(" ");
        writer.WriteEndElement(); // Close <groupingcustomfields>

        writer.WriteEndElement(); // Close <groupings>
    }
}
