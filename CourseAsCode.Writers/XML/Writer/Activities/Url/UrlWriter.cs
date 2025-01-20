using System.Xml;
using CourseAsCode.Model.Activities.Resources;

namespace CourseAsCode.Writers.XML.Writer.Activities.Url
{
    public class UrlWriter : XmlWriterBase
    {
        private int _currentModuleId = 59; // Initial moduleId

        /// <summary>
        /// Writes the url activity to an XML file.
        /// </summary>
        /// <param name="outputPath">The output path where the label XML file will be saved.</param>
        /// <param name="urlObject">The TextAndMediaArea activity to write.</param>
        public void WriteLabel(string outputPath, URL urlObject)
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, "url.xml"));
            using XmlWriter writer = XmlWriter.Create(filePath, Settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("activity");

            WriteActivityAttributes(writer, urlObject);
            WriteLabelElement(writer, urlObject);

            writer.WriteEndElement(); // End of activity
            writer.WriteEndDocument();

            _currentModuleId++; // Increment moduleId for the next label
        }

        /// <summary>
        /// Writes the attributes of the activity element.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="urlObject">The TextAndMediaArea activity to write.</param>
        private void WriteActivityAttributes(XmlWriter writer, URL urlObject)
        {
            writer.WriteAttributeString("id", urlObject.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", _currentModuleId.ToString());
            writer.WriteAttributeString("modulename", urlObject.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", urlObject.Section?.SystemDefaults.SectionId.ToString() ?? "142");
        }

        /// <summary>
        /// Writes the url element and its child elements.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="urlObject">The TextAndMediaArea activity to write.</param>
        private void WriteLabelElement(XmlWriter writer, URL urlObject)
        {
            writer.WriteStartElement("url");
            writer.WriteAttributeString("id", urlObject.ActivityId.ToString());

            writer.WriteElementString("name", urlObject.Name);
            writer.WriteElementString("intro", urlObject.Description ?? string.Empty);
            writer.WriteElementString("introformat", "1");
            writer.WriteElementString("externalurl", urlObject.ExternalUrl);
            writer.WriteElementString("display", "0");
            writer.WriteElementString("displayoptions", "a:1:{s:10:\"printintro\";i:1;}");
            writer.WriteElementString("parameters", "a:0:{}");
            writer.WriteElementString("timemodified", new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString());

            writer.WriteEndElement(); // End of url
        }
    }
}

