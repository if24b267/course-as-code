using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Enrolments;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Static;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.Course
{
    public class EnrolmentsWriter : XmlWriterBase
    {
        private const string DEFAULT_ENROLMENTS_RELATIVE_PATH = "course/enrolments.xml";
        private int _enrolmentIdCounter = 1; // used to assign ascending IDs to each enrolment
        private readonly IGeneralHelper _helper;


        /// <summary>
        /// Creates a new instance of the <see cref="EnrolmentsWriter"/> class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to <see cref="EnrolmentsWriter"/></param>
        /// <param name="helper">General Helper providing various helper methods</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null</exception>
        public EnrolmentsWriter(IXmlWriterFactory xmlWriterFactory, ILogger<EnrolmentsWriter> logger, IGeneralHelper helper)
            : base(xmlWriterFactory, logger)
        {
            _helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }


        public void WriteEnrolment(Model.Course course, string outputPath)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_ENROLMENTS_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("enrolments");
                    writer.WriteStartElement("enrols");

                    int enrolId = 439; // Start-ID wie im Beispiel
                    foreach (var enrolmentMethod in course.EnrolmentMethods)
                    {
                        if (enrolmentMethod is ManualEnrolment manual)
                        {
                            WriteManualEnrolSection(writer, course, manual, enrolId++);
                        }
                        else if (enrolmentMethod is GuestEnrolment guest)
                        {
                            WriteGuestEnrolSection(writer, course, guest, enrolId++);
                        }
                        else if (enrolmentMethod is SelfEnrolment self)
                        {
                            WriteSelfEnrolSection(writer, course, self, enrolId++);
                        }
                    }

                    writer.WriteEndElement(); // </enrols>
                    writer.WriteEndElement(); // </enrolments>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {DEFAULT_ENROLMENTS_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {DEFAULT_ENROLMENTS_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteManualEnrolSection(XmlWriter writer, Model.Course course, ManualEnrolment manualEnrolment, int enrolId)
        {
            WriteStartElementWithAttributes(writer, "enrol", new Dictionary<string, string>
    {
        { "id", enrolId.ToString() }
    });

            // Use tag names as in your sample XML
            var elements = new Dictionary<string, string>
    {
        { "enrol", "manual" },
        { "status", manualEnrolment.Enabled ? "0" : "1" },
        { "name", "$@NULL@$" },
        { "enrolperiod", manualEnrolment.EnrolmentDuration?.ToString() ?? "0" },
        { "enrolstartdate", "0" },
        { "enrolenddate", "0" },
        { "expirynotify", manualEnrolment.NotifyEnroller ? (manualEnrolment.NotifyStudent ? "2" : "1") : "0" },
        { "expirythreshold", manualEnrolment.NotifyThreshold?.ToString() ?? "86400" },
        { "notifyall", manualEnrolment.NotifyStudent ? "1" : "0" },
        { "password", "$@NULL@$" },
        { "cost", "$@NULL@$" },
        { "currency", "$@NULL@$" },
        { "roleid", manualEnrolment.DefaultRole.SystemDefaults.RoleId.ToString() },
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
        { "timemodified", _helper.DateTimeToUnixTimestamp(manualEnrolment.SystemDefaults.TimeModified) },
    };

            WriteElements(writer, elements);

            //TODO: Add custom fields if needed
            writer.WriteStartElement("user_enrolments");
            writer.WriteStartElement("enrolment");
            writer.WriteAttributeString("id", "17");
            writer.WriteElementString("status", "0");
            writer.WriteElementString("userid", "2");
            writer.WriteElementString("timestart", "0");
            writer.WriteElementString("timeend", "0");
            writer.WriteElementString("modifierid", "2");
            writer.WriteElementString("timemodified", "1745514076");
            writer.WriteEndElement();
            writer.WriteEndElement(); // </user_enrolments>
            writer.WriteEndElement(); // </enrol>
        }

        private void WriteGuestEnrolSection(XmlWriter writer, Model.Course course, GuestEnrolment guestEnrolment, int enrolId)
        {
            WriteStartElementWithAttributes(writer, "enrol", new Dictionary<string, string>
    {
        { "id", enrolId.ToString() }
    });

            var elements = new Dictionary<string, string>
    {
        { "enrol", "guest" },
        { "status", guestEnrolment.Enabled ? "0" : "1" },
        { "name", "$@NULL@$" },
        { "enrolperiod", "0" },
        { "enrolstartdate", "0" },
        { "enrolenddate", "0" },
        { "expirynotify", "0" },
        { "expirythreshold", "0" },
        { "notifyall", "0" },
        { "password", guestEnrolment.Password ?? "" },
        { "cost", "$@NULL@$" },
        { "currency", "$@NULL@$" },
        { "roleid", guestEnrolment.DefaultRole.SystemDefaults.RoleId.ToString() },
        { "customint1", "$@NULL@$" },
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
        { "customtext1", "$@NULL@$" },
        { "customtext2", "$@NULL@$" },
        { "customtext3", "$@NULL@$" },
        { "customtext4", "$@NULL@$" },
        { "timecreated", _helper.DateTimeToUnixTimestamp(guestEnrolment.SystemDefaults.TimeCreated) },
        { "timemodified", _helper.DateTimeToUnixTimestamp(guestEnrolment.SystemDefaults.TimeModified) },
    };

            WriteElements(writer, elements);

            writer.WriteStartElement("user_enrolments");
            foreach (User user in course.EnrolledUsers)
            {
                if (user.EnrolmentMethod is GuestEnrolment)
                {
                    WriteUserEnrolmentSection(writer, user, 0); // Use your own logic for ID if needed
                }
            }
            writer.WriteEndElement(); // </user_enrolments>
            writer.WriteEndElement(); // </enrol>
        }

        private void WriteSelfEnrolSection(XmlWriter writer, Model.Course course, SelfEnrolment selfEnrolment, int enrolId)
        {
            WriteStartElementWithAttributes(writer, "enrol", new Dictionary<string, string>
    {
        { "id", enrolId.ToString() }
    });

            var elements = new Dictionary<string, string>
    {
        { "enrol", "self" },
        { "status", selfEnrolment.Enabled ? "0" : "1" },
        { "name", "$@NULL@$" },
        { "enrolperiod", selfEnrolment.EnrolmentDuration?.ToString() ?? "0" },
        { "enrolstartdate", selfEnrolment.EnrolmentStartDate != null ? _helper.DateTimeToUnixTimestamp((DateTime)selfEnrolment.EnrolmentStartDate) : "0" },
        { "enrolenddate", selfEnrolment.EnrolmentEndDate != null ? _helper.DateTimeToUnixTimestamp((DateTime)selfEnrolment.EnrolmentEndDate) : "0" },
        { "expirynotify", selfEnrolment.NotifyEnroller ? (selfEnrolment.NotifyStudent ? "2" : "1") : "0" },
        { "expirythreshold", selfEnrolment.NotifyThreshold?.ToString() ?? "86400" },
        { "notifyall", selfEnrolment.NotifyStudent ? "1" : "0" },
        { "password", selfEnrolment.Password ?? "$@NULL@$" },
        { "cost", "$@NULL@$" },
        { "currency", "$@NULL@$" },
        { "roleid", selfEnrolment.DefaultRole.SystemDefaults.RoleId.ToString() },
        { "customint1", "0" },
        { "customint2", selfEnrolment.UnenrolInactiveAfter?.ToString() ?? "0" },
        { "customint3", selfEnrolment.MaximumNumberOfEnrolments?.ToString() ?? "0" },
        { "customint4", "1" },
        { "customint5", "0" },
        { "customint6", "1" },
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
        { "timemodified", _helper.DateTimeToUnixTimestamp(selfEnrolment.SystemDefaults.TimeModified) },
    };

            WriteElements(writer, elements);

            writer.WriteStartElement("user_enrolments");
            foreach (User user in course.EnrolledUsers)
            {
                if (user.EnrolmentMethod is SelfEnrolment)
                {
                    WriteUserEnrolmentSection(writer, user, 0); // Use your own logic for ID if needed
                }
            }
            writer.WriteEndElement(); // </user_enrolments>
            writer.WriteEndElement(); // </enrol>
        }

        private void WriteUserEnrolmentSection(XmlWriter writer, User user, int enrolmentId)
        {
            WriteStartElementWithAttributes(writer, "enrolment", new Dictionary<string, string>
    {
        { "id", enrolmentId.ToString() }
    });

            var elements = new Dictionary<string, string>
    {
        { "status", user.SystemDefaults.EnrolmentSuspended ? "1" : "0" },
        { "userid", user.SystemDefaults.Id.ToString() },
        { "timestart", "0" }, // Passe ggf. an
        { "timeend", "0" },   // Passe ggf. an
        { "modifierid", "2" }, // Passe ggf. an
        { "timemodified", "1745514076" } // Passe ggf. an
    };

            WriteElements(writer, elements);
            writer.WriteEndElement();
        }
    }
}
