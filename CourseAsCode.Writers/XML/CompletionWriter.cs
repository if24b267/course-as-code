using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class CompletionWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true, // Makes the XML output more readable
        NewLineOnAttributes = false, // Keeps attributes in the same line
        OmitXmlDeclaration = true // Removes the XML declaration
    };
    
    public void WriteCompletion(string filePath = "completion.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            // <course_completion> starts here
            writer.WriteStartElement("course_completion");

            // </course_completion> ends here
            writer.WriteEndElement();

            writer.WriteEndDocument();
        }
    }
}