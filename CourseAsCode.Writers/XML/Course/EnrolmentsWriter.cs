using System.Xml;

namespace CourseAsCode.Writers.XML.Course;

public class EnrolmentsWriter
{
	private readonly XmlWriterSettings _settings = new()
	{
		Indent = true, // Makes the XML output more readable
		NewLineOnAttributes = false, // Keeps attributes in the same line
		OmitXmlDeclaration = true // Removes the XML declaration
	};

	public void WriteEnrolment(string outputPath)
	{
		string filePath = Path.GetFullPath(Path.Combine(outputPath, "course/enrolments.xml"));
		using (XmlWriter writer = XmlWriter.Create(filePath, _settings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("enrolments"); // <enrolments> starts here
			writer.WriteStartElement("enrols"); // <enrols> starts here

			// <enrol> starts here with with attributes
			writer.WriteStartElement("enrol");
			writer.WriteAttributeString("id", "1");

			// child elements of <enrol>
			writer.WriteElementString("enrol", "manual");
			writer.WriteElementString("status", "0");
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
			writer.WriteElementString("customint1", "1");
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

			// <enrolment> starts here with with attributes 
			writer.WriteStartElement("enrolment");
			writer.WriteAttributeString("id", "1");

			// child elements of <enrolment>
			writer.WriteElementString("status", "0");
			writer.WriteElementString("userid", "3");
			writer.WriteElementString("timestart", "0");
			writer.WriteElementString("timeend", "0");
			writer.WriteElementString("modifierid", "2");
			writer.WriteElementString("timemodified", "1731136235");

			// </enrolment> ends here
			writer.WriteEndElement();

			// </user_enrolments> ends here 
			writer.WriteEndElement();

			// </enrol> ends here 
			writer.WriteEndElement();

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

	/*private void WriteEnrolments(XmlWriter writer, User user, int i)
	{
	    WriteStartElementWithAttributes(writer, "enrol", new Dictionary<string, string>
	    {
	        { "id", i.ToString() }
	    });

	    var elements = new Dictionary<string, string>
	    {
	        { "enrol", user.Role.ToString() },
	        { "status", user.SystemDefaults.Id.ToString() },
	        { "name", user.SystemDefaults.TimeModified.ToString() },
	        { "enrolperiod", user.SystemDefaults.TimeModified.ToString() },
	        { "enrolstartdate", user.SystemDefaults.TimeModified.ToString() },
	        { "enrolenddate", user.SystemDefaults.TimeModified.ToString() },
	        { "expirynotify", user.SystemDefaults.TimeModified.ToString() },
	        { "expirythreshold", user.SystemDefaults.TimeModified.ToString() },
	        { "notifyall", user.SystemDefaults.TimeModified.ToString() },
	        { "password", user.SystemDefaults.TimeModified.ToString() },
	        { "cost", user.SystemDefaults.TimeModified.ToString() },
	        { "currency", user.SystemDefaults.TimeModified.ToString() },
	        { "roleid", user.SystemDefaults.TimeModified.ToString() },
	        { "customint1", user.SystemDefaults.TimeModified.ToString() },
	        { "customint2", user.SystemDefaults.TimeModified.ToString() },
	        { "customint3", user.SystemDefaults.TimeModified.ToString() },
	        { "customint4", user.SystemDefaults.TimeModified.ToString() },
	        { "customint5", user.SystemDefaults.TimeModified.ToString() },
	        { "customint6", user.SystemDefaults.TimeModified.ToString() },
	        { "customint7", user.SystemDefaults.TimeModified.ToString() },
	        { "customint8", user.SystemDefaults.TimeModified.ToString() },
	        { "customchar1", user.SystemDefaults.TimeModified.ToString() },
	        { "customchar2", user.SystemDefaults.TimeModified.ToString() },
	        { "customchar3", user.SystemDefaults.TimeModified.ToString() },
	        { "customdec1", user.SystemDefaults.TimeModified.ToString() },
	        { "customdec2", user.SystemDefaults.TimeModified.ToString() },
	        { "customtext1", user.SystemDefaults.TimeModified.ToString() },
	        { "customtext2", user.SystemDefaults.TimeModified.ToString() },
	        { "customtext3", user.SystemDefaults.TimeModified.ToString() },
	        { "customtext4", user.SystemDefaults.TimeModified.ToString() },
	        { "timecreated", user.SystemDefaults.TimeModified.ToString() },
	        { "timemodified", user.SystemDefaults.TimeModified.ToString() },

	          writer.WriteEndElement(); // </enrol> ends here

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
	    };

	    WriteElements(writer, elements);

	    writer.WriteEndElement(); // </assignment> ends here
	}*/
}