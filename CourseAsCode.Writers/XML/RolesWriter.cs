using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML;

public class RolesWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteRoles(List<Role> roles, string filePath = "roles.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("roles_definition");

            foreach (var role in roles)
            {
                writer.WriteStartElement("role");
                writer.WriteAttributeString("id", role.SystemDefaults.Id.ToString());

                writer.WriteElementString("name", role.Name ?? string.Empty);
                writer.WriteElementString("shortname", role.ShortName);
                writer.WriteElementString("nameincourse", role.LocalName ?? "$@NULL@$");
                writer.WriteElementString("description", role.Description ?? string.Empty);
                writer.WriteElementString("sortorder", role.SystemDefaults.Id.ToString());
                writer.WriteElementString("archetype", role.Archetype.ToString().ToLower());
                writer.WriteEndElement(); // Close <role>
            }

            writer.WriteEndElement(); // Close <roles_definition>
            writer.WriteEndDocument();
        }
    }
}


