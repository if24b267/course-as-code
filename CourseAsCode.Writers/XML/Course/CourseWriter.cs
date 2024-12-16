using System.Xml;
using CourseModel = CourseAsCode.Model.Course;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Course;

public class CourseWriter : XmlWriterBase
{
	public void WriteCourse(CourseModel course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/course.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();

			// Wurzelelement <course> mit Attributen
			writer.WriteStartElement("course");
			writer.WriteAttributeString("id", course.SystemDefaults.Id.ToString());
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
			{ "idnumber", course.SensibleDefaults.CourseIdNumber.ToString() },
			{
				"summary",
				course.SensibleDefaults.NumberOfAnnouncementsToDisplay.HasValue
					? $"<p>{course.Summary ?? "Default Summary"}</p>"
					: "<p></p>"
			},
			{ "summaryformat", Helper.Helper.EnumIntegerToString(course.SystemDefaults.SummaryFormat) },
			{ "format", Helper.Helper.GetCourseFormatString(course.SensibleDefaults.Format)},
			{ "showgrades", course.SensibleDefaults.ShowGrades ? "1" : "0" },
			{ "newsitems", course.SensibleDefaults.NumberOfAnnouncementsToDisplay?.ToString() ?? "0" },
			{ "startdate", ((int)(course.StartDate - new DateTime(1970, 1, 1)).TotalSeconds).ToString() },
			{ "enddate", ((int)(course.EndDate - new DateTime(1970, 1, 1)).TotalSeconds).ToString() },
			{ "marker", "0" },
			{ "maxbytes", course.SensibleDefaults.MaximumUploadSize.ToString() },
			{ "legacyfiles", course.SystemDefaults.UsesLegacyFiles ? "1" : "0" },
			{ "showreports", "0" },
			{ "visible", course.SensibleDefaults.VisibleToStudents ? "1" : "0" },
			{ "groupmode", Helper.Helper.EnumIntegerToString(course.GroupMode) },
			{ "groupmodeforce", course.SensibleDefaults.ForceGroupMode ? "1" : "0" },
			{ "defaultgroupingid", "0" },
			{ "lang", course.SystemDefaults.CourseLanguage?.ToString() ?? "en" },
			{ "theme", course.SensibleDefaults.Theme ?? "" },
			{
				"timecreated", Helper.Helper.ConvertToUnixTimestamp(course.SystemDefaults.TimeCreated).ToString()
			},
			{
				"timemodified",
				Helper.Helper.ConvertToUnixTimestamp(course.SystemDefaults.TimeModified).ToString()
			},
			{ "requested", "0" },
			{ "showactivitydates", course.SensibleDefaults.ShowActivityDates ? "1" : "0" },
			{ "showcompletionconditions", course.SensibleDefaults.ShowActivityCompletionConditions ? "1" : "0" },
			{ "pdfexportfont", course.SystemDefaults.PdfExportFont ?? "$@NULL@$" },
			{ "enablecompletion", course.SensibleDefaults.EnableCompletionTracking ? "1" : "0" },
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
		//foreach (var category in course.Categories)
		//{ Laut datenmodell nur eine Kategorie erlaubt
		writer.WriteStartElement("category");
		writer.WriteAttributeString("id", "2");
		writer.WriteElementString("name", course.SensibleDefaults.CategoryName);
		writer.WriteElementString("description", course.SensibleDefaults.CategoryDescription ?? "$@NULL@$");
		writer.WriteEndElement(); // Schlieﬂt <category>
		//}
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
		writer.WriteString(course.SystemDefaults.CustomFields ?? " ");
		writer.WriteEndElement(); // Schlieﬂt <customfields>
	}

	private void WriteCourseFormatOptions(XmlWriter writer, CourseModel course)
	{
		writer.WriteStartElement("courseformatoptions");
		int i = 1;
		foreach (var section in course.Sections)
		{
			writer.WriteStartElement("courseformatoption");
			writer.WriteElementString("format", Helper.Helper.GetCourseFormatString(course.SensibleDefaults.Format));
			writer.WriteElementString("sectionid", i.ToString());
			writer.WriteElementString("name", "coursedisplay");
			writer.WriteElementString("value", "0");
			writer.WriteEndElement(); // Schlieﬂt <courseformatoption>
			i++;
		}

		writer.WriteEndElement(); // Schlieﬂt <courseformatoptions>
	}
}