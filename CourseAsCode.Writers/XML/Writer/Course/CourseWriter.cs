using System.Xml;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Static;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class CourseWriter : XmlWriterBase
{
	private readonly CourseHelper _courseHelper = CourseHelper.Instance;
	private readonly GeneralHelper _helper = GeneralHelper.Instance;


	public void WriteCourse(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/course.xml"));

		using XmlWriter writer = XmlWriter.Create(filePath, Settings);
		writer.WriteStartDocument();

		writer.WriteStartElement("course"); // Root <course> tag with attributes
		writer.WriteAttributeString("id", course.SystemDefaults.Id.ToString());
		writer.WriteAttributeString("contextid", GeneralDefaults.COURSECONTEXTID.ToString());

		WriteGeneralInformationToCourseSection(writer, course);
		WriteCategorySection(writer, course);
		WriteTagsSection(writer, course);
		WriteCustomFieldsSection(writer, course);
		WriteCourseFormatOptionsSection(writer, course);

		writer.WriteEndElement(); // Close <course>

		writer.WriteEndDocument();
	}


	/// <summary>
	/// writes general information about the course; to be used within the course section of course/course.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteGeneralInformationToCourseSection(XmlWriter writer, Model.Course course)
	{
		var elements = new Dictionary<string, string>
		{
			{ "shortname", course.ShortName },
			{ "fullname", course.FullName },
			{ "idnumber", course.SensibleDefaults.CourseIdNumber.ToString() ?? "" },
			{ "summary", course.Summary ?? " " },
			{ "summaryformat", _helper.EnumIntegerToString(course.SystemDefaults.SummaryFormat) },
			{ "format", _courseHelper.GetCourseFormatFromEnum(course.SensibleDefaults.Format)},
			{ "showgrades", course.SensibleDefaults.ShowGrades ? "1" : "0" },
			{ "newsitems", course.SensibleDefaults.NumberOfAnnouncementsToDisplay.ToString() ?? "0" },
			{ "startdate", _helper.DateTimeToUnixTimestamp(course.StartDate) },
			{ "enddate", course.EndDate != null ? _helper.DateTimeToUnixTimestamp((DateTime)course.EndDate) : "0" },
			{ "marker", "0" },
			{ "maxbytes", course.SensibleDefaults.MaximumUploadSize.ToString() ?? "0" },
			{ "legacyfiles", course.SystemDefaults.UsesLegacyFiles ? "1" : "0" },
			{ "showreports", "0" },
			{ "visible", course.SensibleDefaults.VisibleToStudents ? "1" : "0" },
			{ "groupmode", _helper.EnumIntegerToString(course.GroupMode) },
			{ "groupmodeforce", course.SensibleDefaults.ForceGroupMode ? "1" : "0" },
			{ "defaultgroupingid", "0" },
			{ "lang", _helper.GetLanguageFromEnum(course.CourseLanguage) },
			{ "theme", course.SensibleDefaults.Theme ?? " " },
			{ "timecreated", _helper.DateTimeToUnixTimestamp(course.SystemDefaults.TimeCreated) },
			{ "timemodified", _helper.DateTimeToUnixTimestamp(course.SystemDefaults.TimeModified) },
			{ "requested", "0" },
			{ "showactivitydates", course.SensibleDefaults.ShowActivityDates ? "1" : "0" },
			{ "showcompletionconditions", course.SensibleDefaults.ShowActivityCompletionConditions ? "1" : "0" },
			{ "pdfexportfont", course.SystemDefaults.PdfExportFont ?? "$@NULL@$" },
			{ "enablecompletion", course.SensibleDefaults.EnableCompletionTracking ? "1" : "0" },
			{ "completionnotify", "0" }
		};

		WriteElements(writer, elements);
	}


	/// <summary>
	/// writes the category section in course/course.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteCategorySection(XmlWriter writer, Model.Course course)
	{
		// A moodle course can only have a single category assigned to it

		writer.WriteStartElement("category");

		writer.WriteAttributeString("id", "0");
		writer.WriteElementString("name", course.SensibleDefaults.CategoryName);
		writer.WriteElementString("description", course.SensibleDefaults.CategoryDescription ?? "$@NULL@$");

		writer.WriteEndElement(); // Closes <category>
	}


	/// <summary>
	/// writes the tags section in course/course.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteTagsSection(XmlWriter writer, Model.Course course)
	{
		if (course.Tags == null || course.Tags.Count == 0)
		{
			return;
		}

		writer.WriteStartElement("tags");

		for (int i = 0; i < course.Tags.Count; i++)
		{
			writer.WriteStartElement("tag");

			writer.WriteAttributeString("id", (i + 1).ToString());
			writer.WriteElementString("name", course.Tags[i]);
			writer.WriteElementString("rawname", course.Tags[i]);

			writer.WriteEndElement(); // Close <tag>
		}

		writer.WriteEndElement(); // Close <tags>
	}


	/// <summary>
	/// writes the custom fields section in course/course.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteCustomFieldsSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("customfields");

		writer.WriteString(course.SystemDefaults.CustomFields ?? "\n  ");

		writer.WriteEndElement(); // Close <customfields>
	}


	/// <summary>
	/// writes the course format options section in course/course.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteCourseFormatOptionsSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("courseformatoptions");

		for (int i = 0; i < 2; i++)
		{
			writer.WriteStartElement("courseformatoption");

			// TODO: Determine what these settings do, see https://moodledev.io/docs/4.5/apis/plugintypes/format#course-format-options
			writer.WriteElementString("format", _courseHelper.GetCourseFormatFromEnum(course.SensibleDefaults.Format));
			writer.WriteElementString("sectionid", "0");
			writer.WriteElementString("name", i == 0 ? "coursedisplay" : "hiddensections");
			writer.WriteElementString("value", i == 0 ? "0" : "1");

			writer.WriteEndElement(); // Close <courseformatoption>
		}

		writer.WriteEndElement(); // Close <courseformatoptions>
	}
}