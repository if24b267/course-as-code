using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.Wiki
{
    public class ModuleWriterWiki : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;
        private readonly ITagsWriter _tagsWriter;


        /// <summary>
        /// Creates a new instance of the <see cref="ModuleWriterWiki"/> class
        /// </summary>
        public ModuleWriterWiki(IXmlWriterFactory xmlWriterFactory, IMoodleVersionHelper moodleVersionHelper, IGeneralHelper generalHelper, ILogger<ModuleWriterWiki> logger, ITagsWriter tagsWriter)
            : base(xmlWriterFactory, logger)
        {
            _moodleVersionHelper = moodleVersionHelper ?? throw new ArgumentNullException(nameof(moodleVersionHelper));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _tagsWriter = tagsWriter ?? throw new ArgumentNullException(nameof(tagsWriter));
        }


        /// <summary>
        /// Writes the module.xml file for a wiki activity.
        /// </summary>
        public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
        {
            try
            {
                // Cast the activityObject to Wiki
                if (activityObject is not Model.Activities.Resources.Wiki wikiObject)
                {
                    throw new InvalidCastException("The provided activity is not of type Wiki");
                }

                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("module");

                    writer.WriteAttributeString("id", wikiObject.ModuleId.ToString());
                    writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    WriteGeneralInformationToModules(writer, outputPath, course, section, wikiObject);

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
        private void WriteGeneralInformationToModules(XmlWriter writer, string outputPath, Model.Course course, Section section, Model.Activities.Resources.Wiki wikiObject)
        {
            var elements = new Dictionary<string, string>
            {
                { "modulename", "wiki" },
                { "sectionid", section.SystemDefaults.SectionId.ToString() },
                { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                { "idnumber", wikiObject.CustomId?.ToString() ?? "$@NULL@$" },
                { "added", _generalHelper.DateTimeToUnixTimestamp(wikiObject.TimeCreated) },
                { "score", "0" },
                { "indent", "0" },
                { "visible", wikiObject.VisibleToStudents ? "1" : "0" },
                { "visibleoncoursepage", wikiObject.VisibleToStudents ? "1" : "0" },
                { "visibleold", wikiObject.VisibleToStudents ? "1" : "0" },
                { "groupmode", _generalHelper.EnumIntegerToString(wikiObject.GroupMode) },
                { "groupingid", "0" },
                { "completion", "0" },
                { "completiongradeitemnumber", "$@NULL@$" },
                { "completionpassgrade", "0" },
                { "completionview", "0" },
                { "completionexpected", "0" },
                { "availability", "$@NULL@$" },
                { "showdescription", wikiObject.DisplayDescription ? "1" : "0" },
                { "downloadcontent", "1" },
                { "lang", _generalHelper.GetLanguageFromEnum(wikiObject.Language) },
            };
            WriteElements(writer, elements);

            _tagsWriter.WriteTags(writer, wikiObject.Tags, outputPath);
        }
    }
}
