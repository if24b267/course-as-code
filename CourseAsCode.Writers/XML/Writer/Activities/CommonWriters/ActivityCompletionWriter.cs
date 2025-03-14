using System.Xml;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityCompletionWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_COMPLETION_RELATIVE_PATH = "completion.xml";
        
        
        /// <summary>
        /// Writes the completion.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteCompletion(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_COMPLETION_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
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
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_COMPLETION_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_COMPLETION_RELATIVE_PATH}");
                throw;
            }
        }
    }
}