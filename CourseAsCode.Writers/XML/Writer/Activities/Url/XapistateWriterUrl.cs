using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.Url;

public class XapistateWriterUrl : XmlWriterBase
{
    public void WriteState(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "xapistate.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("states");

        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");

        writer.WriteEndElement(); // Close <states>

        writer.WriteEndDocument();
    }
}