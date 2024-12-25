using System.Xml;

namespace CourseAsCode.Writers.XML;

public class FilesWriter : XmlWriterBase
{
	public void WriteFiles(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "files.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("files");
			writer.WriteEndElement(); // Close <files>
			writer.WriteEndDocument();
		}
	}
}