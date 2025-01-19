using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class RolesWriterLabel : XmlWriterBase
{
    public void WriteRoles(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "roles.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("roles");
        writer.WriteStartElement("role_overrides");
        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <role_overrides>
        writer.WriteStartElement("role_assignments");
        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <role_assignments>
        writer.WriteEndElement(); // Close <roles>

        writer.WriteEndDocument();
    }
}