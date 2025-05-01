using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.File
{
    public class FileWriter : XmlWriterBase
    {
        private const string DEFAULT_FILE_RELATIVE_PATH = "resource.xml";
        private readonly IGeneralHelper _generalHelper;


        /// <summary>
        /// Creates a new instance of the <see cref="FileWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="FileWriter"/></param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public FileWriter(IXmlWriterFactory xmlWriterFactory, ILogger<FileWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }


        /// <summary>
        /// writes the file (resource) activity to an XML file.
        /// </summary>
        /// <param name="outputPath">the output path where the resource.xml file will be saved.</param>
        /// <param name="moodleFile">the file to write.</param>
        public void WriteFile(string outputPath, MoodleFile moodleFile)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_FILE_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, moodleFile);
                    WriteResourceTag(writer, moodleFile);

                    writer.WriteEndElement(); // Close <activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_FILE_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_FILE_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// writes the activity tag and its attributes.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="moodleFile">initialized MoodleFile object to write.</param>
        private void WriteActivityAttributes(XmlWriter writer, MoodleFile moodleFile)
        {
            writer.WriteAttributeString("id", moodleFile.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", moodleFile.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("modulename", moodleFile.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", _generalHelper.EnumIntegerToString(moodleFile.Context));
        }

        /// <summary>
        /// writes the resource element and its child elements.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        /// <param name="moodleFile">initialized MoodleFile object to write.</param>
        private void WriteResourceTag(XmlWriter writer, MoodleFile moodleFile)
        {
            writer.WriteStartElement("resource");
            writer.WriteAttributeString("id", moodleFile.ActivityId.ToString());

            // Write contents of resource tag
            var elements = new Dictionary<string, string>
            {
                { "name", moodleFile.FileName },
                { "intro", moodleFile.Description ?? "" },
                { "introformat", _generalHelper.EnumIntegerToString(moodleFile.DescriptionFormat) },
                { "tobemigrated", "0" }, // TODO: Determine meaning of this value
                { "legacyfiles", "0" }, // TODO: Determine meaning of this value
                { "legacyfileslast", "$@NULL@$" }, // TODO: Determine meaning of this value
                { "display", "0" }, // TODO: Determine meaning of this value
                { "displayoptions", "a:2:{s:10:\"printintro\";i:1;s:8:\"showtype\";i:1;}" }, // TODO: Determine meaning of this value
                { "filterfiles", "0" }, // TODO: Determine meaning of this value
                { "revision", "1" }, // TODO: Determine meaning of this value
                { "timemodified", _generalHelper.DateTimeToUnixTimestamp(moodleFile.TimeModified) },
            };

            WriteElements(writer, elements);

            writer.WriteEndElement(); // Close <resource>
        }
    }
}
