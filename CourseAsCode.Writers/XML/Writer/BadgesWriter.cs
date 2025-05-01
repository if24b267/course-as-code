using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class BadgesWriter : XmlWriterBase
    {
        private const string DEFAULT_BADGES_RELATIVE_PATH = "badges.xml";


        /// <summary>
        /// Creates a new instance of the BadgesWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to BadgesWriter</param>
        public BadgesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<BadgesWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the badges XML file to the specified output path.
        /// </summary>
        /// <param name="outputPath">The directory where the badges.xml file will be created.</param>
        public void WriteBadges(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_BADGES_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("badges");
                    writer.WriteEndElement(); // Close <badges>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_BADGES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_BADGES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
