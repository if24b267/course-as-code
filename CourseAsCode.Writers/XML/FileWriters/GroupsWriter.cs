using System.Xml;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class GroupsWriter : XmlWriterBase
    {
        private const string DEFAULT_GROUPS_RELATIVE_PATH = "groups.xml";


        /// <summary>
        /// Creates a new instance of the GroupsWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to GroupsWriter</param>
        public GroupsWriter(IXmlWriterFactory xmlWriterFactory, ILogger<GroupsWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        public void WriteGroups(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_GROUPS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("groups");

                    WriteGroupCustomFieldsSection(writer, course);
                    WriteGroupingsSection(writer, course);

                    writer.WriteEndElement(); // Close <groups>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_GROUPS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_GROUPS_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// Writes the `groupcustomfields` section of the groups.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">Initialized Moodle Course.</param>
        private void WriteGroupCustomFieldsSection(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("groupcustomfields");
            writer.WriteEndElement(); // Close <groupcustomfields>
        }

        /// <summary>
        /// Writes the `groupings` section of the groups.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="course">Initialized Moodle Course.</param>
        private void WriteGroupingsSection(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("groupings");

            writer.WriteStartElement("groupingcustomfields");
            writer.WriteEndElement(); // Close <groupingcustomfields>

            writer.WriteEndElement(); // Close <groupings>
        }
    }
}
