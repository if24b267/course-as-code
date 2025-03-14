using System.Xml;
using CourseAsCode.Model;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Sections;

public class SectionWriter : XmlWriterBase
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	
	public void WriteSections(string outputPath, List<Section> sections)
	{
		foreach (Section section in sections)
		{
			string relativePath = $"{section.SystemDefaults.PathToSection}/section.xml";
			
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

					writer.WriteStartElement("section");
					writer.WriteAttributeString("id", section.SystemDefaults.SectionId.ToString());

					var elements = new Dictionary<string, string>
					{
						{ "number", section.SystemDefaults.SectionId.ToString() },
						{ "name", section.Name ?? "$@NULL@$" },
						{ "summary", section.Summary ?? "" },
						{ "summaryformat", ((int)section.SystemDefaults.SummaryFormat).ToString() },
						{ "sequence", "" },
						{ "visible", section.VisibleToStudents ? "1" : "0" },
						{ "availabilityjson", "$@NULL@$" },
						{ "component", section.SystemDefaults.Component?.ToString() ?? "$@NULL@$" },
						{ "itemid", "$@NULL@$" },
						{
							"timemodified",
							section.SystemDefaults.LastModified.HasValue
								? ((DateTimeOffset)section.SystemDefaults.LastModified.Value).ToUnixTimeSeconds()
								.ToString()
								: "$@NULL@$"
						}
					};

					WriteElements(writer, elements);

					writer.WriteEndElement(); // Close <section>

					writer.WriteEndDocument();
					
					Logger.Debug($"Created {relativePath} for Section {section.SystemDefaults.SectionId}: {section.Name}");
				}
			}
			catch (Exception e)
			{
				Logger.Error(e, $"Error writing {relativePath} for Section {section.SystemDefaults.SectionId}: {section.Name}");
				throw;
			}
		}
	}
}
