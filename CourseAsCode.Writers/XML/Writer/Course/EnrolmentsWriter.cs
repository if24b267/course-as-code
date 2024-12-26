using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Enrolments;
using CourseAsCode.Writers.Helper;
using CourseAsCode.Writers.Static;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class EnrolmentsWriter : XmlWriterBase
{
	private int _enrolmentIdCounter = 1;
	
	private new readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = false
	};
	
	private readonly GeneralHelper _helper = GeneralHelper.Instance;

	
	public void WriteEnrolment(Model.Course course, string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/enrolments.xml"));

		using XmlWriter writer = XmlWriter.Create(filePath, _settings);
		writer.WriteStartDocument();
		
		writer.WriteStartElement("enrolments");
		writer.WriteStartElement("enrols");
			
		WriteManualEnrolSection(writer, course);
		WriteGuestEnrolSection(writer, course);
		WriteSelfEnrolSection(writer, course);
			
		writer.WriteEndElement(); // Close <enrols>
		writer.WriteEndElement(); // Close <enrolments>
			
		writer.WriteEndDocument();
	}
	
	
	/// <summary>
	/// writes the `enrol` section for the manual enrolment method; to be used within the `enrols` section
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteManualEnrolSection(XmlWriter writer, Model.Course course)
	{
		ManualEnrolment manualEnrolment = (course.EnrolmentMethods.FirstOrDefault(e => e is ManualEnrolment) as ManualEnrolment)!;
		
		WriteStartElementWithAttributes(writer, "enrol", new Dictionary<string, string>
		{
			{ "id", "1" }
		});
		
		var elements = new Dictionary<string, string>
		{
			{ "enrol", "manual" },
			{ "status", manualEnrolment is { Enabled: true } ? "0" : "1" }, // 0 -> active; 1 -> inactive | Whyyy? (:
			{ "name", "$@NULL@$" },
			{ "enrol_period", manualEnrolment.EnrolmentDuration?.ToString() ?? "0" },
			{ "enrolstartdate", "0" }, // Always 0, setting doesn't apply to this enrolment method
			{ "enrolenddate", "0" }, // Always 0, setting doesn't apply to this enrolment method
			{ "expiry_notify", manualEnrolment.NotifyEnroller ? manualEnrolment.NotifyStudent ? "2" : "1" : "0" },
			{ "expiry_threshhold", manualEnrolment.SensibleDefaultsManualEnrolment.NotifyThreshold?.ToString() ?? "0" },
			{ "notifyall", manualEnrolment.NotifyStudent ? "1" : "0" }, 
			{ "password", "$@NULL@$" }, // Always null, setting doesn't apply to this enrolment method
			{ "cost", "$@NULL@$" }, // Always null, setting doesn't apply to this enrolment method
			{ "currency", "$@NULL@$" }, // Always null, setting doesn't apply to this enrolment method
			{ "roleid", manualEnrolment.DefaultRole.SystemDefaults.Id.ToString() },
			// TODO: Find out what all these <customXXX> values mean
			{ "customint1", "1" }, 
			{ "customint2", "$@NULL@$" }, // Not applicable to this enrolment method
			{ "customint3", "$@NULL@$" }, // Not applicable to this enrolment method
			{ "customint4", "$@NULL@$" },
			{ "customint5", "$@NULL@$" },
			{ "customint6", "$@NULL@$" },
			{ "customint7", "$@NULL@$" },
			{ "customint8", "$@NULL@$" },
			{ "customchar1", "$@NULL@$" },
			{ "customchar2", "$@NULL@$" },
			{ "customchar3", "$@NULL@$" },
			{ "customdec1", "$@NULL@$" },
			{ "customdec2", "$@NULL@$" },
			{ "customtext1", manualEnrolment.WelcomeMessage ?? "$@NULL@$" },
			{ "customtext2", "$@NULL@$" },
			{ "customtext3", "$@NULL@$" },
			{ "customtext4", "$@NULL@$" },
			{ "timecreated", _helper.DateTimeToUnixTimestamp(manualEnrolment.SystemDefaults.TimeCreated) },
			{ "timemodified", _helper.DateTimeToUnixTimestamp(manualEnrolment.SystemDefaults.TimeModified) }
		};
		
		WriteElements(writer, elements);
		
		// All users enrolled with this method
		writer.WriteStartElement("user_enrolments");

		foreach (User user in course.EnrolledUsers)
		{
			if (user.EnrolmentMethod is ManualEnrolment)
			{
				WriterUserEnrolmentSection(writer, course, user);
			}
		}
		
		writer.WriteEndElement(); // Close <user_enrolments>
		writer.WriteEndElement(); // Close <enrol>
	}

	
	/// <summary>
	/// writes the `enrol` section for the guest enrolment method; to be used within the `enrols` section
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteGuestEnrolSection(XmlWriter writer, Model.Course course)
	{
		GuestEnrolment guestEnrolment = (course.EnrolmentMethods.FirstOrDefault(e => e is GuestEnrolment) as GuestEnrolment)!;
		
		WriteStartElementWithAttributes(writer, "enrol", new Dictionary<string, string>
		{
			{ "id", "2" }
		});
		
		var elements = new Dictionary<string, string>
		{
			{ "enrol", "guest" },
			{ "status", guestEnrolment is { Enabled: true } ? "0" : "1" },
			{ "name", "$@NULL@$" }, // Always 0, setting doesn't apply to this enrolment method
			{ "enrol_period", "0" }, // Always 0, setting doesn't apply to this enrolment method
			{ "enrolstartdate", "0" }, // Always 0, setting doesn't apply to this enrolment method
			{ "enrolenddate", "0" }, // Always 0, setting doesn't apply to this enrolment method
			{ "expiry_notify", "0" }, // Always 0, setting doesn't apply to this enrolment method
			{ "expiry_threshhold", "0" }, // Always 0, setting doesn't apply to this enrolment method
			{ "notifyall", "0" }, // Always 0, setting doesn't apply to this enrolment method
			{ "password", guestEnrolment?.Password ?? " " }, 
			{ "cost", "$@NULL@$" }, // Always null, setting doesn't apply to this enrolment method
			{ "currency", "$@NULL@$" }, // Always null, setting doesn't apply to this enrolment method
			{ "roleid", guestEnrolment.DefaultRole.SystemDefaults.Id.ToString() },
			{ "customint1", "$@NULL@$" }, 
			{ "customint2", "$@NULL@$" }, // Not applicable to this enrolment method
			{ "customint3", "$@NULL@$" }, // Not applicable to this enrolment method
			{ "customint4", "$@NULL@$" },
			{ "customint5", "$@NULL@$" },
			{ "customint6", "$@NULL@$" },
			{ "customint7", "$@NULL@$" },
			{ "customint8", "$@NULL@$" },
			{ "customchar1", "$@NULL@$" },
			{ "customchar2", "$@NULL@$" },
			{ "customchar3", "$@NULL@$" },
			{ "customdec1", "$@NULL@$" },
			{ "customdec2", "$@NULL@$" },
			{ "customtext1", "$@NULL@$" },
			{ "customtext2", "$@NULL@$" },
			{ "customtext3", "$@NULL@$" },
			{ "customtext4", "$@NULL@$" },
			{ "timecreated", _helper.DateTimeToUnixTimestamp(guestEnrolment.SystemDefaults.TimeCreated) },
			{ "timemodified", _helper.DateTimeToUnixTimestamp(guestEnrolment.SystemDefaults.TimeModified) }
		};
		
		WriteElements(writer, elements);
		
		// All users enrolled with this method
		writer.WriteStartElement("user_enrolments");

		foreach (User user in course.EnrolledUsers)
		{
			if (user.EnrolmentMethod is GuestEnrolment)
			{
				WriterUserEnrolmentSection(writer, course, user);
			}
		}
		
		writer.WriteEndElement(); // Close <user_enrolments>
		writer.WriteEndElement(); // Close <enrol>
	}
	

	/// <summary>
	/// writes the `enrol` section for the self enrolment method; to be used within the `enrols` section
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	private void WriteSelfEnrolSection(XmlWriter writer, Model.Course course)
	{
		SelfEnrolment selfEnrolment = (course.EnrolmentMethods.FirstOrDefault(e => e is SelfEnrolment) as SelfEnrolment)!;
		
		WriteStartElementWithAttributes(writer, "enrol", new Dictionary<string, string>
		{
			{ "id", "3" }
		});
		
		var elements = new Dictionary<string, string>
		{
			{ "enrol", "self" },
			{ "status", selfEnrolment is { Enabled: true } ? "0" : "1" }, // 0 -> active; 1 -> inactive | Whyyy? (:
			{ "name", "$@NULL@$" },
			{ "enrol_period", selfEnrolment.EnrolmentDuration?.ToString() ?? "0" },
			{ "enrolstartdate", selfEnrolment.SensibleDefaultsSelfEnrolment.EnrolmentStartDate != null ? _helper.DateTimeToUnixTimestamp((DateTime)selfEnrolment.SensibleDefaultsSelfEnrolment.EnrolmentStartDate) : "0" },
			{ "enrolenddate", selfEnrolment.SensibleDefaultsSelfEnrolment.EnrolmentEndDate != null ? _helper.DateTimeToUnixTimestamp((DateTime)selfEnrolment.SensibleDefaultsSelfEnrolment.EnrolmentEndDate) : "0" },
			{ "expiry_notify", selfEnrolment.NotifyEnroller ? selfEnrolment.NotifyStudent ? "2" : "1" : "0" },
			{ "expiry_threshhold", selfEnrolment.SensibleDefaultsSelfEnrolment.NotifyThreshold?.ToString() ?? "0" },
			{ "notifyall", selfEnrolment.NotifyStudent ? "1" : "0" }, 
			{ "password", selfEnrolment.Password ?? " " },
			{ "cost", "$@NULL@$" }, // Always null, setting doesn't apply to this enrolment method
			{ "currency", "$@NULL@$" }, // Always null, setting doesn't apply to this enrolment method
			{ "roleid", selfEnrolment.DefaultRole.SystemDefaults.Id.ToString() },
			{ "customint1", "0" }, 
			{ "customint2", selfEnrolment.SensibleDefaultsSelfEnrolment.UnenrolInactiveAfter?.ToString() ?? "$@NULL@$"}, // Unenrol Inactive Users
			{ "customint3", selfEnrolment.SensibleDefaultsSelfEnrolment.MaximumNumberOfEnrolments?.ToString() ?? "$@NULL@$" }, // Max. Enrolled users
			{ "customint4", "$@NULL@$" },
			{ "customint5", "$@NULL@$" },
			{ "customint6", "$@NULL@$" },
			{ "customint7", "$@NULL@$" },
			{ "customint8", "$@NULL@$" },
			{ "customchar1", "$@NULL@$" },
			{ "customchar2", "$@NULL@$" },
			{ "customchar3", "$@NULL@$" },
			{ "customdec1", "$@NULL@$" },
			{ "customdec2", "$@NULL@$" },
			{ "customtext1", selfEnrolment.WelcomeMessage ?? "$@NULL@$" },
			{ "customtext2", "$@NULL@$" },
			{ "customtext3", "$@NULL@$" },
			{ "customtext4", "$@NULL@$" },
			{ "timecreated", _helper.DateTimeToUnixTimestamp(selfEnrolment.SystemDefaults.TimeCreated) },
			{ "timemodified", _helper.DateTimeToUnixTimestamp(selfEnrolment.SystemDefaults.TimeModified) }
		};
		
		WriteElements(writer, elements);
		
		// All users enrolled with this method
		writer.WriteStartElement("user_enrolments");

		foreach (User user in course.EnrolledUsers)
		{
			if (user.EnrolmentMethod is SelfEnrolment)
			{
				WriterUserEnrolmentSection(writer, course, user);
			}
		}
		
		writer.WriteEndElement(); // Close <user_enrolments>
		writer.WriteEndElement(); // Close <enrol>
	}
	
	
	/// <summary>
	/// Writes a single `enrolment` section for one specific user; to be used within the `user_enrolments` section for each user enrolled using that method
	/// </summary>
	/// <param name="writer">XmlWriter initialized on the correct document</param>
	/// <param name="course">initialized Moodle Course</param>
	/// <param name="user">the user enrolled using the enrolment method as specified in the parent tag</param>
	private void WriterUserEnrolmentSection(XmlWriter writer, Model.Course course, User user)
	{
		WriteStartElementWithAttributes(writer, "enrolment", new Dictionary<string, string>
		{
			{ "id", _enrolmentIdCounter.ToString() }
		});
		_enrolmentIdCounter++;
		
		var elements = new Dictionary<string, string>
		{
			{ "status", user.SystemDefaults.EnrolmentSuspended ? "1" : "0" }, // 1 -> Suspended; 0 -> Active
			{ "userid", user.SystemDefaults.Id.ToString() },
			{ "timestart", _helper.DateTimeToUnixTimestamp(user.SensibleDefaults.EnrolmentStartDate) ?? "0" },
			{ "timeend", user.SensibleDefaults.EnrolmentEndDate != null ? _helper.DateTimeToUnixTimestamp((DateTime)user.SensibleDefaults.EnrolmentEndDate) : "0" },
			{ "modifierid", GeneralDefaults.DEFAULT_ADMIN_USER_ID.ToString() },
			{ "timemodified", _helper.DateTimeToUnixTimestamp(DateTime.Now) }
		};
				
		WriteElements(writer, elements);
				
		writer.WriteEndElement(); // Close <enrolment>
	}
}