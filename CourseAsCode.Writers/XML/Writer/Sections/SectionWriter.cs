using System.Xml;
using CourseAsCode.Model;
using NLog;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Writer.Sections
{
    public class SectionWriter : XmlWriterBase
    {
        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public SectionWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }
        
        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public SectionWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
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
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
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
                            { "timemodified", section.SystemDefaults.LastModified.HasValue
                                ? ((DateTimeOffset)section.SystemDefaults.LastModified.Value).ToUnixTimeSeconds().ToString()
                                : "$@NULL@$" }
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
