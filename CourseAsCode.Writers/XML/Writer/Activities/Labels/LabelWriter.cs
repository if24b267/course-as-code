using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using System.Xml;

namespace CourseAsCode.Writers.XML.Writer.Activities.Labels
{
    public class LabelWriter : XmlWriterBase
    {
        private int _currentModuleId = 22; // Initial moduleId

        public void WriteLabel(string outputPath, TextAndMediaArea activity)
        {
            string filePath = Path.GetFullPath(Path.Combine(outputPath, "label.xml"));
            using XmlWriter writer = XmlWriter.Create(filePath, Settings);

            writer.WriteStartDocument();

            writer.WriteStartElement("activity");
            writer.WriteAttributeString("id", activity.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", _currentModuleId.ToString());
            writer.WriteAttributeString("modulename", activity.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", activity.Section?.SystemDefaults.SectionId.ToString() ?? "0");

            writer.WriteStartElement("label");
            writer.WriteAttributeString("id", activity.ActivityId.ToString());

            writer.WriteElementString("name", activity.Name);
            writer.WriteElementString("intro", activity.Content ?? string.Empty);
            writer.WriteElementString("introformat", "1"); // Assuming introformat is 1 for simplicity
            writer.WriteElementString("timemodified", new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString());

            writer.WriteEndElement(); // End of label

            writer.WriteEndElement(); // End of activity

            writer.WriteEndDocument();

            _currentModuleId++; // Increment moduleId for the next label
        }
    }
}
