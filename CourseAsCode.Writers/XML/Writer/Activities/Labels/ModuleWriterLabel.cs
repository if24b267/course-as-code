using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Labels
{
    public class ModuleWriterLabel : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;
        private readonly ITagsWriter _tagsWriter;


        /// <summary>
        /// Creates a new instance of the <see cref="ModuleWriterLabel"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="ModuleWriterLabel"/></param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="moodleVersionHelper">Helper method providing specific helper methods focused on Moodle version management</param>
        /// <param name="tagsWriter">Helper method providing specific helper methods focused on tag management</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public ModuleWriterLabel(IXmlWriterFactory xmlWriterFactory, IMoodleVersionHelper moodleVersionHelper, IGeneralHelper generalHelper, ILogger<ModuleWriterLabel> logger, ITagsWriter tagsWriter)
            : base(xmlWriterFactory, logger)
        {
            _moodleVersionHelper = moodleVersionHelper ?? throw new ArgumentNullException(nameof(moodleVersionHelper));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _tagsWriter = tagsWriter ?? throw new ArgumentNullException(nameof(tagsWriter));
        }


        /// <summary>
        /// Writes the module.xml file for a label activity.
        /// </summary>
        /// <param name="outputPath">The directory where module.xml will be saved.</param>
        /// <param name="course">The course model.</param>
        /// <param name="section">The section model.</param>
        /// <param name="activityObject">The activity object to write.</param>
        public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
        {
            try
            {
                // Cast the activityObject to TextAndMediaArea/Label
                if (activityObject is not TextAndMediaArea labelObject)
                {
                    throw new InvalidCastException("The provided activity is not of type Label");
                }

                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("module");

                    writer.WriteAttributeString("id", labelObject.SystemDefaults.ModuleId.ToString());
                    writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    WriteGeneralInformationToModules(writer, outputPath, course, section, labelObject);

                    writer.WriteEndElement(); // Close <module>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
                throw;
            }
        }

        /// <summary>
        /// Writes the general information elements into the module tag.
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document.</param>
        private void WriteGeneralInformationToModules(XmlWriter writer, string outputPath, Model.Course course, Section section, TextAndMediaArea labelObject)
        {
            var elements = new Dictionary<string, string>
            {
                { "modulename", labelObject.SystemDefaults.ModuleName },
                { "sectionid", section.SystemDefaults.SectionId.ToString() },
                { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                { "idnumber", "$@NULL@$" },
                { "added", _generalHelper.DateTimeToUnixTimestamp(labelObject.TimeCreated) },
                { "score", "0" },
                { "indent", "0" },
                { "visible", labelObject.VisibleToStudents ? "1" : "0" },
                { "visibleoncoursepage", labelObject.VisibleToStudents ? "1" : "0" }, // TODO: Determine difference between these 3 visibility attributes
                { "visibleold", labelObject.VisibleToStudents ? "1" : "0" },
                { "groupmode", _generalHelper.EnumIntegerToString(labelObject.GroupMode) },
                { "groupingid", "0" },
                { "completion", "0" },
                { "completiongradeitemnumber", "$@NULL@$" },
                { "completionpassgrade", "0" },
                { "completionview", "0" },
                { "completionexpected", "0" },
                { "availability", "$@NULL@$" },
                { "showdescription", labelObject.DisplayDescription ? "1" : "0" },
                { "downloadcontent", "1" },
                { "lang", _generalHelper.GetLanguageFromEnum(labelObject.Language) },
            };
            WriteElements(writer, elements);

            _tagsWriter.WriteTags(writer, labelObject.Tags, outputPath);
        }
    }
}
