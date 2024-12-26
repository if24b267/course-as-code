using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class FiltersWriter
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = false
	};

	
	public void WriteFilters(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/filters.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, _settings);
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