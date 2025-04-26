using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Logic;

/// <summary>
/// Default implementation of the IXmlWriterFactory Interface that creates an XmlWriter using XmlWriter.Create.
/// </summary>
public class DefaultXmlWriterFactory : IXmlWriterFactory
{
    public XmlWriter Create(string filePath, XmlWriterSettings settings)
    {
        return XmlWriter.Create(filePath, settings);
    }
}
