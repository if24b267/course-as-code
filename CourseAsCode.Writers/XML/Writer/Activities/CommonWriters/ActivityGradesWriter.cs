using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityGradesWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADES_RELATIVE_PATH = "grades.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public ActivityGradesWriter() 
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public ActivityGradesWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        /// <summary>
        /// Writes the grades.xml file required for an activity
        /// </summary>
        /// <param name="outputPath">The output directory path</param>
        public void WriteGrade(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GRADES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
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

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_GRADES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_GRADES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
