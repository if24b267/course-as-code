using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class FiltersWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_FILTERS_RELATIVE_PATH = "course/filters.xml";
        
        public void WriteFilters(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FILTERS_RELATIVE_PATH));
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("filters");

                    writer.WriteStartElement("filter_actives");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close </filter_actives>

                    writer.WriteStartElement("filter_configs");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close </filter_configs>

                    writer.WriteEndElement(); // Close </filters>
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_FILTERS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_FILTERS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}