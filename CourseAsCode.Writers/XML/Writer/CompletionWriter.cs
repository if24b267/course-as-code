using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class CompletionWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPLETION_RELATIVE_PATH = "completion.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public CompletionWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Initializes a new instance of the CompletionWriter class with the specified XML writer factory and logger.
        /// </summary>
        /// <param name="xmlWriterFactory">Factory used to create XmlWriter instances.</param>
        /// <param name="logger">Logger used for logging messages.</param>
        public CompletionWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        /// <summary>
        /// Stores the information about which users have completed the course; NOT IMPLEMENTED (yet?).
        /// </summary>
        /// <param name="course">The course data.</param>
        /// <param name="outputPath">The directory where the completion.xml file will be created.</param>
        public void WriteCompletion(Model.Course course, string outputPath = "completion.xml")
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPLETION_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_completion");

                    // Add whitespace to prevent a self-closing tag
                    //writer.WriteWhitespace("\n");

                    writer.WriteEndElement(); // Close </course_completion>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_COMPLETION_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_COMPLETION_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
