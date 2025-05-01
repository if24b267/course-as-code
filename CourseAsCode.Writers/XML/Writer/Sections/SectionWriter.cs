using System.Xml;

using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Sections
{
    public class SectionWriter : XmlWriterBase
    {
        private readonly IFileService _fileService;


        /// <summary>
        /// Creates a new instance of the SectionWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to SectionWriter</param>
        /// <param name="fileService">Service that handles operations on the file system</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public SectionWriter(IXmlWriterFactory xmlWriterFactory, ILogger<SectionWriter> logger, IFileService fileService)
            : base(xmlWriterFactory, logger)
        {
            _fileService = fileService;
        }


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
                        _fileService.CreateDirectory(Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException());
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, $"Error creating directory for section {section.SystemDefaults.SectionId}: {section.Name}");
                        LogError($"Skipping section {section.SystemDefaults.SectionId}: {section.Name}");
                        continue;
                    }

                    using (XmlWriter writer = CreateXmlWriter(filePath))
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
                                "timemodified", section.SystemDefaults.LastModified.HasValue
                                ? ((DateTimeOffset)section.SystemDefaults.LastModified.Value).ToUnixTimeSeconds().ToString()
                                : "$@NULL@$"
                            },
                        };

                        WriteElements(writer, elements);

                        writer.WriteEndElement(); // Close <section>
                        writer.WriteEndDocument();

                        LogDebug($"Created {relativePath} for Section {section.SystemDefaults.SectionId}: {section.Name}");
                    }
                }
                catch (Exception e)
                {
                    LogError(e, $"Error writing {relativePath} for Section {section.SystemDefaults.SectionId}: {section.Name}");
                    throw;
                }
            }
        }
    }
}
