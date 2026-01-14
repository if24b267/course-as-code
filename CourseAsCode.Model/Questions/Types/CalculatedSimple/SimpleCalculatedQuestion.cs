using System.ComponentModel;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions.Types.CalculatedSimple
{
    public class SimpleCalculatedQuestion : Question
    {
        [Description("List of answers with expressions referencing dataset variables")]
        public List<CalculatedHelper.CalculatedAnswer> Answers { get; set; } = new();

        [Description("Numerical units (unused by default)")]
        public List<CalculatedHelper.NumericalUnit> NumericalUnits { get; set; } = new();

        [Description("Numerical options configuration")]
        public CalculatedHelper.NumericalOption NumericalOptions { get; set; } = new();

        [Description("Dataset definitions used by expressions")]
        public List<CalculatedHelper.DatasetDefinition> DatasetDefinitions { get; set; } = new();

        [Description("Calculated records per answer")]
        public List<CalculatedHelper.CalculatedRecord> CalculatedRecords { get; set; } = new();

        // Simple calculated options block (empty in sample; keep for future use)
        [Description("Options for simple calculated question (empty by default)")]
        public Dictionary<string, string> CalculatedOptions { get; set; } = new();
    }
}
