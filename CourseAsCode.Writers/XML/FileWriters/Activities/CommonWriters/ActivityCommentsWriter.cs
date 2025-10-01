using System.Xml;

using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCommentsWriter : XmlWriterBase
    {
        private const string DEFAULT_COMMENTS_RELATIVE_PATH = "comments.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityCommentsWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ActivityCommentsWriter"/></param>
        public ActivityCommentsWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityCommentsWriter> logger)
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
