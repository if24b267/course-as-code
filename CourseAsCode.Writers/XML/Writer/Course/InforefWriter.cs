using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class InforefWriter : XmlWriterBase
{
	public void WriteInforef(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/inforef.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, Settings);

		writer.WriteStartDocument();

		writer.WriteStartElement("inforef");

		// Reference all enrolled users
		writer.WriteStartElement("userref");

		foreach (var user in course.EnrolledUsers)
		{
			WriteOneInforefSection(writer, user, "user");
		}

		writer.WriteEndElement(); // Close </userref>

		// Reference all roles
		writer.WriteStartElement("roleref");

		foreach (var role in course.Roles)
		{
			WriteOneInforefSection(writer, role, "role");
		}

		writer.WriteEndElement(); // Close </roleref>

		// Reference all files
		writer.WriteStartElement("fileref");

		// TODO: Implement as soon as the file model is available
		/*foreach (var file in files)
			{
			    WriteOneInforefSection(writer, file, "file");
			}*/

		writer.WriteEndElement(); // Close </fileref>

		writer.WriteEndElement(); // Close </inforef>
		writer.WriteEndDocument();
	}


	/// <summary>
	/// writes the part of the `inforef` section for the corresponding object type
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="instance">instance of the object to be referenced</param>
	/// <param name="type">type of the object to be referenced</param>
	private void WriteOneInforefSection(XmlWriter writer, dynamic instance, string type)
	{
		writer.WriteStartElement(type);

		writer.WriteElementString("id", instance.SystemDefaults.Id.ToString());

		writer.WriteEndElement(); // Close </type>
	}
}