using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class ScalesWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_SCALES_RELATIVE_PATH = "scales.xml";
        
        public void WriteScales(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_SCALES_RELATIVE_PATH));

                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    
                    writer.WriteStartElement("scales_definition");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close <scales_definition>
                    
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_SCALES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_SCALES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}