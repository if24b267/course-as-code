using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model;

public class BackupMetadata
{
    [Description("the name of the backup file")]
    public required string Name { get; set; }
    [Description("the version of Moodle that the backup was created in")]
    public MoodleVersion MoodleVersion { get; set; }
    [Description("the URL of the Moodle site that the backup was created from")]
    public required Uri MoodleUrl { get; set; }
    
		// Defaults
		public class SensibleDefaults
		{
			[Description("whether files are included in the backup")]
			public bool FilesIncluded { get; set; }
		}
		
		public class SystemDefaults
		{
			[Description("the date and time that the backup was created")]
			public DateTime CreationDate { get; set; }
			[Description("Moodle Network is used to establish a link with another Moodle site and share resources with the users of that Moodle, DEPRECATED")]
			public int MNetRemoteUsers { get; set; }
			[Description("Unknown Purpose")]
			public bool IncludeFileReferencesToExternalContent { get; set; }
			[Description("unique ID of the backup")]
			public required string BackupId { get; set; }
			[Description("whether the backup is a whole site backup or only a single course backup")]
			public bool WholeSiteBackup { get; set; }
			[Description("whether the backup uses the 'new' Moodle Backup 2.0")]
			public bool UsesNewBackupFormat { get; set; }
		}

		// Backup settings (as defined in the backup creation process)
		public class BackupSettings
		{
			public bool IncludeEnrolledUsers { get; set; }
			public bool AnonymizeUserInformation { get; set; }
			public bool IncludeUserRoleAssignments { get; set; }
			public bool IncludeActivitiesAndResources { get; set; }
			public bool IncludeBlocks { get; set; }
			public bool IncludeFiles { get; set; }
			public bool IncludeFilters { get; set; }
			public bool IncludeComments { get; set; }
			public bool IncludeBadges { get; set; }
			public bool IncludeCalendarEvents { get; set; }
			public bool IncludeUserCompletionDetails { get; set; }
			public bool IncludeCourseLogs { get; set; }
			public bool IncludeGradeHistory { get; set; }
			public bool IncludeQuestionBank { get; set; }
			public bool IncludeGroupsAndGroupings { get; set; }
			public bool IncludeCompetencies { get; set; }
			public bool IncludeCustomFields { get; set; }
			public bool IncludeContentBankContent { get; set; }
			public bool IncludeUserStateInContent { get; set; }
			public bool IncludeLegacyCourseFiles { get; set; }
		}
}