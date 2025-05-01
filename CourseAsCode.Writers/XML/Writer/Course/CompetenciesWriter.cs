using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CompetenciesWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPETENCIES_RELATIVE_PATH = "course/competencies.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="CompetenciesWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="CompetenciesWriter"/></param>
        public CompetenciesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<CompetenciesWriter> logger)
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
                    writer.WriteEndElement(); // Close </competencies>

                    writer.WriteStartElement("user_competencies");
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
