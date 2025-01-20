using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class InforefWriterLabel : XmlWriterBase
{
    public void WriteInforefLabel(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "inforef.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("inforef");

        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");

        writer.WriteEndElement(); // Close <events>

        writer.WriteEndDocument();
    }
}