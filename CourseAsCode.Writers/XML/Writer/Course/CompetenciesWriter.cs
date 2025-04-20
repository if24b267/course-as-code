using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CompetenciesWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPETENCIES_RELATIVE_PATH = "course/competencies.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public CompetenciesWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public CompetenciesWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteCompetencies(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPETENCIES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
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

                LogDebug($"Created {DEFAULT_COMPETENCIES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_COMPETENCIES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
