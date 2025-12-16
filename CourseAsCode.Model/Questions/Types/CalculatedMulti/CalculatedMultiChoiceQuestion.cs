using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.CalculatedMulti
{
    public class CalculatedMultiChoiceQuestion : Question
    {
        public CalculatedMultiChoiceQuestion()
        {
            Answers = new List<CalculatedMultiAnswer>();
            NumericalOptions = new NumericalOption();
            CalculatedOptions = new CalculatedOption();
            DatasetDefinitions = new List<DatasetDefinition>();
            CalculatedRecords = new List<CalculatedRecord>();
        }

        [Description("List of answer choices (with calculated expressions and fractions)")]
        public List<CalculatedMultiAnswer> Answers { get; set; }

        // Numerical units (kept empty by default, can be extended)
        [Description("Numerical units for answers")]
        public List<NumericalUnit> NumericalUnits { get; set; } = new();

        public class NumericalUnit
        {
            public string UnitName { get; set; } = string.Empty;
            public float Multiplier { get; set; } = 1.0f;
        }

        // Numerical options (block may be empty as in sample)
        public class NumericalOption
        {
            public int? ShowUnits { get; set; } // null means omit
            public bool? UnitsLeft { get; set; }
            public int? UnitGradingType { get; set; }
            public float? UnitPenalty { get; set; }
        }
        [Description("Numerical options block")]
        public NumericalOption NumericalOptions { get; set; }

        // Dataset definitions and items
        public class DatasetItem
        {
            public int Number { get; set; }
            public float Value { get; set; }
        }
        public class DatasetDefinition
        {
            public int Category { get; set; } = 0;
            public required string Name { get; set; }
            public int Type { get; set; } = 1;
            public required string Options { get; set; }
            public List<DatasetItem> Items { get; set; } = new();
        }
        [Description("Dataset variables used in expressions")]
        public List<DatasetDefinition> DatasetDefinitions { get; set; }

        // Calculated records (per answer tolerance/config)
        public class CalculatedRecord
        {
            [Description("Will be set to the generated answer ID by caller")]
            public int AnswerRefId { get; set; }
            public float Tolerance { get; set; } = 0.01f;
            public int ToleranceType { get; set; } = 1;
            public int CorrectAnswerLength { get; set; } = 2;
            public int CorrectAnswerFormat { get; set; } = 1;
        }
        [Description("Tolerance and formatting settings per answer choice")]
        public List<CalculatedRecord> CalculatedRecords { get; set; }

        // Calculated multichoice options
        public class CalculatedOption
        {
            public int Synchronize { get; set; } = 0;
            public int Single { get; set; } = 1; // 1 = single choice in sample
            public bool ShuffleAnswers { get; set; } = true;

            public string? CorrectFeedback { get; set; }
            public FormattingFormat CorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;
            public string? PartiallyCorrectFeedback { get; set; }
            public FormattingFormat PartiallyCorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;
            public string? IncorrectFeedback { get; set; }
            public FormattingFormat IncorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;

            public string AnswerNumbering { get; set; } = "abc";
            public bool ShowNumCorrect { get; set; } = true;
        }
        [Description("Options for calculated multichoice")]
        public CalculatedOption CalculatedOptions { get; set; }
    }
}
