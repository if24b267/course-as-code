using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;

public class ActivityRolesWriter : XmlWriterBase
{
    /// <summary>
    /// writes the roles.xml file required for an activity
    /// </summary>
    /// <param name="outputPath"></param>
    public void WriteRoles(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "roles.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("roles");
        
        writer.WriteStartElement("role_overrides");
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <role_overrides>
        
        writer.WriteStartElement("role_assignments");
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <role_assignments>
        
        writer.WriteEndElement(); // Close <roles>

        writer.WriteEndDocument();
    }
}