using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Helper;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;
using User = CourseAsCode.Model.User;

namespace CourseAsCode.Writers.XML.Writer;

public class UsersWriter : XmlWriterBase
{
	private readonly GeneralHelper _helper = GeneralHelper.Instance;
	private readonly UserHelper _userHelper = new UserHelper();
	
	
	public void WriteUsers(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "users.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, Settings);
		
		writer.WriteStartDocument();
		writer.WriteStartElement("users");
		
		foreach (var user in course.EnrolledUsers)
		{
			WriteSingleUser(writer, user);
		}

		writer.WriteEndElement(); // Close <users>
		writer.WriteEndDocument();
	}
	

	/// <summary>
	/// writes a single user to the users.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="user">initialized Moodle User</param>
	private void WriteSingleUser(XmlWriter writer, User user)
	{
		WriteStartElementWithAttributes(writer, "user", new Dictionary<string, string>
		{
			{ "id", user.SystemDefaults.Id.ToString() },
			{ "contextid", "15" } // TODO: Dynamically determine this value
		});

		WriteUserElements(writer, user);
		WriteCustomFieldsSection(writer, user);
		WriteTagsSection(writer, user);
		WritePreferencesSection(writer, user);
		WriteRolesSection(writer, user);

		writer.WriteEndElement(); // Close <user>
	}

	
	/// <summary>
	/// writes the majority of a single user's data in the `users` tag of the users.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="user">initialized Moodle User</param>
	private void WriteUserElements(XmlWriter writer, User user)
	{
		var elements = new Dictionary<string, string>
		{
			{ "username", user.Username },
			{ "idnumber", user.SystemDefaults.ExternalId.ToString() ?? "" },
			{ "email", user.EMail.ToString() },
			{ "phone1", user.SensibleDefaults.PhoneNumber ?? ""},
			{ "phone2", user.SensibleDefaults.MobilePhoneNumber ?? ""},
			{ "deparment", user.SensibleDefaults.Department ?? ""},
			{ "institution", user.SensibleDefaults.Institution ?? ""},
			{ "address", user.SensibleDefaults.Address ?? ""},
			{ "city", user.City ?? ""},
			{ "country", _userHelper.GetCountryFromEnum(user.Country ?? Country.Austria) },
			{ "lastip", user.SystemDefaults.LastIP?.ToString() ?? "" },
			{ "picture", "0"}, // TODO: Add ability to reference the user's picture here
			{ "description", user.Description ?? "$@NULL@$" },
			{ "descriptionformat", _helper.EnumIntegerToString(user.SystemDefaults.DescriptionFormat) },
			{ "imagealt", user.PictureDescription ?? "$@NULL@$"},
			{ "auth", _userHelper.GetAuthMethodFromEnum(user.AuthMethod) },
			{ "firstnamephonetic", user.SensibleDefaults.FirstNamePhonetic ?? "$@NULL@$"},
			{ "lastnamephonetic", user.SensibleDefaults.LastNamePhonetic ?? "$@NULL@$"},
			{ "middlename" , user.SensibleDefaults.MiddleName ?? "$@NULL@$"},
			{ "alternatename", user.SensibleDefaults.AlternateName ?? "$@NULL"},
			{ "firstname", user.FirstName},
			{ "lastname", user.LastName},
			{ "confirmed", user.SystemDefaults.IsConfirmed ? "1" : "0"},
			{ "policyagreed", user.SystemDefaults.AcceptedPolicy ? "1" : "0"},
			{ "deleted", user.SystemDefaults.IsDeleted ? "1" : "0"},
			{ "lang", _helper.GetLanguageFromEnum(user.UserPreferences.PreferredLanguage)},
			{ "theme", user.SystemDefaults.Theme ?? "" },
			{ "timezone", _helper.GetTimeZoneFromEnum(user.SensibleDefaults.TimeZone ?? TimeZone.ServerDefault) },
			{ "firstaccess" , user.SystemDefaults.FirstAccess != null ? _helper.DateTimeToUnixTimestamp((DateTime)user.SystemDefaults.FirstAccess) : "0" }, // TODO: Determine if default value is really 0
			{ "lastaccess", user.SystemDefaults.LastAccess != null ? _helper.DateTimeToUnixTimestamp((DateTime)user.SystemDefaults.LastAccess) : "0" }, // TODO: Determine if default value is really 0
			{ "lastlogin", user.SystemDefaults.LastLogin != null ? _helper.DateTimeToUnixTimestamp((DateTime)user.SystemDefaults.LastLogin) : "0" }, // TODO: Determine if default value is really 0
			{ "currentlogin", _helper.DateTimeToUnixTimestamp(DateTime.Now) }, // TODO: Determine function of this field
			{ "mailformat", "1" }, // TODO: Determine function of this field
			{ "maildigest", _helper.EnumIntegerToString(user.UserPreferences.EMailDigestType ?? EMailDigestType.NoDigest) },
			{ "maildisplay", _helper.EnumIntegerToString(user.UserPreferences.EMailVisibility ?? EMailVisibility.VisibleToEveryone) }, 
			{ "autosubscribe", user.UserPreferences.AutoSubscribeToForums ? "1" : "0"},
			{ "trackforums", user.UserPreferences.TrackReadPosts ? "1" : "0"},
			{ "timecreated", _helper.DateTimeToUnixTimestamp(user.SystemDefaults.TimeCreated) },
			{ "timemodified", _helper.DateTimeToUnixTimestamp(user.SystemDefaults.TimeModified) },
			{ "trustbitmask", "0" } // TODO: Determine what this value means
		};

		WriteElements(writer, elements);
	}


	/// <summary>
	/// writes the `customfields` section for a specific user in the users.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="user">initialized Moodle User</param>
	private void WriteCustomFieldsSection(XmlWriter writer, User user)
	{
		writer.WriteStartElement("custom_fields");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <custom_fields>
	}
	
	
	/// <summary>
	/// writes the `preferences` section for a specific user in the users.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="user">initialized Moodle User</param>
	private void WriteTagsSection(XmlWriter writer, User user)
	{
		writer.WriteStartElement("tags");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <tags>
	}
	
	
	/// <summary>
	/// writes the `preferences` section for a specific user in the users.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="user">initialized Moodle User</param>
	private void WritePreferencesSection(XmlWriter writer, User user)
	{
		writer.WriteStartElement("preferences");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <preferences>
	}
	
	
	/// <summary>
	/// writes the `roles` section for a specific user in the users.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="user">initialized Moodle User</param>
	private void WriteRolesSection(XmlWriter writer, User user)
	{
		writer.WriteStartElement("roles");
		
		writer.WriteStartElement("role_overrides");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <role_overrides>
		
		writer.WriteStartElement("role_assignments");
		writer.WriteString(" ");
		writer.WriteEndElement(); // Close <role_assignments>
		
		writer.WriteEndElement(); // Close <roles>
	}
}