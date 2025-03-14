using System.Xml;

namespace CourseAsCode.Writers.XML.Writer
{
    public class BadgesWriter : XmlWriterBase
    {
        private const string DEFAULT_BADGES_RELATIVE_PATH = "badges.xml";
        
        /// <summary>
        /// Writes the badges XML file to the specified output path
        /// </summary>
        /// <param name="outputPath">The directory where the badges.xml file will be created</param>
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