using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class OutcomesWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_OUTCOMES_RELATIVE_PATH = "outcomes.xml";
        
        public void WriteOutcomes(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_OUTCOMES_RELATIVE_PATH));

                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    
                    writer.WriteStartElement("outcomes_definition");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close <outcomes_definition>
                    
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_OUTCOMES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_OUTCOMES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}