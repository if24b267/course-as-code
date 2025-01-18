using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.Sections;

public class SectionWriter : XmlWriterBase
{
    public void WriteSections(string outputPath, List<Section> sections)
    {
        int number = 0;

        foreach (var section in sections)
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, $"sections/section_{section.SystemDefaults.SectionId}/section.xml"));
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("section");
                writer.WriteAttributeString("id", section.SystemDefaults.SectionId.ToString());

                var elements = new Dictionary<string, string>
                {
                    { "number", number.ToString() },
                    { "name", section.Name ?? "$@NULL@$" },
                    { "summary", section.Summary ?? "" },
                    { "summaryformat", ((int)section.SystemDefaults.SummaryFormat).ToString() },
                    { "sequence", "" },
                    { "visible", section.VisibleToStudents ? "1" : "0" },
                    { "availabilityjson", "$@NULL@$" },
                    { "component", section.SystemDefaults.Component?.ToString() ?? "$@NULL@$" },
                    { "itemid", "$@NULL@$" },
                    { "timemodified", section.SystemDefaults.LastModified.HasValue ? ((DateTimeOffset)section.SystemDefaults.LastModified.Value).ToUnixTimeSeconds().ToString() : "$@NULL@$" }
                };

                WriteElements(writer, elements);

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            number++;
        }
    }
}
