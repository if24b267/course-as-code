using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityGradehistoryWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_GRADE_HISTORY_RELATIVE_PATH = "grade_history.xml";
        
        
        /// <summary>
        /// Writes the grade_history.xml file required for an activity.
        /// </summary>
        /// <param name="course">The course for which grade history is generated.</param>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteGradeHistory(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GRADE_HISTORY_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("grade_history");

                    WriteGradeGradesSection(writer, course);

                    writer.WriteEndElement(); // Close <grade_history>
                    writer.WriteEndDocument();
                }
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_GRADE_HISTORY_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_GRADE_HISTORY_RELATIVE_PATH}");
                throw;
            }
        }
        
        
        /// <summary>
        /// Writes the `grade_grades` section of the grade_history.xml file required for an activity.
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
