using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.XML.Helper;

public class CourseHelper : ICourseHelper
{
    private static CourseHelper? _instance;

    public static CourseHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CourseHelper();
            }

            return _instance;
        }
    }

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
