using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.Tests.Fakes;

/// <summary>
/// An implementation of IXmlWriterFactory that writes to an in-memory StringWriter
/// </summary>
public class XmlWriterFactoryTest : IXmlWriterFactory
{
    public StringWriter StringWriter { get; } = new StringWriter();

    public XmlWriter Create(string filePath, XmlWriterSettings settings)
    {
        return XmlWriter.Create(StringWriter, settings);
    }
}