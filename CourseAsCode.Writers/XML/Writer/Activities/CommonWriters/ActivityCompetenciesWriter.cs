using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCompetenciesWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_COMPETENCIES_RELATIVE_PATH = "competencies.xml";
        
        
        /// <summary>
        /// Writes the competencies.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteCompetencies(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPETENCIES_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_module_competencies");
                    
                    writer.WriteStartElement("competencies");
                    
                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");
                    
                    writer.WriteEndElement(); // Close <competencies>
                    writer.WriteEndElement(); // Close <course_module_competencies>
                    writer.WriteEndDocument();
                }
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_COMPETENCIES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_COMPETENCIES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}