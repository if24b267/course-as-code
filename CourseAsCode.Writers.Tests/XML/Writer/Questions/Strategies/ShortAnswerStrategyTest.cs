using CourseAsCode.Model.Questions.Types.ShortAnswer;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class ShortAnswerStrategyTests
    {
        private ILogger<ShortAnswerStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IXmlService _mockXmlService;
        private ShortAnswerStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";

        [SetUp]
        public void SetUp()
        {
            // Create Mocks
            _mockLogger = Substitute.For<ILogger<ShortAnswerStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockXmlService = Substitute.For<IXmlService>();

            // Configure default mock behaviors
            var idCounter = 1;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => idCounter++);

            // Mock the enum conversion (e.g. HTML -> "1")
            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            // Initialize System Under Test
            // Note: ShortAnswerStrategy does not need QuizHelper, so we don't pass it here
            _strategy = new ShortAnswerStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsShortAnswer()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("shortanswer"));
        }

        [Test]
        public void WritePluginData_WithInvalidQuestionType_ThrowsArgumentException()
        {
            // Arrange - We intentionally create a wrong question type
            var invalidQuestion = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _strategy.WritePluginData(_mockXmlService, invalidQuestion));
            Assert.That(ex.Message, Does.Contain("Expected ShortAnswerQuestion"));
        }

        [Test]
        public void WritePluginData_WritesRootStructure()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicShortAnswerQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert call order
            _mockXmlService.Received().WriteElement("plugin_qtype_shortanswer_question");
            _mockXmlService.Received().WriteElement("answers");
            _mockXmlService.Received().WriteElementWithAttributes("shortanswer", Arg.Any<Dictionary<string, string>>());

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call =>
                call.GetMethodInfo().Name == "WriteElement" ||
                call.GetMethodInfo().Name == "WriteElementWithAttributes");

            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesAnswersWithCorrectData()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicShortAnswerQuestion(1, "CatA");
            // We override the answers from the factory with our own test data
            question.Answers = new List<ShortAnswerOptions>
            {
                new ShortAnswerOptions
                {
                    AnswerText = "Test Answer",
                    AnswerGrade = 1.0f,
                    AnswerFeedback = "Great!",
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            // 1. Was the <answer> tag written with the ID attribute?
            _mockXmlService.Received().WriteElementWithAttributes("answer", Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("id")));

            // 2. Were the inner values written correctly?
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "Test Answer" &&
                d["answerformat"] == MOCKED_FORMAT_STRING &&
                d["fraction"] == "1.0000000" && // Important: Invariant Culture Test
                d["feedback"] == "Great!" &&
                d["feedbackformat"] == MOCKED_FORMAT_STRING));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesUseCase_True_AsOne()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicShortAnswerQuestion(1, "CatA");
            question.UseCase = true; // Case Sensitivity ON

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            // We search in the dictionary passed to WriteElements for "usecase" == "1"
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("usecase") && d["usecase"] == "1"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesUseCase_False_AsZero()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicShortAnswerQuestion(1, "CatA");
            question.UseCase = false; // Case Sensitivity OFF

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("usecase") && d["usecase"] == "0"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }
    }
}
