using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Wikis;

namespace CourseAsCode.Model.Defaults.Activities.Resources;

public class WikiDefaults
{
    // Wiki Configuration
    public const string DEFAULT_INTRO = "";
    public const FormattingFormat DEFAULT_INTRO_FORMAT = FormattingFormat.HTML;
    public const string DEFAULT_FIRST_PAGE_TITLE = "First Page";
    public const WikiMode DEFAULT_WIKI_MODE = WikiMode.Collaborative;
    public const WikiFormat DEFAULT_WIKI_FORMAT = WikiFormat.Html;
    public const bool DEFAULT_FORCE_FORMAT = false;
    public const long DEFAULT_EDIT_BEGIN = 0;
    public const long DEFAULT_EDIT_END = 0;

    // Subwiki
    public const int DEFAULT_GROUP_ID = 0;
    public const int DEFAULT_USER_ID = 0;

    // Page
    public const string DEFAULT_CACHED_CONTENT = "";
    public static readonly DateTime DEFAULT_PAGE_TIME_CREATED = DateTime.Now;
    public static readonly DateTime DEFAULT_PAGE_TIME_MODIFIED = DateTime.Now;
    public static readonly DateTime DEFAULT_PAGE_TIME_RENDERED = DateTime.Now;
    public const int DEFAULT_PAGE_USER_ID = 2;
    public const int DEFAULT_PAGE_VIEWS = 0;
    public const bool DEFAULT_READ_ONLY = false;

    // Version
    public const string DEFAULT_VERSION_CONTENT = "";
    public const WikiFormat DEFAULT_CONTENT_FORMAT = WikiFormat.Html;
    public const int DEFAULT_VERSION_NUMBER = 0;
    public static readonly DateTime DEFAULT_VERSION_TIME_CREATED = DateTime.Now;
    public const int DEFAULT_VERSION_USER_ID = 2;
}
