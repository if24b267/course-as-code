using System.Collections.Generic;
using System.ComponentModel;
using CourseAsCode.Model.Activities.Defaults;

namespace CourseAsCode.Model
{
    public class Grade
    {
        // --- Grade Item Properties ---
        [Description("Unique identifier for this grade item")]
        public int Id { get; set; } = GradeDefaults.DEFAULT_ID;

        [Description("Category ID for this grade item")]
        public int CategoryId { get; set; } = GradeDefaults.DEFAULT_CATEGORY_ID;

        [Description("Name of the grade item")]
        public string ItemName { get; set; } = GradeDefaults.DEFAULT_ITEM_NAME;

        [Description("Type of the grade item")]
        public string ItemType { get; set; } = GradeDefaults.DEFAULT_ITEM_TYPE;

        [Description("Module name for the grade item")]
        public string ItemModule { get; set; } = GradeDefaults.DEFAULT_ITEM_MODULE;

        [Description("Instance ID of the module")]
        public int ItemInstance { get; set; } = GradeDefaults.DEFAULT_ITEM_INSTANCE;

        [Description("Item number")]
        public int ItemNumber { get; set; } = GradeDefaults.DEFAULT_ITEM_NUMBER;

        [Description("Additional info for the grade item")]
        public string? ItemInfo { get; set; } = GradeDefaults.DEFAULT_ITEM_INFO;

        [Description("ID number for the grade item")]
        public string? IdNumber { get; set; } = GradeDefaults.DEFAULT_ID_NUMBER;

        [Description("Calculation formula for the grade item")]
        public string? Calculation { get; set; } = GradeDefaults.DEFAULT_CALCULATION;

        [Description("Type of grade (e.g. value, scale)")]
        public int GradeType { get; set; } = GradeDefaults.DEFAULT_GRADE_TYPE;

        [Description("Maximum grade value")]
        public string GradeMax { get; set; } = GradeDefaults.DEFAULT_GRADE_MAX;

        [Description("Minimum grade value")]
        public string GradeMin { get; set; } = GradeDefaults.DEFAULT_GRADE_MIN;

        [Description("Scale ID if using a scale")]
        public string? ScaleId { get; set; } = GradeDefaults.DEFAULT_SCALE_ID;

        [Description("Outcome ID if using an outcome")]
        public string? OutcomeId { get; set; } = GradeDefaults.DEFAULT_OUTCOME_ID;

        [Description("Grade required to pass")]
        public string GradePass { get; set; } = GradeDefaults.DEFAULT_GRADE_PASS;

        [Description("Multiplication factor for grade calculation")]
        public string MultFactor { get; set; } = GradeDefaults.DEFAULT_MULT_FACTOR;

        [Description("Addition factor for grade calculation")]
        public string PlusFactor { get; set; } = GradeDefaults.DEFAULT_PLUS_FACTOR;

        [Description("Aggregation coefficient")]
        public string AggregationCoef { get; set; } = GradeDefaults.DEFAULT_AGGREGATION_COEF;

        [Description("Second aggregation coefficient")]
        public string AggregationCoef2 { get; set; } = GradeDefaults.DEFAULT_AGGREGATION_COEF2;

        [Description("Weight override for aggregation")]
        public int WeightOverride { get; set; } = GradeDefaults.DEFAULT_WEIGHT_OVERRIDE;

        [Description("Sort order of the grade item")]
        public int SortOrder { get; set; } = GradeDefaults.DEFAULT_SORT_ORDER;

        [Description("Display type for the grade item")]
        public int Display { get; set; } = GradeDefaults.DEFAULT_DISPLAY;

        [Description("Number of decimals to display")]
        public string? Decimals { get; set; } = GradeDefaults.DEFAULT_DECIMALS;

        [Description("Whether this grade item is hidden")]
        public int Hidden { get; set; } = GradeDefaults.DEFAULT_HIDDEN;

        [Description("Whether this grade item is locked")]
        public int Locked { get; set; } = GradeDefaults.DEFAULT_LOCKED;

        [Description("Lock time (Unix timestamp)")]
        public int LockTime { get; set; } = GradeDefaults.DEFAULT_LOCK_TIME;

        [Description("Whether this grade item needs update")]
        public int NeedsUpdate { get; set; } = GradeDefaults.DEFAULT_NEEDS_UPDATE;

