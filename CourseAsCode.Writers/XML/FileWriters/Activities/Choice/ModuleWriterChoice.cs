using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Choice
{
    public class ModuleWriterChoice : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;
        private readonly ITagsWriter _tagsWriter;

        public ModuleWriterChoice(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterChoice> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, logger)
        {
            _moodleVersionHelper = moodleVersionHelper ?? throw new ArgumentNullException(nameof(moodleVersionHelper));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _tagsWriter = tagsWriter ?? throw new ArgumentNullException(nameof(tagsWriter));
        }

        /// <summary>
        /// Writes the modules.xml file required for a choice activity.
        /// </summary>
        /// <param name="outputPath">The output directory path.</param>
        /// <param name="course">The course model.</param>
        /// <param name="section">The section model.</param>
        /// <param name="activityObject">The activity object to write.</param>
        public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
        {
            try
            {
                if (activityObject is not CourseAsCode.Model.Activities.Choice.Choice choiceObject)
                {
                    throw new InvalidCastException("The provided activity is not of type Choice");
                }

                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    // <module id="..." version="...">
                    writer.WriteStartElement("module");
                    writer.WriteAttributeString("id", choiceObject.SystemDefaults.ModuleId.ToString());
                    writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    // module content
                    var elements = new Dictionary<string, string>
                    {
                        { "modulename", choiceObject.SystemDefaults.ModuleName },
                        { "sectionid", section.SystemDefaults.SectionId.ToString() },
                        { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                        { "idnumber", "$@NULL@$" },
                        { "added", _generalHelper.DateTimeToUnixTimestamp(choiceObject.TimeCreated) },
                        { "score", "0" },
                        { "indent", "0" },
                        { "visible", choiceObject.VisibleToStudents ? "1" : "0" },
                        { "visibleoncoursepage", choiceObject.VisibleToStudents ? "1" : "0" },
                        { "visibleold", choiceObject.VisibleToStudents ? "1" : "0" },
                        { "groupmode", _generalHelper.EnumIntegerToString(choiceObject.GroupMode) },
                        { "groupingid", "0" },
                        { "completion", "0" },
                        { "completiongradeitemnumber", "$@NULL@$" },
                        { "completionpassgrade", "0" },
                        { "completionview", "0" },
                        { "completionexpected", "0" },
                        { "availability", "$@NULL@$" },
                        { "showdescription", choiceObject.DisplayDescription ? "1" : "0" },
                        { "downloadcontent", "1" },
                        { "lang", _generalHelper.GetLanguageFromEnum(choiceObject.Language) },
                    };

                    WriteElements(writer, elements);

                    // plagiarism plugin placeholder (keeps parity with sample)
                    writer.WriteStartElement("plugin_plagiarism_turnitinsim_module");
                    writer.WriteStartElement("turnitinsim_mods");
                    writer.WriteEndElement();
                    writer.WriteStartElement("turnitinsim_subs");
                    writer.WriteStartElement("turnitinsim_sub");
                    writer.WriteAttributeString("id", "");
                    writer.WriteEndElement(); // </turnitinsim_sub>
                    writer.WriteEndElement(); // </turnitinsim_subs>
                    writer.WriteStartElement("turnitinsim_usrs");
                    writer.WriteEndElement();
                    writer.WriteEndElement(); // </plugin_plagiarism_turnitinsim_module>

                    // tags
                    _tagsWriter.WriteTags(writer, choiceObject.Tags, outputPath);

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
    }
}
