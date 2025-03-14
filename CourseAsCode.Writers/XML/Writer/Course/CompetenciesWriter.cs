using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CompetenciesWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_COMPETENCIES_RELATIVE_PATH = "course/competencies.xml";
        
        public void WriteCompetencies(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPETENCIES_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_competencies");

                    writer.WriteStartElement("competencies");
                    writer.WriteString("\n  ");
                    writer.WriteEndElement(); // Close </competencies>

                    writer.WriteStartElement("user_competencies");
                    writer.WriteString("\n  ");
                    writer.WriteEndElement(); // Close </user_competencies>

                    writer.WriteEndElement(); // Close </course_competencies>
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_COMPETENCIES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_COMPETENCIES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}