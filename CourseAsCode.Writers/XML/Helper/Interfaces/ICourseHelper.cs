using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.XML.Helper.Interfaces;

public interface ICourseHelper
{
    string GetCourseFormatFromEnum(CourseFormat format);
}
