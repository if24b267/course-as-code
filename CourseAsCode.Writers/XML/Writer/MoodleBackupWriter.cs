using System.Xml;
using CourseAsCode.Writers.Helper;
using BackupModel =  CourseAsCode.Model.BackupMetadata;

namespace CourseAsCode.Writers.XML.Writer;

public class MoodleBackupWriter : XmlWriterBase
{
	private readonly GeneralHelper _helper = GeneralHelper.Instance;
	private readonly MoodleVersionHelper _moodleVersionHelper = MoodleVersionHelper.Instance;
	private readonly CourseHelper _courseHelper = CourseHelper.Instance;
	
	public void WriteMoodleBackup(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "moodle_backup.xml"));
		
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			
			writer.WriteStartElement("moodle_backup");
			writer.WriteStartElement("information");

			WriteGeneralSettingsToInformationSection(writer, course);
			WriteDetailsSection(writer, course);
			WriteContentsSection(writer, course);
			WriteSettingsSection(writer, course);
			
			writer.WriteEndElement(); // Closes <information>
			writer.WriteEndElement(); // Closes <moodle_backup>
			
			writer.WriteEndDocument();
		}
	}

	
	/// <summary>
	/// writes the general settings within the information section of moodle_backup.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteGeneralSettingsToInformationSection(XmlWriter writer, Model.Course course)
	{
		var elements = new Dictionary<string, string>
		{
			{ "name", _helper.GenerateBackupName(course) },
			{ "moodle_version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion) },
			{ "moodle_release", _moodleVersionHelper.GetMoodleReleaseStringFromEnum(course.BackupMetadata.MoodleVersion) },
			{ "backup_version", _moodleVersionHelper.GetMoodleVersionStringFromEnum(course.BackupMetadata.MoodleVersion) },
			{ "backup_release", _moodleVersionHelper.GetMoodleReleaseStringFromEnum(course.BackupMetadata.MoodleVersion) },
			{ "backup_date", _helper.DateTimeToUnixTimestamp(course.BackupMetadata.SystemDefaults.CreationDate) },
			{ "mnet_remoteusers", course.BackupMetadata.SystemDefaults.MNetRemoteUsers.ToString() },
			{ "include_files", course.BackupMetadata.SensibleDefaults.FilesIncluded ? "1" : "0" },
			{ "include_file_references_to_external_content", course.BackupMetadata.SystemDefaults.IncludeFileReferencesToExternalContent ? "1" : "0" },
			{ "original_wwwroot", course.BackupMetadata.MoodleUrl.ToString() },
			{ "original_site_identifier_hash", "0" }, // should be 0 if site not involved with mnet, see https://moodle.org/mod/forum/discuss.php?d=34679
			{ "original_course_id", course.SystemDefaults.Id.ToString() },
			{ "original_course_format", _courseHelper.GetCourseFormatFromEnum(course.SensibleDefaults.Format) },
			{ "original_course_fullname", course.FullName },
			{ "original_course_shortname", course.ShortName },
			{ "original_course_startdate", _helper.DateTimeToUnixTimestamp(course.StartDate) },
			{ "original_course_enddate", course.EndDate != null ? _helper.DateTimeToUnixTimestamp((DateTime)course.EndDate) : "0" },
			{ "original_course_contextid", Defaults.COURSECONTEXTID.ToString() },
			{ "original_system_contextid", Defaults.SYSTEMCONTEXTID.ToString() }
		};
		
		WriteElements(writer, elements);
	}

	
	/// <summary>
	/// writes the information section of moodle_backup.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteDetailsSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("details");

		WriteStartElementWithAttributes(writer, "detail", new Dictionary<string, string>
		{
			{ "backup_id", course.BackupMetadata.SystemDefaults.BackupId }
		});

		var elements = new Dictionary<string, string>
		{
			{ "type", course.BackupMetadata.SystemDefaults.WholeSiteBackup ? "site" : "course" }, // TODO: Determine if value "site" is correct
			{ "format", course.BackupMetadata.SystemDefaults.UsesNewBackupFormat ? "moodle2" : "moodle1" }, // TODO: Determine if value "moodle1" is correct
			{ "interactive", "1" }, // TODO: Determine the purpose of this field
			{ "mode", "10" }, // TODO: Determine the purpose of this field
			{ "execution", "1" }, // TODO: Determine the purpose of this field
			{ "executiontime", "0" } // TODO: Determine the purpose of this field
		};

		WriteElements(writer, elements);

		writer.WriteEndElement(); // Closes <detail>
		writer.WriteEndElement(); // Closes <details>
	}

	
	/// <summary>
	/// writes the contents section of moodle_backup.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteContentsSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("contents");
		
		// Activities Subsection
		// TODO: All activities must be listed here
		
		// Sections Subsection
		// TODO: All sections must be listed here
		
		// Course Subsection
		WriteContentsCourseSubsection(writer, course);
		
		writer.WriteEndElement(); // Closes <contents>
	}


	/// <summary>
	/// writes the course subsection of the contents section of moodle_backup.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteContentsCourseSubsection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("course");
		
		var elements = new Dictionary<string, string>
		{
			{ "courseid", course.SystemDefaults.Id.ToString() },
			{ "title", course.ShortName },
			{ "directory", "course" }
		};

		WriteElements(writer, elements);
		
		writer.WriteEndElement(); // Closes <course>
	}

	
	/// <summary>
	/// writes the settings section of moodle_backup.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteSettingsSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("settings");

		// Level: Root
		WriteRootLevelSettings(writer, course);
		
		// Level: Section
		// TODO: For each section, we must specify here whether it is included in the backup
		
		// Level: Activity
		// TODO: For each activity, we must specify here whether it is included in the backup
		
		writer.WriteEndElement(); // Closes <settings>
	}


	/// <summary>
	/// writes the root level settings of the settings section of moodle_backup.xml, corresponds to the general backup settings selected by the user in the Moodle UI
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteRootLevelSettings(XmlWriter writer, Model.Course course)
	{ 
		WriteSingleBackupSettingTag(writer, "root", "filename", _helper.GenerateBackupName(course));
		WriteSingleBackupSettingTag(writer, "root", "users", course.BackupMetadata.BackupSettings.IncludeEnrolledUsers ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "anonymize", course.BackupMetadata.BackupSettings.AnonymizeUserInformation ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "role_assignments", course.BackupMetadata.BackupSettings.IncludeUserRoleAssignments ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "activities", course.BackupMetadata.BackupSettings.IncludeActivitiesAndResources ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "blocks", course.BackupMetadata.BackupSettings.IncludeBlocks ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "files", course.BackupMetadata.BackupSettings.IncludeFiles ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "filters", course.BackupMetadata.BackupSettings.IncludeFilters ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "comments", course.BackupMetadata.BackupSettings.IncludeComments ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "badges", course.BackupMetadata.BackupSettings.IncludeBadges ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "calendarevents", course.BackupMetadata.BackupSettings.IncludeCalendarEvents ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "userscompletion", course.BackupMetadata.BackupSettings.IncludeUserCompletionDetails ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "logs", course.BackupMetadata.BackupSettings.IncludeCourseLogs ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "grade_histories", course.BackupMetadata.BackupSettings.IncludeGradeHistory ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "questionbank", course.BackupMetadata.BackupSettings.IncludeQuestionBank ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "groups", course.BackupMetadata.BackupSettings.IncludeGroupsAndGroupings ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "competencies", course.BackupMetadata.BackupSettings.IncludeCompetencies ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "customfield", course.BackupMetadata.BackupSettings.IncludeCustomFields ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "contentbankcontent", course.BackupMetadata.BackupSettings.IncludeContentBankContent ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "xapistate", course.BackupMetadata.BackupSettings.IncludeUserStateInContent ? "1" : "0");
		WriteSingleBackupSettingTag(writer, "root", "legacyfiles", course.BackupMetadata.BackupSettings.IncludeLegacyCourseFiles ? "1" : "0");
	}
	
	
	/// <summary>
	/// writes a single setting tag, for use inside the settings section of moodle_backup.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="level">value of the level tag inside the setting tag</param>
	/// <param name="name">value of the name tag inside the setting tag</param>
	/// <param name="value">value of the value tag inside the setting tag</param>
	private void WriteSingleBackupSettingTag(XmlWriter writer, string level, string name, string value)
	{
		writer.WriteStartElement("setting");
		
		writer.WriteElementString("level", level);
		writer.WriteElementString("name", name);
		writer.WriteElementString("value", value);
		
		writer.WriteEndElement(); // Closes <setting>
	}
}