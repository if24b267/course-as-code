using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;
using PageModel = CourseAsCode.Model.Activities.Resources.Page;

namespace CourseAsCode.Writers.XML.Writer.Activities.Pages
{
    public class ModuleWriterPage : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;
        private readonly ITagsWriter _tagsWriter;

        public ModuleWriterPage(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterPage> logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, logger)
        {
            _moodleVersionHelper = moodleVersionHelper ?? throw new ArgumentNullException(nameof(moodleVersionHelper));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _tagsWriter = tagsWriter ?? throw new ArgumentNullException(nameof(tagsWriter));
        }

        public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
        {
            try
            {
                if (activityObject is not PageModel pageObject)
                {
                    throw new InvalidCastException("The provided activity is not of type Page");
                }

                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("module");
                    writer.WriteAttributeString("id", pageObject.SystemDefaults.ModuleId.ToString());
                    writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    var elements = new Dictionary<string, string>
                    {
                        { "modulename", pageObject.SystemDefaults.ModuleName },
                        { "sectionid", section.SystemDefaults.SectionId.ToString() },
                        { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                        { "idnumber", string.Empty },
                        { "added", _generalHelper.DateTimeToUnixTimestamp(pageObject.TimeCreated) },
                        { "score", "0" },
                        { "indent", "0" },
                        { "visible", pageObject.VisibleToStudents ? "1" : "0" },
                        { "visibleoncoursepage", pageObject.VisibleToStudents ? "1" : "0" },
                        { "visibleold", pageObject.VisibleToStudents ? "1" : "0" },
                        { "groupmode", _generalHelper.EnumIntegerToString(pageObject.GroupMode) },
                        { "groupingid", "0" },
                        { "completion", "0" },
                        { "completiongradeitemnumber", "$@NULL@$" },
                        { "completionpassgrade", "0" },
                        { "completionview", "0" },
                        { "completionexpected", "0" },
                        { "availability", "$@NULL@$" },
                        { "showdescription", pageObject.DisplayDescription ? "1" : "0" },
                        { "downloadcontent", "1" },
                        { "lang", "$@NULL@$" },
                    };

                    WriteElements(writer, elements);

                    // plugin placeholders
                    writer.WriteStartElement("plugin_plagiarism_turnitinsim_module");
                    writer.WriteStartElement("turnitinsim_mods");
                    writer.WriteEndElement();
                    writer.WriteStartElement("turnitinsim_subs");
                    writer.WriteStartElement("turnitinsim_sub");
                    writer.WriteAttributeString("id", "");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteStartElement("turnitinsim_usrs");
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    // tags
                    _tagsWriter.WriteTags(writer, pageObject.Tags, outputPath);

                    writer.WriteEndElement(); // </module>
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
