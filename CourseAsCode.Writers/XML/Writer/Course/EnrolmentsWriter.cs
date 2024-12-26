using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Enrolments;
using CourseAsCode.Writers.Helper;

namespace CourseAsCode.Writers.XML.Writer.Course;

public class EnrolmentsWriter : XmlWriterBase
{
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
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartElement("enrolments");
			writer.WriteStartElement("enrols");
			
			WriteManualEnrolmentSection(writer, course);

			// <enrol> starts here with with attributes
			writer.WriteStartElement("enrol");
			writer.WriteAttributeString("id", "2");

			// child elements of <enrol>
			writer.WriteElementString("enrol", "guest");
			writer.WriteElementString("status", "1");
			writer.WriteElementString("name", "$@NULL@$");
			writer.WriteElementString("enrolperiod", "0");
			writer.WriteElementString("enrolstartdate", "0");
			writer.WriteElementString("enrolenddate", "0");
			writer.WriteElementString("expirynotify", "0");
			writer.WriteElementString("expirythreshold", "0");
			writer.WriteElementString("notifyall", "0");
			writer.WriteElementString("password", "");
			writer.WriteElementString("cost", "$@NULL@$");
			writer.WriteElementString("currency", "$@NULL@$");
			writer.WriteElementString("roleid", "0");
			writer.WriteElementString("customint1", "$@NULL@$");
			writer.WriteElementString("customint2", "$@NULL@$");
			writer.WriteElementString("customint3", "$@NULL@$");
			writer.WriteElementString("customint4", "$@NULL@$");
			writer.WriteElementString("customint5", "$@NULL@$");
			writer.WriteElementString("customint6", "$@NULL@$");
			writer.WriteElementString("customint7", "$@NULL@$");
			writer.WriteElementString("customint8", "$@NULL@$");
			writer.WriteElementString("customchar1", "$@NULL@$");
			writer.WriteElementString("customchar2", "$@NULL@$");
			writer.WriteElementString("customchar3", "$@NULL@$");
			writer.WriteElementString("customdec1", "$@NULL@$");
			writer.WriteElementString("customdec2", "$@NULL@$");
			writer.WriteElementString("customtext1", "$@NULL@$");
			writer.WriteElementString("customtext2", "$@NULL@$");
			writer.WriteElementString("customtext3", "$@NULL@$");
			writer.WriteElementString("customtext4", "$@NULL@$");
			writer.WriteElementString("timecreated", "1731001588");
			writer.WriteElementString("timemodified", "1731001588");

			// <user_enrolments> starts here 
			writer.WriteStartElement("user_enrolments");

			// </user_enrolments> ends here 
			writer.WriteEndElement();

			// </enrol> ends here 
			writer.WriteEndElement();

			// <enrol> starts here with with attributes
			writer.WriteStartElement("enrol");
			writer.WriteAttributeString("id", "3");

			// child elements of <enrol>
			writer.WriteElementString("enrol", "self");
			writer.WriteElementString("status", "1");
			writer.WriteElementString("name", "$@NULL@$");
			writer.WriteElementString("enrolperiod", "0");
			writer.WriteElementString("enrolstartdate", "0");
			writer.WriteElementString("enrolenddate", "0");
			writer.WriteElementString("expirynotify", "0");
			writer.WriteElementString("expirythreshold", "86400");
			writer.WriteElementString("notifyall", "0");
			writer.WriteElementString("password", "$@NULL@$");
			writer.WriteElementString("cost", "$@NULL@$");
			writer.WriteElementString("currency", "$@NULL@$");
			writer.WriteElementString("roleid", "5");
			writer.WriteElementString("customint1", "0");
			writer.WriteElementString("customint2", "0");
			writer.WriteElementString("customint3", "0");
			writer.WriteElementString("customint4", "1");
			writer.WriteElementString("customint5", "0");
			writer.WriteElementString("customint6", "1");
			writer.WriteElementString("customint7", "$@NULL@$");
			writer.WriteElementString("customint8", "$@NULL@$");
			writer.WriteElementString("customchar1", "$@NULL@$");
			writer.WriteElementString("customchar2", "$@NULL@$");
			writer.WriteElementString("customchar3", "$@NULL@$");
			writer.WriteElementString("customdec1", "$@NULL@$");
			writer.WriteElementString("customdec2", "$@NULL@$");
			writer.WriteElementString("customtext1", "$@NULL@$");
			writer.WriteElementString("customtext2", "$@NULL@$");
			writer.WriteElementString("customtext3", "$@NULL@$");
			writer.WriteElementString("customtext4", "$@NULL@$");
			writer.WriteElementString("timecreated", "1731001588");
			writer.WriteElementString("timemodified", "1731001588");

			// <user_enrolments> starts here 
			writer.WriteStartElement("user_enrolments");

			// </user_enrolments> ends here 
			writer.WriteEndElement();

			// </enrol> ends here 
			writer.WriteEndElement();


			writer.WriteEndElement(); // </enrols> ends here 
			writer.WriteEndElement(); // </enrolments> ends here 
			writer.WriteEndDocument();
		}
	}
	
	
	private void WriteManualEnrolmentSection(XmlWriter writer, Model.Course course)
	{
		ManualEnrolment? manualEnrolment = (course.EnrolmentMethods.FirstOrDefault(e => e is ManualEnrolment) as ManualEnrolment);
		if (manualEnrolment == null)
		{
			return;
		}
		
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
			{ "customint2", "$@NULL@$" },
			{ "customint3", "$@NULL@$" },
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
		
		writer.WriteEndElement(); // Close <enrol>
	}
}