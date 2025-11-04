using System;
using CourseAsCode.Model.Defaults;

namespace CourseAsCode.Model
{
    public class CourseCompletion
    {
        public int Id { get; set; } = CompletionDefaults.DEFAULT_ID;
        public int UserId { get; set; } = CompletionDefaults.DEFAULT_USER_ID;
        public int CourseId { get; set; } = CompletionDefaults.DEFAULT_COURSE_ID;
        public long TimeEnrolled { get; set; } = CompletionDefaults.DEFAULT_TIME_ENROLLED;
        public long TimeStarted { get; set; } = CompletionDefaults.DEFAULT_TIME_STARTED;
        public long? TimeCompleted { get; set; } = CompletionDefaults.DEFAULT_TIME_COMPLETED;
        public long Reaggregate { get; set; } = CompletionDefaults.DEFAULT_REAGGREGATE;
    }
}
