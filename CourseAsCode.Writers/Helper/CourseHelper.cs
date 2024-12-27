using CourseAsCode.Model.Enums;

namespace CourseAsCode.Writers.Helper;

public class CourseHelper
{
    #region Singleton
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
    #endregion
    
    public string GetCourseFormatFromEnum(CourseFormat format)
    {
        return format switch
        {
            CourseFormat.CustomSections => "topics",
            CourseFormat.WeeklySections => "weeks",
            CourseFormat.SingleActivity => "singleactivity",
            CourseFormat.Social => "social",
            _ => "topics"
        };
    }
}