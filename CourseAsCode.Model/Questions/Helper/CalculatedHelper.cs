using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Helper
{
    /// <summary>
    /// Contains shared models used by all Calculated question types (Simple, Normal, MultiChoice).
    /// </summary>
    public class CalculatedHelper
    {
        public class NumericalOption
        {
            [Description("Where to show units: 0 hidden, 1 optional, 2 required, 3 not used")]
            public int ShowUnits { get; set; } = 3;

            [Description("If true units appear left of the value")]
            public bool UnitsLeft { get; set; } = false;

            [Description("Unit grading type")]
            public int UnitGradingType { get; set; } = 0;

            [Description("Penalty applied for wrong unit")]
            public float UnitPenalty { get; set; } = 0.1f;
        }

        public class NumericalUnit
        {
            [Description("Unit string, e.g. cm")]
            public string UnitName { get; set; } = string.Empty;

            [Description("Multiplier associated with unit")]
            public float Multiplier { get; set; } = 1.0f;
        }

        public class DatasetDefinition
        {
            [Description("Dataset category (0 = private to question)")]
            public int Category { get; set; } = 0;

            [Description("Variable name, e.g. b or h")]
            public required string Name { get; set; }

            [Description("Type (1 = private)")]
            public int Type { get; set; } = 1;

            [Description("Options string, e.g. uniform:1.0:10.0:1")]
            public required string Options { get; set; }

            public List<DatasetItem> Items { get; set; } = new();
        }

        public class DatasetItem
        {
            public int Number { get; set; }
            public float Value { get; set; }
        }

        public class CalculatedRecord
        {
            // Hint: AnswerRefId will be set intern, not in the YAML
            public int AnswerRefId { get; set; }

            public float Tolerance { get; set; } = 0.01f;
            public int ToleranceType { get; set; } = 2;
            public int CorrectAnswerLength { get; set; } = 2;
            public int CorrectAnswerFormat { get; set; } = 1;
        }

        public class CalculatedOption
        {
            public int Synchronize { get; set; } = 0;
            public int Single { get; set; } = 0;
            public bool ShuffleAnswers { get; set; } = true;
            public string? CorrectFeedback { get; set; }
            public FormattingFormat CorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;
            public string? PartiallyCorrectFeedback { get; set; }
            public FormattingFormat PartiallyCorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;
            public string? IncorrectFeedback { get; set; }
            public FormattingFormat IncorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;
            public string AnswerNumbering { get; set; } = "abc";
            public bool ShowNumCorrect { get; set; } = false;
        }
        public class CalculatedAnswer
        {
            public string AnswerText { get; set; } = string.Empty;
            public FormattingFormat AnswerTextFormat { get; set; } = FormattingFormat.PlainText;
            public float Fraction { get; set; } = 1.0f;
            public string? Feedback { get; set; }
            public FormattingFormat FeedbackFormat { get; set; } = FormattingFormat.PlainText;
        }
    }
}
