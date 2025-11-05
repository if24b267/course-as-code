using System.ComponentModel;
using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Logic
{
    public class XmlService : IXmlService
    {
        [Description("The XmlWriter initialized on the document that will be written to")]
        private XmlWriter _xmlWriter;
        private readonly ILogger<XmlService> _logger;

        private bool documentStarted = false;
        private bool documentEnded = false;
        private int openElements = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlService"/> class.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the document that will be written to</param>
        /// <param name="logger">Logger used for logging messages</param>
        /// <exception cref="ArgumentNullException">Thrown if XmlWriter or logger is null.</exception>
        public XmlService(XmlWriter writer, ILogger<XmlService> logger)
        {
            _xmlWriter = writer ?? throw new ArgumentNullException(nameof(writer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Writes the start tag for the specified XML element to the file to which XmlHelper is initialized
        /// </summary>
        /// <param name="elementName">Name of the XML tag</param>
        public void WriteElement(string elementName)
        {
            if (!documentStarted)
            {
                StartDocument();
                documentStarted = true;
            }

            _xmlWriter.WriteStartElement(elementName); openElements++;
        }

        /// <summary>
        /// Writes the end tag for the current XML element to the file to which XmlHelper is initialized
        /// </summary>
        public void CloseElement()
        {
            if (openElements <= 0)
            {
                _logger.LogWarning("XmlHelper: There are no open elements in the XML document. Cannot close an element.");
                return;
            }

            _xmlWriter.WriteEndElement(); openElements--;
        }

        /// <summary>
        /// Writes all key-value pairs from the provided dictionary as XML tags to the file to which XmlHelper is initialized
        /// </summary>
        /// <param name="elements">Dictionary (key-value pairs) representing XML tag names and their values.</param>
        public void WriteElements(Dictionary<string, string> elements)
        {
            if (!documentStarted)
            {
                StartDocument();
                documentStarted = true;
            }

            foreach (var element in elements)
            {
                _xmlWriter.WriteElementString(element.Key, element.Value);
            }
        }

        /// <summary>
        /// Writes a single XML tag with the provided attributes to the file to which XmlHelper is initialized
        /// </summary>
        /// <param name="elementName">Name of the XML tag.</param>
        /// <param name="attributes">Dictionary (key-value pairs) representing the XML tag attributes' names and values.</param>
        public void WriteElementWithAttributes(string elementName, Dictionary<string, string> attributes)
        {
            if (!documentStarted)
            {
                StartDocument();
                documentStarted = true;
            }

            _xmlWriter.WriteStartElement(elementName); openElements++;
            foreach (var attribute in attributes)
            {
                _xmlWriter.WriteAttributeString(attribute.Key, attribute.Value);
            }
        }

        /// <summary>
        /// Writes whitespace inside the current XML element to the file to which XmlHelper is initialized
        /// </summary>
        public void WriteWhitespace(string whitespace = " ")
        {
            if (openElements <= 0)
            {
                _logger.LogWarning("XmlHelper: There are no open elements in the XML document. Cannot write whitespace.");
                return;
            }

            _xmlWriter.WriteWhitespace(whitespace);
        }

        /// <summary>
        /// Starts the XML document by writing the XML declaration to the file to which the XmlWriter is initialized
        /// </summary>
        private void StartDocument()
        {
            _xmlWriter.WriteStartDocument();
        }

        /// <summary>
        /// Ends the XML document by properly closing the XML document and disposing of any resources used by the XmlWriter
        /// </summary>
        private void EndDocument()
        {
            if (documentEnded)
            {
                return;
            }

            documentEnded = true;
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Dispose();
        }

        /// <summary>
        /// Closes the XML document and releases resources used by the XmlWriter
        /// </summary>
        public void Dispose()
        {
            if (openElements > 0)
            {
                _logger.LogWarning("XmlHelper: There are still open elements in the XML document that need to be closed. Closing them now...");
                for (int i = 0; i < openElements; i++)
                {
                    _xmlWriter.WriteEndElement();
                }
            }

            EndDocument();
            GC.SuppressFinalize(this);
        }
    }
}
