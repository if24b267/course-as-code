using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML;

public class RolesWriter : XmlWriterBase
{
	public void WriteRoles(List<Role> roles, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "roles.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("roles_definition");

			foreach (var role in roles)
			{
				WriteRole(writer, role);
			}

			writer.WriteEndElement(); // Close <roles_definition>
			writer.WriteEndDocument();
		}
	}

	private void WriteRole(XmlWriter writer, Role role)
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
			{ "sortorder", role.SystemDefaults.Id.ToString() },
			{ "archetype", role.Archetype.ToString().ToLower() }
		};

		WriteElements(writer, elements);

		writer.WriteEndElement(); // Close <role>
	}
}