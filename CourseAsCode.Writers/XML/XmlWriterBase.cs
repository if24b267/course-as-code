using System.Xml;

namespace CourseAsCode.Writers.XML;

public abstract class XmlWriterBase
{
    protected readonly XmlWriterSettings Settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = false
    };

    
    /// <summary>
    /// writes all key-value pairs from the provided dictionary as XML tags to the file to which the XmlWriter is initialized
    /// </summary>
    /// <param name="writer">XmlWriter initialized on the correct document</param>
    /// <param name="elements">Dictionary (key-value pairs) representing XML Tag + value</param>
    protected void WriteElements(XmlWriter writer, Dictionary<string, string> elements)
    {
        foreach (var element in elements)
        {
            writer.WriteElementString(element.Key, element.Value);
        }
    }

    
    /// <summary>
    /// writes a single XML tag with the provided attributes to the file to which the XmlWriter is initialized
    /// </summary>
    /// <param name="writer">XmlWriter initialized on the correct document</param>
    /// <param name="elementName">Name of the XML tag</param>
    /// <param name="attributes">Dictionary (key-value pairs) representing the xml tag attributes' name and value</param>
    protected void WriteStartElementWithAttributes(XmlWriter writer, string elementName, Dictionary<string, string> attributes)
    {
        writer.WriteStartElement(elementName);
        foreach (var attribute in attributes)
        {
            writer.WriteAttributeString(attribute.Key, attribute.Value);
        }
    }
}
