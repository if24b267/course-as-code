using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.Assignment
{
    public class ModuleWriterAssignment : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public ModuleWriterAssignment()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
            _moodleVersionHelper = MoodleVersionHelper.Instance;
            _generalHelper = GeneralHelper.Instance;
        }

        /// <summary>
        /// Constructor using Dependency Injection for Unit Testing.
        /// </summary>
        /// <param name="xmlWriterFactory">Injected XML writer factory.</param>
        /// <param name="moodleVersionHelper">Injected MoodleVersionHelper instance.</param>
        /// <param name="generalHelper">Injected GeneralHelper instance.</param>
        /// <param name="logger">Injected logger instance.</param>
        public ModuleWriterAssignment(IXmlWriterFactory xmlWriterFactory, IMoodleVersionHelper moodleVersionHelper, IGeneralHelper generalHelper, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
            _moodleVersionHelper = moodleVersionHelper;
            _generalHelper = generalHelper;
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
                // Cast the activityObject to Assignment
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
                { "indent", "0" },
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

            TagsWriter.WriteTags(writer, assignmentObject.Tags, outputPath, Logger);
        }

    }
}
