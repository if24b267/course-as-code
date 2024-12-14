using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class InforefWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = true // Removes the XML declaration
    };
    
    public void WriteInforef(string filePath = "course/inforef.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            // <inforef> starts here
            writer.WriteStartElement("inforef");

                // <userref> starts here
                writer.WriteStartElement("userref");

                    // <user> starts here
                    writer.WriteStartElement("user");

                        // child elements of <user>
                        writer.WriteElementString("id", "3");

                    // </user> ends here
                    writer.WriteEndElement();

                // </userref> ends here
                writer.WriteEndElement();

                // <roleref> starts here
                writer.WriteStartElement("roleref");

                    // First <role> starts here
                    writer.WriteStartElement("role");

                        // child elements of first <role>
                        writer.WriteElementString("id", "3");

                    // </role> ends here
                    writer.WriteEndElement();

                    // Second <role> starts here
                    writer.WriteStartElement("role");

                        // child elements of second <role>
                        writer.WriteElementString("id", "5");

                    // </role> ends here
                    writer.WriteEndElement();

                // </roleref> ends here
                writer.WriteEndElement();

                // <fileref> starts here
                writer.WriteStartElement("fileref");

                    // First <file> starts here
                    writer.WriteStartElement("file");

                        // child elements of first <file>
                        writer.WriteElementString("id", "10");

                    // </file> ends here
                    writer.WriteEndElement();

                    // Second <file> starts here
                    writer.WriteStartElement("file");

                        // child elements of second <file>
                        writer.WriteElementString("id", "11");

                    // </file> ends here
                    writer.WriteEndElement();

                // </fileref> ends here
                writer.WriteEndElement();

            // </inforef> ends here
            writer.WriteEndElement();

            writer.WriteEndDocument();
        }
    }
}