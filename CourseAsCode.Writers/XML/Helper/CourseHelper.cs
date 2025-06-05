using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Helper
{
    /// <summary>
    /// Provides utility methods for handling and converting course-related data, particularly for course formats.
    /// </summary>
    public class CourseHelper : ICourseHelper
    {
        public string GetCourseFormatFromEnum(CourseFormat format)
        {
            return format switch
            {
                CourseFormat.CustomSections => "topics",
                CourseFormat.WeeklySections => "weeks",
                CourseFormat.SingleActivity => "singleactivity",
                CourseFormat.Social => "social",
                _ => "topics",
            };
        }
    }
}
