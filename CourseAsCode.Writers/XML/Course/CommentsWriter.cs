using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class CommentsWriter : XmlWriterBase
{
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

