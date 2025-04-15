using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Defaults;

public static class SectionDefaults
{
    // General Defaults
    public const string DEFAULT_SECTION_NAME = "CaC Demo Section";
    public const string? DEFAULT_SECTION_SUMMARY = null;
    public const bool DEFAULT_VISIBLE_TO_STUDENTS = true;
    public static readonly List<Activity> DEFAULT_ACTIVITIES = new List<Activity>();

    // Sensible Defaults

    // System Defaults
    public const int DEFAULT_SECTION_ID = 1;
    public const string DEFAULT_PATH_TO_SECTION = "sections/section_1";
    public const FormattingFormat DEFAULT_SUMMARY_FORMAT = FormattingFormat.HTML;
    public static readonly DateTime DEFAULT_LAST_MODIFIED = DateTime.Now;
    public static readonly Component? DEFAULT_COMPONENT = null;

    /// <summary>
    /// Returns a section object with all properties set to the default values
    /// </summary>
    public static Section GetDefaultSection()
    {
        return new Section()
        {
            Name = DEFAULT_SECTION_NAME,
            Summary = DEFAULT_SECTION_SUMMARY,
            VisibleToStudents = DEFAULT_VISIBLE_TO_STUDENTS,
            Activities = DEFAULT_ACTIVITIES,
            SensibleDefaults = new Section.SensibleDefaultsSection(),
            SystemDefaults = new Section.SystemDefaultsSection()
            {
                SectionId = DEFAULT_SECTION_ID,
                PathToSection = DEFAULT_PATH_TO_SECTION,
                SummaryFormat = DEFAULT_SUMMARY_FORMAT,
                LastModified = DEFAULT_LAST_MODIFIED,
                Component = DEFAULT_COMPONENT,
            },
        };
    }
}
