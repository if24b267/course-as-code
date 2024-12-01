using System.Xml;

namespace CourseAsCode.Writers.XML;

public class GradebookWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteGradebook(string filePath = "gradebook.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("gradebook"); // Root element
            
            writer.WriteStartElement("attributes");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close </attributes>

            

            writer.WriteEndElement(); // Close </gradebook>
            writer.WriteEndDocument();
        }
    }
}