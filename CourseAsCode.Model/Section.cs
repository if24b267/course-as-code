using System.ComponentModel;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Model.Enums;
using Component = CourseAsCode.Model.Enums.Component;

namespace CourseAsCode.Model;

public class Section
{
    public required string Name { get; set; } = SectionDefaults.DEFAULT_SECTION_NAME;
    public string? Summary { get; set; } = SectionDefaults.DEFAULT_SECTION_SUMMARY;
    [Description("whether the section is visible to students or teachers only")]
    public bool VisibleToStudents { get; set; } = SectionDefaults.DEFAULT_VISIBLE_TO_STUDENTS;
    [Description("ordered list of the activities and resources in the section")]
    public List<Activity> Activities { get; set; } = SectionDefaults.DEFAULT_ACTIVITIES;
    
    public SensibleDefaultsSection SensibleDefaults { get; set; } = new SensibleDefaultsSection();
    public SystemDefaultsSection SystemDefaults { get; set; } = new SystemDefaultsSection();

	// Defaults
	public class SensibleDefaultsSection
    {
        
    }
    
    public class SystemDefaultsSection
    {
        public int SectionId { get; set; } = SectionDefaults.DEFAULT_SECTION_ID;
        [Description("path to the corresponding section folder in the backup directory")]
        public string PathToSection { get; set; } = SectionDefaults.DEFAULT_PATH_TO_SECTION;
        [Description("the format in which the SectionSummary is stored")]
        public FormattingFormat SummaryFormat { get; set; } = SectionDefaults.DEFAULT_SUMMARY_FORMAT;
        [Description("the date and time that the section was last modified")]
        public DateTime? LastModified { get; set; } = SectionDefaults.DEFAULT_LAST_MODIFIED;
        [Description("refers to the subsystem or plugin responsible for the section")]
        public Component? Component { get; set; } = SectionDefaults.DEFAULT_COMPONENT;
    }
}