using System.Xml;

using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer
{
    public class RolesWriter : XmlWriterBase
    {
        private const string DEFAULT_ROLES_RELATIVE_PATH = "roles.xml";

        private readonly IRolesHelper _rolesHelper;

        public RolesWriter(IXmlWriterFactory xmlWriterFactory, ILogger<RolesWriter> logger, IRolesHelper rolesHelper)
            : base(xmlWriterFactory, logger)
        {
            _rolesHelper = rolesHelper ?? throw new ArgumentNullException(nameof(rolesHelper));
        }

        public void WriteRoles(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_ROLES_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("roles_definition");

                    // Hardcoded roles as per your sample XML
                    WriteSingleRole(writer, 3, "editingteacher", "$@NULL@$", "", "3", "editingteacher");
                    WriteSingleRole(writer, 5, "student", "$@NULL@$", "", "5", "student");

                    writer.WriteEndElement(); // Close <roles_definition>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_ROLES_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_ROLES_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// Hardcoded: writes a single role to the roles.xml file as in your sample.
        /// </summary>
        private void WriteSingleRole(XmlWriter writer, int id, string shortname, string nameincourse, string description, string sortorder, string archetype)
        {
            WriteStartElementWithAttributes(writer, "role", new Dictionary<string, string>
            {
                { "id", id.ToString() }
            });

            var elements = new Dictionary<string, string>
            {
                { "name", "" },
                { "shortname", shortname },
                { "nameincourse", nameincourse },
                { "description", description },
                { "sortorder", sortorder },
                { "archetype", archetype }
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Close <role>
        }
    }
}
