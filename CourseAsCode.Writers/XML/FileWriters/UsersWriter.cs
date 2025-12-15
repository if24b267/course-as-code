using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

using TimeZone = CourseAsCode.Model.Enums.TimeZone;
using User = CourseAsCode.Model.User;

namespace CourseAsCode.Writers.XML.Writer
{
    public class UsersWriter : XmlWriterBase
    {
        private const string DEFAULT_USERS_RELATIVE_PATH = "users.xml";

        private readonly IGeneralHelper _helper;
        private readonly IUserHelper _userHelper;

        public UsersWriter(IXmlWriterFactory xmlWriterFactory, ILogger<UsersWriter> logger, IGeneralHelper helper, IUserHelper userHelper)
            : base(xmlWriterFactory, logger)
        {
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
        }

        public void WriteUsers(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_USERS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("users");

                    foreach (var user in course.EnrolledUsers)
                    {
                        WriteSingleUser(writer, user);
                    }

                    writer.WriteEndElement(); // </users>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_USERS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_USERS_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteSingleUser(XmlWriter writer, User user)
        {
            WriteStartElementWithAttributes(writer, "user", new Dictionary<string, string>
            {
                { "id", user.SystemDefaults.Id.ToString() },
                { "contextid", user.SystemDefaults.ExternalId?.ToString() ?? "0" } // Passe ggf. an
            });

            WriteUserElements(writer, user);
            WriteCustomFieldsSection(writer, user);
            WriteTagsSection(writer, user);
            WritePreferencesSection(writer, user);
            WriteRolesSection(writer, user);

            writer.WriteEndElement(); // </user>
            LogTrace($"Added user {user.SystemDefaults.Id}: '{user.Username}' to {DEFAULT_USERS_RELATIVE_PATH}");
        }

        private void WriteUserElements(XmlWriter writer, User user)
        {
            var elements = new Dictionary<string, string>
            {
                { "username", user.Username },
                { "idnumber", user.SystemDefaults.ExternalId?.ToString() ?? "" },
                { "email", user.EMail.ToString() },
                { "phone1", user.SensibleDefaults.PhoneNumber ?? "" },
                { "phone2", user.SensibleDefaults.MobilePhoneNumber ?? "" },
                { "institution", user.SensibleDefaults.Institution ?? "" },
                { "department", user.SensibleDefaults.Department ?? "" },
                { "address", user.SensibleDefaults.Address ?? "" },
                { "city", user.City ?? "" },
                { "country", _userHelper.GetCountryFromEnum(user.Country ?? Country.Austria) },
                { "lastip", user.SystemDefaults.LastIP?.ToString() ?? "" },
                { "picture", user.HasPicture ? "1" : "0" },
                { "description", user.Description ?? "$@NULL@$" },
                { "descriptionformat", _helper.EnumIntegerToString(user.SystemDefaults.DescriptionFormat) },
                { "imagealt", user.PictureDescription ?? "$@NULL@$" },
                { "auth", _userHelper.GetAuthMethodFromEnum(user.AuthMethod) },
                { "firstnamephonetic", user.SensibleDefaults.FirstNamePhonetic ?? "$@NULL@$" },
                { "lastnamephonetic", user.SensibleDefaults.LastNamePhonetic ?? "$@NULL@$" },
                { "middlename", user.SensibleDefaults.MiddleName ?? "$@NULL@$" },
                { "alternatename", user.SensibleDefaults.AlternateName ?? "$@NULL@$" },
                { "firstname", user.FirstName },
                { "lastname", user.LastName },
                { "confirmed", user.SystemDefaults.IsConfirmed ? "1" : "0" },
                { "policyagreed", user.SystemDefaults.AcceptedPolicy ? "1" : "0" },
                { "deleted", user.SystemDefaults.IsDeleted ? "1" : "0" },
                { "lang", _helper.GetLanguageFromEnum(user.UserPreferences.PreferredLanguage) },
                { "theme", user.SystemDefaults.Theme ?? "" },
                { "timezone", _helper.GetTimeZoneFromEnum(user.SensibleDefaults.TimeZone ?? TimeZone.ServerDefault) },
                { "firstaccess", user.SystemDefaults.FirstAccess != null ? _helper.DateTimeToUnixTimestamp((DateTime)user.SystemDefaults.FirstAccess) : "0" },
                { "lastaccess", user.SystemDefaults.LastAccess != null ? _helper.DateTimeToUnixTimestamp((DateTime)user.SystemDefaults.LastAccess) : "0" },
                { "lastlogin", user.SystemDefaults.LastLogin != null ? _helper.DateTimeToUnixTimestamp((DateTime)user.SystemDefaults.LastLogin) : "0" },
                { "currentlogin", _helper.DateTimeToUnixTimestamp(DateTime.Now) },
                { "mailformat", "1" },
                { "maildigest", _helper.EnumIntegerToString(user.UserPreferences.EMailDigestType ?? EMailDigestType.NoDigest) },
                { "maildisplay", _helper.EnumIntegerToString(user.UserPreferences.EMailVisibility ?? EMailVisibility.VisibleToEveryone) },
                { "autosubscribe", user.UserPreferences.AutoSubscribeToForums ? "1" : "0" },
                { "trackforums", user.UserPreferences.TrackReadPosts ? "1" : "0" },
                { "timecreated", _helper.DateTimeToUnixTimestamp(user.SystemDefaults.TimeCreated) },
                { "timemodified", _helper.DateTimeToUnixTimestamp(user.SystemDefaults.TimeModified) },
                { "trustbitmask", "0" }
            };

            WriteElements(writer, elements);
        }

        private void WriteCustomFieldsSection(XmlWriter writer, User user)
        {
            writer.WriteStartElement("custom_fields");
            if (user.CustomFields != null)
            {
                foreach (var field in user.CustomFields)
                {
                    writer.WriteStartElement("custom_field");
                    writer.WriteAttributeString("id", field.Id.ToString());
                    writer.WriteElementString("field_name", field.FieldName ?? string.Empty);
                    writer.WriteElementString("field_type", field.FieldType ?? string.Empty);
                    writer.WriteElementString("field_data", field.FieldData ?? string.Empty);
                    writer.WriteEndElement(); // </custom_field>
                }
            }
            writer.WriteEndElement(); // </custom_fields>

        }

        private void WriteTagsSection(XmlWriter writer, User user)
        {
            writer.WriteStartElement("tags");
            if (user.Tags != null)
            {
                foreach (var tag in user.Tags)
                {
                    writer.WriteStartElement("tag");
                    writer.WriteAttributeString("id", tag.Id.ToString());
                    writer.WriteElementString("name", tag.Name);
                    writer.WriteElementString("rawname", tag.RawName);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

        private void WritePreferencesSection(XmlWriter writer, User user)
        {
            writer.WriteStartElement("preferences");
            if (user.Preferences != null)
            {
                foreach (var pref in user.Preferences)
                {
                    writer.WriteStartElement("preference");
                    writer.WriteAttributeString("id", pref.Id.ToString());
                    writer.WriteElementString("name", pref.Name);
                    writer.WriteElementString("value", pref.Value ?? string.Empty);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

        private void WriteRolesSection(XmlWriter writer, User user)
        {
            writer.WriteStartElement("roles");
            writer.WriteStartElement("role_overrides");
            writer.WriteEndElement();
            writer.WriteStartElement("role_assignments");
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
