using System.Xml;
using UsersModel = CourseAsCode.Model.User;
using RolesModel = CourseAsCode.Model.Role;
//using FileModel = CourseAsCode.Model.File; doesnt exist yet

namespace CourseAsCode.Writers.XML.Course;

public class InforefWriter : XmlWriterBase
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = true // Removes the XML declaration
	};

	public void WriteInforef(List<UsersModel> users, List<RolesModel> roles, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/inforef.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("inforef"); // <inforef> starts here
			writer.WriteStartElement("userref"); // <userref> starts here

			foreach (var user in users)
			{
				WriteInforef(writer, user, "user");
			}

			writer.WriteEndElement(); // </userref> ends here
			writer.WriteStartElement("roleref"); // <roleref> starts here

			foreach (var role in roles)
			{
				WriteInforef(writer, role, "role");
			}

			writer.WriteEndElement(); // </roleref> ends here
			writer.WriteStartElement("fileref"); // <fileref> starts here

			/*foreach (var file in files)
			{
			    WriteInforef(writer, file, "file");
			}*/

			writer.WriteEndElement(); // </fileref> ends here
			writer.WriteEndElement(); // </inforef> ends here
			writer.WriteEndDocument();
		}
	}

	private void WriteInforef(XmlWriter writer, object model, string type)
	{
		var modelName = model as dynamic;

		writer.WriteStartElement(type); // Element starts here

		// Später auf Fehler Überprüfungen ----------------------
		var elements = new Dictionary<string, string>
		{
			{ "id", modelName.SystemDefaults.Id.ToString() }
		};

		WriteElements(writer, elements);

		writer.WriteEndElement(); // Element ends here
	}
}