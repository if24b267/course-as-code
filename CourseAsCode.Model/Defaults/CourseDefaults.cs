using CourseAsCode.Model.Enrolments;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults;

public static class CourseDefaults
{
    public const string DEFAULT_SHORT_NAME = "CaC";
    public const string DEFAULT_FULL_NAME = "CourseAsCode";
    
    public const string DEFAULT_CATEGORY_NAME = "Category 1";

    public const GroupMode DEFAULT_GROUP_MODE = GroupMode.NoGroups;
    
    public static readonly DateTime DEFAULT_START_DATE = DateTime.Now;
    public static readonly DateTime DEFAULT_TIME_CREATED = DateTime.Now;
    public static readonly DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;
    
    public static readonly List<EnrolmentMethod> DEFAULT_ENROLMENT_METHODS = new List<EnrolmentMethod>
    {
        new ManualEnrolment(),
        new SelfEnrolment(),
        new GuestEnrolment()
    };
} 