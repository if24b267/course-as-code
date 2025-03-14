using System.Xml;

namespace CourseAsCode.Writers.XML.Logic.Interfaces;

/// <summary>
/// Provides a mechanism to create XmlWriter instances
/// </summary>
public interface IXmlWriterFactory
{
    /// <summary>
    /// Creates an XmlWriter for the given file path and settings
    /// </summary>
    /// <param name="filePath">The output file path</param>
    /// <param name="settings">The XmlWriterSettings to use</param>
    /// <returns>An XmlWriter instance</returns>
    XmlWriter Create(string filePath, XmlWriterSettings settings);
}