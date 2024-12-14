using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class RolesCoursWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = true // Removes the XML declaration
    };
    
    public void WriteRolesCours(string filePath = "course/roles.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            // <roles> starts here
            writer.WriteStartElement("roles");

                // <role_overrides> starts here and ends immediately since it's empty
                writer.WriteStartElement("role_overrides");
                writer.WriteEndElement();

                // <role_assignments> starts here
                writer.WriteStartElement("role_assignments");

                    // <assignment> starts here with attributes
                    writer.WriteStartElement("assignment");
                    writer.WriteAttributeString("id", "1");

                        // child elements of <assignment>
                        writer.WriteElementString("roleid", "3");
                        writer.WriteElementString("userid", "3");
                        writer.WriteElementString("timemodified", "1731136235");
                        writer.WriteElementString("modifierid", "2");
                        writer.WriteElementString("component", "");
                        writer.WriteElementString("itemid", "0");
                        writer.WriteElementString("sortorder", "0");

                    // </assignment> ends here
                    writer.WriteEndElement();

                // </role_assignments> ends here
                writer.WriteEndElement();

            // </roles> ends here
            writer.WriteEndElement();

            writer.WriteEndDocument();
        }
    }
}