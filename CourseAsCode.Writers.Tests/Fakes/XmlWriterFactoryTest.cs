using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.Tests.Fakes;

/// <summary>
/// An implementation of IXmlWriterFactory that writes to an in-memory StringWriter
/// </summary>
public class XmlWriterFactoryTest : IXmlWriterFactory
{
    public StringWriter StringWriter { get; } = new StringWriter();

    /// <summary>
    /// Creates an XmlWriter that writes to the in-memory StringWriter
    /// </summary>
    public XmlWriter Create(string filePath, XmlWriterSettings settings)
    {
        // Configure settings to ensure proper XML formatting
        if (settings == null)
        {
            settings = new XmlWriterSettings();
        }

        settings.ConformanceLevel = ConformanceLevel.Document;
        settings.Indent = true;
        settings.OmitXmlDeclaration = false;

        return XmlWriter.Create(StringWriter, settings);
    }

    /// <summary>
    /// Clears the internal StringWriter to prepare for a new test
    /// </summary>
    public void ClearContents()
    {
        StringWriter.GetStringBuilder().Clear();
    }
}
