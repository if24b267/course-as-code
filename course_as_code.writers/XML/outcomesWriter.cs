using System;
using System.Xml;

namespace course_as_code.writers.XML;

public class OutcomesWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteOutcomes(string filePath = "outcomes.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("outcomes_definition"); // Root element
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close </outcomes_definition>

            writer.WriteEndDocument();
        }
    }
}