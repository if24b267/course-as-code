using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class CompletionWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_COMPLETION_RELATIVE_PATH = "completion.xml";
        
        /// <summary>
        /// Stores the information about which users have completed the course; NOT IMPLEMENTED (yet?)
        /// </summary>
        public void WriteCompletion(Model.Course course, string outputPath = "completion.xml")
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPLETION_RELATIVE_PATH));

                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                
                    writer.WriteStartElement("course_completion");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close </course_completion>
                
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_COMPLETION_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_COMPLETION_RELATIVE_PATH}");
                throw;
            }
        }
    }
}