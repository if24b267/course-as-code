using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces
{
    public interface ICourseHelper
    {
        /// <summary>
        /// Converts a <see cref="CourseFormat"/> enum value to its corresponding string representation as used in Moodle.
        /// </summary>
        /// <param name="format">The <see cref="CourseFormat"/> value to be converted.</param>
        /// <returns>
        /// A string representing the course format. Returns "topics" for unrecognized enum values.
        /// </returns>
        string GetCourseFormatFromEnum(CourseFormat format);
    }
}
