using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class ContentbankWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_CONTENTBANK_RELATIVE_PATH = "course/contentbank.xml";
        
        public void WriteContentbank(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_CONTENTBANK_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("contents");

                    writer.WriteString("\n");

                    writer.WriteEndElement(); // Close </contents>
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_CONTENTBANK_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_CONTENTBANK_RELATIVE_PATH}");
                throw;
            }
        }
    }
}