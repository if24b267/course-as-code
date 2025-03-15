using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper;

public interface ICourseHelper
{
    string GetCourseFormatFromEnum(CourseFormat format);
}