using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class QuestionsWriter : XmlWriterBase
    {
        private const string DEFAULT_QUESTIONS_RELATIVE_PATH = "questions.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public QuestionsWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public QuestionsWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger) : base(xmlWriterFactory, logger)
        {
        }

        public void WriteQuestions(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_QUESTIONS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    
                    writer.WriteStartElement("question_categories");
                    writer.WriteString(" ");
                    writer.WriteEndElement(); // Close <question_categories>
                    
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_QUESTIONS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_QUESTIONS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}