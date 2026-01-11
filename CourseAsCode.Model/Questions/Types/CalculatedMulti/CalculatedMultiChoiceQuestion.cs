using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;

namespace CourseAsCode.Model.Questions.Types.CalculatedMulti
{
    public class CalculatedMultiChoiceQuestion : Question
    {
        [Description("List of answer choices (with calculated expressions and fractions)")]
        public List<CalculatedHelper.CalculatedAnswer> Answers { get; set; } = new();

        [Description("Numerical units for answers")]
        public List<CalculatedHelper.NumericalUnit> NumericalUnits { get; set; } = new();

        [Description("Numerical options block")]
        public CalculatedHelper.NumericalOption NumericalOptions { get; set; } = new();

        [Description("Dataset variables used in expressions")]
        public List<CalculatedHelper.DatasetDefinition> DatasetDefinitions { get; set; } = new();

        [Description("Calculated records referencing answers with tolerance settings")]
        public List<CalculatedHelper.CalculatedRecord> CalculatedRecords { get; set; } = new();

        [Description("Options for calculated multichoice")]
        public CalculatedHelper.CalculatedOption CalculatedOptions { get; set; } = new();
    }
}
