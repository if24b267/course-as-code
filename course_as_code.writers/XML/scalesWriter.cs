using System;
using System.Xml;

namespace course_as_code.writers.XML;

public class ScalesWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteScales(string filePath = "scales.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("scales_definition"); // Root element
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close </scales_definition>

            writer.WriteEndDocument();
        }
    }
}