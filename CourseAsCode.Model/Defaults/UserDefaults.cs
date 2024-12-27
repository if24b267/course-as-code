using System.Net;
using System.Net.Mail;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults.Roles;
using CourseAsCode.Model.Enrolments;
using CourseAsCode.Model.Enums;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;

namespace CourseAsCode.Model.Defaults;

public static class UserDefaults
{
    // General Defaults
    public const string DEFAULT_USERNAME = "CaC_User";
    public static readonly MailAddress DEFAULT_EMAIL = new MailAddress("user@example.com");
    public const string DEFAULT_FIRST_NAME = "CourseAsCode";
    public const string DEFAULT_LAST_NAME = "DemoUser";
    public const string DEFAULT_CITY = "Vienna";
    public const Country DEFAULT_COUNTRY = Country.Austria;
    public const string? DEFAULT_DESCRIPTION = null;
    public const bool DEFAULT_HAS_PICTURE = false;
    public const string? DEFAULT_PICTURE_DESCRIPTION = null;
    public const MoodleFile? DEFAULT_PICTURE = null;
    public const AuthenticationMethod DEFAULT_AUTH_METHOD = AuthenticationMethod.ManualCreation;
    public static readonly EnrolmentMethod DEFAULT_ENROLMENT_METHOD = new ManualEnrolment(); // TODO: Set default enrolment method
    public static readonly Role DEFAULT_ROLE = DefaultStudent.GetDefaultStudent();
    
    // Sensible Defaults
    public const string? DEFAULT_FIRST_NAME_PHONETIC = null;
    public const string? DEFAULT_LAST_NAME_PHONETIC = null;
    public const string? DEFAULT_MIDDLE_NAME = null;
    public const string? DEFAULT_ALTERNATE_NAME = null;
    public const List<string>? DEFAULT_INTERESTS = null;
    public const string? DEFAULT_INSTITUTION = null;
    public const string? DEFAULT_DEPARTMENT = null;
    public const string? DEFAULT_PHONE_NUMBER = null;
    public const string? DEFAULT_MOBILE_PHONE_NUMBER = null;
    public const string? DEFAULT_ADDRESS = null;
    public const TimeZone DEFAULT_TIME_ZONE = TimeZone.ServerDefault;
    public static readonly DateTime DEFAULT_ENROLMENT_START_DATE = DateTime.Now;
    public static readonly DateTime? DEFAULT_ENROLMENT_END_DATE = null;
    
    // System Defaults
    public const int DEFAULT_USER_ID = 0;
    public static readonly int? DEFAULT_EXTERNAL_ID = null;
    public static readonly IPAddress? DEFAULT_LAST_IP = null;
    public static readonly FormattingFormat DEFAULT_DESCRIPTION_FORMAT = FormattingFormat.HTML;
    public const bool DEFAULT_IS_CONFIRMED = false;
    public const bool DEFAULT_ACCEPTED_POLICY = false;
    public const bool DEFAULT_IS_DELETED = false;
    public static readonly DateTime DEFAULT_TIME_CREATED = DateTime.Now;
    public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;
    public static readonly DateTime? DEFAULT_FIRST_ACCESS = null;
    public static readonly DateTime? DEFAULT_LAST_ACCESS = null;
    public static readonly DateTime? DEFAULT_LAST_LOGIN = null;
    public static readonly DateTime DEFAULT_ROLE_CHANGE_DATE = DateTime.Now;
    public const string? DEFAULT_THEME = null;
    public const bool DEFAULT_ENROLMENT_SUSPENDED = false;
    
    // User Preferences
    public const EMailVisibility DEFAULT_EMAIL_VISIBILITY = EMailVisibility.VisibleToEveryone;
    public const Language DEFAULT_PREFERRED_LANGUAGE = Language.English;
    public const EMailDigestType DEFAULT_EMAIL_DIGEST_TYPE = EMailDigestType.NoDigest;
    public const bool DEFAULT_AUTO_SUBSCRIBE_TO_FORUMS = true;
    public const bool DEFAULT_USE_NESTED_DISCUSSION_VIEW = false;
    public const bool DEFAULT_TRACK_READ_POSTS = false;
    public const bool DEFAULT_NOTIFICATION_MARKS_POST_AS_READ = true;
    public const EditorType DEFAULT_EDITOR_TYPE = EditorType.Default;
    public const bool DEFAULT_USE_24_HOUR_TIME = true;
    public const Weekday DEFAULT_FIRST_DAY_OF_WEEK = Weekday.Monday;
    public const int DEFAULT_MAX_EVENTS = 10;
    public const int DEFAULT_LOOK_AHEAD_PERIOD = 7;
    public const bool DEFAULT_PERSIST_FILTER_SETTINGS = false;
    public const bool DEFAULT_CONTENT_BANK_VISIBLE = true;
    public const bool DEFAULT_ACCEPT_MESSAGES_FROM_CONTACTS_ONLY = false;


