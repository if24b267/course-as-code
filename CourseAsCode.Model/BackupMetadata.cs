using System.ComponentModel;

using CourseAsCode.Model.Defaults;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model;

public class BackupMetadata
{
    [Description("the name of the backup file")]
    public string Name { get; set; } = BackupMetadataDefaults.DEFAULT_NAME;
    [Description("the version of Moodle that the backup was created in")]
    public MoodleVersion MoodleVersion { get; set; } = BackupMetadataDefaults.DEFAULT_MOODLE_VERSION;
    [Description("the URL of the Moodle site that the backup was created from")]
    public Uri MoodleUrl { get; set; } = BackupMetadataDefaults.DEFAULT_MOODLE_URL;

    public SensibleDefaultsBackupMetadata SensibleDefaults { get; set; } = new SensibleDefaultsBackupMetadata();
    public SystemDefaultsBackupMetadata SystemDefaults { get; set; } = new SystemDefaultsBackupMetadata();
    public BackupSettingsClass BackupSettings { get; set; } = new BackupSettingsClass();

    // Defaults
    public class SensibleDefaultsBackupMetadata
    {
        [Description("whether files are included in the backup")]
        public bool FilesIncluded { get; set; } = BackupMetadataDefaults.DEFAULT_FILES_INCLUDED;
    }

    public class SystemDefaultsBackupMetadata
    {
        [Description("the date and time that the backup was created")]
        public DateTime CreationDate { get; set; } = BackupMetadataDefaults.DEFAULT_CREATION_DATE;
        [Description("Moodle Network is used to establish a link with another Moodle site and share resources with the users of that Moodle, DEPRECATED")]
        public int? MNetRemoteUsers { get; set; } = BackupMetadataDefaults.DEFAULT_MNET_REMOTE_USERS;
        [Description("Unknown Purpose")]
        public bool IncludeFileReferencesToExternalContent { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_FILE_REFERENCES_TO_EXTERNAL_CONTENT;
        [Description("unique ID of the backup")]
        public string BackupId { get; set; } = BackupMetadataDefaults.DEFAULT_BACKUP_ID;
        [Description("whether the backup is a whole site backup or only a single course backup")]
        public bool WholeSiteBackup { get; set; } = BackupMetadataDefaults.DEFAULT_WHOLE_SITE_BACKUP;
        [Description("whether the backup uses the 'new' Moodle Backup 2.0")]
        public bool UsesNewBackupFormat { get; set; } = BackupMetadataDefaults.DEFAULT_USES_NEW_BACKUP_FORMAT;
    }

    // Backup settings (as defined in the backup creation process)
    public class BackupSettingsClass
    {
        public bool IncludeEnrolledUsers { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_ENROLLED_USERS;
        public bool AnonymizeUserInformation { get; set; } = BackupMetadataDefaults.DEFAULT_ANONYMIZE_USER_INFORMATION;
        public bool IncludeUserRoleAssignments { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_USER_ROLE_ASSIGNMENTS;
        public bool IncludeActivitiesAndResources { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_ACTIVITIES_AND_RESOURCES;
        public bool IncludeBlocks { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_BLOCKS;
        public bool IncludeFiles { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_FILES;
        public bool IncludeFilters { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_FILTERS;
        public bool IncludeComments { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_COMMENTS;
        public bool IncludeBadges { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_BADGES;
        public bool IncludeCalendarEvents { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_CALENDAR_EVENTS;
        public bool IncludeUserCompletionDetails { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_USER_COMPLETION_DETAILS;
        public bool IncludeCourseLogs { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_COURSE_LOGS;
        public bool IncludeGradeHistory { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_GRADE_HISTORY;
        public bool IncludeQuestionBank { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_QUESTION_BANK;
        public bool IncludeGroupsAndGroupings { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_GROUPS_AND_GROUPINGS;
        public bool IncludeCompetencies { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_COMPETENCIES;
        public bool IncludeCustomFields { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_CUSTOM_FIELDS;
        public bool IncludeContentBankContent { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_CONTENT_BANK_CONTENT;
        public bool IncludeUserStateInContent { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_USER_STATE_IN_CONTENT;
        public bool IncludeLegacyCourseFiles { get; set; } = BackupMetadataDefaults.DEFAULT_INCLUDE_LEGACY_COURSE_FILES;
    }
}
