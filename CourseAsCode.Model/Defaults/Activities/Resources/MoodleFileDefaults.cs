using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults.Activities.Resources;

public class MoodleFileDefaults
{
    // General defaults
    public const string DEFAULT_FILE_NAME = "$@NULL@$";
    public const string DEFAULT_FILE_PATH = "/";

    // Sensible defaults
    public static User? DEFAULT_AUTHOR = null;
    public static License? DEFAULT_LICENSE = null;

    // System defaults
    public const string DEFAULT_CONTENT_HASH = "d7ecb4a8bc606fbc2b104e0f94d6476246b024bf";
    public static Component? DEFAULT_COMPONENT = Component.Resource;
    public static FileArea? DEFAULT_FILE_AREA = FileArea.CourseContent;
    public const int DEFAULT_ITEM_ID = 0;
}
