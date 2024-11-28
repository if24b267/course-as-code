using System.Xml;

namespace course_as_code.writers.XML.course;

public class CommentsWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };
    
    public void WriteComments(string filePath = "course/comments.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            
            writer.WriteStartElement("comments");
            writer.WriteString(" ");
            writer.WriteEndElement();
            
            writer.WriteEndDocument();
        }
    }
}