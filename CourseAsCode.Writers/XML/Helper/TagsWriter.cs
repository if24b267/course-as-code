using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Helper;

public class TagsWriter
{
    /// <summary>
    /// Writes the tags section of module.xml for an arbitrary activity
    /// </summary>
    /// <param name="xmlWriter">The XML writer to write the tags to</param>
    /// <param name="tags">The tags to write</param>
    /// <param name="outputPath">The path to the output file</param>
    /// <param name="logger">The logger to log messages to</param>
    public static void WriteTags(XmlWriter xmlWriter, List<string>? tags, string outputPath, ILogger logger)
    {
        int tagId = 0;
            
        xmlWriter.WriteStartElement("tags");
            
        if (tags != null)
        {
            foreach (string tag in tags)
            {
                xmlWriter.WriteStartElement("tag");
                xmlWriter.WriteAttributeString("id", tagId++.ToString());
                    
                xmlWriter.WriteElementString("name", tag);
                xmlWriter.WriteElementString("rawname", tag);
                    
                xmlWriter.WriteEndElement(); // Close <tag>
                logger.Trace($"Added tag '{tag}' to {Path.GetFileName(outputPath)}");
            }
        }
            
        xmlWriter.WriteEndElement(); // Close <tags>
    }
}