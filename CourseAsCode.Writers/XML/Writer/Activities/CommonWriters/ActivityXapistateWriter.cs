using System.Xml;

using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityXapistateWriter : XmlWriterBase
    {
        private const string DEFAULT_XAPISTATE_RELATIVE_PATH = "xapistate.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityXapistateWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityXapistateWriter"/></param>
        public ActivityXapistateWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityXapistateWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the xapistate.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteState(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_XAPISTATE_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("states");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <states>

                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_XAPISTATE_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_XAPISTATE_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
