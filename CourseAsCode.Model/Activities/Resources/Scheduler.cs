using System;
using System.Collections.Generic;
using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities.Resources
{
    public class Scheduler : CourseAsCode.Model.Activities.Activity
    {
        private static readonly Random _rnd = new();

        [Description("mode of the scheduler (e.g. oneonly)")]
        public string SchedulerMode { get; set; } = SchedulerDefaults.DEFAULT_SCHEDULER_MODE;

        [Description("maximum bookings per user")]
        public int MaxBookings { get; set; } = SchedulerDefaults.DEFAULT_MAXBOOKINGS;

        [Description("guard time in seconds")]
        public int GuardTime { get; set; } = SchedulerDefaults.DEFAULT_GUARDTIME;

        [Description("default slot duration in minutes")]
        public int DefaultSlotDuration { get; set; } = SchedulerDefaults.DEFAULT_DEFAULTSLOTDURATION;

        [Description("allow notifications (0/1)")]
        public int AllowNotifications { get; set; } = SchedulerDefaults.DEFAULT_ALLOWNOTIFICATIONS;

        [Description("staff role name")]
        public string StaffRoleName { get; set; } = SchedulerDefaults.DEFAULT_STAFFROLENAME;

        [Description("scale")]
        public int Scale { get; set; } = SchedulerDefaults.DEFAULT_SCALE;

        [Description("grading strategy")]
        public int GradingStrategy { get; set; } = SchedulerDefaults.DEFAULT_GRADINGSTRATEGY;

        [Description("booking grouping")]
        public int BookingGrouping { get; set; } = SchedulerDefaults.DEFAULT_BOOKINGROUPING;

        [Description("use notes flag")]
        public int UseNotes { get; set; } = SchedulerDefaults.DEFAULT_USENOTES;

        [Description("use booking form flag")]
        public int UseBookingForm { get; set; } = SchedulerDefaults.DEFAULT_USEBOOKINGFORM;

        [Description("booking instructions text")]
        public string BookingInstructions { get; set; } = SchedulerDefaults.DEFAULT_BOOKINGINSTRUCTIONS;

        [Description("format of booking instructions")]
        public FormattingFormat BookingInstructionsFormat { get; set; } = SchedulerDefaults.DEFAULT_BOOKINGINSTRUCTIONSFORMAT;

        [Description("use student notes flag")]
        public int UseStudentNotes { get; set; } = SchedulerDefaults.DEFAULT_USESTUDENTNOTES;

        [Description("require upload flag")]
        public int RequireUpload { get; set; } = SchedulerDefaults.DEFAULT_REQUIREUPLOAD;

        [Description("max files for upload")]
        public int UploadMaxFiles { get; set; } = SchedulerDefaults.DEFAULT_UPLOADMAXFILES;

        [Description("max upload size")]
        public int UploadMaxSize { get; set; } = SchedulerDefaults.DEFAULT_UPLOADMAXSIZE;

        [Description("use captcha flag")]
        public int UseCaptcha { get; set; } = SchedulerDefaults.DEFAULT_USECAPTCHA;

        [Description("time the scheduler was last modified")]
        public DateTime TimeModified { get; set; } = SchedulerDefaults.DEFAULT_TIME_MODIFIED;

        [Description("list of scheduler slots")]
        public List<SchedulerSlot> Slots { get; set; } = new();

        public SensibleDefaultsScheduler SensibleDefaults { get; set; } = new SensibleDefaultsScheduler();
        public SystemDefaultsScheduler SystemDefaults { get; set; } = new SystemDefaultsScheduler();

        public class SensibleDefaultsScheduler
        {
            // placeholder for future sensible defaults
        }

        public class SystemDefaultsScheduler
        {
            public int ModuleId { get; set; } = _rnd.Next(1, 1000);
            public string ModuleName { get; set; } = "scheduler";
        }

        public class SchedulerSlot
        {
            public int Id { get; set; }
            public int StaffId { get; set; }
            public DateTime StartTime { get; set; }
            public int Duration { get; set; }
            public int MaxStudents { get; set; }
            public int Booked { get; set; }
            public DateTime TimeModified { get; set; } = DateTime.Now;
        }
    }
}
