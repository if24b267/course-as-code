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


        /// <summary>
        /// Creates a new instance of the RolesWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to RolesWriter</param>
        /// <param name="rolesHelper">Helper method providing specific helper methods focused on role management</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public RolesWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger, IRolesHelper rolesHelper)
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

                    foreach (var role in course.Roles)
                    {
                        WriteSingleRole(writer, role);
                    }

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
        /// writes a single role to the roles.xml file.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="role">initialized Moodle Role.</param>
        private void WriteSingleRole(XmlWriter writer, Role role)
        {
            WriteStartElementWithAttributes(writer, "role", new Dictionary<string, string>
            {
                { "id", role.SystemDefaults.RoleId.ToString() },
            });

            var elements = new Dictionary<string, string>
            {
                { "name", role.Name ?? string.Empty },
                { "shortname", role.ShortName },
                { "nameincourse", role.LocalName ?? "$@NULL@$" },
                { "description", role.Description ?? string.Empty },
                { "sortorder", role.SystemDefaults.RoleId.ToString() }, // TODO: Determine function of this field, seemingly the same as roleid
                { "archetype", _rolesHelper.GetArchetypeFromEnum(role.Archetype) },
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Close <role>

            LogTrace($"Wrote role {role.Name} to {DEFAULT_ROLES_RELATIVE_PATH}");
        }
    }
}
