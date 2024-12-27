using CourseAsCode.Model.Enrolments;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults;

public static class CourseDefaults
{
    // General Defaults
    public const string DEFAULT_FULL_NAME = "CourseAsCode";
    public const string DEFAULT_SHORT_NAME = "CaC";
    public const string? DEFAULT_SUMMARY = null;
    public static readonly DateTime DEFAULT_START_DATE = DateTime.Now;
    public static readonly DateTime? DEFAULT_END_DATE = null;
    public const GroupMode DEFAULT_GROUP_MODE = GroupMode.NoGroups;
    public static readonly Language? DEFAULT_COURSE_LANGUAGE = null;
    public static readonly List<string>? DEFAULT_TAGS = null;
    public static readonly List<Section> DEFAULT_SECTIONS = new List<Section>();
    public static readonly List<Role> DEFAULT_ROLES = new List<Role>();
    public static readonly List<EnrolmentMethod> DEFAULT_ENROLMENT_METHODS = new List<EnrolmentMethod>
    {
        new ManualEnrolment(),
        new SelfEnrolment(),
        new GuestEnrolment()
    };
    public static readonly List<User> DEFAULT_ENROLLED_USERS = new List<User>();

    // Sensible Defaults
    public static readonly int? DEFAULT_NUMBER_OF_ANNOUNCEMENTS_TO_DISPLAY = 3;
    public const CourseFormat DEFAULT_FORMAT = CourseFormat.CustomSections;
    public const bool DEFAULT_SHOW_GRADES = true;
    public static readonly int? DEFAULT_COURSE_ID_NUMBER = null;
    public static readonly uint? DEFAULT_MAXIMUM_UPLOAD_SIZE = null;
    public const bool DEFAULT_VISIBLE_TO_STUDENTS = true;
    public const bool DEFAULT_FORCE_GROUP_MODE = false;
    public const string? DEFAULT_THEME = null;
    public const bool DEFAULT_SHOW_ACTIVITY_DATES = true;
    public const bool DEFAULT_SHOW_ACTIVITY_COMPLETION_CONDITIONS = true;
    public const bool DEFAULT_ENABLE_COMPLETION_TRACKING = true;
    public const string DEFAULT_CATEGORY_NAME = "Category 1";
    public const string? DEFAULT_CATEGORY_DESCRIPTION = null;
    
    // System Defaults
    public const int DEFAULT_COURSE_ID = 0;
    public static readonly FormattingFormat DEFAULT_SUMMARY_FORMAT = FormattingFormat.HTML;
    public const bool DEFAULT_USES_LEGACY_FILES = false;
    public static readonly DateTime DEFAULT_TIME_CREATED = DateTime.Now;
    public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;
    public const string? DEFAULT_PDF_EXPORT_FONT = null;
    public const string? DEFAULT_CUSTOM_FIELDS = null;
    public const string DEFAULT_FILE_PATH = "course";


    /// <summary>
    /// Returns a course object with all properties set to the default values
    /// </summary>
    public static Course GetDefaultCourse()
    {
        return new Course()
        {
            FullName = DEFAULT_FULL_NAME,
            ShortName = DEFAULT_SHORT_NAME,
            Summary = DEFAULT_SUMMARY,
            StartDate = DEFAULT_START_DATE,
            EndDate = DEFAULT_END_DATE,
            GroupMode = DEFAULT_GROUP_MODE,
            CourseLanguage = DEFAULT_COURSE_LANGUAGE,
            Tags = DEFAULT_TAGS,
            Sections = DEFAULT_SECTIONS,
            Roles = DEFAULT_ROLES,
            EnrolmentMethods = DEFAULT_ENROLMENT_METHODS,
            EnrolledUsers = DEFAULT_ENROLLED_USERS,
            SensibleDefaults = new Course.SensibleDefaultsCourse()
            {
                CourseIdNumber = DEFAULT_COURSE_ID_NUMBER,
                Format = DEFAULT_FORMAT,
                MaximumUploadSize = DEFAULT_MAXIMUM_UPLOAD_SIZE,
                NumberOfAnnouncementsToDisplay = DEFAULT_NUMBER_OF_ANNOUNCEMENTS_TO_DISPLAY,
                ShowGrades = DEFAULT_SHOW_GRADES,
                VisibleToStudents = DEFAULT_VISIBLE_TO_STUDENTS
            },
            SystemDefaults = new Course.SystemDefaultsCourse()
            {
                Id = DEFAULT_COURSE_ID,
                SummaryFormat = DEFAULT_SUMMARY_FORMAT,
                UsesLegacyFiles = DEFAULT_USES_LEGACY_FILES,
                TimeCreated = DEFAULT_TIME_CREATED,
                TimeModified = DEFAULT_TIME_MODIFIED,
                PdfExportFont = DEFAULT_PDF_EXPORT_FONT,
                CustomFields = DEFAULT_CUSTOM_FIELDS,
                FilePath = DEFAULT_FILE_PATH
            }
        };
    }
} 