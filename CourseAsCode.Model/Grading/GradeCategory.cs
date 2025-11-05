using System.ComponentModel;
using CourseAsCode.Model.Defaults.Grading;
using CourseAsCode.Model.Enums.Grading;
using CourseAsCode.Model.Grading.Interfaces;

namespace CourseAsCode.Model.Grading
{
    public class GradeCategory : IGradeable
    {
        [Description("Unique identifier for the grade category, used to reference the category in other files")]
        public int GradeCategoryId { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_CATEGORY_ID;

        [Description("how grades within the category are aggregated")]
        public GradeAggregation AggregationType { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_AGGREGATION;

        [Description("The parent grade category, if any")]
        public GradeCategory? Parent { get; set; } = GradeCategoryDefaults.DEFAULT_PARENT_CATEGORY;

        [Description("enables a specified number of the lowest grades to be excluded from the aggregation")]
        public int? DropLowestGrades { get; set; } = GradeCategoryDefaults.DEFAULT_DROP_LOWEST_GRADES;

        [Description("when the grade category was created")]
        public DateTime? Created { get; set; } = GradeCategoryDefaults.DEFAULT_CREATED;

        [Description("when the grade category was last modified")]
        public DateTime? Modified { get; set; } = GradeCategoryDefaults.DEFAULT_MODIFIED;

        // ────────────────────────────────────────────────────────────────────────────
        // IGradeable | Serves as a default for all activities in that Grade Category
        // ────────────────────────────────────────────────────────────────────────────
        [Description("The name of the grade category as it will be displayed in the gradebook")]
        public string? GradeName { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_NAME;

        [Description("general information about the grade category; optional and will NOT be displayed in the gradebook")]
        public string? GradeInfo { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_INFO;

        [Description("the minimum amount of points that can be earned in this grade category | only applicable when using the 'Value' GradeType")]
        public float? MinGrade { get; set; } = GradeCategoryDefaults.DEFAULT_MINIMUM_GRADE;

        [Description("the maximum amount of points that can be earned in this grade category | only applicable when using the 'Value' GradeType")]
        public float MaxGrade { get; set; } = GradeCategoryDefaults.DEFAULT_MAXIMUM_GRADE;

        [Description("the minimum amount of points that has to be earned in this grade category to pass, null for no passing grade in this category")]
        public float? GradeToPass { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_TO_PASS;

        [Description("how activities in this Grade Category will be graded")]
        public GradeType GradeType { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_TYPE;

        [Description("the weight of this grade category in the grade calculation of its parent grade category in % of 100, null for default weight")]
        public int? GradingWeight { get; set; } = GradeCategoryDefaults.DEFAULT_GRADING_WEIGHT;

        [Description("specifies the number of decimal points to display for the grades in this Grade Category | has no effect on grade calculations, which are made with an accuracy of 5 decimal places")]
        public int? GradeDecimalPoints { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_DECIMAL_POINTS;

        [Description("Factor by which the grades in this Grade Category will be multiplied, with a maximum value of the maximum grade | For example, if the multiplicator is 2 and the maximum grade is 100, then all grades less than 50 are doubled, and all grades 50 and above are changed to 100")]
        public float? GradeMultiplicator { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_MULTIPLICATOR;

        [Description("Number that will be added to every grade in this Grade Category, after the Multiplicator is applied. (can be negative)")]
        public float? GradeOffset { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_OFFSET;

        [Description("whether the grades in this Grade Category should be hidden from students in the gradebook")]
        public bool GradeHidden { get; set; } = GradeCategoryDefaults.DEFAULT_HIDE_GRADE;

        [Description("An optional date after which the Grade Category will no longer be hidden in the gradebook (i.e. a grades release date)")]
        public DateTime? GradeHiddenUntil { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_HIDDEN_UNTIL;

        [Description("Whether or not to lock the grading for this Grade Category. If enabled, grades in this Grade Category can no longer be updated by the activities in it")]
        public bool GradingLocked { get; set; } = GradeCategoryDefaults.DEFAULT_LOCKED_GRADE;

        [Description("A date after which the locking will be effective, usually after the grades release date.")]
        public DateTime? GradingLockedAfter { get; set; } = GradeCategoryDefaults.DEFAULT_GRADE_LOCKED_UNTIL;
    }
}
