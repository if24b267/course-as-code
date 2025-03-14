using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCompletionWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPLETION_RELATIVE_PATH = "completion.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public ActivityCompletionWriter() 
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public ActivityCompletionWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        /// <summary>
        /// Writes the completion.xml file required for an activity
        /// </summary>
        /// <param name="outputPath">The output directory path</param>
        public void WriteCompletion(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPLETION_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("completions");
                    
                    writer.WriteStartElement("completionviews");
                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <completionviews>
                    
                    writer.WriteEndElement(); // Close <completions>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_COMPLETION_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_COMPLETION_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
