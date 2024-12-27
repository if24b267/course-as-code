using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Defaults;

namespace CourseAsCode.Model;

public class User
{
    public string Username { get; set; } = UserDefaults.DEFAULT_USERNAME;
    public MailAddress EMail { get; set; } = UserDefaults.DEFAULT_EMAIL;
    public string FirstName { get; set; } = UserDefaults.DEFAULT_FIRST_NAME;
    public string LastName { get; set; } = UserDefaults.DEFAULT_LAST_NAME;
    public string? City { get; set; } = UserDefaults.DEFAULT_CITY;
    public Country? Country { get; set; } = UserDefaults.DEFAULT_COUNTRY;
    [Description("will be displayed on the users' profile page")]
    public string? Description { get; set; } = UserDefaults.DEFAULT_DESCRIPTION;
    [Description("whether the user has uploaded a profile picture")]
    public bool HasPicture { get; set; } = UserDefaults.DEFAULT_HAS_PICTURE;
    [Description("Alt text for the profile picture")]
    public string? PictureDescription { get; set; } = UserDefaults.DEFAULT_PICTURE_DESCRIPTION;
    [Description("the user's profile picture")]
    public MoodleFile? Picture { get; set; } = UserDefaults.DEFAULT_PICTURE;
    [Description("the authentication method used for when the user logs in")]
    public AuthenticationMethod AuthMethod { get; set; } = UserDefaults.DEFAULT_AUTH_METHOD;
    [Description("the enrolment method used for when the user was enrolled in this course")]
    public EnrolmentMethod EnrolmentMethod { get; set; } = UserDefaults.DEFAULT_ENROLMENT_METHOD;
    [Description("the user's role in the system")]
    public Role Role { get; set; } = UserDefaults.DEFAULT_ROLE;

    public SensibleDefaultsUser SensibleDefaults { get; set; } = new SensibleDefaultsUser();
    public SystemDefaultsUser SystemDefaults { get; set; } = new SystemDefaultsUser();
    public UserPreferencesClass UserPreferences { get; set; } = new UserPreferencesClass();
    

    // Defaults
    public class SensibleDefaultsUser
    {
        [Description("phonetic representation of the user's first name, used for pronunciation")]
        public string? FirstNamePhonetic { get; set; } = UserDefaults.DEFAULT_FIRST_NAME_PHONETIC;
        [Description("phonetic representation of the user's last name, used for pronunciation")]
        public string? LastNamePhonetic { get; set; } = UserDefaults.DEFAULT_LAST_NAME_PHONETIC;
        public string? MiddleName { get; set; } = UserDefaults.DEFAULT_MIDDLE_NAME;
        public string? AlternateName { get; set; } = UserDefaults.DEFAULT_ALTERNATE_NAME;
        [Description("list of interests of the user, will be displayed on the users' profile page as tags")]
        public List<string>? Interests { get; set; } = UserDefaults.DEFAULT_INTERESTS;
        public string? Institution { get; set; } = UserDefaults.DEFAULT_INSTITUTION;
        public string? Department { get; set; } = UserDefaults.DEFAULT_DEPARTMENT;
        public string? PhoneNumber { get; set; } = UserDefaults.DEFAULT_PHONE_NUMBER;
        public string? MobilePhoneNumber { get; set; } = UserDefaults.DEFAULT_MOBILE_PHONE_NUMBER;
        public string? Address { get; set; } = UserDefaults.DEFAULT_ADDRESS;
        public Enums.TimeZone? TimeZone { get; set; } = UserDefaults.DEFAULT_TIME_ZONE;
        [Description("the date and time the user was enrolled in the course")]
        public DateTime EnrolmentStartDate { get; set; } = UserDefaults.DEFAULT_ENROLMENT_START_DATE;
        [Description("the date and time the enrolment ends -> the user loses access to the course, blank for unlimited")]
        public DateTime? EnrolmentEndDate { get; set; } = UserDefaults.DEFAULT_ENROLMENT_END_DATE;
    }

    
    public class UserPreferencesClass
    {
        // E-Mail preferences
        [Description("determines who can view the user's e-mail address")]
        public EMailVisibility? EMailVisibility { get; set; } = UserDefaults.DEFAULT_EMAIL_VISIBILITY;
        
        // Language preferences
        [Description("If the language of a course is not forced by the “Force language” setting, courses are displayed to the user in this language")]
        public Language PreferredLanguage { get; set; } = UserDefaults.DEFAULT_PREFERRED_LANGUAGE;
        
        // Forum preferences
        [Description("the format in which the user prefers to receive e-mails")]
        public EMailDigestType? EMailDigestType { get; set; } = UserDefaults.DEFAULT_EMAIL_DIGEST_TYPE;

        [Description("whether the user is automatically subscribed to a forum when they post in it")]
        public bool AutoSubscribeToForums { get; set; } = UserDefaults.DEFAULT_AUTO_SUBSCRIBE_TO_FORUMS;

