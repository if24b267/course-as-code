using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class CommentsWriter : XmlWriterBase
    {
        private const string DEFAULT_COMMENTS_RELATIVE_PATH = "course/comments.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public CommentsWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
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