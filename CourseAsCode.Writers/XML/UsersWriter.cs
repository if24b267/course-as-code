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
            { "id", user.SystemDefaults.Id.ToString() },
            { "contextid", user.SystemDefaults.ExternalId.ToString() }
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
            { "preferredlanguage", user.UserPreferences.PreferredLanguage.ToString() },
            { "emaildigesttype", user.UserPreferences.EMailDigestType.ToString() },
            { "autosubscribetoforums", user.UserPreferences.AutoSubscribeToForums ? "1" : "0" },
            { "usenesteddiscussionview", user.UserPreferences.UseNestedDiscussionView ? "1" : "0" },
            { "trackreadposts", user.UserPreferences.TrackReadPosts ? "1" : "0" },
            { "notificationmarkspostasread", user.UserPreferences.NotificationMarksPostAsRead ? "1" : "0" },
            { "editortype", user.UserPreferences.EditorType.ToString() },
            { "use24hourtime", user.UserPreferences.Use24HourTime.HasValue ? (user.UserPreferences.Use24HourTime.Value ? "1" : "0") : string.Empty },
            { "firstdayofweek", user.UserPreferences.FirstDayOfWeek?.ToString() ?? string.Empty },
            { "maxevents", user.UserPreferences.MaxEvents.ToString() },
            { "lookaheadperiod", user.UserPreferences.LookAheadPeriod.ToString() },
            { "persistfiltersettings", user.UserPreferences.PersistFilerSettings ? "1" : "0" },
            { "contentbankvisible", user.UserPreferences.ContentBankVisible ? "1" : "0" },
            { "acceptmessagesfromcontactsonly", user.UserPreferences.AcceptMessagesFromContactsOnly ? "1" : "0" }
        };

        WriteElements(writer, preferences);

        writer.WriteEndElement(); // Close <preferences>
    }

    private void WriteSystem(XmlWriter writer, User user)
    {
        writer.WriteStartElement("system");

        var systemElements = new Dictionary<string, string>
        {
            { "externalid", user.SystemDefaults.ExternalId.ToString() },
            { "lastip", user.SystemDefaults.LastIP.ToString() },
            { "descriptionformat", ((int)user.SystemDefaults.DescriptionFormat).ToString() },
            { "isconfirmed", user.SystemDefaults.IsConfirmed ? "1" : "0" },
            { "acceptedpolicy", user.SystemDefaults.AcceptedPolicy ? "1" : "0" },
            { "isdeleted", user.SystemDefaults.IsDeleted ? "1" : "0" },
            { "timecreated", ((DateTimeOffset)user.SystemDefaults.TimeCreated).ToUnixTimeSeconds().ToString() },
            { "timemodified", ((DateTimeOffset)user.SystemDefaults.TimeModified).ToUnixTimeSeconds().ToString() },
            { "firstaccess", ((DateTimeOffset)user.SystemDefaults.FirstAccess).ToUnixTimeSeconds().ToString() },
            { "lastaccess", ((DateTimeOffset)user.SystemDefaults.LastAccess).ToUnixTimeSeconds().ToString() },
            { "lastlogin", ((DateTimeOffset)user.SystemDefaults.LastLogin).ToUnixTimeSeconds().ToString() },
            { "rolechange", ((DateTimeOffset)user.SystemDefaults.RoleChange).ToUnixTimeSeconds().ToString() },
            { "theme", user.SystemDefaults.Theme ?? string.Empty },
            { "enrolmentsuspended", user.SystemDefaults.EnrolmentSuspended ? "1" : "0" }
        };

        WriteElements(writer, systemElements);

        writer.WriteEndElement(); // Close <system>
    }
}
