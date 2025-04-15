using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities;

public class ActivityDefaults
{
    // General defaults
    public static string DEFAULT_NAME = "CaC Demo Activity";
    public static string? DEFAULT_DESCRIPTION = null;
    public static FormattingFormat DEFAULT_FORMATTING_FORMAT = FormattingFormat.Markdown;
    public static GroupMode DEFAULT_GROUP_MODE = GroupMode.NoGroups;
    public static List<string>? DEFAULT_TAGS = null;

    public static bool DEFAULT_VISIBLE_TO_STUDENTS = true;
    public static bool DEFAULT_DISPLAY_DESCRIPTION = true;

    public static int DEFAULT_ACTIVITY_ID = 1;
    public static int? DEFAULT_CUSTOM_ID = null;
    public static Language? DEFAULT_LANGUAGE = null;

    public static Context DEFAULT_CONTEXT = Context.Module;
    public static DateTime DEFAULT_TIME_CREATED = DateTime.Now;
    public static DateTime DEFAULT_TIME_MODIFIED = DateTime.Now;
}
