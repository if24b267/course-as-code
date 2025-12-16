using System.Globalization;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.CalculatedMulti;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class CalculatedMultiStrategy : IQuestionStrategy
    {
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;
        private readonly IQuizHelper _quizHelper;

        public CalculatedMultiStrategy(ILogger<CalculatedMultiStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator, IQuizHelper quizHelper)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
            _quizHelper = quizHelper ?? throw new ArgumentNullException(nameof(quizHelper));
        }

        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not CalculatedMultiChoiceQuestion calcMulti)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(CalculatedMultiStrategy)}. Expected {nameof(CalculatedMultiChoiceQuestion)} but got {question.GetType().Name}.");
            }

            xmlService.WriteElement("plugin_qtype_calculatedmulti_question");

            // Answers ------------------------------------------------------------
            var generatedAnswerIds = new List<int>();
            xmlService.WriteElement("answers");
            foreach (var ans in calcMulti.Answers)
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
            // Emit units if provided
            foreach (var unit in calcMulti.NumericalUnits)
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
            xmlService.WriteElement("numerical_options");
            if (calcMulti.NumericalOptions.ShowUnits.HasValue ||
                calcMulti.NumericalOptions.UnitsLeft.HasValue ||
                calcMulti.NumericalOptions.UnitGradingType.HasValue ||
                calcMulti.NumericalOptions.UnitPenalty.HasValue)
            {
                int numericalOptionId = _sequentialIdGenerator.GetNextAnswerId();
                xmlService.WriteElementWithAttributes("numerical_option", new Dictionary<string, string>
                {
                    { "id", numericalOptionId.ToString() },
                });
                var opt = new Dictionary<string, string>();
                if (calcMulti.NumericalOptions.ShowUnits.HasValue)
                    opt["showunits"] = calcMulti.NumericalOptions.ShowUnits.Value.ToString();
                if (calcMulti.NumericalOptions.UnitsLeft.HasValue)
                    opt["unitsleft"] = calcMulti.NumericalOptions.UnitsLeft.Value ? "1" : "0";
                if (calcMulti.NumericalOptions.UnitGradingType.HasValue)
                    opt["unitgradingtype"] = calcMulti.NumericalOptions.UnitGradingType.Value.ToString();
                if (calcMulti.NumericalOptions.UnitPenalty.HasValue)
                    opt["unitpenalty"] = calcMulti.NumericalOptions.UnitPenalty.Value.ToString("0.0000000", CultureInfo.InvariantCulture);

                xmlService.WriteElements(opt);
                xmlService.CloseElement(); // </numerical_option>
            }
            xmlService.CloseElement(); // </numerical_options>

            // Dataset definitions ------------------------------------------------
            xmlService.WriteElement("dataset_definitions");
            foreach (var def in calcMulti.DatasetDefinitions)
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
            foreach (var rec in calcMulti.CalculatedRecords)
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
            int calcOptId = _sequentialIdGenerator.GetNextAnswerId();
            xmlService.WriteElement("calculated_options");
            xmlService.WriteElementWithAttributes("calculated_option", new Dictionary<string, string>
            {
                { "id", calcOptId.ToString() },
            });

            xmlService.WriteElements(new Dictionary<string, string>
            {
                { "synchronize", calcMulti.CalculatedOptions.Synchronize.ToString() },
                { "single", calcMulti.CalculatedOptions.Single.ToString() },
                { "shuffleanswers", calcMulti.CalculatedOptions.ShuffleAnswers ? "1" : "0" },
                { "correctfeedback", calcMulti.CalculatedOptions.CorrectFeedback ?? string.Empty },
                { "correctfeedbackformat", _generalHelper.EnumIntegerToString(calcMulti.CalculatedOptions.CorrectFeedbackFormat) },
                { "partiallycorrectfeedback", calcMulti.CalculatedOptions.PartiallyCorrectFeedback ?? string.Empty },
                { "partiallycorrectfeedbackformat", _generalHelper.EnumIntegerToString(calcMulti.CalculatedOptions.PartiallyCorrectFeedbackFormat) },
                { "incorrectfeedback", calcMulti.CalculatedOptions.IncorrectFeedback ?? string.Empty },
                { "incorrectfeedbackformat", _generalHelper.EnumIntegerToString(calcMulti.CalculatedOptions.IncorrectFeedbackFormat) },
                { "answernumbering", calcMulti.CalculatedOptions.AnswerNumbering },
                { "shownumcorrect", calcMulti.CalculatedOptions.ShowNumCorrect ? "1" : "0" },
            });

            xmlService.CloseElement(); // </calculated_option>
            xmlService.CloseElement(); // </calculated_options>

            xmlService.CloseElement(); // </plugin_qtype_calculatedmulti_question>
        }

        public string GetQuestionTypeString()
        {
            return "calculatedmulti";
        }
    }
}
