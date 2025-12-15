using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Plugins;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.QuestionsWriter.Plugins
{
    [TestFixture]
    public class CodeRunnerPluginWriterTest
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<CodeRunnerPluginWriter> _logger;
        private IGeneralHelper _generalHelper;
        private ISequentialIdGenerator _idGenerator;
        private CodeRunnerPluginWriter _codeRunnerWriter;
        private XmlWriter _xmlWriter;

        private const string DEFAULT_FORMAT_MAPPED = "1";

        // Normalize platform line endings to LF for deterministic assertions
        private static string NormalizeLf(string? s) =>
            (s ?? string.Empty).Replace("\r\n", "\n").Replace("\r", "\n");

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest();
            _logger = Substitute.For<ILogger<CodeRunnerPluginWriter>>();

            // Mock helpers
            _generalHelper = Substitute.For<IGeneralHelper>();
            _generalHelper.EnumIntegerToString(FormattingFormat.HTML).Returns(DEFAULT_FORMAT_MAPPED);
            _generalHelper.EnumIntegerToString(FormattingFormat.PlainText).Returns("2");
            _generalHelper.EnumIntegerToString(FormattingFormat.Markdown).Returns("4");
            _generalHelper.EnumIntegerToString(Arg.Any<FormattingFormat>()).Returns(DEFAULT_FORMAT_MAPPED);

            // Mock ID generator
            int optionIdCounter = 1;
            int testCaseIdCounter = 1;
            _idGenerator = Substitute.For<ISequentialIdGenerator>();
            _idGenerator.GetNextCodeRunnerOptionId().Returns(_ => optionIdCounter++);
            _idGenerator.GetNextCodeRunnerTestCaseId().Returns(_ => testCaseIdCounter++);

            // Create the writer under test
            _codeRunnerWriter = new CodeRunnerPluginWriter(_testFactory, _logger);

            // Create a fresh StringWriter for each test
            _testFactory.ClearContents();

            // Create an XML writer
            _xmlWriter = _testFactory.Create("test.xml", new XmlWriterSettings());
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("root");
        }

        [TearDown]
        public void TearDown()
        {
            // Only close if not already closed in the test
            try
            {
                _xmlWriter.WriteEndElement();
                _xmlWriter.WriteEndDocument();
                _xmlWriter.Flush();
            }
            catch (InvalidOperationException)
            {
                // The XML writer might already be closed in the test
            }
            finally
            {
                // Always close the writer
                _xmlWriter?.Close();
            }
        }

        [Test]
        public void WritePluginData_CreatesCorrectStructure()
        {
            // Arrange
            var question = CreateBasicCodeRunnerQuestion();

            // Act
            _codeRunnerWriter.WritePluginData(_xmlWriter, question, _generalHelper, _idGenerator);

            // Close the XML elements before reading
            _xmlWriter.WriteEndElement(); // Close the root element
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();

            string output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Check main structure
            var pluginNode = doc.SelectSingleNode("//plugin_qtype_coderunner_question");
            Assert.IsNotNull(pluginNode, "CodeRunner plugin node not found");

            var optionsNode = pluginNode.SelectSingleNode("coderunner_options/coderunner_option");
            Assert.IsNotNull(optionsNode, "CodeRunner options node not found");

            var testCasesNode = pluginNode.SelectSingleNode("coderunner_testcases");
            Assert.IsNotNull(testCasesNode, "CodeRunner testcases node not found");
            Assert.That(testCasesNode.SelectNodes("coderunner_testcase")?.Count, Is.EqualTo(question.TestCases.Count));

            // Reset the XML writer for the next test
            _xmlWriter = _testFactory.Create("test.xml", new XmlWriterSettings());
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("root");
        }

        [Test]
        public void WritePluginData_WritesOptionsCorrectly()
        {
            // Arrange
            var question = CreateBasicCodeRunnerQuestion();

            // Act
            _codeRunnerWriter.WritePluginData(_xmlWriter, question, _generalHelper, _idGenerator);

            // Close the XML elements before reading
            _xmlWriter.WriteEndElement(); // Close the root element
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();

            string output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Check options content
            var optionNode = doc.SelectSingleNode("//coderunner_option");
            Assert.IsNotNull(optionNode);

            Assert.That(optionNode.SelectSingleNode("coderunnertype")?.InnerText, Is.EqualTo(question.CodeRunnerType));
            Assert.That(optionNode.SelectSingleNode("prototypetype")?.InnerText, Is.EqualTo(question.PrototypeType.ToString()));
            Assert.That(optionNode.SelectSingleNode("allornothing")?.InnerText, Is.EqualTo(question.AllOrNothing ? "1" : "0"));
            Assert.That(optionNode.SelectSingleNode("penaltyregime")?.InnerText, Is.EqualTo(question.PenaltyRegime));
            Assert.That(optionNode.SelectSingleNode("precheck")?.InnerText, Is.EqualTo(question.Precheck.ToString()));
            Assert.That(optionNode.SelectSingleNode("hidecheck")?.InnerText, Is.EqualTo(question.HideCheck.ToString()));
            Assert.That(optionNode.SelectSingleNode("showsource")?.InnerText, Is.EqualTo(question.ShowSource.ToString()));
            Assert.That(optionNode.SelectSingleNode("answerboxlines")?.InnerText, Is.EqualTo(question.AnswerBoxLines.ToString()));
            Assert.That(optionNode.SelectSingleNode("answerboxcolumns")?.InnerText, Is.EqualTo(question.AnswerBoxColumns.ToString()));
            Assert.That(NormalizeLf(optionNode.SelectSingleNode("answerpreload")?.InnerText), Is.EqualTo(NormalizeLf(question.AnswerPreload)));
            Assert.That(NormalizeLf(optionNode.SelectSingleNode("globalextra")?.InnerText), Is.EqualTo(NormalizeLf(question.GlobalExtra)));
            Assert.That(NormalizeLf(optionNode.SelectSingleNode("answer")?.InnerText), Is.EqualTo(NormalizeLf(question.Answer)));
            Assert.That(optionNode.SelectSingleNode("validateonsave")?.InnerText, Is.EqualTo(question.ValidateOnSave ? "1" : "0"));
            Assert.That(NormalizeLf(optionNode.SelectSingleNode("templateparams")?.InnerText), Is.EqualTo(NormalizeLf(question.TemplateParams)));
            Assert.That(optionNode.SelectSingleNode("hoisttemplateparams")?.InnerText, Is.EqualTo(question.HoistTemplateParams ? "1" : "0"));
            Assert.That(optionNode.SelectSingleNode("extractcodefromjson")?.InnerText, Is.EqualTo(question.ExtractCodeFromJson ? "1" : "0"));
            Assert.That(optionNode.SelectSingleNode("templateparamslang")?.InnerText, Is.EqualTo(question.TemplateParamsLang));
            Assert.That(optionNode.SelectSingleNode("templateparamsevalpertry")?.InnerText, Is.EqualTo(question.TemplateParamsEvalPerTry ? "1" : "0"));
            Assert.That(NormalizeLf(optionNode.SelectSingleNode("templateparamsevald")?.InnerText), Is.EqualTo(NormalizeLf(question.TemplateParamsEvald)));
            Assert.That(optionNode.SelectSingleNode("twigall")?.InnerText, Is.EqualTo(question.TwigAll ? "1" : "0"));
            Assert.That(NormalizeLf(optionNode.SelectSingleNode("uiparameters")?.InnerText), Is.EqualTo(NormalizeLf(question.UiParameters)));
            Assert.That(optionNode.SelectSingleNode("attachments")?.InnerText, Is.EqualTo(question.Attachments.ToString()));
            Assert.That(optionNode.SelectSingleNode("attachmentsrequired")?.InnerText, Is.EqualTo(question.AttachmentsRequired.ToString()));
            Assert.That(optionNode.SelectSingleNode("maxfilesize")?.InnerText, Is.EqualTo(question.MaxFileSize.ToString()));
            Assert.That(optionNode.SelectSingleNode("filenamesregex")?.InnerText, Is.EqualTo(question.FilenamesRegex));
            Assert.That(optionNode.SelectSingleNode("filenamesexplain")?.InnerText, Is.EqualTo(question.FilenamesExplain));
            Assert.That(optionNode.SelectSingleNode("displayfeedback")?.InnerText, Is.EqualTo(question.DisplayFeedback ? "1" : "0"));
            Assert.That(optionNode.SelectSingleNode("giveupallowed")?.InnerText, Is.EqualTo(question.GiveUpAllowed ? "1" : "0"));

            // Reset the XML writer for the next test
            _xmlWriter = _testFactory.Create("test.xml", new XmlWriterSettings());
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("root");
        }

        [Test]
        public void WritePluginData_WritesTestCasesCorrectly()
        {
            // Arrange
            var question = CreateBasicCodeRunnerQuestion();

            // Act
            _codeRunnerWriter.WritePluginData(_xmlWriter, question, _generalHelper, _idGenerator);

            // Close the XML elements before reading
            _xmlWriter.WriteEndElement(); // Close the root element
            _xmlWriter.WriteEndDocument();
            _xmlWriter.Flush();

            string output = _testFactory.StringWriter.ToString();

            // Assert
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Check test cases
            var testCaseNodes = doc.SelectNodes("//coderunner_testcase");
            Assert.IsNotNull(testCaseNodes);
            Assert.That(testCaseNodes.Count, Is.EqualTo(question.TestCases.Count));

            // Check first test case
            var firstTestCase = question.TestCases[0];
            var firstTestCaseNode = testCaseNodes![0];
            Assert.That(NormalizeLf(firstTestCaseNode.SelectSingleNode("testcode")?.InnerText), Is.EqualTo(NormalizeLf(firstTestCase.TestCode)));
            Assert.That(firstTestCaseNode.SelectSingleNode("testtype")?.InnerText, Is.EqualTo(firstTestCase.TestType.ToString()));
            Assert.That(NormalizeLf(firstTestCaseNode.SelectSingleNode("expected")?.InnerText), Is.EqualTo(NormalizeLf(firstTestCase.Expected)));
            Assert.That(firstTestCaseNode.SelectSingleNode("useasexample")?.InnerText, Is.EqualTo(firstTestCase.UseAsExample ? "1" : "0"));
            Assert.That(firstTestCaseNode.SelectSingleNode("display")?.InnerText, Is.EqualTo(firstTestCase.Display));
            Assert.That(firstTestCaseNode.SelectSingleNode("hiderestiffail")?.InnerText, Is.EqualTo(firstTestCase.HideRestIfFail ? "1" : "0"));
            Assert.That(firstTestCaseNode.SelectSingleNode("mark")?.InnerText, Is.EqualTo(firstTestCase.Mark.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)));
            Assert.That(NormalizeLf(firstTestCaseNode.SelectSingleNode("stdin")?.InnerText), Is.EqualTo(NormalizeLf(firstTestCase.StdIn)));
            Assert.That(NormalizeLf(firstTestCaseNode.SelectSingleNode("extra")?.InnerText), Is.EqualTo(NormalizeLf(firstTestCase.Extra)));

            // Check second test case
            var secondTestCase = question.TestCases[1];
            var secondTestCaseNode = testCaseNodes![1];
            Assert.That(NormalizeLf(secondTestCaseNode.SelectSingleNode("testcode")?.InnerText), Is.EqualTo(NormalizeLf(secondTestCase.TestCode)));
            Assert.That(secondTestCaseNode.SelectSingleNode("testtype")?.InnerText, Is.EqualTo(secondTestCase.TestType.ToString()));
            Assert.That(NormalizeLf(secondTestCaseNode.SelectSingleNode("expected")?.InnerText), Is.EqualTo(NormalizeLf(secondTestCase.Expected)));
            Assert.That(secondTestCaseNode.SelectSingleNode("useasexample")?.InnerText, Is.EqualTo(secondTestCase.UseAsExample ? "1" : "0"));
            Assert.That(secondTestCaseNode.SelectSingleNode("display")?.InnerText, Is.EqualTo(secondTestCase.Display));
            Assert.That(secondTestCaseNode.SelectSingleNode("hiderestiffail")?.InnerText, Is.EqualTo(secondTestCase.HideRestIfFail ? "1" : "0"));
            Assert.That(secondTestCaseNode.SelectSingleNode("mark")?.InnerText, Is.EqualTo(secondTestCase.Mark.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)));
            Assert.That(NormalizeLf(secondTestCaseNode.SelectSingleNode("stdin")?.InnerText), Is.EqualTo(NormalizeLf(secondTestCase.StdIn)));
            Assert.That(NormalizeLf(secondTestCaseNode.SelectSingleNode("extra")?.InnerText), Is.EqualTo(NormalizeLf(secondTestCase.Extra)));

            // Reset the XML writer for the next test
            _xmlWriter = _testFactory.Create("test.xml", new XmlWriterSettings());
            _xmlWriter.WriteStartDocument();
            _xmlWriter.WriteStartElement("root");
        }

        [Test]
        public void GetQuestionTypeString_ReturnsCoderunner()
        {
            // Act
            string type = _codeRunnerWriter.GetQuestionTypeString();

            // Assert
            Assert.That(type, Is.EqualTo("coderunner"));
        }

        [Test]
        public void WritePluginData_ThrowsArgumentException_WhenNotCodeRunnerQuestion()
        {
            // Arrange - create a different question type (not CodeRunner)
            var wrongQuestion = new DropdownQuestion
            {
                QuestionId = 999,
                QuestionName = "Wrong Type Question",
                Category = "Test Category",
                QuestionText = "Test question text"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _codeRunnerWriter.WritePluginData(_xmlWriter, wrongQuestion, _generalHelper, _idGenerator));
        }

        private CodeRunnerQuestion CreateBasicCodeRunnerQuestion()
        {
            return new CodeRunnerQuestion
            {
                QuestionId = 100,
                Category = "Programming",
                QuestionName = "Hello World in Java",
                QuestionText = "<p>Write a Java program that outputs \"Hello, World!\"</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                GeneralFeedback = "<p>Make sure you understand how to create a main class in Java.</p>",
                GeneralFeedbackFormat = FormattingFormat.HTML,
                CodeRunnerType = "java_program",
                PrototypeType = 0,
                AllOrNothing = true,
                PenaltyRegime = "10, 20, ...",
                Precheck = 0,
                HideCheck = 0,
                ShowSource = 0,
                AnswerBoxLines = 18,
                AnswerBoxColumns = 100,
                AnswerPreload = "",
                GlobalExtra = "",
                Answer = "public class Main {\n    public static void main(String[] args) {\n        System.out.println(\"Hello, World!\");\n    }\n}",
                ValidateOnSave = true,
                TemplateParams = "",
                HoistTemplateParams = true,
                ExtractCodeFromJson = true,
                TemplateParamsLang = "None",
                TemplateParamsEvalPerTry = false,
                TemplateParamsEvald = "{}",
                TwigAll = false,
                UiParameters = "",
                Attachments = 0,
                AttachmentsRequired = 0,
                MaxFileSize = 10240,
                FilenamesRegex = "",
                FilenamesExplain = "",
                DisplayFeedback = true,
                GiveUpAllowed = false,
                TestCases = new List<CodeRunnerTestCase>
                {
                    new CodeRunnerTestCase
                    {
                        Id = 1,
                        TestCode = "Hello World Test",
                        TestType = 0,
                        Expected = "Hello, World!",
                        UseAsExample = false,
                        Display = "SHOW",
                        HideRestIfFail = false,
                        Mark = 1.000m,
                        StdIn = "",
                        Extra = ""
                    },
                    new CodeRunnerTestCase
                    {
                        Id = 2,
                        TestCode = "Empty Input Test",
                        TestType = 0,
                        Expected = "Hello, World!",
                        UseAsExample = true,
                        Display = "HIDE_IF_FAIL",
                        HideRestIfFail = true,
                        Mark = 0.500m,
                        StdIn = "",
                        Extra = "Some extra data"
                    }
                }
            };
        }
    }
}
