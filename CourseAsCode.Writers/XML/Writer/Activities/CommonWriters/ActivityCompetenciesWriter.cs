using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCompetenciesWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPETENCIES_RELATIVE_PATH = "competencies.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityCompetenciesWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityCompetenciesWriter"/></param>
        public ActivityCompetenciesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityCompetenciesWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the competencies.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteCompetencies(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPETENCIES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_module_competencies");

                    writer.WriteStartElement("competencies");

                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");

                    writer.WriteEndElement(); // Close <competencies>
                    writer.WriteEndElement(); // Close <course_module_competencies>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_COMPETENCIES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_COMPETENCIES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
