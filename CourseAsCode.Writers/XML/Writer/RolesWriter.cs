using CourseAsCode.Model;
using System.Xml;
using CourseAsCode.Writers.XML.Helper;
using NLog;

namespace CourseAsCode.Writers.XML.Writer;

public class RolesWriter : XmlWriterBase
{
	private readonly Logger _logger = LogManager.GetCurrentClassLogger();
	private const string DEFAULT_ROLES_RELATIVE_PATH = "roles.xml";
	
	private readonly RolesHelper _rolesHelper;
	
	
	// Constructor
	public RolesWriter()
	{
		_rolesHelper = new RolesHelper();
	}
	
	
	// Constructor using Dependency Injection for Unit Testing
	public RolesWriter(RolesHelper rolesHelper)
	{
		_rolesHelper = rolesHelper;
	}


	public void WriteRoles(Model.Course course, string outputPath)
	{
		try
		{
			string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_ROLES_RELATIVE_PATH));

			using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
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
			
			_logger.Debug($"Created {DEFAULT_ROLES_RELATIVE_PATH}");
		}
		catch (Exception e)
		{
			_logger.Error(e, $"Error writing {DEFAULT_ROLES_RELATIVE_PATH}");
			throw;
		}
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
			{ "id", role.SystemDefaults.RoleId.ToString() }
		});

		var elements = new Dictionary<string, string>
		{
			{ "name", role.Name ?? string.Empty },
			{ "shortname", role.ShortName },
			{ "nameincourse", role.LocalName ?? "$@NULL@$" },
			{ "description", role.Description ?? string.Empty },
			{ "sortorder", role.SystemDefaults.RoleId.ToString() }, // TODO: Determine function of this field, seemingly the same as roleid
			{ "archetype", _rolesHelper.GetArchetypeFromEnum(role.Archetype) }
		};

		WriteElements(writer, elements);

		writer.WriteEndElement(); // Close <role>
		
		_logger.Trace($"Wrote role {role.Name} to {DEFAULT_ROLES_RELATIVE_PATH}");
	}
}