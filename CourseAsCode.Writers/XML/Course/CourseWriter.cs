using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML.Course;

public class CourseWriter : XmlWriterBase
{
    public void WriteCourse(Course course, string filePath = "course/course.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            WriteStartElementWithAttributes(writer, "course", new Dictionary<string, string>
            {
                { "id", course.Id.ToString() },
                { "contextid", "14" }
            });

            WriteCourseElements(writer, course);
            WriteCategory(writer, course);
            WriteTags(writer, course);
            WriteCustomFields(writer, course);
            WriteCourseFormatOptions(writer, course);

            writer.WriteEndElement(); // Close <course>
            writer.WriteEndDocument();
        }
    }

    private void WriteCourseElements(XmlWriter writer, Course course)
    {
        var elements = new Dictionary<string, string>
        {
            { "shortname", course.ShortName },
            { "fullname", course.FullName },
            { "idnumber", course.CourseIdNumber.ToString() },
            { "summary", course.Summary ?? string.Empty },
            { "summaryformat", ((int)course.SummaryFormat).ToString() },
            { "format", course.Format.ToString().ToLower() },
            { "showgrades", course.ShowGrades ? "1" : "0" },
            { "newsitems", course.NumberOfAnnouncementsToDisplay?.ToString() ?? "0" },
            { "startdate", ((DateTimeOffset)course.StartDate).ToUnixTimeSeconds().ToString() },
            { "enddate", ((DateTimeOffset)course.EndDate).ToUnixTimeSeconds().ToString() },
            { "marker", "0" },
            { "maxbytes", course.MaximumUploadSize.ToString() },
            { "legacyfiles", course.UsesLegacyFiles ? "1" : "0" },
            { "showreports", "0" },
            { "visible", course.VisibleToStudents ? "1" : "0" },
            { "groupmode", ((int)course.GroupMode).ToString() },
            { "groupmodeforce", course.ForceGroupMode ? "1" : "0" },
            { "defaultgroupingid", "0" },
            { "lang", course.CourseLanguage?.ToString() ?? string.Empty },
            { "theme", course.Theme ?? string.Empty },
            { "timecreated", ((DateTimeOffset)course.TimeCreated).ToUnixTimeSeconds().ToString() },
            { "timemodified", ((DateTimeOffset)course.TimeModified).ToUnixTimeSeconds().ToString() },
            { "requested", "0" },
            { "showactivitydates", course.ShowActivityDates ? "1" : "0" },
            { "showcompletionconditions", course.ShowActivityCompletionConditions ? "1" : "0" },
            { "pdfexportfont", course.PdfExportFont ?? "$@NULL@$" },
            { "enablecompletion", course.EnableCompletionTracking ? "1" : "0" },
            { "completionnotify", "0" }
        };

        WriteElements(writer, elements);
    }

    private void WriteCategory(XmlWriter writer, Course course)
    {
        WriteStartElementWithAttributes(writer, "category", new Dictionary<string, string>
        {
            { "id", "1" }
        });
        writer.WriteElementString("name", course.CategoryName);
        writer.WriteElementString("description", course.CategoryDescription ?? "$@NULL@$");
        writer.WriteEndElement(); // Close <category>
    }

    private void WriteTags(XmlWriter writer, Course course)
    {
        writer.WriteStartElement("tags");
        if (course.Tags != null)
        {
            foreach (var tag in course.Tags)
            {
                WriteStartElementWithAttributes(writer, "tag", new Dictionary<string, string>
                {
                    { "id", "1" }
                });
                writer.WriteElementString("name", tag);
                writer.WriteElementString("rawname", tag);
                writer.WriteEndElement(); // Close <tag>
            }
        }
        writer.WriteEndElement(); // Close <tags>
    }

    private void WriteCustomFields(XmlWriter writer, Course course)
    {
        writer.WriteStartElement("customfields");
        writer.WriteString(course.CustomFields ?? " ");
        writer.WriteEndElement(); // Close <customfields>
    }

    private void WriteCourseFormatOptions(XmlWriter writer, Course course)
    {
        writer.WriteStartElement("courseformatoptions");

        var formatOptions = new List<(string name, string value)>
        {
            ("coursedisplay", "0"),
            ("hiddensections", "1")
        };

        foreach (var option in formatOptions)
        {
            writer.WriteStartElement("courseformatoption");
            writer.WriteElementString("format", course.Format.ToString().ToLower());
            writer.WriteElementString("sectionid", "0");
            writer.WriteElementString("name", option.name);
            writer.WriteElementString("value", option.value);
            writer.WriteEndElement(); // Close <courseformatoption>
        }

        writer.WriteEndElement(); // Close <courseformatoptions>
    }
}
