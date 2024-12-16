using System.Xml;
using CourseAsCode.Model;
using UsersCourseModel = CourseAsCode.Model.User;

namespace CourseAsCode.Writers.XML.Course;

public class RolesCourseWriter : XmlWriterBase
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = false // Removes the XML declaration
	};

	public void WriteRolesCourse(List<UsersCourseModel> users, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/roles.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("roles"); // <roles> starts here

			writer.WriteStartElement("role_overrides"); // <role_overrides> starts here
			writer.WriteEndElement(); // </role_overrides> starts here

			writer.WriteStartElement("role_assignments"); // <role_assignments> starts here

			int i = 0;
			foreach (var user in users)
			{
				i++;
				WriteRolesCourse(writer, user, i);
			}


			// Attributes here if it doesnt work


			writer.WriteEndElement(); // </role_assignments> ends here
			writer.WriteEndElement(); // </roles> ends here
			writer.WriteEndDocument();
		}
	}

	private void WriteRolesCourse(XmlWriter writer, User user, int i)
	{
		WriteStartElementWithAttributes(writer, "assignment", new Dictionary<string, string>
		{
			{ "id", i.ToString() }
			// <assignment> starts here with attributes
			//writer.WriteStartElement("assignment");
			//writer.WriteAttributeString("id", "1");
		});

		var elements = new Dictionary<string, string>
		{
			{ "roleid", user.Role.ToString() },
			{ "userid", user.SystemDefaults.Id.ToString() },
			{ "timemodified", user.SystemDefaults.TimeModified.ToString() },
			{ "modifierid", "2" },
			{ "component", "" },
			{ "itemid", "0" },
			{ "sortorder", "0" }

			// child elements of <assignment>
			/*writer.WriteElementString("roleid", "3");
			writer.WriteElementString("userid", "3");
			writer.WriteElementString("timemodified", "1731136235");
			writer.WriteElementString("modifierid", "2");
			writer.WriteElementString("component", "");
			writer.WriteElementString("itemid", "0");
			writer.WriteElementString("sortorder", "0");*/
		};

		WriteElements(writer, elements);

		writer.WriteEndElement(); // </assignment> ends here
	}
}