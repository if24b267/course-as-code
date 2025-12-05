using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class DragDropTextStrategyTests
    {
        private ILogger<DragDropTextStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IXmlService _mockXmlService;
        private DragDropTextStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";

        [SetUp]
        public void SetUp()
        {
            // Create Mocks
            _mockLogger = Substitute.For<ILogger<DragDropTextStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockXmlService = Substitute.For<IXmlService>();

            // Configure default mock behaviors
            var idCounter = 1;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => idCounter++);

            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            // Initialize System Under Test
            _strategy = new DragDropTextStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsDdwtos()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("ddwtos"));
        }

        [Test]
        public void WritePluginData_WithInvalidQuestionType_ThrowsArgumentException()
        {
            // Arrange
            var invalidQuestion = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _strategy.WritePluginData(_mockXmlService, invalidQuestion));
            Assert.That(ex.Message, Does.Contain("Expected DragAndDropTextQuestion"));
        }

        [Test]
        public void WritePluginData_WritesRootStructure()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("plugin_qtype_ddwtos_question");
            _mockXmlService.Received().WriteElement("answers");
            _mockXmlService.Received().WriteElementWithAttributes("ddwtos", Arg.Any<Dictionary<string, string>>());

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesAnswerStructureCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");
            question.Answers = new List<TextChoice>()
            {
                new TextChoice { ID = 1, Value = "Choice1", ValueFormat = FormattingFormat.HTML },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes("answer", Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("id")));

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("answertext") && d["answertext"] == "Choice1" &&
                d.ContainsKey("answerformat") && d["answerformat"] == MOCKED_FORMAT_STRING &&
                d.ContainsKey("fraction") && d["fraction"] == "0.0000000" &&
                d.ContainsKey("feedback") && // PHP string checked in specific tests
                d.ContainsKey("feedbackformat") && d["feedbackformat"] == "0"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_UncategorizedAnswer_MapsToGroupOne()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");
            question.Answers = new List<TextChoice>
            {
                new TextChoice { ID = 1, Value = "NoCategoryChoice", Category = null },
            };

            // PHP string for Group 1: s:9:"draggroup";s:1:"1";
            string expectedPhp = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"1\";s:8:\"infinite\";b:0;}";

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "NoCategoryChoice" &&
                d["feedback"] == expectedPhp));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_SingleNamedCategory_MapsToGroupOne()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");
            question.Answers = new List<TextChoice>
            {
                new TextChoice { ID = 1, Value = "CatChoice", Category = "Animals" },
            };

            // Even if named, if it's the first alphabetical distinct category, it gets ID 1
            string expectedPhp = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"1\";s:8:\"infinite\";b:0;}";

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "CatChoice" &&
                d["feedback"] == expectedPhp));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_MultipleCategories_MapsToSequentialIdsAlphabetically()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");
            question.Answers = new List<TextChoice>
            {
                new TextChoice { ID = 1, Value = "Z_Choice", Category = "Zebra" },
                new TextChoice { ID = 2, Value = "A_Choice", Category = "Ant" },
            };

            // Ant -> Group 1
            string expectedAntPhp = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"1\";s:8:\"infinite\";b:0;}";
            // Zebra -> Group 2
            string expectedZebraPhp = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"2\";s:8:\"infinite\";b:0;}";

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "A_Choice" && d["feedback"] == expectedAntPhp));

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "Z_Choice" && d["feedback"] == expectedZebraPhp));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_EmptyAndNullCategory_BothMapToGroupOne()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");
            question.Answers = new List<TextChoice>
            {
                new TextChoice { ID = 1, Value = "NullCat", Category = null },
                new TextChoice { ID = 2, Value = "EmptyCat", Category = "" },
            };

            string expectedPhp = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"1\";s:8:\"infinite\";b:0;}";

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "NullCat" && d["feedback"] == expectedPhp));
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "EmptyCat" && d["feedback"] == expectedPhp));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesDdwtosOptionsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");
            question.ShuffleAnswers = true;
            question.CorrectResponseFeedback = "Good job";
            question.PartiallyCorrectResponseFeedback = "Not quite";
            question.IncorrectResponseFeedback = "Bad";
            question.Hints = null;

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes("ddwtos", Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("id")));

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["shuffleanswers"] == "1" &&
                d["correctfeedback"] == "Good job" &&
                d["correctfeedbackformat"] == MOCKED_FORMAT_STRING &&
                d["partiallycorrectfeedback"] == "Not quite" &&
                d["incorrectfeedback"] == "Bad" &&
                d["shownumcorrect"] == "0")); // Hints were null

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_ShowNumCorrect_TrueIfHintEnabled()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");
            question.Hints = new List<Hint>
            {
                new Hint
                {
                    ShowNumberOfCorrectAnswers = false,
                    HintText = "Some hint",
                },
                new Hint
                {
                    ShowNumberOfCorrectAnswers = true,
                    HintText = "Some other hint",
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: If any of the hints have ShowNumberOfCorrectAnswers set to true, it should be true in the XML
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("shownumcorrect") && d["shownumcorrect"] == "1"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_ShowNumCorrect_FalseIfAllHintsDisabled()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");
            question.Hints = new List<Hint>
            {
                new Hint
                {
                    ShowNumberOfCorrectAnswers = false,
                    HintText = "Some hint",
                },
                new Hint
                {
                    ShowNumberOfCorrectAnswers = false,
                    HintText = "Some other hint",
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("shownumcorrect") && d["shownumcorrect"] == "0"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }
    }
}
