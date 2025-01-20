using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;

public class ActivityXapistateWriter : XmlWriterBase
{
    /// <summary>
    /// writes the xapistate.xml file required for an activity
    /// </summary>
    /// <param name="outputPath"></param>
    public void WriteState(string outputPath)
    {
        string filePath = Path.GetFullPath(Path.Combine(outputPath, "xapistate.xml"));
        using XmlWriter writer = XmlWriter.Create(filePath, Settings);

        writer.WriteStartDocument();

        writer.WriteStartElement("states");
        writer.WriteWhitespace("\n");
        writer.WriteEndElement(); // Close <states>

        writer.WriteEndDocument();
    }
}