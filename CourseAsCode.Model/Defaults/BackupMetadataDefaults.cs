using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults;

public class BackupMetadataDefaults
{
	// General defaults
	public const string DEFAULT_NAME = "backup-moodle2-course-2-unde-19700101-0000";
	public const MoodleVersion DEFAULT_MOODLE_VERSION = MoodleVersion.Moodle450;
	public static readonly Uri DEFAULT_MOODLE_URL = new("https://moodle.sanctus.cx");

	// Sensible defaults
	public const bool DEFAULT_FILES_INCLUDED = true;

	// System defaults
	public static readonly DateTime DEFAULT_CREATION_DATE = DateTime.Now;
	public static readonly int? DEFAULT_MNET_REMOTE_USERS = null;
	public const bool DEFAULT_INCLUDE_FILE_REFERENCES_TO_EXTERNAL_CONTENT = false;
	public static readonly string DEFAULT_BACKUP_ID = Guid.NewGuid().ToString().Replace("-", "");
	public const bool DEFAULT_WHOLE_SITE_BACKUP = false;
	public const bool DEFAULT_USES_NEW_BACKUP_FORMAT = true;

	// Backup settings (as defined in the backup creation process)
	public const bool DEFAULT_INCLUDE_ENROLLED_USERS = true;
	public const bool DEFAULT_ANONYMIZE_USER_INFORMATION = false;
	public const bool DEFAULT_INCLUDE_USER_ROLE_ASSIGNMENTS = true;
	public const bool DEFAULT_INCLUDE_ACTIVITIES_AND_RESOURCES = true;
	public const bool DEFAULT_INCLUDE_BLOCKS = true;
	public const bool DEFAULT_INCLUDE_FILES = true;
	public const bool DEFAULT_INCLUDE_FILTERS = true;
	public const bool DEFAULT_INCLUDE_COMMENTS = true;
	public const bool DEFAULT_INCLUDE_BADGES = true;
	public const bool DEFAULT_INCLUDE_CALENDAR_EVENTS = true;
	public const bool DEFAULT_INCLUDE_USER_COMPLETION_DETAILS = true;
	public const bool DEFAULT_INCLUDE_COURSE_LOGS = false;
	public const bool DEFAULT_INCLUDE_GRADE_HISTORY = false;
	public const bool DEFAULT_INCLUDE_QUESTION_BANK = true;
	public const bool DEFAULT_INCLUDE_GROUPS_AND_GROUPINGS = true;
	public const bool DEFAULT_INCLUDE_COMPETENCIES = true;
	public const bool DEFAULT_INCLUDE_CUSTOM_FIELDS = true;
	public const bool DEFAULT_INCLUDE_CONTENT_BANK_CONTENT = true;
	public const bool DEFAULT_INCLUDE_USER_STATE_IN_CONTENT = true;
	public const bool DEFAULT_INCLUDE_LEGACY_COURSE_FILES = true;


	public BackupMetadata GetDefaultBackupMetadata()
	{
		var backupMetadata = new BackupMetadata
		{
			Name = DEFAULT_NAME,
			MoodleVersion = DEFAULT_MOODLE_VERSION,
			MoodleUrl = DEFAULT_MOODLE_URL,
			SensibleDefaults = new BackupMetadata.SensibleDefaultsBackupMetadata
			{
				FilesIncluded = DEFAULT_FILES_INCLUDED
			},
			SystemDefaults = new BackupMetadata.SystemDefaultsBackupMetadata
			{
				CreationDate = DEFAULT_CREATION_DATE,
				MNetRemoteUsers = DEFAULT_MNET_REMOTE_USERS,
				IncludeFileReferencesToExternalContent = DEFAULT_INCLUDE_FILE_REFERENCES_TO_EXTERNAL_CONTENT,
				BackupId = DEFAULT_BACKUP_ID,
				WholeSiteBackup = DEFAULT_WHOLE_SITE_BACKUP,
				UsesNewBackupFormat = DEFAULT_USES_NEW_BACKUP_FORMAT
			},
			BackupSettings = new BackupMetadata.BackupSettingsClass
			{
				IncludeEnrolledUsers = DEFAULT_INCLUDE_ENROLLED_USERS,
				AnonymizeUserInformation = DEFAULT_ANONYMIZE_USER_INFORMATION,
				IncludeUserRoleAssignments = DEFAULT_INCLUDE_USER_ROLE_ASSIGNMENTS,
				IncludeActivitiesAndResources = DEFAULT_INCLUDE_ACTIVITIES_AND_RESOURCES,
				IncludeBlocks = DEFAULT_INCLUDE_BLOCKS,
				IncludeFiles = DEFAULT_INCLUDE_FILES,
				IncludeFilters = DEFAULT_INCLUDE_FILTERS,
				IncludeComments = DEFAULT_INCLUDE_COMMENTS,
				IncludeBadges = DEFAULT_INCLUDE_BADGES,
				IncludeCalendarEvents = DEFAULT_INCLUDE_CALENDAR_EVENTS,
				IncludeUserCompletionDetails = DEFAULT_INCLUDE_USER_COMPLETION_DETAILS,
				IncludeCourseLogs = DEFAULT_INCLUDE_COURSE_LOGS,
				IncludeGradeHistory = DEFAULT_INCLUDE_GRADE_HISTORY,
				IncludeQuestionBank = DEFAULT_INCLUDE_QUESTION_BANK,
				IncludeGroupsAndGroupings = DEFAULT_INCLUDE_GROUPS_AND_GROUPINGS,
				IncludeCompetencies = DEFAULT_INCLUDE_COMPETENCIES,
				IncludeCustomFields = DEFAULT_INCLUDE_CUSTOM_FIELDS,
				IncludeContentBankContent = DEFAULT_INCLUDE_CONTENT_BANK_CONTENT,
				IncludeUserStateInContent = DEFAULT_INCLUDE_USER_STATE_IN_CONTENT,
				IncludeLegacyCourseFiles = DEFAULT_INCLUDE_LEGACY_COURSE_FILES
			}
		};

		return backupMetadata;
	}
}