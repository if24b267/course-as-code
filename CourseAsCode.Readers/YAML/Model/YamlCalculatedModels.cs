using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlDatasetDefinition
    {
        public string Name { get; set; } = string.Empty;
        public int Category { get; set; } = 0;
        public int Type { get; set; } = 1;
        public string Options { get; set; } = string.Empty;
        public List<YamlDatasetItem> Items { get; set; } = new();
    }

    public class YamlDatasetItem
    {
        public int Number { get; set; }
        public float Value { get; set; }
    }

    public class YamlCalculatedRecord
    {
        public float Tolerance { get; set; } = 0.01f;
        public int ToleranceType { get; set; } = 1;
        public int CorrectAnswerLength { get; set; } = 2;
        public int CorrectAnswerFormat { get; set; } = 1;
    }

    public class YamlNumericalOption
    {
        public int ShowUnits { get; set; } = 3;
        public bool UnitsLeft { get; set; } = false;
        public int UnitGradingType { get; set; } = 0;
        public float UnitPenalty { get; set; } = 0.1f;
    }

    public class YamlNumericalUnit
    {
        public string UnitName { get; set; } = string.Empty;
        public float Multiplier { get; set; } = 1.0f;
    }
    public class YamlCalculatedOption
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
}
