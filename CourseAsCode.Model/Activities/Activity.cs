using System.ComponentModel;
using CourseAsCode.Model.Defaults.Activities;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Activities
{
    public abstract class Activity
    {
        [Description("name of the activity (quiz, file, etc.) as it will be displayed to the user")]
        public required string Name { get; set; } = ActivityDefaults.DEFAULT_NAME;
        public string? Description { get; set; } = "test";
        [Description("the format in which the Activity Description is stored")]
        public FormattingFormat DescriptionFormat { get; set; } = ActivityDefaults.DEFAULT_FORMATTING_FORMAT;
        [Description("which group mode to use for the activity")]
        public GroupMode GroupMode { get; set; } = ActivityDefaults.DEFAULT_GROUP_MODE;
        [Description("list of tags that are used to describe the activity")]
        public List<string>? Tags { get; set; } = ActivityDefaults.DEFAULT_TAGS;

        [Description("whether the activity is visible to students or teachers only")]
        public bool VisibleToStudents { get; set; } = ActivityDefaults.DEFAULT_VISIBLE_TO_STUDENTS;
        [Description("whether the activity description is displayed on the course page")]
        public bool DisplayDescription { get; set; } = ActivityDefaults.DEFAULT_DISPLAY_DESCRIPTION;

        [Description("unique identifier for the activity (quiz, file, etc.) - used for referencing")]
        public int ActivityId { get; set; } = ActivityDefaults.DEFAULT_ACTIVITY_ID;
        [Description("optional identifier for the activity in the course, used for grade calculation and custom reports")]
        public int? CustomId { get; set; } = ActivityDefaults.DEFAULT_CUSTOM_ID;
        [Description("if set, the activity will be displayed in this language despite the user's preferences")]
        public Language? Language { get; set; } = ActivityDefaults.DEFAULT_LANGUAGE;

        [Description("the section to which this activity belongs")]
        public Section? Section { get; set; }

        // Theoretically belonging to SystemDefaults
        [Description("a context in Moodle defines the \"space\" or \"level\" where specific roles, permissions, or settings apply. Contexts are hierarchical, meaning they form a tree-like structure, with permissions inheriting downwards unless overridden")]
        public Context Context { get; set; } = ActivityDefaults.DEFAULT_CONTEXT;
        [Description("the date and time that the file was created")]
        public DateTime TimeCreated { get; set; } = ActivityDefaults.DEFAULT_TIME_CREATED;
        [Description("the date and time that the file was last modified")]
        public DateTime TimeModified { get; set; } = ActivityDefaults.DEFAULT_TIME_MODIFIED;

        public required int ModuleId { get; set; }
        public required string ModuleName { get; set; }
        public object GradeItems { get; set; }
    }
}
