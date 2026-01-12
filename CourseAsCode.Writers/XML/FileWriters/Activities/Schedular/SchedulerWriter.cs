using System;
using System.Collections.Generic;
using System.Xml;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using SchedulerModel = CourseAsCode.Model.Activities.Resources.Scheduler;
using CourseAsCode.Writers.XML.Writer;

namespace CourseAsCode.Writers.XML.Writer.Activities.Scheduler
{
    public class SchedulerWriter : XmlWriterBase
    {
        private const string DEFAULT_SCHEDULER_RELATIVE_PATH = "scheduler.xml";
        private readonly IGeneralHelper _generalHelper;

        public SchedulerWriter(IXmlWriterFactory xmlWriterFactory, ILogger<SchedulerWriter> logger, IGeneralHelper generalHelper)
            : base(xmlWriterFactory, logger)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
        }

        public void WriteScheduler(string outputPath, SchedulerModel scheduler)
        {
            try
            {
                string filePath = Path.GetFullPath(Path.Combine(outputPath, DEFAULT_SCHEDULER_RELATIVE_PATH));

                using (XmlWriter writer = CreateXmlWriter(filePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("activity");

                    WriteActivityAttributes(writer, scheduler);

                    writer.WriteStartElement("scheduler");
                    writer.WriteAttributeString("id", scheduler.ActivityId.ToString());

                    var elements = new Dictionary<string, string>
                    {
                        { "name", scheduler.Name },
                        { "intro", scheduler.Description ?? string.Empty },
                        { "introformat", _generalHelper.EnumIntegerToString(scheduler.DescriptionFormat) },
                        { "schedulermode", scheduler.SchedulerMode ?? string.Empty },
                        { "maxbookings", scheduler.MaxBookings.ToString() },
                        { "guardtime", scheduler.GuardTime.ToString() },
                        { "defaultslotduration", scheduler.DefaultSlotDuration.ToString() },
                        { "allownotifications", scheduler.AllowNotifications.ToString() },
                        { "staffrolename", scheduler.StaffRoleName ?? string.Empty },
                        { "scale", scheduler.Scale.ToString() },
                        { "gradingstrategy", scheduler.GradingStrategy.ToString() },
                        { "bookingrouping", scheduler.BookingGrouping.ToString() },
                        { "usenotes", scheduler.UseNotes.ToString() },
                        { "usebookingform", scheduler.UseBookingForm.ToString() },
                        { "bookinginstructions", scheduler.BookingInstructions ?? string.Empty },
                        { "bookinginstructionsformat", _generalHelper.EnumIntegerToString(scheduler.BookingInstructionsFormat) },
                        { "usestudentnotes", scheduler.UseStudentNotes.ToString() },
                        { "requireupload", scheduler.RequireUpload.ToString() },
                        { "uploadmaxfiles", scheduler.UploadMaxFiles.ToString() },
                        { "uploadmaxsize", scheduler.UploadMaxSize.ToString() },
                        { "usecaptcha", scheduler.UseCaptcha.ToString() },
                        { "timemodified", _generalHelper.DateTimeToUnixTimestamp(scheduler.TimeModified) }
                    };

                    WriteElements(writer, elements);

                    // slots
                    writer.WriteStartElement("slots");
                    foreach (var slot in scheduler.Slots)
                    {
                        writer.WriteStartElement("slot");
                        writer.WriteAttributeString("id", slot.Id.ToString());

                        var slotElements = new Dictionary<string, string>
                        {
                            { "staffid", slot.StaffId.ToString() },
                            { "starttime", _generalHelper.DateTimeToUnixTimestamp(slot.StartTime) },
                            { "duration", slot.Duration.ToString() },
                            { "maxstudents", slot.MaxStudents.ToString() },
                            { "booked", slot.Booked.ToString() },
                            { "timemodified", _generalHelper.DateTimeToUnixTimestamp(slot.TimeModified) }
                        };

                        WriteElements(writer, slotElements);
                        writer.WriteEndElement(); // </slot>
                    }
                    writer.WriteEndElement(); // </slots>

                    writer.WriteEndElement(); // </scheduler>
                    writer.WriteEndElement(); // </activity>
                    writer.WriteEndDocument();
                }

                LogDebug($"Created {Path.GetFileName(outputPath)}/{DEFAULT_SCHEDULER_RELATIVE_PATH}");
            }
            catch (Exception e)
            {
                LogError(e, $"Error writing {Path.GetFileName(outputPath)}/{DEFAULT_SCHEDULER_RELATIVE_PATH}");
                throw;
            }
        }

        private void WriteActivityAttributes(XmlWriter writer, SchedulerModel scheduler)
        {
            writer.WriteAttributeString("id", scheduler.ActivityId.ToString());
            writer.WriteAttributeString("moduleid", scheduler.SystemDefaults.ModuleId.ToString());
            writer.WriteAttributeString("modulename", scheduler.SystemDefaults.ModuleName);
            writer.WriteAttributeString("contextid", scheduler.ActivityId.ToString());
        }
    }
}
