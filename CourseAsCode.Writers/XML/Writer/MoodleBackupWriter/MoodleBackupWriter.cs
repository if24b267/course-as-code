using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Helper;
using BackupModel =  CourseAsCode.Model.BackupMetadata;

namespace CourseAsCode.Writers.XML.Writer.MoodleBackupWriter;

public class MoodleBackupWriter : XmlWriterBase
{
	private readonly GeneralHelper _helper = GeneralHelper.Instance;
	private readonly MoodleVersionHelper _moodleVersionHelper = MoodleVersionHelper.Instance;
	private readonly CourseHelper _courseHelper = CourseHelper.Instance;
	
	public void WriteMoodleBackup(BackupModel backupMetadata, Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "moodle_backup.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();

			writer.WriteStartElement("moodle_backup");

			WriteInformationSection(writer, course);
			WriteDetails(writer, backupMetadata);
			WriteContents(writer, backupMetadata);
			WriteSettings(writer, backupMetadata);
			writer.WriteEndElement(); // information schlieﬂen
			writer.WriteEndElement(); // moodle_backup schlieﬂen
			writer.WriteEndDocument();
		}
	}

	
	/// <summary>
	/// writes the information section of moodle_backup.xml
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteInformationSection(XmlWriter writer, Model.Course course)
	{
		writer.WriteStartElement("information");

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
			{ "original_course_enddate", _helper.DateTimeToUnixTimestamp(course.EndDate) },
			{ "original_course_contextid", Defaults.COURSECONTEXTID.ToString() },
			{ "original_system_contextid", Defaults.SYSTEMCONTEXTID.ToString() }
		};
		
		WriteElements(writer, elements);
		
	}

	private void WriteDetails(XmlWriter writer, BackupModel backupMetadata)
	{
		writer.WriteStartElement("details");

		WriteStartElementWithAttributes(writer, "detail", new Dictionary<string, string>
		{
			{ "backup_id", backupMetadata.SystemDefaults.BackupId }
		});

		var elements = new Dictionary<string, string>
		{
			{ "type", backupMetadata.SystemDefaults.WholeSiteBackup ? "site" : "course" },
			{ "format", backupMetadata.SystemDefaults.UsesNewBackupFormat ? "moodle2" : "moodle1" },
			{ "interactive", "1" },
			{ "mode", "70" },
			{ "execution", "2" },
			{ "executiontime", "0" }
		};

		WriteElements(writer, elements);

		writer.WriteEndElement(); // detail schlieﬂen
		writer.WriteEndElement(); // details schlieﬂen
	}

	private void WriteContents(XmlWriter writer, BackupModel backupMetadata)
	{
		writer.WriteStartElement("contents");
		//Bei bedarf etwas einf¸gen
		writer.WriteEndElement(); // contents schlieﬂen
	}

	private void WriteSettings(XmlWriter writer, BackupModel backupMetadata)
	{
		writer.WriteStartElement("settings");

		// Root-Einstellungen
		WriteBackupSetting(writer, "root", "filename", "backup.mbz");
		WriteBackupSetting(writer, "root", "users", backupMetadata.BackupSettings.IncludeEnrolledUsers ? "1" : "0");
		WriteBackupSetting(writer, "root", "anonymize",
			backupMetadata.BackupSettings.AnonymizeUserInformation ? "1" : "0");
		WriteBackupSetting(writer, "root", "role_assignments",
			backupMetadata.BackupSettings.IncludeUserRoleAssignments ? "1" : "0");
		WriteBackupSetting(writer, "root", "activities",
			backupMetadata.BackupSettings.IncludeActivitiesAndResources ? "1" : "0");
		WriteBackupSetting(writer, "root", "blocks", backupMetadata.BackupSettings.IncludeBlocks ? "1" : "0");
		WriteBackupSetting(writer, "root", "files", backupMetadata.BackupSettings.IncludeFiles ? "1" : "0");
		WriteBackupSetting(writer, "root", "filters", backupMetadata.BackupSettings.IncludeFilters ? "1" : "0");
		WriteBackupSetting(writer, "root", "comments", backupMetadata.BackupSettings.IncludeComments ? "1" : "0");
		WriteBackupSetting(writer, "root", "badges", backupMetadata.BackupSettings.IncludeBadges ? "1" : "0");
		WriteBackupSetting(writer, "root", "calendarevents",
			backupMetadata.BackupSettings.IncludeCalendarEvents ? "1" : "0");
		WriteBackupSetting(writer, "root", "userscompletion",
			backupMetadata.BackupSettings.IncludeUserCompletionDetails ? "1" : "0");
		WriteBackupSetting(writer, "root", "logs", backupMetadata.BackupSettings.IncludeCourseLogs ? "1" : "0");
		WriteBackupSetting(writer, "root", "grade_histories",
			backupMetadata.BackupSettings.IncludeGradeHistory ? "1" : "0");
		WriteBackupSetting(writer, "root", "questionbank", backupMetadata.BackupSettings.IncludeQuestionBank ? "1" : "0");
		WriteBackupSetting(writer, "root", "groups", backupMetadata.BackupSettings.IncludeGroupsAndGroupings ? "1" : "0");
		WriteBackupSetting(writer, "root", "competencies", backupMetadata.BackupSettings.IncludeCompetencies ? "1" : "0");
		WriteBackupSetting(writer, "root", "customfield", backupMetadata.BackupSettings.IncludeCustomFields ? "1" : "0");
		WriteBackupSetting(writer, "root", "contentbankcontent",
			backupMetadata.BackupSettings.IncludeContentBankContent ? "1" : "0");
		WriteBackupSetting(writer, "root", "xapistate",
			backupMetadata.BackupSettings.IncludeUserStateInContent ? "1" : "0");
		WriteBackupSetting(writer, "root", "legacyfiles",
			backupMetadata.BackupSettings.IncludeLegacyCourseFiles ? "1" : "0");

		writer.WriteEndElement(); // settings schlieﬂen
	}

	private void WriteBackupSetting(XmlWriter writer, string level, string name, string value)
	{
		writer.WriteStartElement("setting");
		writer.WriteElementString("level", level);
		writer.WriteElementString("name", name);
		writer.WriteElementString("value", value);
		writer.WriteEndElement();
	}
}