    /// <summary>
    /// Returns a user object with all properties set to the default values
    /// </summary>
    public static User GetDefaultUser()
    {
        return new User
        {
            Username = DEFAULT_USERNAME,
            EMail = DEFAULT_EMAIL,
            FirstName = DEFAULT_FIRST_NAME,
            LastName = DEFAULT_LAST_NAME,
            City = DEFAULT_CITY,
            Country = DEFAULT_COUNTRY,
            Description = DEFAULT_DESCRIPTION,
            HasPicture = DEFAULT_HAS_PICTURE,
            PictureDescription = DEFAULT_PICTURE_DESCRIPTION,
            Picture = DEFAULT_PICTURE,
            AuthMethod = DEFAULT_AUTH_METHOD,
            EnrolmentMethod = DEFAULT_ENROLMENT_METHOD,
            Role = DEFAULT_ROLE,
            SensibleDefaults = new User.SensibleDefaultsUser()
            {
                FirstNamePhonetic = DEFAULT_FIRST_NAME_PHONETIC,
                LastNamePhonetic = DEFAULT_LAST_NAME_PHONETIC,
                MiddleName = DEFAULT_MIDDLE_NAME,
                AlternateName = DEFAULT_ALTERNATE_NAME,
                Interests = DEFAULT_INTERESTS,
                Institution = DEFAULT_INSTITUTION,
                Department = DEFAULT_DEPARTMENT,
                PhoneNumber = DEFAULT_PHONE_NUMBER,
                MobilePhoneNumber = DEFAULT_MOBILE_PHONE_NUMBER,
                Address = DEFAULT_ADDRESS,
                TimeZone = DEFAULT_TIME_ZONE,
                EnrolmentStartDate = DEFAULT_ENROLMENT_START_DATE,
                EnrolmentEndDate = DEFAULT_ENROLMENT_END_DATE
            },
            SystemDefaults = new User.SystemDefaultsUser()
            {
                Id = DEFAULT_USER_ID,
                ExternalId = DEFAULT_EXTERNAL_ID,
                LastIP = DEFAULT_LAST_IP,
                DescriptionFormat = DEFAULT_DESCRIPTION_FORMAT,
                IsConfirmed = DEFAULT_IS_CONFIRMED,
                AcceptedPolicy = DEFAULT_ACCEPTED_POLICY,
                IsDeleted = DEFAULT_IS_DELETED,
                TimeCreated = DEFAULT_TIME_CREATED,
                TimeModified = DEFAULT_TIME_MODIFIED,
                FirstAccess = DEFAULT_FIRST_ACCESS,
                LastAccess = DEFAULT_LAST_ACCESS,
                LastLogin = DEFAULT_LAST_LOGIN,
                RoleChange = DEFAULT_ROLE_CHANGE_DATE,
                Theme = DEFAULT_THEME,
                EnrolmentSuspended = DEFAULT_ENROLMENT_SUSPENDED
            },
            UserPreferences = new User.UserPreferencesClass()
            {
                EMailVisibility = DEFAULT_EMAIL_VISIBILITY,
                PreferredLanguage = DEFAULT_PREFERRED_LANGUAGE,
                EMailDigestType = DEFAULT_EMAIL_DIGEST_TYPE,
                AutoSubscribeToForums = DEFAULT_AUTO_SUBSCRIBE_TO_FORUMS,
                UseNestedDiscussionView = DEFAULT_USE_NESTED_DISCUSSION_VIEW,
                TrackReadPosts = DEFAULT_TRACK_READ_POSTS,
                NotificationMarksPostAsRead = DEFAULT_NOTIFICATION_MARKS_POST_AS_READ,
                EditorType = DEFAULT_EDITOR_TYPE,
                Use24HourTime = DEFAULT_USE_24_HOUR_TIME,
                FirstDayOfWeek = DEFAULT_FIRST_DAY_OF_WEEK,
                MaxEvents = DEFAULT_MAX_EVENTS,
                LookAheadPeriod = DEFAULT_LOOK_AHEAD_PERIOD,
                PersistFilerSettings = DEFAULT_PERSIST_FILTER_SETTINGS,
                ContentBankVisible = DEFAULT_CONTENT_BANK_VISIBLE,
                AcceptMessagesFromContactsOnly = DEFAULT_ACCEPT_MESSAGES_FROM_CONTACTS_ONLY
            }
        };
    }
}