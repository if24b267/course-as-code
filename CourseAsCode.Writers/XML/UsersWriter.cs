using System.Xml;
using UserModel = CourseAsCode.Model.User;

namespace CourseAsCode.Writers.XML;

public class UsersWriter : XmlWriterBase
{
    public void WriteUsers(List<UserModel> users, string filePath = "users.xml")
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

    private void WriteUser(XmlWriter writer, UserModel user)
    {
        var systemDefaults = user.SystemDefaultsInstance;
        var userPreferences = user.UserPreferencesInstance;

        WriteStartElementWithAttributes(writer, "user", new Dictionary<string, string>
        {
            { "id", systemDefaults?.Id.ToString() ?? string.Empty },
            { "contextid", systemDefaults?.ExternalId.ToString() ?? string.Empty }
        });

        WriteUserElements(writer, user);
        WritePreferences(writer, userPreferences);
        WriteSystem(writer, systemDefaults);

        writer.WriteEndElement(); // Close <user>
    }

    private void WriteUserElements(XmlWriter writer, UserModel user)
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

    private void WritePreferences(XmlWriter writer, UserModel.UserPreferences? preferences)
    {
        if (preferences == null) return;

        writer.WriteStartElement("preferences");

        var preferenceElements = new Dictionary<string, string>
        {
            { "preferredlanguage", preferences.PreferredLanguage.ToString() },
            { "emaildigesttype", preferences.EMailDigestType.ToString() },
            { "autosubscribetoforums", preferences.AutoSubscribeToForums ? "1" : "0" },
            { "usenesteddiscussionview", preferences.UseNestedDiscussionView ? "1" : "0" },
            { "trackreadposts", preferences.TrackReadPosts ? "1" : "0" },
            { "notificationmarkspostasread", preferences.NotificationMarksPostAsRead ? "1" : "0" },
            { "editortype", preferences.EditorType.ToString() },
            { "use24hourtime", preferences.Use24HourTime.HasValue ? (preferences.Use24HourTime.Value ? "1" : "0") : string.Empty },
            { "firstdayofweek", preferences.FirstDayOfWeek?.ToString() ?? string.Empty },
            { "maxevents", preferences.MaxEvents.ToString() },
            { "lookaheadperiod", preferences.LookAheadPeriod.ToString() },
            { "persistfiltersettings", preferences.PersistFilerSettings ? "1" : "0" },
            { "contentbankvisible", preferences.ContentBankVisible ? "1" : "0" },
            { "acceptmessagesfromcontactsonly", preferences.AcceptMessagesFromContactsOnly ? "1" : "0" }
        };

        WriteElements(writer, preferenceElements);

        writer.WriteEndElement(); // Close <preferences>
    }

    private void WriteSystem(XmlWriter writer, UserModel.SystemDefaults? systemDefaults)
    {
        if (systemDefaults == null) return;

        writer.WriteStartElement("system");

        var systemElements = new Dictionary<string, string>
        {
            { "externalid", systemDefaults.ExternalId.ToString() },
            { "lastip", systemDefaults.LastIP?.ToString() ?? string.Empty },
            { "descriptionformat", ((int)systemDefaults.DescriptionFormat).ToString() },
            { "isconfirmed", systemDefaults.IsConfirmed ? "1" : "0" },
            { "acceptedpolicy", systemDefaults.AcceptedPolicy ? "1" : "0" },
            { "isdeleted", systemDefaults.IsDeleted ? "1" : "0" },
            { "timecreated", systemDefaults.TimeCreated != default ? ((DateTimeOffset)systemDefaults.TimeCreated).ToUnixTimeSeconds().ToString() : string.Empty },
            { "timemodified", systemDefaults.TimeModified != default ? ((DateTimeOffset)systemDefaults.TimeModified).ToUnixTimeSeconds().ToString() : string.Empty },
            { "firstaccess", systemDefaults.FirstAccess != default ? ((DateTimeOffset)systemDefaults.FirstAccess).ToUnixTimeSeconds().ToString() : string.Empty },
            { "lastaccess", systemDefaults.LastAccess != default ? ((DateTimeOffset)systemDefaults.LastAccess).ToUnixTimeSeconds().ToString() : string.Empty },
            { "lastlogin", systemDefaults.LastLogin != default ? ((DateTimeOffset)systemDefaults.LastLogin).ToUnixTimeSeconds().ToString() : string.Empty },
            { "rolechange", systemDefaults.RoleChange != default ? ((DateTimeOffset)systemDefaults.RoleChange).ToUnixTimeSeconds().ToString() : string.Empty },
            { "theme", systemDefaults.Theme ?? string.Empty },
            { "enrolmentsuspended", systemDefaults.EnrolmentSuspended ? "1" : "0" }
        };

        WriteElements(writer, systemElements);

        writer.WriteEndElement(); // Close <system>
    }
}
