using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Writers.Helper;

namespace CourseAsCode.Writers.XML.Writer;

public class RolesWriter : XmlWriterBase
{
	private readonly RolesHelper _rolesHelper = new RolesHelper();
	
	
	public void WriteRoles(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "roles.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, _settings);
		writer.WriteStartDocument();
		writer.WriteStartElement("roles_definition");

		foreach (var role in course.Roles)
		{
			WriteSingleRole(writer, role);
		}

		writer.WriteEndElement(); // Close <roles_definition>
		writer.WriteEndDocument();
	}

	
	/// <summary>
	/// writes a single role to the roles.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="role">initialized Moodle Role</param>
	private void WriteSingleRole(XmlWriter writer, Role role)
	{
		WriteStartElementWithAttributes(writer, "role", new Dictionary<string, string>
		{
			{ "id", role.SystemDefaults.Id.ToString() }
		});

		var elements = new Dictionary<string, string>
		{
			{ "name", role.Name ?? string.Empty },
			{ "shortname", role.ShortName },
			{ "nameincourse", role.LocalName ?? "$@NULL@$" },
			{ "description", role.Description ?? string.Empty },
			{ "sortorder", role.SystemDefaults.Id.ToString() }, // TODO: Determine function of this field, seemingly the same as roleid
			{ "archetype", _rolesHelper.GetArchetypeFromEnum(role.Archetype) }
		};

		WriteElements(writer, elements);

		writer.WriteEndElement(); // Close <role>
	}
}