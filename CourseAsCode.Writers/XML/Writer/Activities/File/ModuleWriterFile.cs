using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.File
{
    internal class ModuleWriterFile : XmlWriterBase, IActivityModuleWriter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_MODULE_RELATIVE_PATH = "module.xml";
        
        private readonly MoodleVersionHelper _moodleVersionHelper;
        private readonly GeneralHelper _generalHelper;
        
        
        // Constructor
        public ModuleWriterFile()
        {
            _moodleVersionHelper = MoodleVersionHelper.Instance;
            _generalHelper = GeneralHelper.Instance;
        }
        
        
        // Constructor using Dependency Injection for Unit Testing
        public ModuleWriterFile(MoodleVersionHelper moodleVersionHelper, GeneralHelper generalHelper)
        {
            _moodleVersionHelper = moodleVersionHelper;
            _generalHelper = generalHelper;
        }
        
        
        /// <summary>
        /// writes the modules.xml file required for a file (resource) activity
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="course"></param>
        /// <param name="section"></param>
        /// <param name="activityObject"></param>
        public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
        {
            try
            {
                // Cast the activityObject to URL
                MoodleFile? moodleFile = (MoodleFile)activityObject;

                // Write the module.xml file
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_MODULE_RELATIVE_PATH));
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();

                    // Create module tag with attributes
                    writer.WriteStartElement("module");
                    writer.WriteAttributeString("id", moodleFile.SystemDefaults.ModuleId.ToString());
                    writer.WriteAttributeString("version",
                        _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));

                    // Write contents of module tag
                    var elements = new Dictionary<string, string>
                    {
                        { "modulename", moodleFile.SystemDefaults.ModuleName },
                        { "sectionid", section.SystemDefaults.SectionId.ToString() },
                        {
                            "sectionnumber", course.Sections.IndexOf(section).ToString()
                        }, // Determine the index of the current section instance within the course's section list
                        { "idnumber", "$@NULL@$" },
                        { "added", _generalHelper.DateTimeToUnixTimestamp(moodleFile.TimeCreated) },
                        { "score", "0" },
                        { "indent", "0" },
                        {
                            "visible", moodleFile.VisibleToStudents ? "1" : "0"
                        }, // TODO: Determine difference between these visibility flags
                        { "visibleoncoursepage", moodleFile.VisibleToStudents ? "1" : "0" },
                        { "visibleold", moodleFile.VisibleToStudents ? "1" : "0" },
                        { "groupmode", _generalHelper.EnumIntegerToString(moodleFile.GroupMode) },
                        { "groupingid", "0" },
                        { "completion", "0" },
                        { "completiongradeitemnumber", "$@NULL@$" },
                        { "completionpassgrade", "0" },
                        { "completionview", "0" },
                        { "completionexpected", "0" },
                        { "availability", "$@NULL@$" },
                        { "showdescription", moodleFile.DisplayDescription ? "1" : "0" },
                        { "downloadcontent", "1" },
                        { "lang", _generalHelper.GetLanguageFromEnum(moodleFile.Language) },
                    };
                    WriteElements(writer, elements);

                    WriteTags(writer, moodleFile.Tags, outputPath);

                    writer.WriteEndElement(); // Close <module>
                    
                    Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_MODULE_RELATIVE_PATH}");
                throw;
            }
        }


        /// <summary>
        /// writes the tags section of module.xml (File/Resource activity)
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="tags">Tags to be written, represented as a list of strings</param>
        /// <param name="outputPath">The path to the directory where the file is being written, used for logging</param>
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
                    
                    Logger.Trace($"Added tag '{tag}' to {Path.GetFileName(outputPath)}");
                }
            }

            writer.WriteEndElement(); // Close <tags>
        }
    }
}
