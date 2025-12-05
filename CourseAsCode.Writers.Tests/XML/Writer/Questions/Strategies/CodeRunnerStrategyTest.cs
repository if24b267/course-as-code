using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Plugins;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class CodeRunnerStrategyTest
    {
        private ILogger<CodeRunnerStrategy> _mockLogger;
        private ISequentialIdGenerator _mockIdGenerator;
        private IXmlService _mockXmlService;
        private CodeRunnerStrategy _strategy;

        private const string DEFAULT_FORMAT_MAPPED = "1";

        [SetUp]
        public void SetUp()
        {
            // Create Mocks
            _mockLogger = Substitute.For<ILogger<CodeRunnerStrategy>>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockXmlService = Substitute.For<IXmlService>();

            // Configure default mock behaviors
            var codeRunnerOptionId = 1;
            var codeRunnerTestCaseId = 1;
            _mockIdGenerator.GetNextCodeRunnerOptionId().Returns(_ => codeRunnerOptionId++);
            _mockIdGenerator.GetNextCodeRunnerTestCaseId().Returns(_ => codeRunnerTestCaseId++);

            // Initialize System Under Test
            _strategy = new CodeRunnerStrategy(_mockLogger, _mockIdGenerator);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsCodeRunner()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("coderunner"));
        }

        [Test]
        public void WritePluginData_WithInvalidQuestionType_ThrowsArgumentException()
        {
            // Arrange
            var invalidQuestion = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _strategy.WritePluginData(_mockXmlService, invalidQuestion));
            Assert.That(ex.Message, Does.Contain("Expected CodeRunnerQuestion"));
        }

        [Test]
        public void WritePluginData_WritesRootPluginElement()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received(1).WriteElement("plugin_qtype_coderunner_question");

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesBasicStructure()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("plugin_qtype_coderunner_question");
            _mockXmlService.Received().WriteElement("coderunner_options");
            _mockXmlService.Received().WriteElement("coderunner_testcases");

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesCodeRunnerOptionWithGeneratedId()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(999, "CatA");
            _mockIdGenerator.GetNextCodeRunnerOptionId().Returns(50);

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes(
                "coderunner_option",
                Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("id") && d["id"] == "50"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesBasicPropertiesCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(999, "CatA");
            question.CodeRunnerType = "python3";
            question.PenaltyRegime = "10, 20, 30";
            question.Answer = "print('hello')";

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("coderunnertype") && d["coderunnertype"] == "python3" &&
                d.ContainsKey("penaltyregime") && d["penaltyregime"] == "10, 20, 30" &&
                d.ContainsKey("answer") && d["answer"] == "print('hello')"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_CorrectlySetsBooleanOptions()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA");
            question.AllOrNothing = true;
            question.ValidateOnSave = false;
            question.HoistTemplateParams = true;
            question.ExtractCodeFromJson = false;
            question.TwigAll = true;
            question.DisplayFeedback = false;

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["allornothing"] == "1" &&
                d["validateonsave"] == "0" &&
                d["hoisttemplateparams"] == "1" &&
                d["extractcodefromjson"] == "0" &&
                d["twigall"] == "1" &&
                d["displayfeedback"] == "0"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesPlaceholdersForUnusedFields()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: Check a few hardcoded placeholders
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["useace"] == "$@NULL@$" &&
                d["resultcolumns"] == "$@NULL@$" &&
                d["sandbox"] == "$@NULL@$" &&
                d["grader"] == "$@NULL@$"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_CorrectlySetsIntegerOptions()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA");
            question.PrototypeType = 2;
            question.Precheck = 1;
            question.AnswerBoxLines = 20;
            question.Attachments = 3;

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["prototypetype"] == "2" &&
                d["precheck"] == "1" &&
                d["answerboxlines"] == "20" &&
                d["attachments"] == "3"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WithNoTestCases_WritesEmptyTestCasesContainer()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA");
            question.TestCases.Clear();

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("coderunner_testcases");
            _mockXmlService.DidNotReceive().WriteElementWithAttributes("coderunner_testcase", Arg.Any<Dictionary<string, string>>());

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WithTestCases_GeneratesIdsAndWritesElements()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA");
            var tc1 = new CodeRunnerTestCase { TestCode = "print(1)" };
            var tc2 = new CodeRunnerTestCase { TestCode = "print(2)" };
            question.TestCases = new List<CodeRunnerTestCase> { tc1, tc2 };

            _mockIdGenerator.GetNextCodeRunnerTestCaseId().Returns(10, 11);

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: Check that two test cases were written with correct IDs
            _mockXmlService.Received().WriteElementWithAttributes("coderunner_testcase", Arg.Is<Dictionary<string, string>>(d => d["id"] == "10"));
            _mockXmlService.Received().WriteElementWithAttributes("coderunner_testcase", Arg.Is<Dictionary<string, string>>(d => d["id"] == "11"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesTestCasePropertiesCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(1, "CatA");
            var tc = new CodeRunnerTestCase
            {
                TestCode = "assert x == 1",
                Expected = "True",
                Mark = new decimal(1.500),
                UseAsExample = true,
                HideRestIfFail = false,
                Display = "SHOW",
                TestType = 2,
                StdIn = "input data",
                Extra = "extra config",
            };
            question.TestCases = new List<CodeRunnerTestCase> { tc };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["testcode"] == "assert x == 1" &&
                d["expected"] == "True" &&
                d["mark"] == "1.500" && // Formatted string
                d["useasexample"] == "1" &&
                d["hiderestiffail"] == "0" &&
                d["display"] == "SHOW" &&
                d["testtype"] == "2" &&
                d["stdin"] == "input data" &&
                d["extra"] == "extra config"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }
    }
}
