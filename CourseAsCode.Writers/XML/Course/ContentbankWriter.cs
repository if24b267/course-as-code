using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class ContentbankWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };
    
    public void WriteContentbank(string filePath = "course/contentbank.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            
            writer.WriteStartElement("contents");
            writer.WriteString(" ");
            writer.WriteEndElement();
            
            writer.WriteEndDocument();
        }
    }
}