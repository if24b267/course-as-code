using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class ScalesWriter : XmlWriterBase
    {
        private const string DEFAULT_SCALES_RELATIVE_PATH = "scales.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public ScalesWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public ScalesWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger) : base(xmlWriterFactory, logger)
        {
        }

        public void WriteScales(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_SCALES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("scales_definition");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close <scales_definition>

                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_SCALES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_SCALES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}