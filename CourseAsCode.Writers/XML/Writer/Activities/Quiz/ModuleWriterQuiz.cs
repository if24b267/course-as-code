using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.Quiz;

public class ModuleWriterQuiz : XmlWriterBase, IActivityModuleWriter
{
    private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";

    private readonly IMoodleVersionHelper _moodleVersionHelper;
    private readonly IGeneralHelper _generalHelper;


    /// <summary>
    /// Default constructor using default implementations for dependencies
    /// </summary>
    public ModuleWriterQuiz() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
    {
        _moodleVersionHelper = MoodleVersionHelper.Instance;
        _generalHelper = GeneralHelper.Instance;
    }


    /// <summary>
    /// Constructor using Dependency Injection for Unit Testing
    /// </summary>
    /// <param name="xmlWriterFactory">Injected XML writer factory</param>
    /// <param name="logger">Injected logger instance</param>
    /// <param name="moodleVersionHelper">Injected MoodleVersionHelper instance</param>
    /// <param name="generalHelper">Injected GeneralHelper instance</param>
    public ModuleWriterQuiz(IXmlWriterFactory xmlWriterFactory, ILogger logger,
        IMoodleVersionHelper moodleVersionHelper, IGeneralHelper generalHelper) : base(xmlWriterFactory, logger)
    {
        _moodleVersionHelper = moodleVersionHelper;
        _generalHelper = generalHelper;
    }


    /// <summary>
    /// Writes the modules.xml file required for a Quiz activity
    /// </summary>
    /// <param name="outputPath">The directory where module.xml will be saved</param>
    /// <param name="course">The course model</param>
    /// <param name="section">The section model</param>
    /// <param name="activityObject">The activity object to write</param>
    public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
    {
        try
        {
            // Ensure the passed activity is a Quiz
            if (activityObject is not Model.Activities.Resources.Quiz quiz)
            {
                throw new InvalidCastException("The provided activity is not of type Quiz");
            }

            string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));

            using (XmlWriter writer = CreateXmlWriter(filePath))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("module");
                writer.WriteAttributeString("id", quiz.SystemDefaults.ModuleId.ToString());
                writer.WriteAttributeString("version",
                    _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                var moduleElements = new Dictionary<string, string>
                {
                    { "modulename", quiz.SystemDefaults.ModuleName },
                    { "sectionid", section.SystemDefaults.SectionId.ToString() },
                    { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                    { "idnumber", string.Empty },
                    { "added", _generalHelper.DateTimeToUnixTimestamp(quiz.TimeCreated) },
                    { "score", "0" },
                    { "indent", "0" },
                    { "visible", quiz.VisibleToStudents ? "1" : "0" },
                    { "visibleoncoursepage", quiz.VisibleToStudents ? "1" : "0" },
                    { "visibleold", quiz.VisibleToStudents ? "1" : "0" },
                    { "groupmode", _generalHelper.EnumIntegerToString(quiz.GroupMode) },
                    { "groupingid", "0" },
                    { "completion", "0" },
                    { "completiongradeitemnumber", "$@NULL@$" },
                    { "completionpassgrade", "0" },
                    { "completionview", "0" },
                    { "completionexpected", "0" },
                    { "availability", "$@NULL@$" },
                    { "showdescription", quiz.DisplayDescription ? "1" : "0" },
                    { "downloadcontent", "1" },
                    { "lang", _generalHelper.GetLanguageFromEnum(quiz.Language) }
                };

                WriteElements(writer, moduleElements);

                // tags
                TagsWriter.WriteTags(writer, quiz.Tags, outputPath, Logger);

                writer.WriteEndElement(); // </module>
                writer.WriteEndDocument();
            }

            LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
        }
        catch (Exception ex)
        {
            LogError(ex, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
            throw;
        }
    }
}