using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.UnitTests.Fakes;

/// <summary>
/// An implementation of IXmlWriterFactory that always throws an exception
/// </summary>
public class ExceptionThrowingXmlWriterFactory : IXmlWriterFactory
{
    public XmlWriter Create(string filePath, XmlWriterSettings settings)
    {
        throw new InvalidOperationException("Test exception");
    }
}