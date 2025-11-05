namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface IXmlHelper : IDisposable
    {
        /// <summary>
        /// Writes the start tag for the specified XML element to the file to which XmlHelper is initialized
        /// </summary>
        /// <param name="elementName">Name of the XML tag</param>
        void WriteElement(string elementName);

        /// <summary>
        /// Writes the end tag for the current XML element to the file to which XmlHelper is initialized
        /// </summary>
        void CloseElement();

        /// <summary>
        /// Writes all key-value pairs from the provided dictionary as XML tags to the file to which XmlHelper is initialized
        /// </summary>
        /// <param name="elements">Dictionary (key-value pairs) representing XML tag names and their values.</param>
        void WriteElements(Dictionary<string, string> elements);

        /// <summary>
        /// Writes a single XML tag with the provided attributes to the file to which XmlHelper is initialized
        /// </summary>
        /// <param name="elementName">Name of the XML tag.</param>
        /// <param name="attributes">Dictionary (key-value pairs) representing the XML tag attributes' names and values.</param>
        void WriteElementWithAttributes(string elementName, Dictionary<string, string> attributes);

        /// <summary>
        /// Writes whitespace inside the current XML element to the file to which XmlHelper is initialized
        /// </summary>
        void WriteWhitespace(string whitespace = " ");
    }
}
