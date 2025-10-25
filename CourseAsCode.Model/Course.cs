using System.ComponentModel;

using CourseAsCode.Model.Defaults;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Grading;
using CourseAsCode.Model.Questions;

namespace CourseAsCode.Model
{
    public class Course
    {
        public string FullName { get; set; } = CourseDefaults.DEFAULT_FULL_NAME;
        public string ShortName { get; set; } = CourseDefaults.DEFAULT_SHORT_NAME;
        public string? Summary { get; set; } = CourseDefaults.DEFAULT_SUMMARY;
        public DateTime StartDate { get; set; } = CourseDefaults.DEFAULT_START_DATE;
        public DateTime? EndDate { get; set; } = CourseDefaults.DEFAULT_END_DATE;

        [Description("which group mode to use as default in the course")]
        public GroupMode GroupMode { get; set; } = CourseDefaults.DEFAULT_GROUP_MODE;
        [Description("if set, the course will be displayed in this language despite the user's preferences")]
        public Language? CourseLanguage { get; set; } = CourseDefaults.DEFAULT_COURSE_LANGUAGE;
        [Description("list of tags that are used to describe the course")]
        public List<string>? Tags { get; set; } = CourseDefaults.DEFAULT_TAGS;
        [Description("ordered list of sections in the course")]
        public List<Section> Sections { get; set; } = CourseDefaults.DEFAULT_SECTIONS;
        [Description("list of roles that are available in the course")]
        public List<Role> Roles { get; set; } = CourseDefaults.DEFAULT_ROLES;
        [Description("list of enrolment methods that are activated for this course")]
        public List<EnrolmentMethod> EnrolmentMethods { get; set; } = CourseDefaults.DEFAULT_ENROLMENT_METHODS;
        [Description("list of users that are enrolled in the course")]
        public List<User> EnrolledUsers { get; set; } = CourseDefaults.DEFAULT_ENROLLED_USERS;
        [Description("All the questions available in this course for quizzes, i.e. the question bank")]
        public List<Question> Questions { get; set; } = new();
        [Description("All the grade categories available in this course for grading, e.g. a theoretical and a practical part")]
        public List<GradeCategory> GradeCategories { get; set; } = new();

        public BackupMetadata BackupMetadata { get; set; } = new BackupMetadata();
        public SensibleDefaultsCourse SensibleDefaults { get; set; } = new SensibleDefaultsCourse();
        public SystemDefaultsCourse SystemDefaults { get; set; } = new SystemDefaultsCourse();

        // Defaults
        public class SensibleDefaultsCourse
        {
            [Description("how many recent announcements are displayed in the \"latest announcements\" block, blank to disable announcements")]
            public int? NumberOfAnnouncementsToDisplay { get; set; } = CourseDefaults.DEFAULT_NUMBER_OF_ANNOUNCEMENTS_TO_DISPLAY;
            [Description("format of the course as provided in the course creation process")]
            public CourseFormat Format { get; set; } = CourseDefaults.DEFAULT_FORMAT;
            [Description("whether to allow students to view a list of all their grades in the course")]
            public bool ShowGrades { get; set; } = CourseDefaults.DEFAULT_SHOW_GRADES;
            [Description("used to connect the course to external systems (optional)")]
            public int? CourseIdNumber { get; set; } = CourseDefaults.DEFAULT_COURSE_ID_NUMBER;

            [Description("the maximum upload size for student submissions, blank for unlimited")]
            public uint? MaximumUploadSize { get; set; } = CourseDefaults.DEFAULT_MAXIMUM_UPLOAD_SIZE; // int overflow if MaximumUploadSize > ~4GB
            [Description("whether the course is visible to students or teachers only")]
            public bool VisibleToStudents { get; set; } = CourseDefaults.DEFAULT_VISIBLE_TO_STUDENTS;
            [Description("if true, the group mode can't be changed for individual activities")]
            public bool ForceGroupMode { get; set; } = CourseDefaults.DEFAULT_FORCE_GROUP_MODE;
            [Description("the name of the theme that is used for the course, blank for default")]
            public string? Theme { get; set; } = CourseDefaults.DEFAULT_THEME;
            [Description("whether to show activity dates on the course page")]
            public bool ShowActivityDates { get; set; } = CourseDefaults.DEFAULT_SHOW_ACTIVITY_DATES;
            [Description("if enabled, teacher can set and students will be able to see a list of what they must do to complete an activity")]
            public bool ShowActivityCompletionConditions { get; set; } = CourseDefaults.DEFAULT_SHOW_ACTIVITY_COMPLETION_CONDITIONS;
            [Description("if enabled, students can track their progress through the course")]
            public bool EnableCompletionTracking { get; set; } = CourseDefaults.DEFAULT_ENABLE_COMPLETION_TRACKING;
            [Description("Moodle courses can be organized in categories, blank for default category")]
            public string CategoryName { get; set; } = CourseDefaults.DEFAULT_CATEGORY_NAME;
            [Description("description of the course category, blank for default category")]
            public string? CategoryDescription { get; set; } = CourseDefaults.DEFAULT_CATEGORY_DESCRIPTION;
        }

        public class SystemDefaultsCourse
        {
            public int Id { get; set; } = CourseDefaults.DEFAULT_COURSE_ID;
            [Description("the format in which the CourseSummary is stored")]
            public FormattingFormat SummaryFormat { get; set; } = CourseDefaults.DEFAULT_SUMMARY_FORMAT;
            [Description("whether this backup uses the legacy file system")]
            public bool UsesLegacyFiles { get; set; } = CourseDefaults.DEFAULT_USES_LEGACY_FILES;

            [Description("the time the course was created")]
            public DateTime TimeCreated { get; set; } = CourseDefaults.DEFAULT_TIME_CREATED;

            [Description("the time the course was last modified")]
            public DateTime TimeModified { get; set; } = CourseDefaults.DEFAULT_TIME_MODIFIED;
            [Description("the font used for PDF exports, blank for default - relevant for languages with non-latin scripts")]
            public string? PdfExportFont { get; set; } = CourseDefaults.DEFAULT_PDF_EXPORT_FONT;
            [Description("necessary if the course uses custom fields, blank for no custom fields")]
            public string? CustomFields { get; set; } = CourseDefaults.DEFAULT_CUSTOM_FIELDS;
            [Description("MANDATORY! path to the course folder in the backup directory")]
            public string FilePath { get; set; } = CourseDefaults.DEFAULT_FILE_PATH;
        }
    }
}
