using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types.MultipleChoice;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class MultiChoiceStrategyTests
    {
        private ILogger<MultiChoiceStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IQuizHelper _mockQuizHelper;
        private IXmlService _mockXmlService;
        private MultiChoiceStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";
        private const string MOCKED_NUMBERING_FORMAT = "abc";

        [SetUp]
        public void SetUp()
        {
            // Create Mocks
            _mockLogger = Substitute.For<ILogger<MultiChoiceStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockQuizHelper = Substitute.For<IQuizHelper>();
            _mockXmlService = Substitute.For<IXmlService>();

            // Configure default mock behaviors
            var idCounter = 1;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => idCounter++);

            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);
            _mockQuizHelper.MapNumberingFormat(Arg.Any<NumberingFormat>()).Returns(MOCKED_NUMBERING_FORMAT);

            // Initialize System Under Test
            _strategy = new MultiChoiceStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator, _mockQuizHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsMultichoice()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("multichoice"));
        }

        [Test]
        public void WritePluginData_WithInvalidQuestionType_ThrowsArgumentException()
        {
            // Arrange
            var invalidQuestion = FakeQuestionFactory.CreateBasicDragDropTextQuestion(1, "CatA");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _strategy.WritePluginData(_mockXmlService, invalidQuestion));
            Assert.That(ex.Message, Does.Contain("Expected MultipleChoiceQuestion"));
        }

        [Test]
        public void WritePluginData_WritesRootStructure()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert call order
            _mockXmlService.Received().WriteElement("plugin_qtype_multichoice_question");
            _mockXmlService.Received().WriteElement("answers");
            _mockXmlService.Received().WriteElementWithAttributes("multichoice", Arg.Any<Dictionary<string, string>>());

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesAnswersWithCorrectData()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            question.Answers = new List<MultiChoiceAnswer>
            {
                new MultiChoiceAnswer
                {
                    AnswerText = "Option A",
                    AnswerGrade = 1.0f,
                    AnswerFeedback = "Correct!",
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes("answer", Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("id")));

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "Option A" &&
                d["answerformat"] == MOCKED_FORMAT_STRING &&
                d["fraction"] == "1.0000000" &&
                d["feedback"] == "Correct!" &&
                d["feedbackformat"] == MOCKED_FORMAT_STRING));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_HandlesNullFeedbackInAnswers()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            question.Answers = new List<MultiChoiceAnswer>
            {
                new MultiChoiceAnswer
                {
                    AnswerText = "Option B",
                    AnswerGrade = 0f,
                    AnswerFeedback = null,
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["feedback"] == string.Empty));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_GeneratesUniqueIdsForMultipleAnswers()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            question.Answers = new List<MultiChoiceAnswer>
            {
                new MultiChoiceAnswer
                {
                    AnswerText = "Option A",
                    AnswerGrade = 1.0f,
                    AnswerFeedback = "Correct!",
                },
                new MultiChoiceAnswer
                {
                    AnswerText = "Option B",
                    AnswerGrade = 0f,
                    AnswerFeedback = "Incorrect!",
                },
            };

            _mockIdGenerator.GetNextAnswerId().Returns(1, 2, 3); // 1 and 2 for answers, 3 for multichoice element further down in code

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes("answer", Arg.Is<Dictionary<string, string>>(d => d["id"] == "1"));
            _mockXmlService.Received().WriteElementWithAttributes("answer", Arg.Is<Dictionary<string, string>>(d => d["id"] == "2"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesMultichoiceSettingsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            question.SingleChoice = true;
            question.ShuffleAnswers = false;
            question.ShowStandardInstructions = true;
            question.CorrectResponseFeedback = "Good";
            question.PartiallyCorrectResponseFeedback = "Okay";
            question.IncorrectResponseFeedback = "Bad";
            question.ChoiceNumbering = NumberingFormat.AlphabeticLower;

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes("multichoice", Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("id")));

            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["layout"] == "0" &&
                d["single"] == "1" &&
                d["shuffleanswers"] == "0" &&
                d["showstandardinstruction"] == "1" &&
                d["correctfeedback"] == "Good" &&
                d["partiallycorrectfeedback"] == "Okay" &&
                d["incorrectfeedback"] == "Bad" &&
                d["answernumbering"] == MOCKED_NUMBERING_FORMAT));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_ShowNumCorrect_TrueIfHintEnabled()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            question.Hints = new List<Hint>
            {
                new Hint
                {
                    ShowNumberOfCorrectAnswers = false,
                    HintText = "First Hint",
                },
                new Hint
                {
                    ShowNumberOfCorrectAnswers = true,
                    HintText = "Second Hint",
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: As soon as one hint has ShowNumberOfCorrectAnswers set to true, the overall value should be true
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["shownumcorrect"] == "1"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_ShowNumCorrect_FalseIfHintsNull()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            question.Hints = new List<Hint>
            {
                new Hint
                {
                    ShowNumberOfCorrectAnswers = false,
                    HintText = "First Hint",
                },
                new Hint
                {
                    ShowNumberOfCorrectAnswers = false,
                    HintText = "Second Hint",
                },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["shownumcorrect"] == "0"));

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }
    }
}
