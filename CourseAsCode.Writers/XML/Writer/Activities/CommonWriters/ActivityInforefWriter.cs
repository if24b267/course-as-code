using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using NLog;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityInforefWriter : XmlWriterBase
    {
        private const string DEFAULT_INFOREF_RELATIVE_PATH = "inforef.xml";

        /// <summary>
        /// Default constructor using default implementations for dependencies.
        /// </summary>
        public ActivityInforefWriter()
            : base(new DefaultXmlWriterFactory(), LogManager.GetCurrentClassLogger())
        {
        }

        
        /// <summary>
        /// Constructor supporting Dependency Injection for Unit Testing.
        /// </summary>
        public ActivityInforefWriter(IXmlWriterFactory xmlWriterFactory, ILogger logger)
            : base(xmlWriterFactory, logger)
        {
        }

        
        /// <summary>
        /// Writes the inforef.xml file required for an activity.
        /// </summary>
        /// <param name="course">The course model containing sections and activities</param>
        /// <param name="outputPath">The output directory path</param>
        /// <param name="activityInstace">the activity to which this inforef.xml file belongs</param>
        public void WriteInforefLabel(Model.Course course, string outputPath, Activity activityInstace)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_INFOREF_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("inforef");

                    
                    // For the MoodleFile activity, reference the file
                    if (activityInstace is MoodleFile moodleFile)
                    {
                        writer.WriteStartElement("fileref");
                        WriteOneInforefSection(writer, moodleFile.ActivityId, "file");
                        writer.WriteEndElement(); // Close </fileref>
                    }
                    
                    
                    // For the Quiz activity, reference the question categories
                    if (activityInstace is Model.Activities.Resources.Quiz quiz)
                    {
                        writer.WriteStartElement("question_categoryref");
                    
                        foreach (var categoryGrouping in quiz.Questions.GroupBy(q => q.Category))
                        {
                            writer.WriteStartElement("question_category");

                            writer.WriteElementString("id", categoryGrouping.First().CategoryId.ToString());
                        
                            writer.WriteEndElement(); // Close </question_category>
                        }
                    
                        writer.WriteEndElement(); // Close <question_categoryref>
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
        
        
        /// <summary>
        /// writes the part of the `inforef` section for the corresponding object type
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="id">id of the object to be referenced</param>
        /// <param name="type">type of the object to be referenced</param>
        private void WriteOneInforefSection(XmlWriter writer, int id, string type)
        {
            writer.WriteStartElement(type);

            writer.WriteElementString("id", id.ToString());

            writer.WriteEndElement(); // Close </type>
        }
    }
}
