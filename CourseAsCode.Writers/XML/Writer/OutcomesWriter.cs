using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class OutcomesWriter : XmlWriterBase
    {
        private const string DEFAULT_OUTCOMES_RELATIVE_PATH = "outcomes.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public OutcomesWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public OutcomesWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteOutcomes(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_OUTCOMES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("outcomes_definition");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close <outcomes_definition>

                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_OUTCOMES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_OUTCOMES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
