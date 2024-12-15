using System.Xml;
using CourseModel = CourseAsCode.Model.Course;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Course;

public class CourseWriter : XmlWriterBase
{
    public void WriteCourse(CourseModel course, string filePath = "course/course.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            // root element <course> with attributes
            writer.WriteStartElement("course");
            writer.WriteAttributeString("id", CourseModel.SystemDefaults.Id.ToString());
            writer.WriteAttributeString("contextid", "14"); // muss noch besprochen werden

            // child elements of <course>
            writer.WriteElementString("shortname", course.ShortName);
            writer.WriteElementString("fullname", course.FullName);
            writer.WriteElementString("idnumber", CourseModel.SensibleDefaults.CourseIdNumber.ToString());
            writer.WriteElementString("summary", CourseModel.SensibleDefaults.NumberOfAnnouncementsToDisplay.HasValue ? $"<p>{course.Summary ?? "Default Summary"}</p>" : "<p></p>");
            writer.WriteElementString("summaryformat", ((int)CourseModel.SystemDefaults.SummaryFormat).ToString());
            writer.WriteElementString("format", CourseModel.SensibleDefaults.Format.ToString().ToLower());
            writer.WriteElementString("showgrades", CourseModel.SensibleDefaults.ShowGrades ? "1" : "0");
            writer.WriteElementString("newsitems", CourseModel.SensibleDefaults.NumberOfAnnouncementsToDisplay?.ToString() ?? "0");
            writer.WriteElementString("startdate", ((int)(course.StartDate - new DateTime(1970, 1, 1)).TotalSeconds).ToString());
            writer.WriteElementString("enddate", ((int)(course.EndDate - new DateTime(1970, 1, 1)).TotalSeconds).ToString());
            writer.WriteElementString("marker", "0");
            writer.WriteElementString("maxbytes", CourseModel.SensibleDefaults.MaximumUploadSize.ToString());
            writer.WriteElementString("legacyfiles", CourseModel.SystemDefaults.UsesLegacyFiles ? "1" : "0");
            writer.WriteElementString("showreports", "0");
            writer.WriteElementString("visible", CourseModel.SensibleDefaults.VisibleToStudents ? "1" : "0");
            writer.WriteElementString("groupmode", course.GroupMode.ToString().ToLower());
            writer.WriteElementString("groupmodeforce", CourseModel.SensibleDefaults.ForceGroupMode ? "1" : "0");
            writer.WriteElementString("defaultgroupingid", "0");
            writer.WriteElementString("lang", CourseModel.SystemDefaults.CourseLanguage?.ToString() ?? "en");
            writer.WriteElementString("theme", CourseModel.SensibleDefaults.Theme ?? "");
            writer.WriteElementString("timecreated", ((int)(CourseModel.SystemDefaults.TimeCreated - new DateTime(1970, 1, 1)).TotalSeconds).ToString());
            writer.WriteElementString("timemodified", ((int)(CourseModel.SystemDefaults.TimeModified - new DateTime(1970, 1, 1)).TotalSeconds).ToString());
            writer.WriteElementString("requested", "0");
            writer.WriteElementString("showactivitydates", CourseModel.SensibleDefaults.ShowActivityDates ? "1" : "0");
            writer.WriteElementString("showcompletionconditions", CourseModel.SensibleDefaults.ShowActivityCompletionConditions ? "1" : "0");
            writer.WriteElementString("pdfexportfont", CourseModel.SystemDefaults.PdfExportFont ?? "$@NULL@$");
            writer.WriteElementString("enablecompletion", CourseModel.SensibleDefaults.EnableCompletionTracking ? "1" : "0");
            writer.WriteElementString("completionnotify", "0");

            // <category> element
            // TODO: Check if multiple categories are allowed and refactor this code to use a loop
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", "1");
            writer.WriteElementString("name", CourseModel.SensibleDefaults.CategoryName);
            writer.WriteElementString("description", CourseModel.SensibleDefaults.CategoryDescription ?? "$@NULL@$");
            writer.WriteEndElement(); // Close <category>

            if (course.Tags != null && course.Tags.Count > 0)
            {
                writer.WriteStartElement("tags");
                foreach (var tag in course.Tags)
                {
                    writer.WriteStartElement("tag");
                    writer.WriteAttributeString("id", "1"); // Angenommene Tag-ID
                    writer.WriteElementString("name", tag);
                    writer.WriteElementString("rawname", tag);
                    writer.WriteEndElement(); // Schlieﬂt <tag>
                }
                writer.WriteEndElement(); // Schlieﬂt <tags>
            }

            writer.WriteStartElement("customfields");
            writer.WriteString(CourseModel.SystemDefaults.CustomFields ?? " ");
            writer.WriteEndElement(); // Close <customfields>

            // <courseformatoptions> element
            writer.WriteStartElement("courseformatoptions");
            foreach (var section in course.Sections)
            {
                writer.WriteStartElement("courseformatoption");
                writer.WriteElementString("format", CourseModel.SensibleDefaults.Format.ToString().ToLower());
                writer.WriteElementString("sectionid", "0"); // Section ID should be dynamic if available
                writer.WriteElementString("name", "coursedisplay");
                writer.WriteElementString("value", "0");
                writer.WriteEndElement(); // Close <courseformatoption>
            }
            writer.WriteEndElement(); // Close <courseformatoptions>

            writer.WriteEndElement(); // Close <course>

            writer.WriteEndDocument();
        }
    }
}