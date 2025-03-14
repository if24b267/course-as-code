using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class GradehistoryWriter : XmlWriterBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_GRADEHISTORY_RELATIVE_PATH = "gradehistory.xml";
        
        public void WriteGradehistory(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GRADEHISTORY_RELATIVE_PATH));

                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("grade_history");

                    WriteGradeGradesSection(writer, course);

                    writer.WriteEndElement(); // Close <grade_history>
                    writer.WriteEndDocument();
                }
                
                _logger.Debug($"Created {DEFAULT_GRADEHISTORY_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error writing {DEFAULT_GRADEHISTORY_RELATIVE_PATH}");
                throw;
            }
        }
        
        
        /// <summary>
        /// Writes the `grade_grades` section of the gradehistory.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">Initialized Moodle Course.</param>
        private void WriteGradeGradesSection(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("grade_grades");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <grade_grades>
        }
    }
}