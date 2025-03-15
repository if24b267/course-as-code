using System.Xml;
using NLog;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class FiltersWriter : XmlWriterBase
    {
        private const string DEFAULT_FILTERS_RELATIVE_PATH = "course/filters.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public FiltersWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public FiltersWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteFilters(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FILTERS_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("filters");

                    writer.WriteStartElement("filter_actives");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close </filter_actives>

                    writer.WriteStartElement("filter_configs");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close </filter_configs>

                    writer.WriteEndElement(); // Close </filters>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_FILTERS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_FILTERS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}