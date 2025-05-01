using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Helper
{
    public class TagsWriter : ITagsWriter
    {
        private readonly ILogger<TagsWriter> _logger;


        /// <summary>
        /// Creates a new instance of the TagsWriter class
        /// </summary>
        /// <param name="logger">Logger instance specific to TagsWriter</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public TagsWriter(ILogger<TagsWriter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// Writes the tags section of module.xml for an arbitrary activity
        /// </summary>
        /// <param name="xmlWriter">The XML writer to write the tags to</param>
        /// <param name="tags">The tags to write</param>
        /// <param name="outputPath">The path to the output file</param>
        public void WriteTags(XmlWriter xmlWriter, List<string>? tags, string outputPath)
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
                    _logger.LogTrace($"Added tag '{tag}' to {Path.GetFileName(outputPath)}");
                }
            }

            xmlWriter.WriteEndElement(); // Close <tags>
        }
    }
}
