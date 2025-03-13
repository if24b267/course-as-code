using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class QuestionsWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_QUESTIONS_RELATIVE_PATH = "questions.xml";
        
        public void WriteQuestions(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_QUESTIONS_RELATIVE_PATH));

                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    
                    writer.WriteStartElement("question_categories");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close <question_categories>
                    
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_QUESTIONS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_QUESTIONS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}