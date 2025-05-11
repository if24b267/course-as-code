using System.Xml;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces;
using Microsoft.Extensions.Logging;

namespace CourseAsCode.Writers.XML.Writer.QuestionsWriter.Plugins
{
    public class CodeRunnerPluginWriter : XmlWriterBase, IQuestionPluginWriter
    {
        /// <summary>
        /// Creates a new instance of the CodeRunnerPluginWriter class
        /// </summary>
        /// <param name="xmlWriterFactory">Factory for creating XmlWriters</param>
        /// <param name="logger">Logger instance specific to CodeRunnerPluginWriter</param>
        public CodeRunnerPluginWriter(IXmlWriterFactory xmlWriterFactory, ILogger<CodeRunnerPluginWriter> logger)
            : base(xmlWriterFactory, logger)
        {
        }

        /// <summary>
        /// Writes the `plugin_qtype_coderunner_question` element that is used for CodeRunner questions
        /// </summary>
        /// <param name="writer">XmlWriter initialized on the correct document</param>
        /// <param name="question">the question to save</param>
        /// <param name="generalHelper">Instance of a general helper providing various helper methods</param>
        /// <param name="sequentialIdGenerator">Instance of a sequential ID generator for creating unique identifiers</param>
        public void WritePluginData(XmlWriter writer, Question question, IGeneralHelper generalHelper, ISequentialIdGenerator sequentialIdGenerator)
        {
            if (question is not CodeRunnerQuestion codeRunnerQuestion)
            {
                throw new ArgumentException($"Incorrect question type passed to {nameof(CodeRunnerPluginWriter)}. Expected {nameof(CodeRunnerQuestion)} but got {question.GetType().Name}.");
            }

            writer.WriteStartElement("plugin_qtype_coderunner_question");

            // Write coderunner options
            writer.WriteStartElement("coderunner_options");
            int optionId = sequentialIdGenerator.GetNextCodeRunnerOptionId();
            writer.WriteStartElement("coderunner_option");
            writer.WriteAttributeString("id", optionId.ToString());

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
            WriteElements(writer, optionElements);

            writer.WriteEndElement(); // </coderunner_option>
            writer.WriteEndElement(); // </coderunner_options>

            // Write test cases
            writer.WriteStartElement("coderunner_testcases");
            foreach (var testCase in codeRunnerQuestion.TestCases)
            {
                int testCaseId = sequentialIdGenerator.GetNextCodeRunnerTestCaseId();
                writer.WriteStartElement("coderunner_testcase");
                writer.WriteAttributeString("id", testCaseId.ToString());

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
                WriteElements(writer, testCaseElements);

                writer.WriteEndElement(); // </coderunner_testcase>
            }

            writer.WriteEndElement(); // </coderunner_testcases>

            writer.WriteEndElement(); // </plugin_qtype_coderunner_question>
        }

        public string GetQuestionTypeString()
        {
            return "coderunner";
        }
    }
}
