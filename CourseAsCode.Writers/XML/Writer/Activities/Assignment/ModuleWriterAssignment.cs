using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Assignment
{
    public class ModuleWriterAssignment : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;
        private readonly ITagsWriter _tagsWriter;

        public ModuleWriterAssignment(IXmlWriterFactory xmlWriterFactory, IMoodleVersionHelper moodleVersionHelper, IGeneralHelper generalHelper, ILogger<ModuleWriterAssignment> logger, ITagsWriter tagsWriter)
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
                if (activityObject is not Model.Activities.Resources.Assignment assignmentObject)
                {
                    throw new InvalidCastException("The provided activity is not of type Assignment");
                }

                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("module");

                    writer.WriteAttributeString("id", assignmentObject.ActivityId.ToString());
                    writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    WriteGeneralInformationToModules(writer, outputPath, course, section, assignmentObject);
                    WritePlagiarismTurnitinSimModule(writer, assignmentObject);

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

        private void WriteGeneralInformationToModules(XmlWriter writer, string outputPath, Model.Course course, Section section, Model.Activities.Resources.Assignment assignmentObject)
        {
            var elements = new Dictionary<string, string>
            {
                { "modulename", "assign" },
                { "sectionid", section.SystemDefaults.SectionId.ToString() },
                { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                { "idnumber", "$@NULL@$" },
                { "added", _generalHelper.DateTimeToUnixTimestamp(assignmentObject.TimeCreated) },
                { "score", "0" },
                { "indent", "1" },
                { "visible", assignmentObject.VisibleToStudents ? "1" : "0" },
                { "visibleoncoursepage", assignmentObject.VisibleToStudents ? "1" : "0" },
                { "visibleold", assignmentObject.VisibleToStudents ? "1" : "0" },
                { "groupmode", _generalHelper.EnumIntegerToString(assignmentObject.GroupMode) },
                { "groupingid", assignmentObject.TeamSubmissionGroupingId.ToString() },
                { "completion", assignmentObject.CompletionSubmit ? "1" : "0" },
                { "completiongradeitemnumber", "$@NULL@$" },
                { "completionpassgrade", "0" },
                { "completionview", "0" },
                { "completionexpected", "0" },
                { "availability", "$@NULL@$" },
                { "showdescription", assignmentObject.DisplayDescription ? "1" : "0" },
                { "downloadcontent", "1" },
                { "lang", _generalHelper.GetLanguageFromEnum(assignmentObject.Language) },
            };
            WriteElements(writer, elements);

            _tagsWriter.WriteTags(writer, assignmentObject.Tags, outputPath);
        }

        private void WritePlagiarismTurnitinSimModule(XmlWriter writer, Model.Activities.Resources.Assignment assignmentObject)
        {
            writer.WriteStartElement("plugin_plagiarism_turnitinsim_module");

            writer.WriteStartElement("turnitinsim_mods");
            writer.WriteStartElement("turnitinsim_mod");
            writer.WriteAttributeString("id", assignmentObject.ActivityId.ToString());

            var turnitinElements = new Dictionary<string, string>
            {
                { "turnitinenabled", "0" },
                { "reportgeneration", "0" },
                { "queuedrafts", "0" },
                { "addtoindex", "0" },
                { "excludequotes", "0" },
                { "excludebiblio", "0" },
                { "accessstudents", "0" },
            };
            WriteElements(writer, turnitinElements);

            writer.WriteEndElement(); // Close <turnitinsim_mod>
            writer.WriteEndElement(); // Close <turnitinsim_mods>

            writer.WriteStartElement("turnitinsim_subs");
            writer.WriteStartElement("turnitinsim_sub");
            writer.WriteAttributeString("id", "");
            writer.WriteEndElement(); // Close <turnitinsim_sub>
            writer.WriteEndElement(); // Close <turnitinsim_subs>

            writer.WriteStartElement("turnitinsim_usrs");
            writer.WriteEndElement(); // Close <turnitinsim_usrs>

            writer.WriteEndElement(); // Close <plugin_plagiarism_turnitinsim_module>
        }
    }
}
