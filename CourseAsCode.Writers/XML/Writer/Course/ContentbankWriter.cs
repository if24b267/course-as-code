using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class ContentbankWriter : XmlWriterBase
    {
        private const string DEFAULT_CONTENTBANK_RELATIVE_PATH = "course/contentbank.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ContentbankWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ContentbankWriter"/></param>
        public ContentbankWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ContentbankWriter> logger)
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
