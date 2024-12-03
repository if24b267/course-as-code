using System.Xml;

namespace CourseAsCode.Writers.XML;

public class RolesWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };
    public void WriteRoles(string filePath = "roles.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("roles_definition"); 

            writer.WriteStartElement("role");
            writer.WriteAttributeString("id", "3");

            writer.WriteElementString("name", "");
            writer.WriteElementString("shortname", "editingteacher");
            writer.WriteElementString("nameincourse", "$@NULL@$");
            writer.WriteElementString("description", "");
            writer.WriteElementString("sortorder", "3");
            writer.WriteElementString("archetype", "editingteacher");
            writer.WriteEndElement();

            writer.WriteStartElement("role");
            writer.WriteAttributeString("id", "5");

            writer.WriteElementString("name", "");
            writer.WriteElementString("shortname", "student");
            writer.WriteElementString("nameincourse", "$@NULL@$");
            writer.WriteElementString("description", "");
            writer.WriteElementString("sortorder", "5");
            writer.WriteElementString("archetype", "student");
            writer.WriteEndElement();

            writer.WriteEndElement(); 
            writer.WriteEndDocument();
        }
    }
}