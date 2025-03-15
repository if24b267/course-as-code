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

namespace CourseAsCode.Writers.XML.Writer.Activities.Url
{
    public class ModuleWriterUrl : XmlWriterBase, IActivityModuleWriter
    {
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";
        
        private readonly IMoodleVersionHelper _moodleVersionHelper;
        private readonly IGeneralHelper _generalHelper;
        
        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public ModuleWriterUrl() : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
            _moodleVersionHelper = MoodleVersionHelper.Instance;
            _generalHelper = GeneralHelper.Instance;
        }
        
        /// <summary>
        /// Constructor using Dependency Injection for Unit Testing
        /// </summary>
        /// <param name="moodleVersionHelper">Injected MoodleVersionHelper instance</param>
        /// <param name="generalHelper">Injected GeneralHelper instance</param>
        public ModuleWriterUrl(IXmlWriterFactory xmlWriterFactory, ILogger Logger, IMoodleVersionHelper moodleVersionHelper, IGeneralHelper generalHelper) : base(xmlWriterFactory, Logger)
        {
            _moodleVersionHelper = moodleVersionHelper;
            _generalHelper = generalHelper;
        }
        
        /// <summary>
        /// Writes the modules.xml file required for a URL activity
        /// </summary>
        /// <param name="outputPath">The directory where module.xml will be saved</param>
        /// <param name="course">The course model</param>
        /// <param name="section">The section model</param>
        /// <param name="activityObject">The activity object to write</param>
        public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
        {
            try
            {
                // Cast the activityObject to URL
                if (activityObject is not URL urlObject)
                {
                    throw new InvalidCastException("The provided activity is not of type URL.");
                }
                
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));
                
                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    
                    writer.WriteStartElement("module");
                    writer.WriteAttributeString("id", urlObject.SystemDefaults.ModuleId.ToString());
                    writer.WriteAttributeString("version",
                        _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));
                    
                    // Write contents of module tag
                    var elements = new Dictionary<string, string>
                    {
                        { "modulename", urlObject.SystemDefaults.ModuleName },
                        { "sectionid", section.SystemDefaults.SectionId.ToString() },
                        { "sectionnumber", course.Sections.IndexOf(section).ToString() },
                        { "idnumber", "$@NULL@$" },
                        { "added", _generalHelper.DateTimeToUnixTimestamp(urlObject.TimeCreated) },
                        { "score", "0" },
                        { "indent", "0" },
                        { "visible", urlObject.VisibleToStudents ? "1" : "0" },
                        { "visibleoncoursepage", urlObject.VisibleToStudents ? "1" : "0" },
                        { "visibleold", urlObject.VisibleToStudents ? "1" : "0" },
                        { "groupmode", _generalHelper.EnumIntegerToString(urlObject.GroupMode) },
                        { "groupingid", "0" },
                        { "completion", "0" },
                        { "completiongradeitemnumber", "$@NULL@$" },
                        { "completionpassgrade", "0" },
                        { "completionview", "0" },
                        { "completionexpected", "0" },
                        { "availability", "$@NULL@$" },
                        { "showdescription", urlObject.DisplayDescription ? "1" : "0" },
                        { "downloadcontent", "1" },
                        { "lang", _generalHelper.GetLanguageFromEnum(urlObject.Language) }
                    };
                    WriteElements(writer, elements);
                    
                    WriteTags(writer, urlObject.Tags, outputPath);
                    
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
        /// Writes the tags section of module.xml for a URL activity
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="tags">Tags to be written, represented as a list of strings</param>
        /// <param name="outputPath">The output directory path, used for logging</param>
        private void WriteTags(XmlWriter writer, List<string>? tags, string outputPath)
        {
            int tagId = 0;
            
            writer.WriteStartElement("tags");
            
            if (tags != null)
            {
                foreach (string tag in tags)
                {
                    writer.WriteStartElement("tag");
                    writer.WriteAttributeString("id", tagId++.ToString());
                    
                    writer.WriteElementString("name", tag);
                    writer.WriteElementString("rawname", tag);
                    
                    writer.WriteEndElement(); // Close <tag>
                    LogTrace($"Added tag '{tag}' to {Path.GetFileName(outputPath)}");
                }
            }
            
            writer.WriteEndElement(); // Close <tags>
        }
    }
}
