using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Writer.Activities.Interfaces;

namespace CourseAsCode.Writers.XML.Writer.Activities.Url
{
    internal class ModuleWriterUrl : XmlWriterBase, IActivityModuleWriter
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly MoodleVersionHelper _moodleVersionHelper = new MoodleVersionHelper();
        private readonly GeneralHelper _generalHelper = new GeneralHelper();
        
        
        /// <summary>
        /// writes the modules.xml file required for a URL activity
        /// </summary>
        /// <param name="outputPath"></param>
        /// <param name="course"></param>
        /// <param name="section"></param>
        /// <param name="activityObject"></param>
        public void WriteModule(string outputPath, Model.Course course, Section section, Activity activityObject)
        {
            // Cast the activityObject to URL + Error handling
            URL? urlObject = null;
            try
            {
                urlObject = (URL)activityObject;
            }
            catch (Exception ex)
            {
                _logger.Fatal("Couldn't generate module.xml for URL activity. Error: {0}", ex.Message);
                return;
            }
            
            // Write the module.xml file
            string filePath = Path.GetFullPath(Path.Combine(outputPath, "module.xml"));
            using XmlWriter writer = XmlWriter.Create(filePath, Settings);
            
            writer.WriteStartDocument();
            
            // Create module tag with attributes
            writer.WriteStartElement("module");
            writer.WriteAttributeString("id", urlObject.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion));
            
            // Write contents of module tag
            var elements = new Dictionary<string, string>
            {
                { "modulename", "url" },
                { "sectionid", section.SystemDefaults.SectionId.ToString() },
                { "sectionnumber", course.Sections.IndexOf(section).ToString() }, // Determine the index of the current section instance within the course's section list
                { "idnumber", "$@NULL@$" },
                { "added", _generalHelper.DateTimeToUnixTimestamp(urlObject.TimeCreated) },
                { "score", "0" },
                { "indent", "0" },
                { "visible", urlObject.VisibleToStudents ? "1" : "0" }, // TODO: Determine difference between these visibility flags
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
                { "lang", _generalHelper.GetLanguageFromEnum(urlObject.Language) },
            };
            WriteElements(writer, elements);
            
            WriteTags(writer, urlObject.Tags);

            writer.WriteEndElement(); // Close <module>
        }


        /// <summary>
        /// writes the tags section of module.xml (URL activity)
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="tags">Tags to be written, represented as a list of strings</param>
        private void WriteTags(XmlWriter writer, List<string>? tags)
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
                }
            }

            writer.WriteEndElement(); // Close <tags>
        }
    }
}
