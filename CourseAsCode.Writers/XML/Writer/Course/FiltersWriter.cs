using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class FiltersWriter : XmlWriterBase
{
	public void WriteFilters(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/filters.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, Settings);
		writer.WriteStartDocument();

		writer.WriteStartElement("filters");

		writer.WriteStartElement("filter_actives");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close </filter_actives>

		writer.WriteStartElement("filter_configs");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close </filter_configs>

		writer.WriteEndElement(); // Close </filters>

		writer.WriteEndDocument();
	}
}