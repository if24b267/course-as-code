namespace CourseAsCode.Model.Defaults
{
    public static class ScaleDefaults
    {
        public const int DEFAULT_ID = 1;
        public const int DEFAULT_COURSE_ID = 0;
        public const int DEFAULT_USER_ID = 0;
        public const string DEFAULT_NAME = "Default Scale";
        public const string DEFAULT_SCALE_VALUES = "not reached, partially reached, reached";
        public const string? DEFAULT_DESCRIPTION = null;
        public const int DEFAULT_DESCRIPTION_FORMAT = 1;
        public static readonly long DEFAULT_TIME_MODIFIED = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        /// <summary>
        /// Returns a scale object with all properties set to the default values
        /// </summary>
        public static Scale GetDefaultScale()
        {
            return new Scale
            {
                Id = DEFAULT_ID,
                CourseId = DEFAULT_COURSE_ID,
                UserId = DEFAULT_USER_ID,
                Name = DEFAULT_NAME,
                ScaleValues = DEFAULT_SCALE_VALUES,
                Description = DEFAULT_DESCRIPTION,
                DescriptionFormat = DEFAULT_DESCRIPTION_FORMAT,
                TimeModified = DEFAULT_TIME_MODIFIED
            };
        }

        /// <summary>
        /// Example default scales
        /// </summary>
        public static readonly List<Scale> DEFAULT_SCALES = new()
        {
            new Scale
            {
                Id = 1,
                CourseId = 1,
                UserId = 1,
                Name = "Learning Outcomes",
                ScaleValues = "not reached, partially reached, reached",
                Description = "",
                DescriptionFormat = 1,
                TimeModified = 1737734484
            }
        };
    }
}
