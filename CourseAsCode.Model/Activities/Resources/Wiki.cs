using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Wikis;

namespace CourseAsCode.Model.Activities.Resources;

public class Wiki : Activity
{
    // ────────────────────────────────────────────────────────────────────────────
    // Wiki Configuration
    // ────────────────────────────────────────────────────────────────────────────
    [Description("Introduction text for the wiki")]
    public string Intro { get; set; } = WikiDefaults.DEFAULT_INTRO;

    [Description("Format of the introduction text (e.g., HTML, PlainText)")]
    public FormattingFormat IntroFormat { get; set; } = WikiDefaults.DEFAULT_INTRO_FORMAT;

    [Description("Title of the wiki's first page")]
    public string FirstPageTitle { get; set; } = WikiDefaults.DEFAULT_FIRST_PAGE_TITLE;

    [Description("Wiki collaboration mode (Collaborative or Individual)")]
    public WikiMode WikiMode { get; set; } = WikiDefaults.DEFAULT_WIKI_MODE;

    [Description("Default format for wiki content (Html, Creole, Nwiki)")]
    public WikiFormat DefaultFormat { get; set; } = WikiDefaults.DEFAULT_WIKI_FORMAT;

    [Description("Whether to force the default format (users cannot change)")]
    public bool ForceFormat { get; set; } = WikiDefaults.DEFAULT_FORCE_FORMAT;

    [Description("Start time for editing period (0 = no restriction)")]
    public long EditBegin { get; set; } = WikiDefaults.DEFAULT_EDIT_BEGIN;

    [Description("End time for editing period (0 = no restriction)")]
    public long EditEnd { get; set; } = WikiDefaults.DEFAULT_EDIT_END;

    // ────────────────────────────────────────────────────────────────────────────
    // Subwikis
    // ────────────────────────────────────────────────────────────────────────────
    [Description("List of subwikis (separate wiki instances per group/user)")]
    public List<WikiSubwiki> Subwikis { get; set; } = new();

    public SensibleDefaultsWiki SensibleDefaults { get; set; } = new();
    public SystemDefaultsWiki SystemDefaults { get; set; } = new();

    public class SensibleDefaultsWiki
    {
        // Future use
    }

    public class SystemDefaultsWiki
    {
        // Future use
    }
}

/// <summary>
/// Represents a subwiki within a wiki activity
/// </summary>
public class WikiSubwiki
{
    [Description("Unique identifier for the subwiki")]
    public int SubwikiId { get; set; }

    [Description("Group ID this subwiki belongs to (0 = no group)")]
    public int GroupId { get; set; } = WikiDefaults.DEFAULT_GROUP_ID;

    [Description("User ID this subwiki belongs to (0 = not user-specific)")]
    public int UserId { get; set; } = WikiDefaults.DEFAULT_USER_ID;

    [Description("List of pages in this subwiki")]
    public List<WikiPage> Pages { get; set; } = new();
}

/// <summary>
/// Represents a page within a subwiki
/// </summary>
public class WikiPage
{
    [Description("Unique identifier for the page")]
    public int PageId { get; set; }

    [Description("Title of the page")]
    public string Title { get; set; } = string.Empty;

    [Description("Rendered HTML content of the page")]
    public string CachedContent { get; set; } = WikiDefaults.DEFAULT_CACHED_CONTENT;

    [Description("Timestamp when the page was created")]
    public DateTime TimeCreated { get; set; } = WikiDefaults.DEFAULT_PAGE_TIME_CREATED;

    [Description("Timestamp when the page was last modified")]
    public DateTime TimeModified { get; set; } = WikiDefaults.DEFAULT_PAGE_TIME_MODIFIED;

    [Description("Timestamp when the content was last rendered")]
    public DateTime TimeRendered { get; set; } = WikiDefaults.DEFAULT_PAGE_TIME_RENDERED;

    [Description("User ID of the user who last modified the page")]
    public int UserId { get; set; } = WikiDefaults.DEFAULT_PAGE_USER_ID;

    [Description("Number of times the page has been viewed")]
    public int PageViews { get; set; } = WikiDefaults.DEFAULT_PAGE_VIEWS;

    [Description("Whether the page is read-only")]
    public bool ReadOnly { get; set; } = WikiDefaults.DEFAULT_READ_ONLY;

    [Description("Version history of this page")]
    public List<WikiPageVersion> Versions { get; set; } = new();
}

/// <summary>
/// Represents a version in the page history
/// </summary>
public class WikiPageVersion
{
    [Description("Unique identifier for the version")]
    public int VersionId { get; set; } = WikiDefaults.DEFAULT_VERSION_ID;

    [Description("Content of this version")]
    public string Content { get; set; } = WikiDefaults.DEFAULT_VERSION_CONTENT;

    [Description("Format of the content")]
    public WikiFormat ContentFormat { get; set; } = WikiDefaults.DEFAULT_CONTENT_FORMAT;

    [Description("Version number (starts at 0)")]
    public int Version { get; set; } = WikiDefaults.DEFAULT_VERSION_NUMBER;

    [Description("Timestamp when this version was created")]
    public DateTime TimeCreated { get; set; } = WikiDefaults.DEFAULT_VERSION_TIME_CREATED;

    [Description("User ID of the user who created this version")]
    public int UserId { get; set; } = WikiDefaults.DEFAULT_VERSION_USER_ID;
}
