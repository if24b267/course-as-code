using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCommentsWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_COMMENTS_RELATIVE_PATH = "comments.xml";
        
        
        /// <summary>
        /// Writes the comments.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteComments(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMMENTS_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("comments");
                    
                    // Add whitespace to prevent a self-closing tag
                    writer.WriteWhitespace("\n");
                    
                    writer.WriteEndElement(); // Close <comments>
                    writer.WriteEndDocument();
                }
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_COMMENTS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_COMMENTS_RELATIVE_PATH}");
                throw;
            }
        }
    }
}