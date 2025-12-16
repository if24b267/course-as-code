using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.Calculated
{
    public class CalculatedQuestion : Question
    {
        public CalculatedQuestion()
        {
            Answers = new List<CalculatedAnswer>();
            NumericalOptions = new NumericalOption();
            CalculatedOptions = new CalculatedOption();
            DatasetDefinitions = new List<DatasetDefinition>();
            CalculatedRecords = new List<CalculatedRecord>();
        }

        [Description("List of acceptable answers (expressions referencing dataset variables)")]
        public List<CalculatedAnswer> Answers { get; set; }

        // Numerical units block (left empty like sample; can be extended later)
        [Description("Numerical units (left empty when not using units)")]
        public List<NumericalUnit> NumericalUnits { get; set; } = new();

        // Numerical options block
        public class NumericalOption
        {
            [Description("Where to show units: 0 hidden, 1 optional, 2 required, 3 not used (sample uses 3)")]
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
            [Description("Unit string, e.g. cm2")]
            public string UnitName { get; set; } = string.Empty;
            [Description("Multiplier associated with unit")]
            public float Multiplier { get; set; } = 1.0f;
        }
        [Description("Numerical options for units handling")]
        public NumericalOption NumericalOptions { get; set; }

        // Dataset definitions
        public class DatasetItem
        {
            [Description("Sequence number of the dataset item")]
            public int Number { get; set; }
            [Description("The value for this item")]
            public float Value { get; set; }
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
            [Description("Items belonging to the dataset")]
            public List<DatasetItem> Items { get; set; } = new();
        }
        [Description("Definitions of datasets used by the question")]
        public List<DatasetDefinition> DatasetDefinitions { get; set; }

        // Calculated records (link answers with tolerance)
        public class CalculatedRecord
        {
            [Description("Answer ID reference written by writer")]
            public int AnswerRefId { get; set; }
            [Description("Tolerance for numeric matching")]
            public float Tolerance { get; set; } = 0.01f;
            [Description("Tolerance type (2 = relative, 1 = nominal, 3 = percentage)")]
            public int ToleranceType { get; set; } = 2;
            [Description("Correct answer length (number of decimals)")]
            public int CorrectAnswerLength { get; set; } = 2;
            [Description("Correct answer format (1 = decimals)")]
            public int CorrectAnswerFormat { get; set; } = 1;
        }
        [Description("Calculated records referencing answers with tolerance settings")]
        public List<CalculatedRecord> CalculatedRecords { get; set; }

        // Calculated options (similar to multichoice section)
        public class CalculatedOption
        {
            [Description("Synchronize datasets (0 = separate)")]
            public int Synchronize { get; set; } = 0;
            [Description("If true only a single answer may be selected (usually 0 for calculated)")]
            public int Single { get; set; } = 0;
            [Description("Shuffle display of answers")]
            public bool ShuffleAnswers { get; set; } = true;
            public string? CorrectFeedback { get; set; }
            public FormattingFormat CorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;
            public string? PartiallyCorrectFeedback { get; set; }
            public FormattingFormat PartiallyCorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;
            public string? IncorrectFeedback { get; set; }
            public FormattingFormat IncorrectFeedbackFormat { get; set; } = FormattingFormat.PlainText;
            [Description("Answer numbering style (e.g. 'abc')")]
            public string AnswerNumbering { get; set; } = "abc";
            [Description("Show number of correct answers hints")]
            public bool ShowNumCorrect { get; set; } = false;
        }
        [Description("Options section for calculated question")]
        public CalculatedOption CalculatedOptions { get; set; }
    }
}
