using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// writes the <c>tags</c> section of the module.xml file for an arbitrary activity
    /// </summary>
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


        public void WriteTags(IXmlService xmlHelper, List<string>? tags)
        {
            int tagId = 0;

            xmlHelper.WriteElement("tags");

            if (tags != null)
            {
                foreach (string tag in tags)
                {
                    xmlHelper.WriteElementWithAttributes("tag", new Dictionary<string, string>
                    {
                        { "id", tagId++.ToString() }
                    });

                    xmlHelper.WriteElements(new Dictionary<string, string>
                    {
                        { "name", tag },
                        { "rawname", tag },
                    });

                    xmlHelper.CloseElement(); // Close <tag>
                }
            }

            xmlHelper.CloseElement(); // Close <tags>
        }
    }
}
