using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityFiltersWriter : XmlWriterBase
    {
        private const string DEFAULT_FILTERS_RELATIVE_PATH = "filters.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityFiltersWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityFiltersWriter"/></param>
        public ActivityFiltersWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityFiltersWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the filters.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteFilters(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FILTERS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("filters");

                    writer.WriteStartElement("filter_actives");

                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <filter_actives>

                    writer.WriteStartElement("filter_configs");

                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <filter_configs>

                    writer.WriteEndElement(); // Close <filters>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_FILTERS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_FILTERS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
