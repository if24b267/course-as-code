using CourseAsCode.Model.Activities.Resources;
using System.Xml;
using CourseAsCode.Writers.XML.Helper;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.Labels
{
    public class LabelWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_LABEL_RELATIVE_PATH = "label.xml";
        
        private int _currentModuleId = 22;
        private readonly GeneralHelper _generalHelper;
        
        
        // Constructor
        public LabelWriter()
        {
            _generalHelper = GeneralHelper.Instance;
        }
        
        
        // Constructor using Dependency Injection for Unit Testing
        public LabelWriter(GeneralHelper generalHelper)
        {
            _generalHelper = generalHelper;
        }
        
        
        /// <summary>
        /// Writes the label activity to an XML file.
        /// </summary>
        /// <param name="outputPath">The output path where the label XML file will be saved.</param>
        /// <param name="activity">The TextAndMediaArea activity to write.</param>
        public void WriteLabel(string outputPath, TextAndMediaArea activity)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_LABEL_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");
                    
                    WriteActivityAttributes(writer, activity);
                    WriteLabelElement(writer, activity);
                    
                    writer.WriteEndElement(); // Close <activity>
                    writer.WriteEndDocument();
                }
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_LABEL_RELATIVE_PATH}");
                _currentModuleId++; // Increment moduleId for the next label
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_LABEL_RELATIVE_PATH}");
                throw;
            }
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
            writer.WriteElementString("introformat", _generalHelper.EnumIntegerToString(activity.FormattingFormat));
            writer.WriteElementString("timemodified", _generalHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
            
            writer.WriteEndElement(); // Close <label>
        }
    }
}
