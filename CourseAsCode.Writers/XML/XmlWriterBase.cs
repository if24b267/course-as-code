using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML
{
    public abstract class XmlWriterBase
    {
        /// <summary>
        /// The settings used for creating XmlWriter instances
        /// </summary>
        protected readonly XmlWriterSettings Settings = new()
        {
            Indent = true,
            NewLineOnAttributes = false,
            OmitXmlDeclaration = false
        };

        /// <summary>
        /// The logger used for logging messages at various levels.
        /// </summary>
        protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Writes all key-value pairs from the provided dictionary as XML tags to the file to which the XmlWriter is initialized
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="elements">Dictionary (key-value pairs) representing XML tag names and their values</param>
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
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="elementName">Name of the XML tag</param>
        /// <param name="attributes">Dictionary (key-value pairs) representing the XML tag attributes' names and values</param>
        protected void WriteStartElementWithAttributes(XmlWriter writer, string elementName, Dictionary<string, string> attributes)
        {
            writer.WriteStartElement(elementName);
            foreach (var attribute in attributes)
            {
                writer.WriteAttributeString(attribute.Key, attribute.Value);
            }
        }

        /// <summary>
        /// Creates and returns an instance of an XmlWriter based on the provided file path and the current settings.
        /// This method is virtual so that derived classes can override it to customize the XmlWriter creation process,
        /// such as writing to an in-memory stream for testing.
        /// </summary>
        /// <param name="filePath">The file path where the XML content will be written</param>
        /// <returns>An XmlWriter instance initialized with the current settings</returns>
        protected virtual XmlWriter CreateXmlWriter(string filePath)
        {
            return XmlWriter.Create(filePath, Settings);
        }
        
        // The following methods are all virtual to allow for testing and customization in derived classes
        // -> They can be overridden in derived classes to provide custom logging behavior (or mock logging for testing)

        /// <summary>
        /// Logs a trace-level message using the configured logger
        /// </summary>
        /// <param name="message">The trace message to log</param>
        protected virtual void LogTrace(string message)
        {
            Logger.Trace(message);
        }

        /// <summary>
        /// Logs an informational message using the configured logger
        /// </summary>
        /// <param name="message">The informational message to log</param>
        protected virtual void LogInfo(string message)
        {
            Logger.Info(message);
        }

        /// <summary>
        /// Logs a warning-level message using the configured logger
        /// </summary>
        /// <param name="message">The warning message to log</param>
        protected virtual void LogWarn(string message)
        {
            Logger.Warn(message);
        }

        /// <summary>
        /// Logs a warning-level message along with an associated exception using the configured logger
        /// </summary>
        /// <param name="ex">The exception associated with the warning</param>
        /// <param name="message">The warning message to log</param>
        protected virtual void LogWarn(Exception ex, string message)
        {
            Logger.Warn(ex, message);
        }

        /// <summary>
        /// Logs an error-level message along with an associated exception using the configured logger
        /// </summary>
        /// <param name="ex">The exception associated with the error</param>
        /// <param name="message">The error message to log</param>
        protected virtual void LogError(Exception ex, string message)
        {
            Logger.Error(ex, message);
        }

        /// <summary>
        /// Logs a fatal-level message using the configured logger.
        /// </summary>
        /// <param name="message">The fatal error message to log</param>
        protected virtual void LogFatal(string message)
        {
            Logger.Fatal(message);
        }

        /// <summary>
        /// Logs a fatal-level message along with an associated exception using the configured logger
        /// </summary>
        /// <param name="ex">The exception associated with the fatal error</param>
        /// <param name="message">The fatal error message to log</param>
        protected virtual void LogFatal(Exception ex, string message)
        {
            Logger.Fatal(ex, message);
        }

        /// <summary>
        /// Logs a debug-level message using the configured logger
        /// </summary>
        /// <param name="message">The debug message to log</param>
        protected virtual void LogDebug(string message)
        {
            Logger.Debug(message);
        }
    }
}
