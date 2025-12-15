using System;
using System.Collections.Generic;
using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using FolderModel = CourseAsCode.Model.Activities.Resources.Folder;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;
using CourseAsCode.Writers.XML.Writer;

namespace CourseAsCode.Writers.XML.Writer.Activities.Folder
{
    public class ModuleWriterFolder : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;
        private readonly ITagsWriter _tagsWriter;

        public ModuleWriterFolder(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger<ModuleWriterFolder> logger,
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
                if (activityObject is not FolderModel folderObject)
                {
                    throw new InvalidCastException("The provided activity is not of type Folder");
                }

                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("module");
                    writer.WriteAttributeString("id", folderObject.SystemDefaults.ModuleId.ToString());
                    writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    var elements = new Dictionary<string, string>
                    {
                        { "modulename", folderObject.SystemDefaults.ModuleName },
                        { "sectionid", section.SystemDefaults.SectionId.ToString() },
                        { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                        { "idnumber", string.Empty },
                        { "added", _generalHelper.DateTimeToUnixTimestamp(folderObject.TimeCreated) },
                        { "score", "0" },
                        { "indent", "0" },
                        { "visible", folderObject.VisibleToStudents ? "1" : "0" },
                        { "visibleoncoursepage", folderObject.VisibleToStudents ? "1" : "0" },
                        { "visibleold", folderObject.VisibleToStudents ? "1" : "0" },
                        { "groupmode", _generalHelper.EnumIntegerToString(folderObject.GroupMode) },
                        { "groupingid", "0" },
                        { "completion", "0" },
                        { "completiongradeitemnumber", "$@NULL@$" },
                        { "completionpassgrade", "0" },
                        { "completionview", "0" },
                        { "completionexpected", "0" },
                        { "availability", "$@NULL@$" },
                        { "showdescription", folderObject.DisplayDescription ? "1" : "0" },
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
                    _tagsWriter.WriteTags(writer, folderObject.Tags, outputPath);

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
