using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities
{
    /// <summary>
    /// Generic abstract base class for activity module writers. Consolidates the shared XML structure
    /// of module.xml files across all activity types, exposing abstract and virtual methods for the
    /// small number of values that vary per activity.
    /// </summary>
    /// <typeparam name="TActivity">The concrete activity model type (e.g., Assignment, Quiz).</typeparam>
    public abstract class ModuleWriterBase<TActivity> : XmlWriterBase, IActivityModuleWriter
        where TActivity : Activity
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        protected readonly IMoodleVersionHelper MoodleVersionHelper;
        protected readonly IGeneralHelper GeneralHelper;
        protected readonly ITagsWriter TagsWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleWriterBase{TActivity}"/> class with the
        /// shared dependencies required by all module writers.
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriter instances.</param>
        /// <param name="moodleVersionHelper">Helper for Moodle version string resolution.</param>
        /// <param name="generalHelper">General helper providing timestamp conversion, language mapping, etc.</param>
        /// <param name="logger">Logger instance.</param>
        /// <param name="tagsWriter">Helper for writing activity tags into module XML.</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null.</exception>
        protected ModuleWriterBase(
            IXmlWriterFactory xmlWriterFactory,
            IMoodleVersionHelper moodleVersionHelper,
            IGeneralHelper generalHelper,
            ILogger logger,
            ITagsWriter tagsWriter)
            : base(xmlWriterFactory, logger)
        {
            MoodleVersionHelper = moodleVersionHelper ?? throw new ArgumentNullException(nameof(moodleVersionHelper));
            GeneralHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            TagsWriter = tagsWriter ?? throw new ArgumentNullException(nameof(tagsWriter));
        }

        // ──────────────────────────────────────────────
        //  Abstract methods — MUST be overridden
        // ──────────────────────────────────────────────

        /// <summary>
        /// Returns the value for the <c>id</c> attribute on the root <c>&lt;module&gt;</c> element.
        /// </summary>
        protected abstract string GetModuleId(TActivity activity);

        /// <summary>
        /// Returns the value for the <c>modulename</c> element (e.g., "assign", "quiz").
        /// </summary>
        protected abstract string GetModuleName(TActivity activity);

        /// <summary>
        /// Returns the value for the <c>idnumber</c> element (e.g., "$@NULL@$", "", or a custom value).
        /// </summary>
        protected abstract string GetIdNumber(TActivity activity);

        // ──────────────────────────────────────────────
        //  Virtual methods — override only when needed
        // ──────────────────────────────────────────────

        /// <summary>
        /// Returns the value for the <c>indent</c> element. Default: "0".
        /// </summary>
        protected virtual string GetIndent(TActivity activity) => "0";

        /// <summary>
        /// Returns the value for the <c>groupingid</c> element. Default: "0".
        /// </summary>
        protected virtual string GetGroupingId(TActivity activity) => "0";

        /// <summary>
        /// Returns the value for the <c>completion</c> element. Default: "0".
        /// </summary>
        protected virtual string GetCompletion(TActivity activity) => "0";

        /// <summary>
        /// Returns the value for the <c>lang</c> element. Default: language resolved via <see cref="IGeneralHelper"/>.
        /// </summary>
        protected virtual string GetLang(TActivity activity) =>
            GeneralHelper.GetLanguageFromEnum(activity.Language);

        /// <summary>
        /// Hook for writing additional XML content after the tags and before
        /// the closing <c>&lt;/module&gt;</c> tag. Override to write plagiarism plugin blocks or
        /// other activity-specific content. Default: no-op.
        /// </summary>
        protected virtual void WriteAdditionalContent(XmlWriter writer, TActivity activity)
        {
            // No-op by default — subclasses override as needed.
        }

        // ──────────────────────────────────────────────
        //  WriteModule — the single shared implementation
        // ──────────────────────────────────────────────

        /// <inheritdoc />
        public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
        {
            try
            {
                if (activityObject is not TActivity typed)
                {
                    throw new InvalidCastException(
                        $"The provided activity is not of type {typeof(TActivity).Name}");
                }

                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("module");
                    writer.WriteAttributeString("id", GetModuleId(typed));
                    writer.WriteAttributeString("version",
                        MoodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    var elements = new Dictionary<string, string>
                    {
                        { "modulename", GetModuleName(typed) },
                        { "sectionid", section.SystemDefaults.SectionId.ToString() },
                        { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                        { "idnumber", GetIdNumber(typed) },
                        { "added", GeneralHelper.DateTimeToUnixTimestamp(typed.TimeCreated) },
                        { "score", "0" },
                        { "indent", GetIndent(typed) },
                        { "visible", typed.VisibleToStudents ? "1" : "0" },
                        { "visibleoncoursepage", typed.VisibleToStudents ? "1" : "0" },
                        { "visibleold", typed.VisibleToStudents ? "1" : "0" },
                        { "groupmode", GeneralHelper.EnumIntegerToString(typed.GroupMode) },
                        { "groupingid", GetGroupingId(typed) },
                        { "completion", GetCompletion(typed) },
                        { "completiongradeitemnumber", "$@NULL@$" },
                        { "completionpassgrade", "0" },
                        { "completionview", "0" },
                        { "completionexpected", "0" },
                        { "availability", "$@NULL@$" },
                        { "showdescription", typed.DisplayDescription ? "1" : "0" },
                        { "downloadcontent", "1" },
                        { "lang", GetLang(typed) },
                    };

                    WriteElements(writer, elements);

                    TagsWriter.WriteTags(writer, typed.Tags, outputPath);

                    WriteAdditionalContent(writer, typed);

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

        // ──────────────────────────────────────────────
        //  Shared helper for empty plagiarism plugin block
        // ──────────────────────────────────────────────

        /// <summary>
        /// Writes the empty plagiarism plugin placeholder block used by Choice, Feedback, Folder,
        /// Page and Scheduler module writers. Call this from <see cref="WriteAdditionalContent"/>
        /// in subclasses that need the placeholder.
        /// </summary>
        protected void WriteEmptyPlagiarismPlugin(XmlWriter writer)
        {
            writer.WriteStartElement("plugin_plagiarism_turnitinsim_module");

            writer.WriteStartElement("turnitinsim_mods");
            writer.WriteEndElement(); // </turnitinsim_mods>

            writer.WriteStartElement("turnitinsim_subs");
            writer.WriteStartElement("turnitinsim_sub");
            writer.WriteAttributeString("id", "");
            writer.WriteEndElement(); // </turnitinsim_sub>
            writer.WriteEndElement(); // </turnitinsim_subs>

            writer.WriteStartElement("turnitinsim_usrs");
            writer.WriteEndElement(); // </turnitinsim_usrs>

            writer.WriteEndElement(); // </plugin_plagiarism_turnitinsim_module>
        }
    }
}
