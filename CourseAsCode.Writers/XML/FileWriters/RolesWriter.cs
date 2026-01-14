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

                    if (course.Roles != null)
                    {
                        foreach (var role in course.Roles)
                        {
                            WriteSingleRole(writer, role);
                        }
                    }

                    writer.WriteEndElement(); // </roles_definition>
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

        private void WriteSingleRole(XmlWriter writer, Role role)
        {
            WriteStartElementWithAttributes(writer, "role", new Dictionary<string, string>
            {
                { "id", role.SystemDefaults.RoleId.ToString() }
            });

            var elements = new Dictionary<string, string>
            {
                { "name", role.Name ?? string.Empty },
                { "shortname", role.ShortName },
                { "nameincourse", role.LocalName ?? "$@NULL@$" },
                { "description", role.Description ?? string.Empty },
                { "sortorder", role.SystemDefaults.RoleId.ToString() }, // Use RoleId as sortorder, adjust if needed
                { "archetype", role.Archetype.ToString().ToLowerInvariant() }
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // </role>
            LogTrace($"Added role {role.SystemDefaults.RoleId}: '{role.Name}' to {DEFAULT_ROLES_RELATIVE_PATH}");
        }
    }
}
