using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CommentsWriter : XmlWriterBase
    {
        private const string DEFAULT_COMMENTS_RELATIVE_PATH = "course/comments.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="CommentsWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="CommentsWriter"/></param>
        public CommentsWriter(IXmlWriterFactory xmlWriterFactory, ILogger<CalendarWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public CommentsWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        public void WriteComments(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMMENTS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("comments");

                    // Add whitespace to prevent self-closing tag
                    //writer.WriteString("\n");

                    writer.WriteEndElement(); // Close <comments>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_COMMENTS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_COMMENTS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
