using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.Course;

public class CourseWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteCourse(Course course, string filePath = "course/course.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("course");
            writer.WriteAttributeString("id", course.Id.ToString());
            writer.WriteAttributeString("contextid", "14");

            // child elements of <course>
            writer.WriteElementString("shortname", course.ShortName);
            writer.WriteElementString("fullname", course.FullName);
            writer.WriteElementString("idnumber", course.CourseIdNumber.ToString());
            writer.WriteElementString("summary", course.Summary ?? string.Empty);
            writer.WriteElementString("summaryformat", ((int)course.SummaryFormat).ToString());
            writer.WriteElementString("format", course.Format.ToString().ToLower());
            writer.WriteElementString("showgrades", course.ShowGrades ? "1" : "0");
            writer.WriteElementString("newsitems", course.NumberOfAnnouncementsToDisplay?.ToString() ?? "0");
            writer.WriteElementString("startdate", ((DateTimeOffset)course.StartDate).ToUnixTimeSeconds().ToString());
            writer.WriteElementString("enddate", ((DateTimeOffset)course.EndDate).ToUnixTimeSeconds().ToString());
            writer.WriteElementString("marker", "0");
            writer.WriteElementString("maxbytes", course.MaximumUploadSize.ToString());
            writer.WriteElementString("legacyfiles", course.UsesLegacyFiles ? "1" : "0");
            writer.WriteElementString("showreports", "0");
            writer.WriteElementString("visible", course.VisibleToStudents ? "1" : "0");
            writer.WriteElementString("groupmode", ((int)course.GroupMode).ToString());
            writer.WriteElementString("groupmodeforce", course.ForceGroupMode ? "1" : "0");
            writer.WriteElementString("defaultgroupingid", "0");
            writer.WriteElementString("lang", course.CourseLanguage?.ToString() ?? string.Empty);
            writer.WriteElementString("theme", course.Theme ?? string.Empty);
            writer.WriteElementString("timecreated", ((DateTimeOffset)course.TimeCreated).ToUnixTimeSeconds().ToString());
            writer.WriteElementString("timemodified", ((DateTimeOffset)course.TimeModified).ToUnixTimeSeconds().ToString());
            writer.WriteElementString("requested", "0");
            writer.WriteElementString("showactivitydates", course.ShowActivityDates ? "1" : "0");
            writer.WriteElementString("showcompletionconditions", course.ShowActivityCompletionConditions ? "1" : "0");
            writer.WriteElementString("pdfexportfont", course.PdfExportFont ?? "$@NULL@$");
            writer.WriteElementString("enablecompletion", course.EnableCompletionTracking ? "1" : "0");
            writer.WriteElementString("completionnotify", "0");

            // <category> element
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", "1");
            writer.WriteElementString("name", course.CategoryName);
            writer.WriteElementString("description", course.CategoryDescription ?? "$@NULL@$");
            writer.WriteEndElement(); // Close <category>

            // <tags> element
            writer.WriteStartElement("tags");
            if (course.Tags != null)
            {
                foreach (var tag in course.Tags)
                {
                    writer.WriteStartElement("tag");
                    writer.WriteAttributeString("id", "1");
                    writer.WriteElementString("name", tag);
                    writer.WriteElementString("rawname", tag);
                    writer.WriteEndElement(); // Close <tag>
                }
            }
            writer.WriteEndElement(); // Close <tags>

            writer.WriteStartElement("customfields");
            writer.WriteString(course.CustomFields ?? " ");
            writer.WriteEndElement(); // Close <customfields>

            // <courseformatoptions> element
            writer.WriteStartElement("courseformatoptions");

            // Add <courseformatoption> elements
            writer.WriteStartElement("courseformatoption");
            writer.WriteElementString("format", course.Format.ToString().ToLower());
            writer.WriteElementString("sectionid", "0");
            writer.WriteElementString("name", "coursedisplay");
            writer.WriteElementString("value", "0");
            writer.WriteEndElement(); // Close <courseformatoption>

            writer.WriteStartElement("courseformatoption");
            writer.WriteElementString("format", course.Format.ToString().ToLower());
            writer.WriteElementString("sectionid", "0");
            writer.WriteElementString("name", "hiddensections");
            writer.WriteElementString("value", "1");
            writer.WriteEndElement(); // Close <courseformatoption>

            writer.WriteEndElement(); // Close <courseformatoptions>

            writer.WriteEndElement(); // Close <course>

            writer.WriteEndDocument();
        }
    }
}
