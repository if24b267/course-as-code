using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions.Types.Calculated
{
    public class CalculatedQuestion : Question
    {
        [Description("List of acceptable answers (expressions referencing dataset variables)")]
        public List<CalculatedHelper.CalculatedAnswer> Answers { get; set; } = new();

        [Description("Numerical units (left empty when not using units)")]
        public List<CalculatedHelper.NumericalUnit> NumericalUnits { get; set; } = new();

        [Description("Numerical options for units handling")]
        public CalculatedHelper.NumericalOption NumericalOptions { get; set; } = new();

        [Description("Definitions of datasets used by the question")]
        public List<CalculatedHelper.DatasetDefinition> DatasetDefinitions { get; set; } = new();

        [Description("Calculated records referencing answers with tolerance settings")]
        public List<CalculatedHelper.CalculatedRecord> CalculatedRecords { get; set; } = new();

        [Description("Options section for calculated question")]
        public CalculatedHelper.CalculatedOption CalculatedOptions { get; set; } = new();
    }
}
