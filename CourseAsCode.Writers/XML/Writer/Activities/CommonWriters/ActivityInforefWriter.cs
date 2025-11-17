using System.Xml;
using System.Linq;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Activities.CommonWriters
{
    public class ActivityInforefWriter : XmlWriterBase
    {
        private const string DEFAULT_INFOREF_RELATIVE_PATH = "inforef.xml";
        private readonly IFileHelper _fileHelper;

        public ActivityInforefWriter(IXmlWriterFactory xmlWriterFactory, ILogger<ActivityInforefWriter> logger, IFileHelper fileHelper)
            : base(xmlWriterFactory, logger)
        {
            _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
        }

        /// <summary>
        /// Writes the inforef.xml file for an activity, referencing users, files and grade items from the data model.
        /// </summary>
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

                    // File references (new)
                    writer.WriteStartElement("fileref");
                    // GetFileIdsForActivity returns IEnumerable<int>, implement it in IFileHelper/FileHelper
                    var fileIds = _fileHelper.GetFileIdsForActivity(course, activityInstace) ?? Enumerable.Empty<int>();
                    foreach (var fid in fileIds)
                    {
                        writer.WriteStartElement("file");
                        writer.WriteElementString("id", fid.ToString());
                        writer.WriteEndElement(); // </file>
                    }
                    writer.WriteEndElement(); // </fileref>

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
