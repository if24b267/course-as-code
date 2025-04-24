using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class GradehistoryWriter : XmlWriterBase
    {
        private const string DEFAULT_GRADEHISTORY_RELATIVE_PATH = "gradehistory.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public GradehistoryWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public GradehistoryWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
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
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <grade_grades>
        }
    }
}
