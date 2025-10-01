using System.Xml;

using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class FiltersWriter : XmlWriterBase
    {
        private const string DEFAULT_FILTERS_RELATIVE_PATH = "course/filters.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="FiltersWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="FiltersWriter"/></param>
        public FiltersWriter(IXmlWriterFactory xmlWriterFactory, ILogger<FiltersWriter> logger)
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
                    writer.WriteEndElement(); // Close </filter_actives>

                    writer.WriteStartElement("filter_configs");
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
