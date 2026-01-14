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
            var answerIdByIndex = new List<int>();
            xmlService.WriteElement("answers");
            for (int i = 0; i < calcMulti.Answers.Count; i++)
            {
                var answer = calcMulti.Answers[i];
                int answerId = _sequentialIdGenerator.GetNextAnswerId();
                answerIdByIndex.Add(answerId);

                xmlService.WriteElementWithAttributes("answer", new Dictionary<string, string>
                {
                    { "id", answerId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "answertext", answer.AnswerText },
                    { "answerformat", _generalHelper.EnumIntegerToString(answer.AnswerTextFormat) },
                    { "fraction", answer.Fraction.ToString("0.0000000", CultureInfo.InvariantCulture) },
                    { "feedback", answer.Feedback ?? string.Empty },
                    { "feedbackformat", _generalHelper.EnumIntegerToString(answer.FeedbackFormat) },
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
                    { "multiplier", unit.Multiplier.ToString("0.0000000", CultureInfo.InvariantCulture) },
                    { "unit", unit.UnitName },
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
                { "showunits", calcMulti.NumericalOptions.ShowUnits.ToString() },
                { "unitsleft", calcMulti.NumericalOptions.UnitsLeft ? "1" : "0" },
                { "unitgradingtype", calcMulti.NumericalOptions.UnitGradingType.ToString() },
                { "unitpenalty", calcMulti.NumericalOptions.UnitPenalty.ToString("0.0000000", CultureInfo.InvariantCulture) },
            });
            xmlService.CloseElement(); // </numerical_option>
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
                    { "category", def.Category == 0 ? "0" : question.CategoryId.ToString() },
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
            for (int i = 0; i < calcMulti.CalculatedRecords.Count; i++)
            {
                var rec = calcMulti.CalculatedRecords[i];
                int recId = _sequentialIdGenerator.GetNextAnswerId();

                string linkedAnswerId = (i < answerIdByIndex.Count) ? answerIdByIndex[i].ToString() : "0";

                xmlService.WriteElementWithAttributes("calculated_record", new Dictionary<string, string>
                {
                    { "id", recId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "answer", linkedAnswerId },
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
