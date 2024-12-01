using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class CompletiondefaultsWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };
    
    public void WriteCompletionDefaults(string filePath = "course/completiondefaults.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            
            writer.WriteStartElement("course_completion_defaults");
            writer.WriteString(" ");
            writer.WriteEndElement();
            
            writer.WriteEndDocument();
        }
    }
}