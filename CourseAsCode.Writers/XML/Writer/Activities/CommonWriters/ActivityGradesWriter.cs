using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityGradesWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_GRADES_RELATIVE_PATH = "grades.xml";
        
        
        /// <summary>
        /// Writes the grades.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteGrade(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GRADES_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    
                    writer.WriteStartElement("activity_gradebook");
                    
                    writer.WriteStartElement("grade_items");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <grade_items>
                    
                    writer.WriteStartElement("grade_letters");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <grade_letters>
                    
                    writer.WriteEndElement(); // Close <activity_gradebook>
                    
                    writer.WriteEndDocument();
                }
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_GRADES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_GRADES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}