        [Description("new, experimental forum view")]
        public bool UseNestedDiscussionView { get; set; } = UserDefaults.DEFAULT_USE_NESTED_DISCUSSION_VIEW;
        [Description("if enabled, new posts in forums will be highlighted")]
        public bool TrackReadPosts { get; set; } = UserDefaults.DEFAULT_TRACK_READ_POSTS;
        [Description("if enabled, the forum post will be marked as read when the user receives a notification about it - only relevant if the user has enabled forum tracking")]
        public bool NotificationMarksPostAsRead { get; set; } = UserDefaults.DEFAULT_NOTIFICATION_MARKS_POST_AS_READ;
        
        // Editor preferences
        [Description("the editor that the user prefers to use in text input fields")]
        public EditorType EditorType { get; set; } = UserDefaults.DEFAULT_EDITOR_TYPE;
        
        // Calendar preferences
        [Description("if blank, the format is determined by the user's language")]
        public bool? Use24HourTime { get; set; } = UserDefaults.DEFAULT_USE_24_HOUR_TIME;
        [Description("calendar weeks will be shown starting with the day selected here")]
        public Weekday? FirstDayOfWeek { get; set; } = UserDefaults.DEFAULT_FIRST_DAY_OF_WEEK;
        [Description("maximum number of upcoming events that can be displayed in the calendar block")]
        public int MaxEvents { get; set; } = UserDefaults.DEFAULT_MAX_EVENTS;
        [Description("maximum number of days in the future that an event has to start in order to be considered an upcoming event")]
        public int LookAheadPeriod { get; set; } = UserDefaults.DEFAULT_LOOK_AHEAD_PERIOD;
        [Description("if enabled, Moodle remembers the last event filter settings chosen by the user and restores them on the next login")]
        public bool PersistFilerSettings { get; set; } = UserDefaults.DEFAULT_PERSIST_FILTER_SETTINGS;
        
        // Content bank preferences
        [Description("if enabled, content created in the content bank by this user will be visible by default")]
        public bool ContentBankVisible { get; set; } = UserDefaults.DEFAULT_CONTENT_BANK_VISIBLE;
        
        // Messaging preferences
        [Description("if enabled, the user can only be contacted by other users that are in their contact list")]
        public bool AcceptMessagesFromContactsOnly { get; set; } = UserDefaults.DEFAULT_ACCEPT_MESSAGES_FROM_CONTACTS_ONLY;
    }
	
    
    public class SystemDefaultsUser
    {
        public int Id { get; set; } = UserDefaults.DEFAULT_USER_ID;
        [Description("can be used to map the user to an external system, e.g. LDAP")]
        public int? ExternalId { get; set; } = UserDefaults.DEFAULT_EXTERNAL_ID;
        [Description("the IP address of the user's last login")]
        public IPAddress? LastIP { get; set; } = UserDefaults.DEFAULT_LAST_IP;
        [Description("the format in which the User Description is stored")]
        public FormattingFormat DescriptionFormat { get; set; } = UserDefaults.DEFAULT_DESCRIPTION_FORMAT;
        [Description("if the user has confirmed their account, depends on the authentication method (e.g. confirmation email)")]
        public bool IsConfirmed { get; set; } = UserDefaults.DEFAULT_IS_CONFIRMED;
        [Description("whether the user has agreed to the site policy")]
        public bool AcceptedPolicy { get; set; } = UserDefaults.DEFAULT_ACCEPTED_POLICY;
        public bool IsDeleted { get; set; } = UserDefaults.DEFAULT_IS_DELETED;

        public DateTime TimeCreated { get; set; } = UserDefaults.DEFAULT_TIME_CREATED;
        public DateTime TimeModified { get; set; } = UserDefaults.DEFAULT_TIME_MODIFIED;
        [Description("timestamp of the user's first action in Moodle")]
        public DateTime? FirstAccess { get; set; } = UserDefaults.DEFAULT_FIRST_ACCESS;
        [Description("timestamp of the user's last action in Moodle")]
        public DateTime? LastAccess { get; set; } = UserDefaults.DEFAULT_LAST_ACCESS;
        [Description("timestamp of the user's last successful login attempt")]
        public DateTime? LastLogin { get; set; } = UserDefaults.DEFAULT_LAST_LOGIN;

        [Description("time the user's role was last changed")]
        public DateTime RoleChange { get; set; } = UserDefaults.DEFAULT_ROLE_CHANGE_DATE;
        
        [Description("if allowed by the site policy, the user can choose a custom theme")]
        public string? Theme { get; set; } = UserDefaults.DEFAULT_THEME;
        [Description("if enabled, the enrolment of the user in the course is suspended")]
        public bool EnrolmentSuspended { get; set; } = UserDefaults.DEFAULT_ENROLMENT_SUSPENDED;
    }
}