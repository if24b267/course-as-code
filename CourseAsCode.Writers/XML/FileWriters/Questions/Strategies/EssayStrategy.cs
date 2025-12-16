using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.Essay;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.Questions.Strategies
{
    public class EssayStrategy : IQuestionStrategy
    {
        private readonly ILogger<EssayStrategy> _logger;
        private readonly IGeneralHelper _generalHelper;
        private readonly ISequentialIdGenerator _sequentialIdGenerator;

        /// <summary>
        /// Creates a new instance of the strategy for writing Drag and Drop onto Text questions
        /// </summary>
        /// <param name="logger">Logger instance specific to <see cref="EssayStrategy"/></param>
        /// <param name="generalHelper">General Helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Sequential ID generator for generating unique, ascending IDs</param>
        public EssayStrategy(ILogger<EssayStrategy> logger, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _generalHelper = generalHelper ?? throw new ArgumentNullException(nameof(generalHelper));
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
        }


        /// <summary>
        /// Writes the `plugin_qtype_essay_question` element for Essay questions
        /// </summary>
        /// <param name="xmlService">The XML Service to use to write XML elements</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not EssayQuestion essayQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(EssayStrategy)}. Expected {nameof(EssayQuestion)} but got {question.GetType().Name}.");
            }

            xmlService.WriteElement("plugin_qtype_essay_question");

            // Write Essay Options -----------------------------------------------------
            int essayId = _sequentialIdGenerator.GetNextAnswerId(); // Reuse answer ID counter for simplicity (avoids creating a new counter for just this element)
            xmlService.WriteElementWithAttributes("essay", new Dictionary<string, string>
            {
                { "id", essayId.ToString() },
            });

            var essayElements = new Dictionary<string, string>
            {
                { "responseformat", essayQuestion.ResponseFormat },
                { "responserequired", essayQuestion.ResponseRequired ? "1" : "0" },
                { "responsefieldlines",  essayQuestion.ResponseFieldLines.ToString() },
                { "minwordlimit", essayQuestion.MinWordLimit.HasValue ? essayQuestion.MinWordLimit.Value.ToString() : ""},
                { "maxwordlimit", essayQuestion.MaxWordLimit.HasValue ? essayQuestion.MaxWordLimit.Value.ToString() : "" },
                { "attachments", essayQuestion.AttachmentsAllowed.ToString() },
                { "attachmentsrequired", essayQuestion.AttachmentsRequired.ToString() },
                { "graderinfo", essayQuestion.GraderInfo ?? string.Empty },
                { "graderinfoformat", _generalHelper.EnumIntegerToString(essayQuestion.GraderInfoFormat) },
                { "responsetemplate", essayQuestion.ResponseTemplate ?? string.Empty },
                { "responsetemplateformat", _generalHelper.EnumIntegerToString(essayQuestion.ResponseTemplateFormat) },
                { "filetypeslist", essayQuestion.FileTypesList ?? string.Empty },
                { "maxbytes", essayQuestion.MaxBytes.ToString() }
            };

            xmlService.WriteElements(essayElements);

            xmlService.CloseElement(); // </essay>
            xmlService.CloseElement(); // </plugin_qtype_essay_question>
        }


        public string GetQuestionTypeString()
        {
            return "essay";
        }
    }
}
