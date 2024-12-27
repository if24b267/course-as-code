using System.Xml;

namespace CourseAsCode.Writers.XML.Writer;

public class GroupsWriter : XmlWriterBase
{
	public void WriteGroups(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "groups.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, _settings);
		writer.WriteStartDocument();
		writer.WriteStartElement("groups");

		WriteGroupCustomFieldsSection(writer, course);
		WriteGroupingsSection(writer, course);

		writer.WriteEndElement(); // Close <groups>
		writer.WriteEndDocument();
	}

	
	/// <summary>
	/// writes the `groupcustomfields` section of the groups.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteGroupCustomFieldsSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("groupcustomfields");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <groupcustomfields>
	}

	
	/// <summary>
	/// writes the `groupings` section of the groups.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteGroupingsSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("groupings");

		writer.WriteStartElement("groupingcustomfields");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <groupingcustomfields>

		writer.WriteEndElement(); // Close <groupings>
	}
}