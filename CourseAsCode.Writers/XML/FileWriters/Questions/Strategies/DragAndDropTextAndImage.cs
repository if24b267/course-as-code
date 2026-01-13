using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.DragAndDropTextAndImage;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class DragAndDropTextAndImageStrategy : IQuestionStrategy
    {
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;
        private readonly IQuizHelper _quizHelper;

        public DragAndDropTextAndImageStrategy(ILogger<DragAndDropTextAndImageStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator, IQuizHelper quizHelper)
        {
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
            _quizHelper = quizHelper ?? throw new ArgumentNullException(nameof(quizHelper));
        }

        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not DragAndDropTextAndImageQuestion ddimagortext)
                throw new ArgumentException($"Incorrect question type passed to {nameof(DragAndDropTextAndImageStrategy)}. Expected {nameof(DragAndDropTextAndImageQuestion)} but got {question.GetType().Name}.");

            xmlService.WriteElement("plugin_qtype_ddimageortext_question");

            // ddimageortext Options -----------------------------------------------------
            xmlService.WriteElementWithAttributes("ddimageortext", new Dictionary<string, string>
            {
                { "id", _sequentialIdGenerator.GetNextAnswerId().ToString() },
            });

            var ddimageortextElements = new Dictionary<string, string>
            {
                { "shuffleanswers", ddimagortext.ShuffleAnswers ? "1" : "0" },
                { "correctfeedback", ddimagortext.CorrectResponseFeedback },
                { "correctfeedbackformat", _generalHelper.EnumIntegerToString(ddimagortext.CorrectResponseFeedbackFormat) },
                { "partiallycorrectfeedback", ddimagortext.PartiallyCorrectResponseFeedback },
                { "partiallycorrectfeedbackformat", _generalHelper.EnumIntegerToString(ddimagortext.PartiallyCorrectResponseFeedbackFormat) },
                { "incorrectfeedback", ddimagortext.IncorrectResponseFeedback },
                { "incorrectfeedbackformat", _generalHelper.EnumIntegerToString(ddimagortext.IncorrectResponseFeedbackFormat) },
                { "shownumcorrect", ddimagortext.Hints?.Any(h => h.ShowNumberOfCorrectAnswers) ?? false ? "1" : "0" },
                { "dropzonevisibility", ddimagortext.DropZoneVisibility ? "1" : "0" },
            };
            xmlService.WriteElements(ddimageortextElements);

            xmlService.CloseElement(); // </ddimageortext>

            // Drags (Answer choices) ------------------------------------------------------------
            xmlService.WriteElement("drags");
            foreach (var drag in ddimagortext.Drags)
            {
                int dragId = _sequentialIdGenerator.GetNextAnswerId();

                xmlService.WriteElementWithAttributes("drag", new Dictionary<string, string>
                {
                    { "id", dragId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "no", drag.Number.ToString() },
                    { "draggroup",  drag.DragGroup.ToString() },
                    { "infinite", drag.Infinite ? "1" : "0" },
                    { "label", drag.Label },
                });

                xmlService.CloseElement(); // </drag>
            }
            xmlService.CloseElement(); // </drags>

            // Drops (Correct Answers) ------------------------------------------------------------
            xmlService.WriteElement("drops");
            foreach (var drop in ddimagortext.Drops)
            {
                int dropId = _sequentialIdGenerator.GetNextAnswerId();

                xmlService.WriteElementWithAttributes("drop", new Dictionary<string, string>
                {
                    { "id", dropId.ToString() },
                });

                xmlService.WriteElements(new Dictionary<string, string>
                {
                    { "no", drop.Number.ToString() },
                    { "xleft",  drop.XLeft.ToString() },
                    { "ytop",  drop.YTop.ToString() },
                    { "choice", drop.Choice.ToString() },
                    { "label", drop.Label },
                });

                xmlService.CloseElement(); // </drop>
            }
            xmlService.CloseElement(); // </drops>

            xmlService.CloseElement(); // </plugin_qtype_ddimageortext_question>
        }

        public string GetQuestionTypeString()
        {
            return "ddimageortext";
        }
    }
}
