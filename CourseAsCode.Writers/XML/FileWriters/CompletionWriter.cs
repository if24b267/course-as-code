using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class CompletionWriter : XmlWriterBase
    {
        private const string DEFAULT_COMPLETION_RELATIVE_PATH = "completion.xml";


        /// <summary>
        /// Creates a new instance of the CompletionWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to CompletionWriter</param>
        public CompletionWriter(IXmlWriterFactory xmlWriterFactory, ILogger<CompletionWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Stores the information about which users have completed the course; NOT IMPLEMENTED (yet?).
        /// </summary>
        /// <param name="course">The course data.</param>
        /// <param name="outputPath">The directory where the completion.xml file will be created.</param>
        public void WriteCompletion(Model.Course course, string outputPath = "completion.xml")
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPLETION_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("course_completion");

                    // Add whitespace to prevent a self-closing tag
                    //writer.WriteWhitespace("\n");

                    writer.WriteEndElement(); // Close </course_completion>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_COMPLETION_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_COMPLETION_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
