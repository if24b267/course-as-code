using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.CalculatedSimple
{
    public class SimpleCalculatedQuestion : Question
    {
        public SimpleCalculatedQuestion()
        {
            Answers = new List<SimpleCalculatedAnswer>();
            NumericalOptions = new NumericalOption();
            DatasetDefinitions = new List<DatasetDefinition>();
            CalculatedRecords = new List<CalculatedRecord>();
        }

        [Description("List of answers with expressions referencing dataset variables")]
        public List<SimpleCalculatedAnswer> Answers { get; set; }

        // Numerical units (kept empty by default)
        [Description("Numerical units (unused by default)")]
        public List<NumericalUnit> NumericalUnits { get; set; } = new();

        public class NumericalUnit
        {
            public string UnitName { get; set; } = string.Empty;
            public float Multiplier { get; set; } = 1.0f;
        }

        // Numerical options (single entry)
        public class NumericalOption
        {
            [Description("Where to show units (sample uses 3)")]
            public int ShowUnits { get; set; } = 3;
            [Description("Units to the left of the value")]
            public bool UnitsLeft { get; set; } = false;
            [Description("Unit grading type")]
            public int UnitGradingType { get; set; } = 0;
            [Description("Penalty for incorrect unit")]
            public float UnitPenalty { get; set; } = 0.1f;
        }
        [Description("Numerical options configuration")]
        public NumericalOption NumericalOptions { get; set; }

        // Dataset variable definitions and items
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
        [Description("Dataset definitions used by expressions")]
        public List<DatasetDefinition> DatasetDefinitions { get; set; }

        // Calculated record linking answer and tolerance
        public class CalculatedRecord
        {
            [Description("Answer ID reference (writer uses the generated ID)")]
            public int AnswerRefId { get; set; }
            public float Tolerance { get; set; } = 0.01f;
            public int ToleranceType { get; set; } = 1;
            public int CorrectAnswerLength { get; set; } = 2;
            public int CorrectAnswerFormat { get; set; } = 1;
        }
        [Description("Calculated records per answer")]
        public List<CalculatedRecord> CalculatedRecords { get; set; }

        // Simple calculated options block (empty in sample; keep for future use)
        [Description("Options for simple calculated question (empty by default)")]
        public Dictionary<string, string> CalculatedOptions { get; set; } = new();
    }
}
