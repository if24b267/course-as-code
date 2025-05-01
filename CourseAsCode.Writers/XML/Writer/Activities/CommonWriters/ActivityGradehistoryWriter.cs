using System.Xml;

using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityGradehistoryWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADE_HISTORY_RELATIVE_PATH = "grade_history.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityGradehistoryWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityGradehistoryWriter"/></param>
        public ActivityGradehistoryWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityGradehistoryWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


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

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("grade_history");

                    WriteGradeGradesSection(writer, course);

                    writer.WriteEndElement(); // Close <grade_history>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_GRADE_HISTORY_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_GRADE_HISTORY_RELATIVE_PATH}");
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
