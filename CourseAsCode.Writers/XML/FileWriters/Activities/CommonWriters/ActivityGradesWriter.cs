using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityGradesWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADES_RELATIVE_PATH = "grades.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityGradesWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityGradesWriter"/></param>
        public ActivityGradesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityGradesWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the grades.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
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
