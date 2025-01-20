using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.Labels
{
    public class LabelWriter : XmlWriterBase
    {
        private int _currentModuleId = 22; // Initial moduleId

        /// <summary>
        /// Writes the label activity to an XML file.
        /// </summary>
        /// <param name="outputPath">The output path where the label XML file will be saved.</param>
        /// <param name="activity">The TextAndMediaArea activity to write.</param>
        public void WriteLabel(string outputPath, TextAndMediaArea activity)
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, "label.xml"));
            using XmlWriter writer = XmlWriter.Create(filePath, Settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("activity");

            WriteActivityAttributes(writer, activity);
            WriteLabelElement(writer, activity);

            writer.WriteEndElement(); // End of activity
            writer.WriteEndDocument();

            _currentModuleId++; // Increment moduleId for the next label
        }

        /// <summary>
        /// Writes the attributes of the activity element.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="activity">The TextAndMediaArea activity to write.</param>
        private void WriteActivityAttributes(XmlWriter writer, TextAndMediaArea activity)
        {
            writer.WriteAttributeString("id", activity.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", _currentModuleId.ToString());
            writer.WriteAttributeString("modulename", activity.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", activity.Section?.SystemDefaults.SectionId.ToString() ?? "0");
        }

        /// <summary>
        /// Writes the label element and its child elements.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="activity">The TextAndMediaArea activity to write.</param>
        private void WriteLabelElement(XmlWriter writer, TextAndMediaArea activity)
        {
            writer.WriteStartElement("label");
            writer.WriteAttributeString("id", activity.ActivityId.ToString());

            writer.WriteElementString("name", activity.Name);
            writer.WriteElementString("intro", activity.Content ?? string.Empty);
            writer.WriteElementString("introformat", "4");
            writer.WriteElementString("timemodified", new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString());

            writer.WriteEndElement(); // End of label
        }
    }
}

