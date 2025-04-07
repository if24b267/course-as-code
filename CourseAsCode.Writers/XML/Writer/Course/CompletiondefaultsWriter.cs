using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CompletiondefaultsWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPLETION_DEFAULTS_RELATIVE_PATH = "course/completiondefaults.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public CompletiondefaultsWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public CompletiondefaultsWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteCompletionDefaults(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPLETION_DEFAULTS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_completion_defaults");
                    writer.WriteEndElement(); // Close </course_completion_defaults>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_COMPLETION_DEFAULTS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_COMPLETION_DEFAULTS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}