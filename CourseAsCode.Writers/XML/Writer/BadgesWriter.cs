using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class BadgesWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_BADGES_RELATIVE_PATH = "badges.xml";
        
        public void WriteBadges(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_BADGES_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("badges");

                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");

                    writer.WriteEndElement(); // Close <badges>
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_BADGES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_BADGES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}