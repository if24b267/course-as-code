using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Labels
{
    public class LabelWriter : XmlWriterBase
    {
        private const string DEFAULT_LABEL_RELATIVE_PATH = "label.xml";
        private readonly IGeneralHelper _generalHelper;


        /// <summary>
        /// Creates a new instance of the <see cref="LabelWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="LabelWriter"/></param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public LabelWriter(IXmlWriterFactory xmlWriterFactory, ILogger<LabelWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
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

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, activity);
                    WriteLabelElement(writer, activity);

                    writer.WriteEndElement(); // Close <activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_LABEL_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_LABEL_RELATIVE_PATH}");
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
            writer.WriteAttributeString("moduleid", activity.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("modulename", activity.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(activity.Context));
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
