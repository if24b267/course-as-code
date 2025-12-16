using System.Globalization;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.CalculatedSimple;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class CalculatedSimpleStrategy : IQuestionStrategy
    {
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;
        private readonly IQuizHelper _quizHelper;

        public CalculatedSimpleStrategy(ILogger<CalculatedSimpleStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator, IQuizHelper quizHelper)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
            _quizHelper = quizHelper ?? throw new ArgumentNullException(nameof(quizHelper));
        }

        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not SimpleCalculatedQuestion simple)
                throw new ArgumentException($"Incorrect question type passed to {nameof(CalculatedSimpleStrategy)}. Expected {nameof(SimpleCalculatedQuestion)} but got {question.GetType().Name}.");

            xmlService.WriteElement("plugin_qtype_calculatedsimple_question");

            // Answers ------------------------------------------------------------
            var generatedAnswerIds = new List<int>();
            xmlService.WriteElement("answers");
            foreach (var ans in simple.Answers)
            {
                int answerId = _sequentialIdGenerator.GetNextAnswerId();
                generatedAnswerIds.Add(answerId);

                xmlService.WriteElementWithAttributes("answer", new Dictionary<string, string>
                {
                    { "id", answerId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "answertext", ans.AnswerText },
                    { "answerformat", _generalHelper.EnumIntegerToString(ans.AnswerTextFormat) },
                    { "fraction", ans.Fraction.ToString("0.0000000", CultureInfo.InvariantCulture) },
                    { "feedback", ans.Feedback ?? string.Empty },
                    { "feedbackformat", _generalHelper.EnumIntegerToString(ans.FeedbackFormat) },
                });

                xmlService.CloseElement(); // </answer>
            }
            xmlService.CloseElement(); // </answers>

            // Numerical units ----------------------------------------------------
            xmlService.WriteElement("numerical_units");
            // Emit units if provided (sample empty)
            foreach (var unit in simple.NumericalUnits)
            {
                int unitId = _sequentialIdGenerator.GetNextAnswerId();
                xmlService.WriteElementWithAttributes("numerical_unit", new Dictionary<string, string>
                {
                    { "id", unitId.ToString() },
                });
                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "unit", unit.UnitName },
                    { "multiplier", unit.Multiplier.ToString("0.0000000", CultureInfo.InvariantCulture) },
                });
                xmlService.CloseElement(); // </numerical_unit>
            }
            xmlService.CloseElement(); // </numerical_units>

            // Numerical options --------------------------------------------------
            int numericalOptionId = _sequentialIdGenerator.GetNextAnswerId();
            xmlService.WriteElement("numerical_options");
            xmlService.WriteElementWithAttributes("numerical_option", new Dictionary<string, string>
            {
                { "id", numericalOptionId.ToString() },
            });
            xmlService.WriteElements(new Dictionary<string, string>
            {
                { "showunits", simple.NumericalOptions.ShowUnits.ToString() },
                { "unitsleft", simple.NumericalOptions.UnitsLeft ? "1" : "0" },
                { "unitgradingtype", simple.NumericalOptions.UnitGradingType.ToString() },
                { "unitpenalty", simple.NumericalOptions.UnitPenalty.ToString("0.0000000", CultureInfo.InvariantCulture) },
            });
            xmlService.CloseElement(); // </numerical_option>
            xmlService.CloseElement(); // </numerical_options>

            // Dataset definitions ------------------------------------------------
            xmlService.WriteElement("dataset_definitions");
            foreach (var def in simple.DatasetDefinitions)
            {
                int defId = _sequentialIdGenerator.GetNextAnswerId();
                xmlService.WriteElementWithAttributes("dataset_definition", new Dictionary<string, string>
                {
                    { "id", defId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "category", def.Category.ToString() },
                    { "name", def.Name },
                    { "type", def.Type.ToString() },
                    { "options", def.Options },
                    { "itemcount", def.Items.Count.ToString() },
                });

                xmlService.WriteElement("dataset_items");
                foreach (var item in def.Items)
                {
                    int itemId = _sequentialIdGenerator.GetNextAnswerId();
                    xmlService.WriteElementWithAttributes("dataset_item", new Dictionary<string, string>
                    {
                        { "id", itemId.ToString() },
                    });
                    xmlService.WriteElements(new Dictionary<string, string>
                    {
                        { "number", item.Number.ToString() },
                        { "value", item.Value.ToString(CultureInfo.InvariantCulture) },
                    });
                    xmlService.CloseElement(); // </dataset_item>
                }
                xmlService.CloseElement(); // </dataset_items>

                xmlService.CloseElement(); // </dataset_definition>
            }
            xmlService.CloseElement(); // </dataset_definitions>

            // Calculated records -------------------------------------------------
            xmlService.WriteElement("calculated_records");
            foreach (var rec in simple.CalculatedRecords)
            {
                int recId = _sequentialIdGenerator.GetNextAnswerId();
                xmlService.WriteElementWithAttributes("calculated_record", new Dictionary<string, string>
                {
                    { "id", recId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "answer", rec.AnswerRefId.ToString() },
                    { "tolerance", rec.Tolerance.ToString("0.00", CultureInfo.InvariantCulture) },
                    { "tolerancetype", rec.ToleranceType.ToString() },
                    { "correctanswerlength", rec.CorrectAnswerLength.ToString() },
                    { "correctanswerformat", rec.CorrectAnswerFormat.ToString() },
                });

                xmlService.CloseElement(); // </calculated_record>
            }
            xmlService.CloseElement(); // </calculated_records>

            // Calculated options -------------------------------------------------
            xmlService.WriteElement("calculated_options");
            // Sample shows empty block; if you populate options in model, emit key/values here.
            foreach (var kv in simple.CalculatedOptions)
            {
                // Emit as simple elements under calculated_options, if needed.
                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { kv.Key, kv.Value }
                });
            }
            xmlService.CloseElement(); // </calculated_options>

            xmlService.CloseElement(); // </plugin_qtype_calculatedsimple_question>
        }

        public string GetQuestionTypeString()
        {
            return "calculatedsimple";
        }
    }
}
