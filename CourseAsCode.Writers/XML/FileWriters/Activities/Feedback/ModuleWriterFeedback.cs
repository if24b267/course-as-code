using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;
using FeedbackModel = CourseAsCode.Model.Activities.Resources.Feedback;

namespace CourseAsCode.Writers.XML.Writer.Activities.Feedback;

public class ModuleWriterFeedback : XmlWriterBase, IActivityModuleWriter
{
    private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

    private readonly IMoodleVersionHelper _moodleVersionHelper;
    private readonly IGeneralHelper _generalHelper;
    private readonly ITagsWriter _tagsWriter;

    public ModuleWriterFeedback(
        IXmlWriterFactory xmlWriterFactory,
        IMoodleVersionHelper moodleVersionHelper,
        IGeneralHelper generalHelper,
        ILogger<ModuleWriterFeedback> logger,
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
            if (activityObject is not FeedbackModel feedbackObject)
            {
                throw new InvalidCastException("The provided activity is not of type Feedback");
            }

            string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));
            using (XmlWriter writer = CreateXmlWriter(filePath))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("module");
                writer.WriteAttributeString("id", feedbackObject.SystemDefaults.ModuleId.ToString());
                writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                var elements = new Dictionary<string, string>
                {
                    { "modulename", feedbackObject.SystemDefaults.ModuleName },
                    { "sectionid", section.SystemDefaults.SectionId.ToString() },
                    { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                    { "idnumber", string.Empty },
                    { "added", _generalHelper.DateTimeToUnixTimestamp(feedbackObject.TimeCreated) },
                    { "score", "0" },
                    { "indent", "0" },
                    { "visible", feedbackObject.VisibleToStudents ? "1" : "0" },
                    { "visibleoncoursepage", feedbackObject.VisibleToStudents ? "1" : "0" },
                    { "visibleold", feedbackObject.VisibleToStudents ? "1" : "0" },
                    { "groupmode", _generalHelper.EnumIntegerToString(feedbackObject.GroupMode) },
                    { "groupingid", "0" },
                    { "completion", "0" },
                    { "completiongradeitemnumber", "$@NULL@$" },
                    { "completionpassgrade", "0" },
                    { "completionview", "0" },
                    { "completionexpected", "0" },
                    { "availability", "$@NULL@$" },
                    { "showdescription", feedbackObject.DisplayDescription ? "1" : "0" },
                    { "downloadcontent", "1" },
                    { "lang", "$@NULL@$" },
                };

                WriteElements(writer, elements);

                // plugin placeholders to mirror other module writers
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
                _tagsWriter.WriteTags(writer, feedbackObject.Tags, outputPath);

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
