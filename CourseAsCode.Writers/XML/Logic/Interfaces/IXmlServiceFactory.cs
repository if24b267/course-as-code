using System.Xml;

namespace CourseAsCode.Writers.XML.Logic.Interfaces
{
    public interface IXmlServiceFactory
    {
        /// <summary>
        /// Creates an instance of the XmlHelper class, initialized to write to the specified file
        /// </summary>
        /// <param name="filePath">The file path where the XML content will be written.</param>
        /// <returns>An instance of XmlHelper.</returns>
        IXmlService Create(string filePath);

        /// <summary>
        /// Creates an instance of the XmlHelper class with the pre-initialized XmlWriter
        /// </summary>
        /// <param name="writer">The pre-initialized XmlWriter, initialized on the correct document.</param>
        /// <returns>An instance of XmlHelper.</returns>
        IXmlService Create(XmlWriter writer);
    }
}
