using System.Xml;

namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface IXmlHelper
    {
        /// <summary>
        /// Writes all key-value pairs from the provided dictionary as XML tags to the file to which the XmlWriter is initialized.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="elements">Dictionary (key-value pairs) representing XML tag names and their values.</param>
        void WriteElements(XmlWriter writer, Dictionary<string, string> elements);

        /// <summary>
        /// Writes a single XML tag with the provided attributes to the file to which the XmlWriter is initialized.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="elementName">Name of the XML tag.</param>
        /// <param name="attributes">Dictionary (key-value pairs) representing the XML tag attributes' names and values.</param>
        void WriteElementWithAttributes(XmlWriter writer, string elementName, Dictionary<string, string> attributes);

        /// <summary>
        /// Creates an XmlWriter instance with predefined settings for writing XML content to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path where the XML content will be written.</param>
        /// <returns>An XmlWriter instance initialized with the current settings.</returns>
        XmlWriter CreateXmlWriter(string filePath);
    }
}
