using System.ComponentModel;
using System.Net.Mail;
using System.Net;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Activities.Resources;

namespace CourseAsCode.Model;

public class User
{
    public string Username { get; set; }
    public MailAddress EMail { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public Country Country { get; set; }
    [Description("will be displayed on the users' profile page")]
    public string Description { get; set; }
    
    [Description("whether the user has uploaded a profile picture")]
    public bool HasPicture { get; set; }
    [Description("Alt text for the profile picture")]
    public string? PictureDescription { get; set; }
    [Description("the user's profile picture")]
    public MoodleFile Picture { get; set; }
    
    [Description("the authentication method used for when the user logs in")]
    public AuthenticationMethod AuthMethod { get; set; }
    [Description("the enrolment method used for when the user was enrolled in this course")]
    public EnrolmentMethod EnrolmentMethod { get; set; }
    
    [Description("the user's role in the system")]
    public Role Role { get; set; }

    // Check if David is ok with this
    public SensibleDefaults SensibleDefaultsInstance { get; set; }
    public SystemDefaults SystemDefaultsInstance { get; set; }
    public UserPreferences UserPreferencesInstance { get; set; }

    /*public Role(string categoryName, string filePath)
    {
        SensibleDefaultsInstance = new SensibleDefaults { CategoryName = categoryName };
        SystemDefaultsInstance = new SystemDefaults { FilePath = filePath };
    }*/
    // Till here

    // Defaults
    public class SensibleDefaults
    {
        public string? FirstNamePhonetic { get; set; }
        public string? LastNamePhonetic { get; set; }
        public string? MiddleName { get; set; }
        public string? AlternateName { get; set; }
        [Description("list of interests of the user, will be displayed on the users' profile page as tags")]
        List<string>? Interests { get; set; }
        public string? Institution { get; set; }
        public string? Department { get; set; }
        public string? PhoneNumber { get; set; }
        public string? MobilePhoneNumber { get; set; }
        public string? Address { get; set; }
        
        public Enums.TimeZone TimeZone { get; set; }
        
        [Description("the date and time the user was enrolled in the course")]
        public DateTime EnrolmentStartDate { get; set; }
        [Description("the date and time the enrolment ends -> the user loses access to the course")]
        public DateTime EnrolmentEndDate { get; set; }
    }

    public class UserPreferences
    {
        // Language preferences
        public Language PreferredLanguage { get; set; }
        
        // Forum preferences
        [Description("the format in which the user prefers to receive e-mails")]
        public EMailDigestType EMailDigestType { get; set; }
        [Description("whether the user is automatically subscribed to a forum when they post in it")]
        public bool AutoSubscribeToForums { get; set; }
        [Description("new, experimental forum view")]
        public bool UseNestedDiscussionView { get; set; }
        [Description("if enabled, new posts in forums will be highlighted")]
        public bool TrackReadPosts { get; set; }
        [Description("if enabled, the forum post will be marked as read when the user receives a notification about it - only relevant if the user has enabled forum tracking")]
        public bool NotificationMarksPostAsRead { get; set; }
        
        // Editor preferences
        [Description("the editor that the user prefers to use in text input fields")]
        public EditorType EditorType { get; set; }
        
        // Calendar preferences
        [Description("if blank, the format is determined by the user's language")]
        public bool? Use24HourTime { get; set; }
        [Description("calendar weeks will be shown starting with the day selected here")]
        public Weekday? FirstDayOfWeek { get; set; }
        [Description("maximum number of upcoming events that can be displayed in the calendar block")]
        public int MaxEvents { get; set; }
        [Description("maximum number of days in the future that an event has to start in order to be considered an upcoming event")]
        public int LookAheadPeriod { get; set; }
        [Description("if enabled, Moodle remembers the last event filter settings chosen by the user and restores them on the next login")]
        public bool PersistFilerSettings { get; set; }
        
        // Content bank preferences
        [Description("if enabled, content created in the content bank by this user will be visible by default")]
        public bool ContentBankVisible { get; set; }
        
        // Messaging preferences
        [Description("if enabled, the user can only be contacted by other users that are in their contact list")]
        public bool AcceptMessagesFromContactsOnly { get; set; }
    }
	
    public class SystemDefaults
    {
        public int Id { get; set; }
        [Description("can be used to map the user to an external system, e.g. LDAP")]
        public int ExternalId { get; set; }
        [Description("the IP address of the user's last login")]
        public IPAddress LastIP { get; set; }
        [Description("the format in which the User Description is stored")]
        public FormattingFormat DescriptionFormat { get; set; }
        [Description("if the user has confirmed their account, depends on the authentication method (e.g. confirmation email)")]
        public bool IsConfirmed { get; set; }
        [Description("whether the user has agreed to the site policy")]
        public bool AcceptedPolicy { get; set; }
        public bool IsDeleted { get; set; }
        
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        [Description("timestamp of the user's first action in Moodle")]
        public DateTime FirstAccess { get; set; }
        [Description("timestamp of the user's last action in Moodle")]
        public DateTime LastAccess { get; set; }
        [Description("timestamp of the user's last successful login attempt")]
        public DateTime LastLogin { get; set; }
        [Description("time the user's role was last changed")]
        public DateTime RoleChange { get; set; }
        
        [Description("if allowed by the site policy, the user can choose a custom theme")]
        public string? Theme { get; set; }
        [Description("if enabled, the enrolment of the in the course is suspended")]
        public bool EnrolmentSuspended { get; set; }
    }
}