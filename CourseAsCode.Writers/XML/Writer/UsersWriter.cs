using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Helper;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;
using UserModel = CourseAsCode.Model.User;

namespace CourseAsCode.Writers.XML.Writer;

public class UsersWriter : XmlWriterBase
{
	private readonly GeneralHelper _helper = GeneralHelper.Instance;
	private readonly UserHelper _userHelper = new UserHelper();
	
	
	public void WriteUsers(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "users.xml"));
		using XmlWriter writer = XmlWriter.Create(filePath, _settings);
		
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
	private void WriteSingleUser(XmlWriter writer, UserModel user)
	{
		WriteStartElementWithAttributes(writer, "user", new Dictionary<string, string>
		{
			{ "id", user.SystemDefaults?.Id.ToString() ?? string.Empty },
			{ "contextid", "15" } // TODO: Dynamically determine this value
		});

		WriteUserElements(writer, user);
		WritePreferences(writer, user.UserPreferences);
		WriteSystem(writer, user.SystemDefaults);

		writer.WriteEndElement(); // Close <user>
	}

	
	/// <summary>
	/// writes the majority of a single user's data in the `users` tag of the users.xml file
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="user">initialized Moodle User</param>
	private void WriteUserElements(XmlWriter writer, UserModel user)
	{
		var elements = new Dictionary<string, string>
		{
			{ "username", user.Username },
			{ "idnumber", user.SystemDefaults.ExternalId.ToString() ?? "" },
			{ "email", user.EMail?.ToString() ?? "" },
			{ "phone1", user.SensibleDefaults.PhoneNumber ?? ""},
			{ "phone2", user.SensibleDefaults.MobilePhoneNumber ?? ""},
			{ "institution", user.SensibleDefaults.Institution ?? ""},
			{ "deparment", user.SensibleDefaults.Department ?? ""},
			{ "address", user.SensibleDefaults.Address ?? ""},
			{ "city", user.City ?? ""},
			{ "country", _userHelper.GetCountryFromEnum(user.Country ?? Country.Austria) },
			{ "lastip", user.SystemDefaults.LastIP?.ToString() ?? "" },
			{ "picture", "0"}, // TODO: Add ability to reference the user's picture here
			{ "description", user.Description ?? "$@NULL@$" },
			{ "descriptionformat", _helper.EnumIntegerToString(user.SystemDefaults.DescriptionFormat ?? FormattingFormat.HTML) },
			{ "imagealt", user.PictureDescription ?? "$@NULL@$"},
			{ "auth", _userHelper.GetAuthMethodFromEnum(user.AuthMethod ?? AuthenticationMethod.ManualCreation) },
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
			{ "currentlogin", _helper.DateTimeToUnixTimestamp(DateTime.Now) ?? "0" }, // TODO: Determine function of this field
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

	
	private void WritePreferences(XmlWriter writer, UserModel.UserPreferencesClass? preferences)
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
			{
				"use24hourtime",
				preferences.Use24HourTime.HasValue ? (preferences.Use24HourTime.Value ? "1" : "0") : string.Empty
			},
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

	
	private void WriteSystem(XmlWriter writer, UserModel.SystemDefaultsUser? systemDefaults)
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
			{
				"timecreated",
				systemDefaults.TimeCreated != default
					? ((DateTimeOffset)systemDefaults.TimeCreated).ToUnixTimeSeconds().ToString()
					: string.Empty
			},
			{
				"timemodified",
				systemDefaults.TimeModified != default
					? ((DateTimeOffset)systemDefaults.TimeModified).ToUnixTimeSeconds().ToString()
					: string.Empty
			},
			{
				"firstaccess",
				systemDefaults.FirstAccess != default
					? ((DateTimeOffset)systemDefaults.FirstAccess).ToUnixTimeSeconds().ToString()
					: string.Empty
			},
			{
				"lastaccess",
				systemDefaults.LastAccess != default
					? ((DateTimeOffset)systemDefaults.LastAccess).ToUnixTimeSeconds().ToString()
					: string.Empty
			},
			{
				"lastlogin",
				systemDefaults.LastLogin != default
					? ((DateTimeOffset)systemDefaults.LastLogin).ToUnixTimeSeconds().ToString()
					: string.Empty
			},
			{
				"rolechange",
				systemDefaults.RoleChange != default
					? ((DateTimeOffset)systemDefaults.RoleChange).ToUnixTimeSeconds().ToString()
					: string.Empty
			},
			{ "theme", systemDefaults.Theme ?? string.Empty },
			{ "enrolmentsuspended", systemDefaults.EnrolmentSuspended ? "1" : "0" }
		};

		WriteElements(writer, systemElements);

		writer.WriteEndElement(); // Close <system>
	}
}