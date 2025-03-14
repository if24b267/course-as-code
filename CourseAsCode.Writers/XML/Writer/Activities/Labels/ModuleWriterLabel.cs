using System.Xml;
using CourseAsCode.Model;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.Labels
{
    internal class ModuleWriterLabel : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";
        
        private int _currentModuleId = 0; // Initial moduleId
        
        
        /// <summary>
        /// Writes the module.xml file for a label activity.
        /// </summary>
        /// <param name="outputPath">The directory where module.xml will be saved.</param>
        /// <param name="section">The section model providing context information.</param>
        public void WriteModule(string outputPath, Section section)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("module");
                    
                    writer.WriteAttributeString("id", _currentModuleId.ToString());
                    writer.WriteAttributeString("version", "2024100700");
                    
                    WriteGeneralInformationToModules(writer, section.SystemDefaults.SectionId);
                    
                    writer.WriteEndElement(); // Close <module>
                    writer.WriteEndDocument();
                }
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
                
                _currentModuleId++; // Increment moduleId for the next label
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
                throw;
            }
        }
        
        
        /// <summary>
        /// Writes the general information elements into the module tag.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="sectionId">The section id from the Section model.</param>
        private void WriteGeneralInformationToModules(XmlWriter writer, int sectionId)
        {
            var elements = new Dictionary<string, string>
            {
                { "modulename", "label" },
                { "sectionid", sectionId.ToString() },
                { "sectionnumber", "0" },
                { "idnumber", "$@NULL@$" },
                { "added", new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString() },
                { "score", "0" },
                { "indent", "0" },
                { "visible", "1" },
                { "visibleoncoursepage", "1" },
                { "visibleold", "1" },
                { "groupmode", "0" },
                { "groupingid", "0" },
                { "completion", "0" },
                { "completiongradeitemnumber", "$@NULL@$" },
                { "completionpassgrade", "0" },
                { "completionview", "0" },
                { "completionexpected", "0" },
                { "availability", "$@NULL@$" },
                { "showdescription", "0" },
                { "downloadcontent", "1" },
                { "lang", "$@NULL@$" },
                { "tags", " " },
            };

            WriteElements(writer, elements);
        }
    }
}
