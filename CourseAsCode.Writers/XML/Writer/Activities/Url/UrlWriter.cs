using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper;

namespace CourseAsCode.Writers.XML.Writer.Activities.Url
{
    public class UrlWriter : XmlWriterBase
    {
        private readonly GeneralHelper _generalHelper = new GeneralHelper();
        
        
        /// <summary>
        /// Writes the url activity to an XML file.
        /// </summary>
        /// <param name="outputPath">the output path where the url.xml file will be saved</param>
        /// <param name="urlObject">the URL activity to write</param>
        public void WriteUrl(string outputPath, URL urlObject)
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, "url.xml"));
            using XmlWriter writer = XmlWriter.Create(filePath, Settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("activity");

            WriteActivityAttributes(writer, urlObject);
            WriteUrlTag(writer, urlObject);

            writer.WriteEndElement(); // Close <activity>
            writer.WriteEndDocument();
        }

        
        /// <summary>
        /// writes the activity tag and its attributes
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="urlObject">initialized URL object to write</param>
        private void WriteActivityAttributes(XmlWriter writer, URL urlObject)
        {
            writer.WriteAttributeString("id", urlObject.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", urlObject.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("modulename", urlObject.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(urlObject.Context));
        }

        
        /// <summary>
        /// Writes the url element and its child elements.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="urlObject">The TextAndMediaArea activity to write.</param>
        private void WriteUrlTag(XmlWriter writer, URL urlObject)
        {
            writer.WriteStartElement("url");
            writer.WriteAttributeString("id", urlObject.ActivityId.ToString());

            // Write contents of url tag
            var elements = new Dictionary<string, string>
            {
                { "name", urlObject.Name },
                { "intro", urlObject.Description ?? string.Empty },
                { "introformat", _generalHelper.EnumIntegerToString(urlObject.DescriptionFormat) },
                { "externalurl", urlObject.ExternalUrl },
                { "display", "0" }, // TODO: Determine meaning of this value
                { "displayoptions", "a:1:{s:10:\"printintro\";i:1;}" }, // TODO: Determine meaning of this value
                { "parameters", "a:0:{}" }, // TODO: Determine meaning of this value
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(urlObject.TimeModified) }
            };
            WriteElements(writer, elements);

            writer.WriteEndElement(); // Close <url>
        }
    }
}

