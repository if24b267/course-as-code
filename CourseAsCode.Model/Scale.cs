using System;
using CourseAsCode.Model.Defaults;

namespace CourseAsCode.Model
{
    public class Scale
    {
        public int Id { get; set; } = ScaleDefaults.DEFAULT_ID;
        public int CourseId { get; set; } = ScaleDefaults.DEFAULT_COURSE_ID;
        public int UserId { get; set; } = ScaleDefaults.DEFAULT_USER_ID;
        public string Name { get; set; } = ScaleDefaults.DEFAULT_NAME;
        public string ScaleValues { get; set; } = ScaleDefaults.DEFAULT_SCALE_VALUES;
        public string? Description { get; set; } = ScaleDefaults.DEFAULT_DESCRIPTION;
        public int DescriptionFormat { get; set; } = ScaleDefaults.DEFAULT_DESCRIPTION_FORMAT;
        public long TimeModified { get; set; } = ScaleDefaults.DEFAULT_TIME_MODIFIED;

        /// <summary>
        /// Returns a deep copy of this scale
        /// </summary>
        public Scale Clone()
        {
            return new Scale
            {
                Id = Id,
                CourseId = CourseId,
                UserId = UserId,
                Name = Name,
                ScaleValues = ScaleValues,
                Description = Description,
                DescriptionFormat = DescriptionFormat,
                TimeModified = TimeModified
            };
        }
    }
}