        [Description("Time created (Unix timestamp as string)")]
        public string TimeCreated { get; set; } = GradeDefaults.DEFAULT_TIME_CREATED.ToString();

        [Description("Time modified (Unix timestamp as string)")]
        public string TimeModified { get; set; } = GradeDefaults.DEFAULT_TIME_MODIFIED.ToString();

        // --- Grade Grades (child grades) ---
        [Description("List of grade_grade entries for this grade item")]
        public List<GradeGrade> GradeGrades { get; set; } = new();

        // --- GradeGrade (for grade_grades) ---
        public class GradeGrade
        {
            [Description("Unique identifier for this grade_grade entry")]
            public int Id { get; set; } = GradeDefaults.DEFAULT_GRADE_GRADE_ID;

            [Description("User ID to whom this grade belongs")]
            public int UserId { get; set; } = GradeDefaults.DEFAULT_USER_ID;

            [Description("The raw grade value before any calculation or scaling")]
            public string? RawGrade { get; set; } = GradeDefaults.DEFAULT_RAW_GRADE;

            [Description("Maximum possible raw grade for this item")]
            public string RawGradeMax { get; set; } = GradeDefaults.DEFAULT_RAW_GRADE_MAX;

            [Description("Minimum possible raw grade for this item")]
            public string RawGradeMin { get; set; } = GradeDefaults.DEFAULT_RAW_GRADE_MIN;

            [Description("ID of the scale used, if any")]
            public string? RawScaleId { get; set; } = GradeDefaults.DEFAULT_RAW_SCALE_ID;

            [Description("ID of the user who last modified this grade")]
            public string? UserModified { get; set; } = GradeDefaults.DEFAULT_USER_MODIFIED;

            [Description("Final grade after all calculations")]
            public string? FinalGrade { get; set; } = GradeDefaults.DEFAULT_FINAL_GRADE;

            [Description("Whether this grade is hidden from the user (0 = visible, 1 = hidden)")]
            public int Hidden { get; set; } = GradeDefaults.DEFAULT_HIDDEN;

            [Description("Whether this grade is locked (0 = unlocked, 1 = locked)")]
            public int Locked { get; set; } = GradeDefaults.DEFAULT_LOCKED;

            [Description("Timestamp when the grade was locked (Unix time)")]
            public int LockTime { get; set; } = GradeDefaults.DEFAULT_LOCK_TIME;

            [Description("Whether this grade has been exported (0 = no, 1 = yes)")]
            public int Exported { get; set; } = GradeDefaults.DEFAULT_EXPORTED;

            [Description("Whether this grade has been manually overridden (0 = no, 1 = yes)")]
            public int Overridden { get; set; } = GradeDefaults.DEFAULT_OVERRIDDEN;

            [Description("Whether this grade is excluded from aggregation (0 = included, 1 = excluded)")]
            public int Excluded { get; set; } = GradeDefaults.DEFAULT_EXCLUDED;

            [Description("Feedback text for this grade")]
            public string? Feedback { get; set; } = GradeDefaults.DEFAULT_FEEDBACK;

            [Description("Format of the feedback (e.g. 1 = HTML, 0 = plain text, 2 = Markdown)")]
            public int FeedbackFormat { get; set; } = GradeDefaults.DEFAULT_FEEDBACK_FORMAT;

            [Description("Additional information for this grade")]
            public string? Information { get; set; } = GradeDefaults.DEFAULT_INFORMATION;

            [Description("Format of the information field")]
            public int InformationFormat { get; set; } = GradeDefaults.DEFAULT_INFORMATION_FORMAT;

            [Description("Timestamp when the grade was created (Unix time or $@NULL@$)")]
            public string? TimeCreated { get; set; } = GradeDefaults.DEFAULT_TIME_CREATED_GRADE;

            [Description("Timestamp when the grade was last modified (Unix time or $@NULL@$)")]
            public string? TimeModified { get; set; } = GradeDefaults.DEFAULT_TIME_MODIFIED_GRADE;

            [Description("Status of aggregation for this grade (e.g. 'used')")]
            public string AggregationStatus { get; set; } = GradeDefaults.DEFAULT_AGGREGATION_STATUS;

            [Description("Weight of this grade in aggregation")]
            public string AggregationWeight { get; set; } = GradeDefaults.DEFAULT_AGGREGATION_WEIGHT;
        }
    }
}
