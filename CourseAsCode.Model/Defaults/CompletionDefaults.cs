using System.Collections.Generic;

namespace CourseAsCode.Model.Defaults
{
    public static class CompletionDefaults
    {
        public const int DEFAULT_ID = 1;
        public const int DEFAULT_USER_ID = 0;
        public const int DEFAULT_COURSE_ID = 0;
        public const long DEFAULT_TIME_ENROLLED = 0;
        public const long DEFAULT_TIME_STARTED = 0;
        public static readonly long? DEFAULT_TIME_COMPLETED = null;
        public const long DEFAULT_REAGGREGATE = 0;

        public static CourseCompletion GetDefaultCompletion()
        {
            return new CourseCompletion
            {
                Id = DEFAULT_ID,
                UserId = DEFAULT_USER_ID,
                CourseId = DEFAULT_COURSE_ID,
                TimeEnrolled = DEFAULT_TIME_ENROLLED,
                TimeStarted = DEFAULT_TIME_STARTED,
                TimeCompleted = DEFAULT_TIME_COMPLETED,
                Reaggregate = DEFAULT_REAGGREGATE
            };
        }

        public static readonly List<CourseCompletion> DEFAULT_COMPLETIONS = new()
        {
            new CourseCompletion
            {
                Id = 1,
                UserId = 31,
                CourseId = 13198,
                TimeEnrolled = 1608236822,
                TimeStarted = 0,
                TimeCompleted = null,
                Reaggregate = 0
            }
        };
    }
}
