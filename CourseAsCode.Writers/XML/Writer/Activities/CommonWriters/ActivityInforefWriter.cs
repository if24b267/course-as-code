using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityInforefWriter : XmlWriterBase
    {
        private const string DEFAULT_INFOREF_RELATIVE_PATH = "inforef.xml";

        public ActivityInforefWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityInforefWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }

        /// <summary>
        /// Writes the inforef.xml file for an activity, referencing users and grade items from the data model.
        /// </summary>
        /// <param name="course">The course model containing enrolled users</param>
        /// <param name="outputPath">The output directory path</param>
        /// <param name="activityInstace">The activity to which this inforef.xml file belongs</param>
        public void WriteInforefLabel(Model.Course course, string outputPath, Activity activityInstace)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_INFOREF_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("inforef");

                    // User references
                    writer.WriteStartElement("userref");
                    if (course.EnrolledUsers != null)
                    {
                        foreach (var user in course.EnrolledUsers)
                        {
                            writer.WriteStartElement("user");
                            writer.WriteElementString("id", user.SystemDefaults.Id.ToString());
                            writer.WriteEndElement(); // </user>
                        }
                    }
                    writer.WriteEndElement(); // </userref>

                    // Grade item references
                    writer.WriteStartElement("grade_itemref");
                    if (activityInstace.GradeItems != null)
                    {
                        foreach (var gradeItem in activityInstace.GradeItems)
                        {
                            writer.WriteStartElement("grade_item");
                            writer.WriteElementString("id", gradeItem.Id.ToString());
                            writer.WriteEndElement(); // </grade_item>
                        }
                    }
                    writer.WriteEndElement(); // </grade_itemref>

                    writer.WriteEndElement(); // </inforef>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_INFOREF_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_INFOREF_RELATIVE_PATH}");
                throw;
            }
        }
    }
}
