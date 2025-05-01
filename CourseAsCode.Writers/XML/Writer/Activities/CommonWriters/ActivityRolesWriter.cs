using System.Xml;

using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityRolesWriter : XmlWriterBase
    {
        private const string DEFAULT_ROLES_RELATIVE_PATH = "roles.xml";


        /// <summary>
        /// Creates a new instance of the <see cref="ActivityRolesWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="RolesWriter"/></param>
        public ActivityRolesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityRolesWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }


        /// <summary>
        /// Writes the roles.xml file required for an activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteRoles(string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_ROLES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("roles");

                    writer.WriteStartElement("role_overrides");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <role_overrides>

                    writer.WriteStartElement("role_assignments");
                    writer.WriteWhitespace("\n");
                    writer.WriteEndElement(); // Close <role_assignments>

                    writer.WriteEndElement(); // Close <roles>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_ROLES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_ROLES_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
