using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.Writer.Activities.Labels
{
    internal class ModuleWriterLabel : XmlWriterBase
    {
        private int _currentModuleId = 0; // Initial moduleId

        public void WriteModule(string outputPath, Section section)
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, "module.xml"));

            using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("module");

                writer.WriteAttributeString("id", _currentModuleId.ToString());
                writer.WriteAttributeString("version", "2024100700");
                WriteGeneralInformationToModules(writer, section.SystemDefaults.SectionId);
                writer.WriteEndElement();
            }

            _currentModuleId++; // Increment moduleId for the next label
        }

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
