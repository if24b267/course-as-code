using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Logic
{
    public class XmlServiceFactory : IXmlServiceFactory
    {
        private readonly ILogger<XmlServiceFactory> _xmlHelperFactoryLogger;
        private readonly ILogger<XmlService> _xmlHelperLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlServiceFactory"/> class.
        /// </summary>
        /// <param name="logger">Logger used for logging messages</param>
        /// <exception cref="ArgumentNullException">Thrown if logger is null.</exception>
        public XmlServiceFactory(ILogger<XmlServiceFactory> logger, ILogger<XmlService> xmlHelperLogger)
        {
            _xmlHelperFactoryLogger = logger ?? throw new ArgumentNullException(nameof(logger));
            _xmlHelperLogger = xmlHelperLogger ?? throw new ArgumentNullException(nameof(xmlHelperLogger));
        }

        /// <summary>
        /// Creates an instance of the XmlHelper class, initialized to write to the specified file
        /// </summary>
        /// <param name="filePath">The file path where the XML content will be written.</param>
        /// <returns>An instance of XmlHelper.</returns>
        public IXmlService Create(string filePath)
        {
            var writer = XmlWriter.Create(filePath, _settings);
            return new XmlService(writer, _xmlHelperLogger);
        }

        /// <summary>
        /// Creates an instance of the XmlHelper class with the pre-initialized XmlWriter
        /// </summary>
        /// <param name="writer">The pre-initialized XmlWriter, initialized on the correct document.</param>
        /// <returns>An instance of XmlHelper.</returns>
        public IXmlService Create(XmlWriter writer)
        {
            return new XmlService(writer, _xmlHelperLogger);
        }

        /// <summary>
        /// The settings used for creating XmlWriter instances.
        /// </summary>
        private readonly XmlWriterSettings _settings = new()
        {
            Indent = true,
            NewLineOnAttributes = false,
            OmitXmlDeclaration = false,
        };
    }
}
