using System.Globalization;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.Calculated;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class CalculatedStrategy : IQuestionStrategy
    {
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;
        private readonly IQuizHelper _quizHelper;

        public CalculatedStrategy(ILogger<CalculatedStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator, IQuizHelper quizHelper)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
            _quizHelper = quizHelper ?? throw new ArgumentNullException(nameof(quizHelper));
        }

        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not CalculatedQuestion calcQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(CalculatedStrategy)}. Expected {nameof(CalculatedQuestion)} but got {question.GetType().Name}.");
            }

            xmlService.WriteElement("plugin_qtype_calculated_question");

            // Answers ------------------------------------------------------------
            var answerIdByIndex = new List<int>();
            xmlService.WriteElement("answers");
            for (int i = 0; i < calcQuestion.Answers.Count; i++)
            {
                var answer = calcQuestion.Answers[i];
                int answerId = _sequentialIdGenerator.GetNextAnswerId();
                answerIdByIndex.Add(answerId);

                xmlService.WriteElementWithAttributes("answer", new Dictionary<string, string>
                {
                    { "id", answerId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "answertext", answer.AnswerText },
                    { "answerformat", answer.AnswerTextFormat.ToString() },
                    { "fraction", answer.Fraction.ToString("0.0000000", CultureInfo.InvariantCulture) },
                    { "feedback", answer.Feedback ?? string.Empty },
                    { "feedbackformat", answer.FeedbackFormat.ToString() },
                });

                xmlService.CloseElement(); // </answer>
            }
            xmlService.CloseElement(); // </answers>

            // Numerical units ----------------------------------------------------
            xmlService.WriteElement("numerical_units");
            // If you need units, emit them here similar to answers.
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
                { "showunits", calcQuestion.NumericalOptions.ShowUnits.ToString() },
                { "unitsleft", calcQuestion.NumericalOptions.UnitsLeft ? "1" : "0" },
                { "unitgradingtype", calcQuestion.NumericalOptions.UnitGradingType.ToString() },
                { "unitpenalty", calcQuestion.NumericalOptions.UnitPenalty.ToString("0.0000000", CultureInfo.InvariantCulture) },
            });
            xmlService.CloseElement(); // </numerical_option>
            xmlService.CloseElement(); // </numerical_options>

            // Dataset definitions ------------------------------------------------
            xmlService.WriteElement("dataset_definitions");
            foreach (var def in calcQuestion.DatasetDefinitions)
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
            foreach (var rec in calcQuestion.CalculatedRecords)
            {
                int recId = _sequentialIdGenerator.GetNextAnswerId();
                xmlService.WriteElementWithAttributes("calculated_record", new Dictionary<string, string>
                {
                    { "id", recId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "answer", rec.AnswerRefId.ToString() },
                    { "tolerance", rec.Tolerance.ToString("0.00", CultureInfo.InvariantCulture) }, // sample uses 0.01 without 7 decimals
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
                { "synchronize", calcQuestion.CalculatedOptions.Synchronize.ToString() },
                { "single", calcQuestion.CalculatedOptions.Single.ToString() },
                { "shuffleanswers", calcQuestion.CalculatedOptions.ShuffleAnswers ? "1" : "0" },
                { "correctfeedback", calcQuestion.CalculatedOptions.CorrectFeedback ?? string.Empty },
                { "correctfeedbackformat", _generalHelper.EnumIntegerToString(calcQuestion.CalculatedOptions.CorrectFeedbackFormat) },
                { "partiallycorrectfeedback", calcQuestion.CalculatedOptions.PartiallyCorrectFeedback ?? string.Empty },
                { "partiallycorrectfeedbackformat", _generalHelper.EnumIntegerToString(calcQuestion.CalculatedOptions.PartiallyCorrectFeedbackFormat) },
                { "incorrectfeedback", calcQuestion.CalculatedOptions.IncorrectFeedback ?? string.Empty },
                { "incorrectfeedbackformat", _generalHelper.EnumIntegerToString(calcQuestion.CalculatedOptions.IncorrectFeedbackFormat) },
                { "answernumbering", calcQuestion.CalculatedOptions.AnswerNumbering },
                { "shownumcorrect", calcQuestion.CalculatedOptions.ShowNumCorrect ? "1" : "0" },
            });

            xmlService.CloseElement(); // </calculated_option>
            xmlService.CloseElement(); // </calculated_options>

            xmlService.CloseElement(); // </plugin_qtype_calculated_question>

            // Post-process: If user provided CalculatedRecords without AnswerRefId, auto-link to the first generated answer.
            // Note: We cannot write after closing, but recommend setting AnswerRefId in the model before calling writer.
            // For convenience, you can construct records like: new CalculatedRecord { AnswerRefId = answerIdByIndex[0], ... }
        }

        public string GetQuestionTypeString()
        {
            return "calculated";
        }
    }
}
