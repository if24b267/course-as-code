using System.Xml;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Logic
{
    public class XmlHelperFactory : IXmlHelperFactory
    {
        private readonly ILogger<XmlHelperFactory> _xmlHelperFactoryLogger;
        private readonly ILogger<XmlHelper> _xmlHelperLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlHelperFactory"/> class.
        /// </summary>
        /// <param name="logger">Logger used for logging messages</param>
        /// <exception cref="ArgumentNullException">Thrown if logger is null.</exception>
        public XmlHelperFactory(ILogger<XmlHelperFactory> logger, ILogger<XmlHelper> xmlHelperLogger)
        {
            _xmlHelperFactoryLogger = logger ?? throw new ArgumentNullException(nameof(logger));
            _xmlHelperLogger = xmlHelperLogger ?? throw new ArgumentNullException(nameof(xmlHelperLogger));
        }

        /// <summary>
        /// Creates an instance of the XmlHelper class, initialized to write to the specified file
        /// </summary>
        /// <param name="filePath">The file path where the XML content will be written.</param>
        /// <returns>An instance of XmlHelper.</returns>
        public IXmlHelper Create(string filePath)
        {
            var writer = XmlWriter.Create(filePath, _settings);
            return new XmlHelper(writer, _xmlHelperLogger);
        }

        /// <summary>
        /// Creates an instance of the XmlHelper class with the pre-initialized XmlWriter
        /// </summary>
        /// <param name="writer">The pre-initialized XmlWriter, initialized on the correct document.</param>
        /// <returns>An instance of XmlHelper.</returns>
        public IXmlHelper Create(XmlWriter writer)
        {
            return new XmlHelper(writer, _xmlHelperLogger);
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
