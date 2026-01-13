using System.Xml;

using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Sections
{
    public class InforefSectionsWriter : XmlWriterBase
    {
        private readonly IFileService _fileService;


        /// <summary>
        /// Creates a new instance of the InforefSectionsWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to InforefSectionsWriter</param>
        /// <param name="fileService">Service that handles operations on the file system</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public InforefSectionsWriter(IXmlWriterFactory xmlWriterFactory, ILogger<InforefSectionsWriter> logger, IFileService fileService)
            : base(xmlWriterFactory, logger)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }


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
                        _fileService.CreateDirectory(Path.GetDirectoryName(filePath)!);
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
                        writer.WriteStartElement("inforef");
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }

                    LogDebug($"Created {relativePath} for Section {section.SystemDefaults.SectionId}: {section.Name}");
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
