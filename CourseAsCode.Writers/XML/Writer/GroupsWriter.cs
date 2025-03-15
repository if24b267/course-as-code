using System.Xml;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer
{
    public class GroupsWriter : XmlWriterBase
    {
        private const string DEFAULT_GROUPS_RELATIVE_PATH = "groups.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public GroupsWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public GroupsWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger) : base(xmlWriterFactory, logger)
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
        /// Writes the `groupcustomfields` section of the groups.xml file
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="course">Initialized Moodle Course</param>
        private void WriteGroupCustomFieldsSection(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("groupcustomfields");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <groupcustomfields>
        }

        /// <summary>
        /// Writes the `groupings` section of the groups.xml file
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="course">Initialized Moodle Course</param>
        private void WriteGroupingsSection(XmlWriter writer, Model.Course course)
        {
            writer.WriteStartElement("groupings");

            writer.WriteStartElement("groupingcustomfields");
            writer.WriteString(" ");
            writer.WriteEndElement(); // Close <groupingcustomfields>

            writer.WriteEndElement(); // Close <groupings>
        }
    }
}
