using System.Collections.Generic;
using System.ComponentModel;
using CourseAsCode.Model.Activities.Defaults;

namespace CourseAsCode.Model.Activities
{
    public class Grade
    {
        public int Id { get; set; } = GradeDefaults.DEFAULT_ID;
        public int CategoryId { get; set; } = GradeDefaults.DEFAULT_CATEGORY_ID;
        public string ItemName { get; set; } = GradeDefaults.DEFAULT_ITEM_NAME;
        public string ItemType { get; set; } = GradeDefaults.DEFAULT_ITEM_TYPE;
        public string ItemModule { get; set; } = GradeDefaults.DEFAULT_ITEM_MODULE;
        public int ItemInstance { get; set; } = GradeDefaults.DEFAULT_ITEM_INSTANCE;
        public int ItemNumber { get; set; } = GradeDefaults.DEFAULT_ITEM_NUMBER;
        public string? ItemInfo { get; set; } = GradeDefaults.DEFAULT_ITEM_INFO;
        public string? IdNumber { get; set; } = GradeDefaults.DEFAULT_ID_NUMBER;
        public string? Calculation { get; set; } = GradeDefaults.DEFAULT_CALCULATION;
        public int GradeType { get; set; } = GradeDefaults.DEFAULT_GRADE_TYPE;
        public string GradeMax { get; set; } = GradeDefaults.DEFAULT_GRADE_MAX;
        public string GradeMin { get; set; } = GradeDefaults.DEFAULT_GRADE_MIN;
        public string? ScaleId { get; set; } = GradeDefaults.DEFAULT_SCALE_ID;
        public string? OutcomeId { get; set; } = GradeDefaults.DEFAULT_OUTCOME_ID;
        public string GradePass { get; set; } = GradeDefaults.DEFAULT_GRADE_PASS;
        public string MultFactor { get; set; } = GradeDefaults.DEFAULT_MULT_FACTOR;
        public string PlusFactor { get; set; } = GradeDefaults.DEFAULT_PLUS_FACTOR;
        public string AggregationCoef { get; set; } = GradeDefaults.DEFAULT_AGGREGATION_COEF;
        public string AggregationCoef2 { get; set; } = GradeDefaults.DEFAULT_AGGREGATION_COEF2;
        public int WeightOverride { get; set; } = GradeDefaults.DEFAULT_WEIGHT_OVERRIDE;
        public int SortOrder { get; set; } = GradeDefaults.DEFAULT_SORT_ORDER;
        public int Display { get; set; } = GradeDefaults.DEFAULT_DISPLAY;
        public string? Decimals { get; set; } = GradeDefaults.DEFAULT_DECIMALS;
        public int Hidden { get; set; } = GradeDefaults.DEFAULT_HIDDEN;
        public int Locked { get; set; } = GradeDefaults.DEFAULT_LOCKED;
        public int LockTime { get; set; } = GradeDefaults.DEFAULT_LOCK_TIME;
        public int NeedsUpdate { get; set; } = GradeDefaults.DEFAULT_NEEDS_UPDATE;
        public long TimeCreated { get; set; } = GradeDefaults.DEFAULT_TIME_CREATED;
        public long TimeModified { get; set; } = GradeDefaults.DEFAULT_TIME_MODIFIED;

        public List<Grade> GradeGrades { get; set; } = new();

        public int UserId { get; set; } = GradeDefaults.DEFAULT_USER_ID;
        public string? RawGrade { get; set; } = GradeDefaults.DEFAULT_RAW_GRADE;
        public string RawGradeMax { get; set; } = GradeDefaults.DEFAULT_RAW_GRADE_MAX;
        public string RawGradeMin { get; set; } = GradeDefaults.DEFAULT_RAW_GRADE_MIN;
        public string? RawScaleId { get; set; } = GradeDefaults.DEFAULT_RAW_SCALE_ID;
        public string? UserModified { get; set; } = GradeDefaults.DEFAULT_USER_MODIFIED;
        public string? FinalGrade { get; set; } = GradeDefaults.DEFAULT_FINAL_GRADE;
        public int Exported { get; set; } = GradeDefaults.DEFAULT_EXPORTED;
        public int Overridden { get; set; } = GradeDefaults.DEFAULT_OVERRIDDEN;
        public int Excluded { get; set; } = GradeDefaults.DEFAULT_EXCLUDED;
        public string? Feedback { get; set; } = GradeDefaults.DEFAULT_FEEDBACK;
        public int FeedbackFormat { get; set; } = GradeDefaults.DEFAULT_FEEDBACK_FORMAT;
        public string? Information { get; set; } = GradeDefaults.DEFAULT_INFORMATION;
        public int InformationFormat { get; set; } = GradeDefaults.DEFAULT_INFORMATION_FORMAT;
        public string? TimeCreatedGrade { get; set; } = GradeDefaults.DEFAULT_TIME_CREATED_GRADE;
        public string? TimeModifiedGrade { get; set; } = GradeDefaults.DEFAULT_TIME_MODIFIED_GRADE;
        public string AggregationStatus { get; set; } = GradeDefaults.DEFAULT_AGGREGATION_STATUS;
        public string AggregationWeight { get; set; } = GradeDefaults.DEFAULT_AGGREGATION_WEIGHT;
    }
}
