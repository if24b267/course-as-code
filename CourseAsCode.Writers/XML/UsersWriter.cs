using System.Xml;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.XML;

public class UsersWriter
{
    private readonly XmlWriterSettings _settings = new()
    {
        Indent = true,
        NewLineOnAttributes = false,
        OmitXmlDeclaration = true
    };

    public void WriteUsers(List<User> users, string filePath = "users.xml")
    {
        using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
        {
            writer.WriteStartDocument();

            // root element <users>
            writer.WriteStartElement("users");

            foreach (var user in users)
            {
                // child elements of <users>
                writer.WriteStartElement("user");
                writer.WriteAttributeString("id", user.SystemDefaults.Id.ToString());
                writer.WriteAttributeString("contextid", user.SystemDefaults.ExternalId.ToString());

                writer.WriteElementString("username", user.Username);
                writer.WriteElementString("email", user.EMail.ToString());
                writer.WriteElementString("firstname", user.FirstName);
                writer.WriteElementString("lastname", user.LastName);
                writer.WriteElementString("city", user.City);
                writer.WriteElementString("country", user.Country.ToString());
                writer.WriteElementString("description", user.Description);
                writer.WriteElementString("haspicture", user.HasPicture ? "1" : "0");
                writer.WriteElementString("picturedescription", user.PictureDescription ?? string.Empty);
                writer.WriteElementString("authmethod", user.AuthMethod.ToString());
                writer.WriteElementString("role", user.Role.ShortName);

                writer.WriteStartElement("preferences");
                writer.WriteElementString("preferredlanguage", user.UserPreferences.PreferredLanguage.ToString());
                writer.WriteElementString("emaildigesttype", user.UserPreferences.EMailDigestType.ToString());
                writer.WriteElementString("autosubscribetoforums", user.UserPreferences.AutoSubscribeToForums ? "1" : "0");
                writer.WriteElementString("usenesteddiscussionview", user.UserPreferences.UseNestedDiscussionView ? "1" : "0");
                writer.WriteElementString("trackreadposts", user.UserPreferences.TrackReadPosts ? "1" : "0");
                writer.WriteElementString("notificationmarkspostasread", user.UserPreferences.NotificationMarksPostAsRead ? "1" : "0");
                writer.WriteElementString("editortype", user.UserPreferences.EditorType.ToString());
                writer.WriteElementString("use24hourtime", user.UserPreferences.Use24HourTime.HasValue ? (user.UserPreferences.Use24HourTime.Value ? "1" : "0") : string.Empty);
                writer.WriteElementString("firstdayofweek", user.UserPreferences.FirstDayOfWeek?.ToString() ?? string.Empty);
                writer.WriteElementString("maxevents", user.UserPreferences.MaxEvents.ToString());
                writer.WriteElementString("lookaheadperiod", user.UserPreferences.LookAheadPeriod.ToString());
                writer.WriteElementString("persistfiltersettings", user.UserPreferences.PersistFilerSettings ? "1" : "0");
                writer.WriteElementString("contentbankvisible", user.UserPreferences.ContentBankVisible ? "1" : "0");
                writer.WriteElementString("acceptmessagesfromcontactsonly", user.UserPreferences.AcceptMessagesFromContactsOnly ? "1" : "0");
                writer.WriteEndElement(); // Close <preferences>

                writer.WriteStartElement("system");
                writer.WriteElementString("externalid", user.SystemDefaults.ExternalId.ToString());
                writer.WriteElementString("lastip", user.SystemDefaults.LastIP.ToString());
                writer.WriteElementString("descriptionformat", ((int)user.SystemDefaults.DescriptionFormat).ToString());
                writer.WriteElementString("isconfirmed", user.SystemDefaults.IsConfirmed ? "1" : "0");
                writer.WriteElementString("acceptedpolicy", user.SystemDefaults.AcceptedPolicy ? "1" : "0");
                writer.WriteElementString("isdeleted", user.SystemDefaults.IsDeleted ? "1" : "0");
                writer.WriteElementString("timecreated", ((DateTimeOffset)user.SystemDefaults.TimeCreated).ToUnixTimeSeconds().ToString());
                writer.WriteElementString("timemodified", ((DateTimeOffset)user.SystemDefaults.TimeModified).ToUnixTimeSeconds().ToString());
                writer.WriteElementString("firstaccess", ((DateTimeOffset)user.SystemDefaults.FirstAccess).ToUnixTimeSeconds().ToString());
                writer.WriteElementString("lastaccess", ((DateTimeOffset)user.SystemDefaults.LastAccess).ToUnixTimeSeconds().ToString());
                writer.WriteElementString("lastlogin", ((DateTimeOffset)user.SystemDefaults.LastLogin).ToUnixTimeSeconds().ToString());
                writer.WriteElementString("rolechange", ((DateTimeOffset)user.SystemDefaults.RoleChange).ToUnixTimeSeconds().ToString());
                writer.WriteElementString("theme", user.SystemDefaults.Theme ?? string.Empty);
                writer.WriteElementString("enrolmentsuspended", user.SystemDefaults.EnrolmentSuspended ? "1" : "0");
                writer.WriteEndElement(); // Close <system>

                writer.WriteEndElement(); // Close <user>
            }

            writer.WriteEndElement(); // Close <users>
            writer.WriteEndDocument();
        }
    }
}

