using System.Xml;

namespace CourseAsCode.Writers.XML;

public abstract class XmlWriterBase
{
    protected readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    protected void WriteElements(XmlWriter writer, Dictionary<string, string> elements)
    {
        foreach (var element in elements)
        {
            writer.WriteElementString(element.Key, element.Value);
        }
    }

    protected void WriteStartElementWithAttributes(XmlWriter writer, string elementName, Dictionary<string, string> attributes)
    {
        writer.WriteStartElement(elementName);
        foreach (var attribute in attributes)
        {
            writer.WriteAttributeString(attribute.Key, attribute.Value);
        }
    }
}
