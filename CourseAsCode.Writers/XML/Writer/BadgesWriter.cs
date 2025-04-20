using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class BadgesWriter : XmlWriterBase
    {
        private const string DEFAULT_BADGES_RELATIVE_PATH = "badges.xml";

        /// <summary>
        /// Default constructor, providing default implementations for the XmlWriterFactory and logger.
        /// </summary>
        public BadgesWriter()
        {
            // Calls the default constructor of the base class, which initializes the logger and the XmlWriterFactory with default implementations
        }

        /// <summary>
        /// Initializes a new instance of the BadgesWriter class with the specified XML writer factory and logger.
        /// </summary>
        /// <param name="xmlWriterFactory">Factory used to create XmlWriter instances.</param>
        /// <param name="logger">Logger used for logging messages.</param>
        public BadgesWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
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

                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");

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
