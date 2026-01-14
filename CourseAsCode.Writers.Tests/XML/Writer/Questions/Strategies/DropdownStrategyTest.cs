using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class DropdownStrategyTests
    {
        private ILogger<DropdownStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IXmlService _mockXmlService;
        private DropdownStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";

        [SetUp]
        public void SetUp()
        {
            // Create Mocks
            _mockLogger = Substitute.For<ILogger<DropdownStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockXmlService = Substitute.For<IXmlService>();

            // Configure default mock behaviors
            var idCounter = 1;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => idCounter++);

            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            // Initialize System Under Test
            _strategy = new DropdownStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsMatch()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("match"));
        }

        [Test]
        public void WritePluginData_WithInvalidQuestionType_ThrowsArgumentException()
        {
            // Arrange
            var invalidQuestion = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _strategy.WritePluginData(_mockXmlService, invalidQuestion));
            Assert.That(ex.Message, Does.Contain("Expected DropdownQuestion"));
        }

        [Test]
        public void WritePluginData_WritesRootStructure()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("plugin_qtype_match_question");
            _mockXmlService.Received().WriteElementWithAttributes("matchoptions", Arg.Any<Dictionary<string, string>>());
            _mockXmlService.Received().WriteElement("matches");

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesMatchOptionsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(1, "CatA");
            question.CorrectResponseFeedback = "Right!";
            question.PartiallyCorrectResponseFeedback = "Almost";
            question.IncorrectResponseFeedback = "Wrong";
            question.Hints = new List<Hint> { new Hint { HintText = "Some hint", ShowNumberOfCorrectAnswers = true } };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes("matchoptions", Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("id")));
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["shuffleanswers"] == "1" &&
                d["correctfeedback"] == "Right!" &&
                d["correctfeedbackformat"] == MOCKED_FORMAT_STRING &&
                d["partiallycorrectfeedback"] == "Almost" &&
                d["incorrectfeedback"] == "Wrong" &&
                d["shownumcorrect"] == "1"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_HandlesNullFeedbacksInMatchOptions()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(1, "CatA");
            question.CorrectResponseFeedback = null;
            question.PartiallyCorrectResponseFeedback = null;
            question.IncorrectResponseFeedback = null;

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["correctfeedback"] == string.Empty &&
                d["partiallycorrectfeedback"] == string.Empty &&
                d["incorrectfeedback"] == string.Empty));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesSubQuestionsAsMatches()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(1, "CatA");
            question.SubQuestions.Add(new DropdownSubQuestion
            {
                QuestionText = "Capital of France?",
                CorrectAnswer = "Paris",
                QuestionTextFormat = FormattingFormat.HTML,
            });

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: Find the match element with the question text
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("questiontext") && d["questiontext"] == "Capital of France?" &&
                d.ContainsKey("answertext") && d["answertext"] == "Paris" &&
                d["questiontextformat"] == MOCKED_FORMAT_STRING));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesWrongAnswersAsMatchesWithEmptyQuestionText()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(1, "CatA");
            question.SubQuestions.Add(new DropdownSubQuestion
            {
                QuestionText = "SubQ1",
                CorrectAnswer = "Correct1",
                WrongAnswers = new List<string> { "Distractor1", "Distractor2" },
            });

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: Distractors should be written as matches with empty question text
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["questiontext"] == string.Empty &&
                d["answertext"] == "Distractor1"));

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["questiontext"] == string.Empty &&
                d["answertext"] == "Distractor2"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_ConsolidatesDuplicateWrongAnswers()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(1, "CatA");

            // SubQ1 has "WrongA"
            question.SubQuestions.Add(new DropdownSubQuestion
            {
                QuestionText = "Q1",
                CorrectAnswer = "A",
                WrongAnswers = new List<string> { "WrongA" },
            });

            // SubQ2 also has "WrongA" (same distractor string)
            question.SubQuestions.Add(new DropdownSubQuestion
            {
                QuestionText = "Q2",
                CorrectAnswer = "B",
                WrongAnswers = new List<string> { "WrongA" },
            });

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: "WrongA" should only be written ONCE as a pooled answer
            _mockXmlService.Received(1).WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["questiontext"] == string.Empty &&
                d["answertext"] == "WrongA"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }
    }
}

