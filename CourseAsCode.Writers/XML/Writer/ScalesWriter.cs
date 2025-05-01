using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class ScalesWriter : XmlWriterBase
    {
        private const string DEFAULT_SCALES_RELATIVE_PATH = "scales.xml";


        /// <summary>
        /// Creates a new instance of the ScalesWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to ScalesWriter</param>
        public ScalesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ScalesWriter> logger)
            : base(xmlWriterFactory, logger)
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
