using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.Url;

public class CommentsWriterUrl : XmlWriterBase
{
    public void WriteComments(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "comments.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("comments");

        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");

        writer.WriteEndElement(); // Close <comments>

        writer.WriteEndDocument();
    }
}