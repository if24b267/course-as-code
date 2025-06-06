using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class OutcomesWriter : XmlWriterBase
    {
        private const string DEFAULT_OUTCOMES_RELATIVE_PATH = "outcomes.xml";


        /// <summary>
        /// Creates a new instance of the OutcomesWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to OutcomesWriter</param>
        public OutcomesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<OutcomesWriter> logger)
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
