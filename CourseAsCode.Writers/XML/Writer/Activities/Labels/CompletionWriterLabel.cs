using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class CompletionWriterLabel : XmlWriterBase
{
    public void WriteCompletion(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "completion.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("completions");
        writer.WriteStartElement("completionviews");
        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <completionviews>
        writer.WriteEndElement(); // Close <completions>

        writer.WriteEndDocument();
    }
}