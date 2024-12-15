using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML;

public class UsersWriter : XmlWriterBase
{
    public void WriteUsers(List<User> users, string filePath = "users.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("users");

            foreach (var user in users)
            {
                // Sicherstellen, dass SystemDefaults und UserPreferences nicht null sind
                User.SystemDefaults ??= new User.SystemDefaults();
                User.UserPreferences ??= new User.UserPreferences();

                WriteUser(writer, user);
            }

            writer.WriteEndElement(); // Close <users>
            writer.WriteEndDocument();
        }
    }

    private void WriteUser(XmlWriter writer, User user)
    {
        WriteStartElementWithAttributes(writer, "user", new Dictionary<string, string>
        {
            { "id", User.SystemDefaults.Id.ToString() },
            { "contextid", User.SystemDefaults.ExternalId.ToString() }
        });

        WriteUserElements(writer, user);
        WritePreferences(writer, user);
        WriteSystem(writer, user);

        writer.WriteEndElement(); // Close <user>
    }

    private void WriteUserElements(XmlWriter writer, User user)
    {
        var elements = new Dictionary<string, string>
        {
            { "username", user.Username },
            { "email", user.EMail.ToString() },
            { "firstname", user.FirstName },
            { "lastname", user.LastName },
            { "city", user.City },
            { "country", user.Country.ToString() },
            { "description", user.Description },
            { "haspicture", user.HasPicture ? "1" : "0" },
            { "picturedescription", user.PictureDescription ?? string.Empty },
            { "authmethod", user.AuthMethod.ToString() },
            { "role", user.Role.ShortName }
        };

        WriteElements(writer, elements);
    }

    private void WritePreferences(XmlWriter writer, User user)
    {
        writer.WriteStartElement("preferences");

        var preferences = new Dictionary<string, string>
        {
            { "preferredlanguage", User.UserPreferences.PreferredLanguage.ToString() },
            { "emaildigesttype", User.UserPreferences.EMailDigestType.ToString() },
            { "autosubscribetoforums", User.UserPreferences.AutoSubscribeToForums ? "1" : "0" },
            { "usenesteddiscussionview", User.UserPreferences.UseNestedDiscussionView ? "1" : "0" },
            { "trackreadposts", User.UserPreferences.TrackReadPosts ? "1" : "0" },
            { "notificationmarkspostasread", User.UserPreferences.NotificationMarksPostAsRead ? "1" : "0" },
            { "editortype", User.UserPreferences.EditorType.ToString() },
            { "use24hourtime", User.UserPreferences.Use24HourTime.HasValue ? (User.UserPreferences.Use24HourTime.Value ? "1" : "0") : string.Empty },
            { "firstdayofweek", User.UserPreferences.FirstDayOfWeek?.ToString() ?? string.Empty },
            { "maxevents", User.UserPreferences.MaxEvents.ToString() },
            { "lookaheadperiod", User.UserPreferences.LookAheadPeriod.ToString() },
            { "persistfiltersettings", User.UserPreferences.PersistFilerSettings ? "1" : "0" },
            { "contentbankvisible", User.UserPreferences.ContentBankVisible ? "1" : "0" },
            { "acceptmessagesfromcontactsonly", User.UserPreferences.AcceptMessagesFromContactsOnly ? "1" : "0" }
        };

        WriteElements(writer, preferences);

        writer.WriteEndElement(); // Close <preferences>
    }

    private void WriteSystem(XmlWriter writer, User user)
    {
        writer.WriteStartElement("system");

        var systemElements = new Dictionary<string, string>
        {
            { "externalid", User.SystemDefaults.ExternalId.ToString() },
            { "lastip", User.SystemDefaults.LastIP.ToString() },
            { "descriptionformat", ((int)User.SystemDefaults.DescriptionFormat).ToString() },
            { "isconfirmed", User.SystemDefaults.IsConfirmed ? "1" : "0" },
            { "acceptedpolicy", User.SystemDefaults.AcceptedPolicy ? "1" : "0" },
            { "isdeleted", User.SystemDefaults.IsDeleted ? "1" : "0" },
            { "timecreated", ((DateTimeOffset)User.SystemDefaults.TimeCreated).ToUnixTimeSeconds().ToString() },
            { "timemodified", ((DateTimeOffset)User.SystemDefaults.TimeModified).ToUnixTimeSeconds().ToString() },
            { "firstaccess", ((DateTimeOffset)User.SystemDefaults.FirstAccess).ToUnixTimeSeconds().ToString() },
            { "lastaccess", ((DateTimeOffset)User.SystemDefaults.LastAccess).ToUnixTimeSeconds().ToString() },
            { "lastlogin", ((DateTimeOffset)User.SystemDefaults.LastLogin).ToUnixTimeSeconds().ToString() },
            { "rolechange", ((DateTimeOffset)User.SystemDefaults.RoleChange).ToUnixTimeSeconds().ToString() },
            { "theme", User.SystemDefaults.Theme ?? string.Empty },
            { "enrolmentsuspended", User.SystemDefaults.EnrolmentSuspended ? "1" : "0" }
        };

        WriteElements(writer, systemElements);

        writer.WriteEndElement(); // Close <system>
    }
}
