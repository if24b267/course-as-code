using System.Xml;

using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCommentsWriter : XmlWriterBase
    {
        private const string DEFAULT_COMMENTS_RELATIVE_PATH = "comments.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public ActivityCommentsWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public ActivityCommentsWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        /// <summary>
        /// Writes the comments.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteComments(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMMENTS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("comments");

                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");

                    writer.WriteEndElement(); // Close <comments>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_COMMENTS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_COMMENTS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
