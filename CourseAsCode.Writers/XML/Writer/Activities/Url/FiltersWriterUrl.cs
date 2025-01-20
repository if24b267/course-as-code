using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.Url;

public class FiltersWriterUrl : XmlWriterBase
{
    public void WriteFilters(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "filters.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("filters");
        writer.WriteStartElement("filter_actives");
        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <filter_actives>
        writer.WriteStartElement("filter_configs");
        // Add whitespace to prevent a self-closing tag
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <filter_configs>
        writer.WriteEndElement(); // Close <filters>

        writer.WriteEndDocument();
    }
}