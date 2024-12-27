using System.ComponentModel;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model;

public class Section
{
    public required string Name { get; set; }
    public string? Summary { get; set; }
    [Description("whether the section is visible to students or teachers only")]
    public bool VisibleToStudents { get; set; }

    [Description("ordered list of the activities and resources in the section")]
    public List<Activity> Activities { get; set; } = new List<Activity>();

    public SensibleDefaultsSection SensibleDefaults { get; set; } = new SensibleDefaultsSection();
    public SystemDefaultsSection SystemDefaults { get; set; } = new SystemDefaultsSection();

	// Defaults
	public class SensibleDefaultsSection
    {
        
    }
    
    public class SystemDefaultsSection
    {
        [Description("path to the corresponding section folder in the backup directory")]
        public string PathToSection { get; set; }
        [Description("the format in which the SectionSummary is stored")]
        public FormattingFormat SummaryFormat { get; set; }
        [Description("the date and time that the section was last modified")]
        public DateTime LastModified { get; set; }
    }
}