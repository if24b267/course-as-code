using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;

public class ActivityCommentsWriter : XmlWriterBase
{
    /// <summary>
    /// writes the comments.xml file required for an activity
    /// </summary>
    /// <param name="outputPath"></param>
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