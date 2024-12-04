using System.ComponentModel;

namespace CourseAsCode.Model
{
	public enum CourseFormat
	{
		CustomSections, // course is divided into customisable sections
		WeeklySections, // course is divided into sections corresponding to each week
		SingleActivity, // course contains only one activity or resource
		Social // course is centred around a main forum on the course page
	}

	public enum GroupMode
	{
		NoGroups, // no groups are used in the course
		SeparateGroups, // students are divided into separate groups and can only see their own groups' work
		VisibleGroups // students are divided into separate groups but can see other groups' work
	}
	
	public enum Language
	{
		English
	}
	
	public class Course
	{
		public required string FullName { get; set; }
		public required string ShortName { get; set; }
		public string? Summary { get; set; }
		[Description("how many recent announcements are displayed in the \"latest announcements\" block, blank to disable announcements")]
		public int? NumberOfAnnouncementsToDisplay { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		[Description("which group mode to use as default in the course")]
		public GroupMode GroupMode { get; set; }

		public List<Section> Sections { get; set; } = new List<Section>();
		
		
		// Defaults
		public class SensibleDefaults
		{
			[Description("format of the course as provided in the course creation process")]
			public CourseFormat Format { get; set; }
			[Description("whether to allow students to view a list of all their grades in the course")]
			public bool ShowGrades { get; set; }
			[Description("used to connect the course to external systems (optional)")]
			public int CourseIdNumber { get; set; }
			[Description("the maximum upload size for student submissions")]
			public int MaximumUploadSize { get; set; }
			[Description("whether the course is visible to students or teachers only")]
			public bool VisibleToStudents { get; set; }
			[Description("if true, the group mode can't be changed for individual activities")]
			public bool ForceGroupMode { get; set; }
			[Description("the name of the theme that is used for the course, blank for default")]
			public string? Theme { get; set; }
			[Description("whether to show activity dates on the course page")]
			public bool ShowActivityDates { get; set; }
			[Description("if enabled, teacher can set and students will be able to see a list of what they must do to complete an activity")]
			public bool ShowActivityCompletionConditions { get; set; }
			[Description("if enabled, students can track their progress through the course")]
			public bool EnableCompletionTracking { get; set; }
			[Description("list of tags that are used to describe the course")]
			public List<string> Tags { get; set; }
			[Description("course categories organize courses for all Moodle site participants, blank for default category")]
			public int? CourseCategory { get; set; }
		}
		
		public class SystemDefaults
		{
			public int Id { get; set; }
			public int ContextId { get; set; }
			[Description("the format in which the CourseSummary is stored")]
			public int SummaryFormat { get; set; }
			[Description("whether this backup uses the legacy file system")]
			public bool UsesLegacyFiles { get; set; }
			[Description("if set, the course will be displayed in this language despite the user's preferences")]
			public Language? CourseLanguage { get; set; }
			[Description("the time the course was created")]
			public DateTime TimeCreated { get; set; }
			[Description("the time the course was last modified")]
			public DateTime TimeModified { get; set; }
			[Description("the font used for PDF exports, blank for default")]
			public string? PdfExportFont { get; set; }
			[Description("necessary if the course uses custom fields, blank for no custom fields")]
			public string CustomFields { get; set; }
			[Description("path to the course folder in the backup directory")]
			public string FilePath { get; set; }
		}
	}
}
