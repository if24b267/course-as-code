using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityFiltersWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_FILTERS_RELATIVE_PATH = "filters.xml";
        
        
        /// <summary>
        /// Writes the filters.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteFilters(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FILTERS_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("filters");
                    
                    writer.WriteStartElement("filter_actives");
                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <filter_actives>
                    
                    writer.WriteStartElement("filter_configs");
                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <filter_configs>
                    
                    writer.WriteEndElement(); // Close <filters>
                    writer.WriteEndDocument();
                }
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_FILTERS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_FILTERS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
