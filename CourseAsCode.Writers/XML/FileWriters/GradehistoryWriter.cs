using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class GradehistoryWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADEHISTORY_RELATIVE_PATH = "gradehistory.xml";


        /// <summary>
        /// Creates a new instance of the GradehistoryWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to GradehistoryWriter</param>
        public GradehistoryWriter(IXmlWriterFactory xmlWriterFactory, ILogger<GradehistoryWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        public void WriteGradehistory(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GRADEHISTORY_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("grade_history");

                    WriteGradeGradesSection(writer, course);

                    writer.WriteEndElement(); // Close <grade_history>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_GRADEHISTORY_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_GRADEHISTORY_RELATIVE_PATH}");
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
            writer.WriteEndElement(); // Close <grade_grades>
        }
    }
}
