using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML
{
    /// <summary>
    /// Base class for XML writer classes. Provides common XML settings, a mechanism to create XmlWriter instances via an injected factory and logging capabilities.
    /// </summary>
    public abstract class XmlWriterBase
    {
        protected readonly ILogger Logger;
        private readonly IXmlWriterFactory _xmlWriterFactory;


        /// <summary>
        /// Initializes a new instance of the XmlWriterBase class with the specified
        /// XML writer factory and logger provided via Dependency Injection.
        /// </summary>
        /// <param name="xmlWriterFactory">Factory used to create XmlWriter instances</param>
        /// <param name="logger">Logger used for logging messages</param>
        /// <exception cref="ArgumentNullException">Thrown if xmlWriterFactory or logger is null.</exception>
        protected XmlWriterBase(IXmlWriterFactory xmlWriterFactory, ILogger logger)
        {
            _xmlWriterFactory = xmlWriterFactory ?? throw new ArgumentNullException(nameof(xmlWriterFactory));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// The settings used for creating XmlWriter instances.
        /// </summary>
        protected readonly XmlWriterSettings Settings = new()
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
        protected void WriteElements(XmlWriter writer, Dictionary<string, string> elements)
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
        protected void WriteStartElementWithAttributes(XmlWriter writer, string elementName, Dictionary<string, string> attributes)
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
        protected XmlWriter CreateXmlWriter(string filePath)
        {
            return _xmlWriterFactory.Create(filePath, Settings);
        }

        /// <summary>
        /// Logs a trace-level message using the configured logger.
        /// </summary>
        /// <param name="message">The trace message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogTrace(string message, params object?[] args)
        {
            Logger.LogTrace(message, args);
        }

        /// <summary>
        /// Logs an informational message using the configured logger.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogInfo(string message, params object?[] args)
        {
            Logger.LogInformation(message, args);
        }

        /// <summary>
        /// Logs a warning-level message using the configured logger.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogWarn(string message, params object?[] args)
        {
            Logger.LogWarning(message, args); // Note: LogWarning
        }

        /// <summary>
        /// Logs a warning-level message along with an associated exception.
        /// </summary>
        /// <param name="ex">The exception associated with the warning.</param>
        /// <param name="message">The warning message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogWarn(Exception? ex, string message, params object?[] args)
        {
            Logger.LogWarning(ex, message, args); // Note: LogWarning
        }

        /// <summary>
        /// Logs an error-level message along with an associated exception.
        /// </summary>
        /// <param name="ex">The exception associated with the error.</param>
        /// <param name="message">The error message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogError(Exception? ex, string message, params object?[] args)
        {
            Logger.LogError(ex, message, args);
        }

        /// <summary>
        /// Logs an error-level message using the configured logger.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogError(string message, params object?[] args)
        {
            Logger.LogError(message, args);
        }

        /// <summary>
        /// Logs a fatal-level message using the configured logger.
        /// </summary>
        /// <param name="message">The fatal error message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogFatal(string message, params object?[] args)
        {
            Logger.LogCritical(message, args);
        }

        /// <summary>
        /// Logs a fatal-level message along with an associated exception.
        /// </summary>
        /// <param name="ex">The exception associated with the fatal error.</param>
        /// <param name="message">The fatal error message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogFatal(Exception? ex, string message, params object?[] args)
        {
            Logger.LogCritical(ex, message, args);
        }

        /// <summary>
        /// Logs a debug-level message using the configured logger.
        /// </summary>
        /// <param name="message">The debug message to log.</param>
        /// <param name="args">Optional arguments for string formatting.</param>
        protected void LogDebug(string message, params object?[] args)
        {
            Logger.LogDebug(message, args);
        }
    }
}
