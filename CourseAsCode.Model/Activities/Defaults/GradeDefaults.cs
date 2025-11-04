using System.Collections.Generic;

namespace CourseAsCode.Model.Activities.Defaults
{
    public static class GradeDefaults
    {
        // Grade Item Defaults
        public const int DEFAULT_ID = 1;
        public const int DEFAULT_CATEGORY_ID = 0;
        public const string DEFAULT_ITEM_NAME = "";
        public const string DEFAULT_ITEM_TYPE = "";
        public const string DEFAULT_ITEM_MODULE = "";
        public const int DEFAULT_ITEM_INSTANCE = 0;
        public const int DEFAULT_ITEM_NUMBER = 0;
        public const string? DEFAULT_ITEM_INFO = "$@NULL@$";
        public const string? DEFAULT_ID_NUMBER = "";
        public const string? DEFAULT_CALCULATION = "$@NULL@$";
        public const int DEFAULT_GRADE_TYPE = 1;
        public const string DEFAULT_GRADE_MAX = "100.00000";
        public const string DEFAULT_GRADE_MIN = "0.00000";
        public const string? DEFAULT_SCALE_ID = "$@NULL@$";
        public const string? DEFAULT_OUTCOME_ID = "$@NULL@$";
        public const string DEFAULT_GRADE_PASS = "0.00000";
        public const string DEFAULT_MULT_FACTOR = "1.00000";
        public const string DEFAULT_PLUS_FACTOR = "0.00000";
        public const string DEFAULT_AGGREGATION_COEF = "0.00000";
        public const string DEFAULT_AGGREGATION_COEF2 = "0.14286";
        public const int DEFAULT_WEIGHT_OVERRIDE = 1;
        public const int DEFAULT_SORT_ORDER = 0;
        public const int DEFAULT_DISPLAY = 0;
        public const string? DEFAULT_DECIMALS = "$@NULL@$";
        public const int DEFAULT_HIDDEN = 0;
        public const int DEFAULT_LOCKED = 0;
        public const int DEFAULT_LOCK_TIME = 0;
        public const int DEFAULT_NEEDS_UPDATE = 0;
        public const long DEFAULT_TIME_CREATED = 0;
        public const long DEFAULT_TIME_MODIFIED = 0;

        // GradeGrade Defaults
        public const int DEFAULT_GRADE_GRADE_ID = 1;
        public const int DEFAULT_USER_ID = 0;
        public const string? DEFAULT_RAW_GRADE = "$@NULL@$";
        public const string DEFAULT_RAW_GRADE_MAX = "100.00000";
        public const string DEFAULT_RAW_GRADE_MIN = "0.00000";
        public const string? DEFAULT_RAW_SCALE_ID = "$@NULL@$";
        public const string? DEFAULT_USER_MODIFIED = "$@NULL@$";
        public const string? DEFAULT_FINAL_GRADE = "$@NULL@$";
        public const int DEFAULT_EXPORTED = 0;
        public const int DEFAULT_OVERRIDDEN = 0;
        public const int DEFAULT_EXCLUDED = 0;
        public const string? DEFAULT_FEEDBACK = "$@NULL@$";
        public const int DEFAULT_FEEDBACK_FORMAT = 2;
        public const string? DEFAULT_INFORMATION = "$@NULL@$";
        public const int DEFAULT_INFORMATION_FORMAT = 2;
        public const string? DEFAULT_TIME_CREATED_GRADE = "$@NULL@$";
        public const string? DEFAULT_TIME_MODIFIED_GRADE = "$@NULL@$";
        public const string DEFAULT_AGGREGATION_STATUS = "used";
        public const string DEFAULT_AGGREGATION_WEIGHT = "0.14286";

        public static Grade GetDefaultGrade()
        {
            return new Grade
            {
                Id = DEFAULT_ID,
                CategoryId = DEFAULT_CATEGORY_ID,
                ItemName = DEFAULT_ITEM_NAME,
                ItemType = DEFAULT_ITEM_TYPE,
                ItemModule = DEFAULT_ITEM_MODULE,
                ItemInstance = DEFAULT_ITEM_INSTANCE,
                ItemNumber = DEFAULT_ITEM_NUMBER,
                ItemInfo = DEFAULT_ITEM_INFO,
                IdNumber = DEFAULT_ID_NUMBER,
                Calculation = DEFAULT_CALCULATION,
                GradeType = DEFAULT_GRADE_TYPE,
                GradeMax = DEFAULT_GRADE_MAX,
                GradeMin = DEFAULT_GRADE_MIN,
                ScaleId = DEFAULT_SCALE_ID,
                OutcomeId = DEFAULT_OUTCOME_ID,
                GradePass = DEFAULT_GRADE_PASS,
                MultFactor = DEFAULT_MULT_FACTOR,
                PlusFactor = DEFAULT_PLUS_FACTOR,
                AggregationCoef = DEFAULT_AGGREGATION_COEF,
                AggregationCoef2 = DEFAULT_AGGREGATION_COEF2,
                WeightOverride = DEFAULT_WEIGHT_OVERRIDE,
                SortOrder = DEFAULT_SORT_ORDER,
                Display = DEFAULT_DISPLAY,
                Decimals = DEFAULT_DECIMALS,
                Hidden = DEFAULT_HIDDEN,
                Locked = DEFAULT_LOCKED,
                LockTime = DEFAULT_LOCK_TIME,
                NeedsUpdate = DEFAULT_NEEDS_UPDATE,
                TimeCreated = DEFAULT_TIME_CREATED.ToString(),
                TimeModified = DEFAULT_TIME_MODIFIED.ToString(),
                GradeGrades = new List<Grade.GradeGrade>
                {
                    new Grade.GradeGrade
                    {
                        Id = DEFAULT_GRADE_GRADE_ID,
                        UserId = DEFAULT_USER_ID,
                        RawGrade = DEFAULT_RAW_GRADE,
                        RawGradeMax = DEFAULT_RAW_GRADE_MAX,
                        RawGradeMin = DEFAULT_RAW_GRADE_MIN,
                        RawScaleId = DEFAULT_RAW_SCALE_ID,
                        UserModified = DEFAULT_USER_MODIFIED,
                        FinalGrade = DEFAULT_FINAL_GRADE,
                        Hidden = DEFAULT_HIDDEN,
                        Locked = DEFAULT_LOCKED,
                        LockTime = DEFAULT_LOCK_TIME,
                        Exported = DEFAULT_EXPORTED,
                        Overridden = DEFAULT_OVERRIDDEN,
                        Excluded = DEFAULT_EXCLUDED,
                        Feedback = DEFAULT_FEEDBACK,
                        FeedbackFormat = DEFAULT_FEEDBACK_FORMAT,
                        Information = DEFAULT_INFORMATION,
                        InformationFormat = DEFAULT_INFORMATION_FORMAT,
                        TimeCreated = DEFAULT_TIME_CREATED_GRADE,
                        TimeModified = DEFAULT_TIME_MODIFIED_GRADE,
                        AggregationStatus = DEFAULT_AGGREGATION_STATUS,
                        AggregationWeight = DEFAULT_AGGREGATION_WEIGHT
                    }
                }
            };
        }

        public static readonly List<Grade> DEFAULT_GRADES = new()
        {
            GetDefaultGrade()
        };

        public static int DEFAULT_GRADE_ITEM_ID { get; internal set; }
    }
}
