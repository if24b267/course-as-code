using System.Xml;

namespace CourseAsCode.Writers.XML;

public class OutcomesWriter : XmlWriterBase
{
    public void WriteOutcomes(string filePath = "outcomes.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("outcomes_definition");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <outcomes_definition>
            writer.WriteEndDocument();
        }
    }
}
