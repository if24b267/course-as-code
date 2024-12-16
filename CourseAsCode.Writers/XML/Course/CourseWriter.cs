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

            // Wurzelelement <course> mit Attributen
            writer.WriteStartElement("course");
            writer.WriteAttributeString("id", course.SystemDefaultsInstance.Id.ToString());
            writer.WriteAttributeString("contextid", "14"); // muss noch besprochen werden

            WriteCourseElements(writer, course);
            WriteCategory(writer, course);
            WriteTags(writer, course);
            WriteCustomFields(writer, course);
            WriteCourseFormatOptions(writer, course);

            writer.WriteEndElement(); // Schlieﬂt <course>

            writer.WriteEndDocument();
        }
    }

    private void WriteCourseElements(XmlWriter writer, CourseModel course)
    {
        var elements = new Dictionary<string, string>
        {
            { "shortname", course.ShortName },
            { "fullname", course.FullName },
            { "idnumber", course.SensibleDefaultsInstance.CourseIdNumber.ToString() },
            { "summary", course.SensibleDefaultsInstance.NumberOfAnnouncementsToDisplay.HasValue ? $"<p>{course.Summary ?? "Default Summary"}</p>" : "<p></p>" },
            { "summaryformat", ((int)course.SystemDefaultsInstance.SummaryFormat).ToString() },
            { "format", course.SensibleDefaultsInstance.Format.ToString().ToLower() },
            { "showgrades", course.SensibleDefaultsInstance.ShowGrades ? "1" : "0" },
            { "newsitems", course.SensibleDefaultsInstance.NumberOfAnnouncementsToDisplay?.ToString() ?? "0" },
            { "startdate", ((int)(course.StartDate - new DateTime(1970, 1, 1)).TotalSeconds).ToString() },
            { "enddate", ((int)(course.EndDate - new DateTime(1970, 1, 1)).TotalSeconds).ToString() },
            { "marker", "0" },
            { "maxbytes", course.SensibleDefaultsInstance.MaximumUploadSize.ToString() },
            { "legacyfiles", course.SystemDefaultsInstance.UsesLegacyFiles ? "1" : "0" },
            { "showreports", "0" },
            { "visible", course.SensibleDefaultsInstance.VisibleToStudents ? "1" : "0" },
            { "groupmode", course.GroupMode.ToString().ToLower() },
            { "groupmodeforce", course.SensibleDefaultsInstance.ForceGroupMode ? "1" : "0" },
            { "defaultgroupingid", "0" },
            { "lang", course.SystemDefaultsInstance.CourseLanguage?.ToString() ?? "en" },
            { "theme", course.SensibleDefaultsInstance.Theme ?? "" },
            { "timecreated", ((int)(course.SystemDefaultsInstance.TimeCreated - new DateTime(1970, 1, 1)).TotalSeconds).ToString() },
            { "timemodified", ((int)(course.SystemDefaultsInstance.TimeModified - new DateTime(1970, 1, 1)).TotalSeconds).ToString() },
            { "requested", "0" },
            { "showactivitydates", course.SensibleDefaultsInstance.ShowActivityDates ? "1" : "0" },
            { "showcompletionconditions", course.SensibleDefaultsInstance.ShowActivityCompletionConditions ? "1" : "0" },
            { "pdfexportfont", course.SystemDefaultsInstance.PdfExportFont ?? "$@NULL@$" },
            { "enablecompletion", course.SensibleDefaultsInstance.EnableCompletionTracking ? "1" : "0" },
            { "completionnotify", "0" }
        };

        foreach (var element in elements)
        {
            writer.WriteElementString(element.Key, element.Value);
        }
    }

    private void WriteCategory(XmlWriter writer, CourseModel course)
    {
        // Angenommen, dass mehrere Kategorien erlaubt sind
        foreach (var category in course.Categories)
        {
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", "1");
            writer.WriteElementString("name", category.Name);
            writer.WriteElementString("description", category.Description ?? "$@NULL@$");
            writer.WriteEndElement(); // Schlieﬂt <category>
        }
    }

    private void WriteTags(XmlWriter writer, CourseModel course)
    {
        if (course.Tags == null || course.Tags.Count == 0)
            return;

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

    private void WriteCustomFields(XmlWriter writer, CourseModel course)
    {
        writer.WriteStartElement("customfields");
        writer.WriteString(course.SystemDefaultsInstance.CustomFields ?? " ");
        writer.WriteEndElement(); // Schlieﬂt <customfields>
    }

    private void WriteCourseFormatOptions(XmlWriter writer, CourseModel course)
    {
        writer.WriteStartElement("courseformatoptions");
        foreach (var section in course.Sections)
        {
            writer.WriteStartElement("courseformatoption");
            writer.WriteElementString("format", course.SensibleDefaultsInstance.Format.ToString().ToLower());
            writer.WriteElementString("sectionid", section.Id.ToString());
            writer.WriteElementString("name", "coursedisplay");
            writer.WriteElementString("value", "0");
            writer.WriteEndElement(); // Schlieﬂt <courseformatoption>
        }
        writer.WriteEndElement(); // Schlieﬂt <courseformatoptions>
    }
}
