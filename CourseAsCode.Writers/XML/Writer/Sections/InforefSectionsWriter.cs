using System.Xml;
using CourseAsCode.Model;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Sections;

public class InforefSectionsWriter : XmlWriterBase
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	
	public void WriteInforefSections(string outputPath, List<Section> sections)
	{
		foreach (var section in sections)
		{
			string relativePath = $"{section.SystemDefaults.PathToSection}/inforef.xml";

			try
			{
				string filePath = Path.GetFullPath(Path.Combine(outputPath, relativePath));

				try
				{
					Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
				}
				catch (Exception ex)
				{
					Logger.Error(ex, $"Error creating directory for section {section.SystemDefaults.SectionId}: {section.Name}");
					Logger.Error($"Skipping section {section.SystemDefaults.SectionId}: {section.Name}");
					continue;
				}

				using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
				{
					writer.WriteStartDocument();

					writer.WriteStartElement("inforef");
					writer.WriteEndElement();

					writer.WriteEndDocument();
				}
				
				Logger.Debug($"Created {relativePath} for Section {section.SystemDefaults.SectionId}: {section.Name}");
			}
			catch (Exception e)
			{
				Logger.Error(e, $"Error writing {relativePath} for Section {section.SystemDefaults.SectionId}: {section.Name}");
				throw;
			}
		}
	}
}
