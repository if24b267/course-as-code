using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class ContentbankWriter : XmlWriterBase
    {
        private const string DEFAULT_CONTENTBANK_RELATIVE_PATH = "course/contentbank.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public ContentbankWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public ContentbankWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteContentbank(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_CONTENTBANK_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("contents");

                    writer.WriteString("\n");

                    writer.WriteEndElement(); // Close </contents>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_CONTENTBANK_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_CONTENTBANK_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
