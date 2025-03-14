using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CompletiondefaultsWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_COMPLETION_DEFAULTS_RELATIVE_PATH = "course/completiondefaults.xml";
        
        public void WriteCompletionDefaults(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPLETION_DEFAULTS_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_completion_defaults");

                    writer.WriteString("\n");

                    writer.WriteEndElement(); // Close </course_completion_defaults>
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_COMPLETION_DEFAULTS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_COMPLETION_DEFAULTS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}