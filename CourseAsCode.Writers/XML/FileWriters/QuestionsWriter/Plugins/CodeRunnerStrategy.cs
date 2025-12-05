using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Interfaces;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Plugins
{
    public class CodeRunnerStrategy : IQuestionStrategy
    {
        private readonly ISequentialIdGenerator _sequentialIdGenerator;

        /// <summary>
        /// Creates a new instance of the strategy for writing CodeRunner questions
        /// </summary>
        /// <param name="logger">Logger instance specific to CodeRunnerPluginWriter</param>
        /// <param name="sequentialIdGenerator">Sequential ID generator for generating unique, ascending IDs</param>
        public CodeRunnerStrategy(ILogger<CodeRunnerStrategy> logger, ISequentialIdGenerator sequentialIdGenerator)
        {
            _sequentialIdGenerator = sequentialIdGenerator ?? throw new ArgumentNullException(nameof(sequentialIdGenerator));
        }

        /// <summary>
        /// Writes the `plugin_qtype_coderunner_question` element that is used for CodeRunner questions
        /// </summary>
        /// <param name="xmlService">The XML Service to use to write XML elements</param>
        /// <param name="question">the question to save</param>
        public void WritePluginData(IXmlService xmlService, Question question)
        {
            if (question is not CodeRunnerQuestion codeRunnerQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(CodeRunnerStrategy)}. Expected {nameof(CodeRunnerQuestion)} but got {question.GetType().Name}.");
            }

            xmlService.WriteElement("plugin_qtype_coderunner_question");

            // Write coderunner options
            xmlService.WriteElement("coderunner_options");
            int optionId = _sequentialIdGenerator.GetNextCodeRunnerOptionId();

            xmlService.WriteElementWithAttributes("coderunner_option", new Dictionary<string, string>
            {
                { "id", optionId.ToString() },
            });

            var optionElements = new Dictionary<string, string>
            {
                { "coderunnertype", codeRunnerQuestion.CodeRunnerType },
                { "prototypetype", codeRunnerQuestion.PrototypeType.ToString() },
                { "allornothing", codeRunnerQuestion.AllOrNothing ? "1" : "0" },
                { "penaltyregime", codeRunnerQuestion.PenaltyRegime },
                { "precheck", codeRunnerQuestion.Precheck.ToString() },
                { "hidecheck", codeRunnerQuestion.HideCheck.ToString() },
                { "showsource", codeRunnerQuestion.ShowSource.ToString() },
                { "answerboxlines", codeRunnerQuestion.AnswerBoxLines.ToString() },
                { "answerboxcolumns", codeRunnerQuestion.AnswerBoxColumns.ToString() },
                { "answerpreload", codeRunnerQuestion.AnswerPreload },
                { "globalextra", codeRunnerQuestion.GlobalExtra },
                { "useace", "$@NULL@$" },
                { "resultcolumns", "$@NULL@$" },
                { "template", "$@NULL@$" },
                { "iscombinatortemplate", "$@NULL@$" },
                { "allowmultiplestdins", "$@NULL@$" },
                { "answer", codeRunnerQuestion.Answer },
                { "validateonsave", codeRunnerQuestion.ValidateOnSave ? "1" : "0" },
                { "testsplitterre", "$@NULL@$" },
                { "language", "$@NULL@$" },
                { "acelang", "$@NULL@$" },
                { "sandbox", "$@NULL@$" },
                { "grader", "$@NULL@$" },
                { "cputimelimitsecs", "$@NULL@$" },
                { "memlimitmb", "$@NULL@$" },
                { "sandboxparams", "$@NULL@$" },
                { "templateparams", codeRunnerQuestion.TemplateParams },
                { "hoisttemplateparams", codeRunnerQuestion.HoistTemplateParams ? "1" : "0" },
                { "extractcodefromjson", codeRunnerQuestion.ExtractCodeFromJson ? "1" : "0" },
                { "templateparamslang", codeRunnerQuestion.TemplateParamsLang },
                { "templateparamsevalpertry", codeRunnerQuestion.TemplateParamsEvalPerTry ? "1" : "0" },
                { "templateparamsevald", codeRunnerQuestion.TemplateParamsEvald },
                { "twigall", codeRunnerQuestion.TwigAll ? "1" : "0" },
                { "uiplugin", "$@NULL@$" },
                { "uiparameters", codeRunnerQuestion.UiParameters },
                { "attachments", codeRunnerQuestion.Attachments.ToString() },
                { "attachmentsrequired", codeRunnerQuestion.AttachmentsRequired.ToString() },
                { "maxfilesize", codeRunnerQuestion.MaxFileSize.ToString() },
                { "filenamesregex", codeRunnerQuestion.FilenamesRegex },
                { "filenamesexplain", codeRunnerQuestion.FilenamesExplain },
                { "displayfeedback", codeRunnerQuestion.DisplayFeedback ? "1" : "0" },
                { "giveupallowed", codeRunnerQuestion.GiveUpAllowed ? "1" : "0" },
                { "prototypeextra", "$@NULL@$" },
            };
            xmlService.WriteElements(optionElements);

            xmlService.CloseElement(); // </coderunner_option>
            xmlService.CloseElement(); // </coderunner_options>

            // Write test cases
            xmlService.WriteElement("coderunner_testcases");
            foreach (var testCase in codeRunnerQuestion.TestCases)
            {
                int testCaseId = _sequentialIdGenerator.GetNextCodeRunnerTestCaseId();
                xmlService.WriteElementWithAttributes("coderunner_testcase", new Dictionary<string, string>
                {
                    { "id", testCaseId.ToString() },
                });

                var testCaseElements = new Dictionary<string, string>
                {
                    { "testcode", testCase.TestCode },
                    { "testtype", testCase.TestType.ToString() },
                    { "expected", testCase.Expected },
                    { "useasexample", testCase.UseAsExample ? "1" : "0" },
                    { "display", testCase.Display },
                    { "hiderestiffail", testCase.HideRestIfFail ? "1" : "0" },
                    { "mark", testCase.Mark.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) },
                    { "stdin", testCase.StdIn },
                    { "extra", testCase.Extra },
                };
                xmlService.WriteElements(testCaseElements);

                xmlService.CloseElement(); // </coderunner_testcase>
            }

            xmlService.CloseElement(); // </coderunner_testcases>
            xmlService.CloseElement(); // </plugin_qtype_coderunner_question>
        }

        public string GetQuestionTypeString()
        {
            return "coderunner";
        }
    }
}
