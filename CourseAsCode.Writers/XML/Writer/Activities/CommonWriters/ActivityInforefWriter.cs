using System.Xml;
using NLog;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityInforefWriter : XmlWriterBase
    {
        private const string DEFAULT_INFOREF_RELATIVE_PATH = "inforef.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies
        /// </summary>
        public ActivityInforefWriter() 
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing
        /// </summary>
        public ActivityInforefWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        /// <summary>
        /// Writes the inforef.xml file required for an activity
        /// </summary>
        /// <param name="course">The course model containing sections and activities</param>
        /// <param name="outputPath">The output directory path</param>
        public void WriteInforefLabel(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_INFOREF_RELATIVE_PATH));
                
                using (XmlWriter writer = CreateXmlWriter(filePath))
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
                
                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_INFOREF_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_INFOREF_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
