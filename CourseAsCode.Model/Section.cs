using System.ComponentModel;
using CourseAsCode.Model.Activities;

namespace CourseAsCode.Model;

public class Section
{
    public required string Name { get; set; }
    public string? Summary { get; set; }
    [Description("whether the section is visible to students or teachers only")]
    public bool VisibleToStudents { get; set; }
    
    [Description("ordered list of the activities and resources in the section")]
    List<Activity>? Activities { get; set; }
    
    // Defaults
    public class SensibleDefaults
    {
        
    }
    
    public class SystemDefaults
    {
        [Description("path to the corresponding section folder in the backup directory")]
        public required string PathToSection { get; set; }
        [Description("the format in which the SectionSummary is stored")]
        public int SummaryFormat { get; set; }
        [Description("the date and time that the section was last modified")]
        public DateTime LastModified { get; set; }
    }
}