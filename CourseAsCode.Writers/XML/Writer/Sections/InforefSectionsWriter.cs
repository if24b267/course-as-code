using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.Writer.Sections;

public class InforefSectionsWriter : XmlWriterBase
{
	public void WriteInforefSections(string outputPath, List<Section> sections)
	{
		foreach (var section in sections)
		{
			string filePath = Path.GetFullPath(Path.Combine(outputPath, $"{section.SystemDefaults.PathToSection}/inforef.xml"));
			Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

			using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
			{
				writer.WriteStartDocument();

				writer.WriteStartElement("inforef");
				writer.WriteEndElement();

				writer.WriteEndDocument();
			}
		}
	}
}
