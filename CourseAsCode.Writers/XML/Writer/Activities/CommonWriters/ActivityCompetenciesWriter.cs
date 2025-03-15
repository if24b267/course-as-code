using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCompetenciesWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPETENCIES_RELATIVE_PATH = "competencies.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public ActivityCompetenciesWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public ActivityCompetenciesWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        /// <summary>
        /// Writes the competencies.xml file required for an activity
        /// </summary>
        /// <param name="outputPath">The output directory path</param>
        public void WriteCompetencies(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPETENCIES_RELATIVE_PATH));
                
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_module_competencies");
                    
                    writer.WriteStartElement("competencies");
                    
                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");
                    
                    writer.WriteEndElement(); // Close <competencies>
                    writer.WriteEndElement(); // Close <course_module_competencies>
                    writer.WriteEndDocument();
                }
                
                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_COMPETENCIES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_COMPETENCIES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
