using System.Xml;
using CourseAsCode.Model;
using NLog;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Writer.Sections
{
    public class InforefSectionsWriter : XmlWriterBase
    {
        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public InforefSectionsWriter() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }
        
        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public InforefSectionsWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
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
