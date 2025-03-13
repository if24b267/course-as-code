using System.Xml;
using NLog;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityInforefWriter : XmlWriterBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string DEFAULT_INFOREF_RELATIVE_PATH = "inforef.xml";
        
        
        /// <summary>
        /// Writes the inforef.xml file required for an activity.
        /// </summary>
        /// <param name="course">The course model containing sections and activities.</param>
        /// <param name="outputPath">The output directory path.</param>
        public void WriteInforefLabel(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_INFOREF_RELATIVE_PATH));
                
                using (XmlWriter writer = XmlWriter.Create(filePath, Settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("inforef");

                    foreach (Section section in course.Sections)
                    {
                        foreach (Activity activity in section.Activities)
                        {
                            if (activity is MoodleFile moodleFile)
                            {
                                // TODO: Refactor this to only write fileref once - even if there are multiple files
                                writer.WriteStartElement("fileref");
                                writer.WriteStartElement("file");
                                writer.WriteElementString("id", moodleFile.ActivityId.ToString());
                                writer.WriteEndElement(); // Close <file>
                                writer.WriteEndElement(); // Close <fileref>
                            }
                        }
                    }

                    writer.WriteEndElement(); // Close <inforef>
                    writer.WriteEndDocument();
                }
                
                Logger.Debug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_INFOREF_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_INFOREF_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
