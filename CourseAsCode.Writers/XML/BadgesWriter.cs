using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class BadgesWriter
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = true // Removes the XML declaration
	};

	public void WriteBadges(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "badges.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();

			// <badges> starts here
			writer.WriteStartElement("badges");

			// </badges> ends here
			writer.WriteEndElement();

			writer.WriteEndDocument();
		}
	}
}