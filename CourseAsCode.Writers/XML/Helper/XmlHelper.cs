using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Helper
{
    public class XmlHelper : IXmlHelper
    {
        private readonly ILogger<XmlHelper> _logger;
        private readonly IXmlWriterFactory _xmlWriterFactory;


        /// <summary>
        /// Initializes a new instance of the <see cref="XmlHelper"/> class.
        /// </summary>
        /// <param name="xmlWriterFactory">Factory used to create XmlWriter instances</param>
        /// <param name="logger">Logger used for logging messages</param>
        /// <exception cref="ArgumentNullException">Thrown if xmlWriterFactory or logger is null.</exception>
        public XmlHelper(IXmlWriterFactory xmlWriterFactory, ILogger<XmlHelper> logger)
        {
            _xmlWriterFactory = xmlWriterFactory ?? throw new ArgumentNullException(nameof(xmlWriterFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// The settings used for creating XmlWriter instances.
        /// </summary>
        private readonly XmlWriterSettings Settings = new()
        {
            Indent = true,
            NewLineOnAttributes = false,
            OmitXmlDeclaration = false,
        };

        /// <summary>
        /// Writes all key-value pairs from the provided dictionary as XML tags to the file to which the XmlWriter is initialized.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="elements">Dictionary (key-value pairs) representing XML tag names and their values.</param>
        public void WriteElements(XmlWriter writer, Dictionary<string, string> elements)
        {
            foreach (var element in elements)
            {
                writer.WriteElementString(element.Key, element.Value);
            }
        }

        /// <summary>
        /// Writes a single XML tag with the provided attributes to the file to which the XmlWriter is initialized.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="elementName">Name of the XML tag.</param>
        /// <param name="attributes">Dictionary (key-value pairs) representing the XML tag attributes' names and values.</param>
        public void WriteElementWithAttributes(XmlWriter writer, string elementName, Dictionary<string, string> attributes)
        {
            writer.WriteStartElement(elementName);
            foreach (var attribute in attributes)
            {
                writer.WriteAttributeString(attribute.Key, attribute.Value);
            }
        }

        /// <summary>
        /// Creates an XmlWriter instance using the injected factory.
        /// </summary>
        /// <param name="filePath">The file path where the XML content will be written.</param>
        /// <returns>An XmlWriter instance initialized with the current settings.</returns>
        public XmlWriter CreateXmlWriter(string filePath)
        {
            return _xmlWriterFactory.Create(filePath, Settings);
        }
    }
}
