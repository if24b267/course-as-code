using CourseAsCode.Model.Questions.Types.MultipleChoice;
using CourseAsCode.Model.Questions.Types.TrueFalse;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class TrueFalseStrategyTests
    {
        private ILogger<TrueFalseStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private IXmlService _mockXmlService;
        private TrueFalseStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";

        [SetUp]
        public void SetUp()
        {
            // Create Mocks
            _mockLogger = Substitute.For<ILogger<TrueFalseStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockXmlService = Substitute.For<IXmlService>();

            // Configure default mock behaviors
            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            // Initialize System Under Test
            _strategy = new TrueFalseStrategy(_mockLogger, _mockGeneralHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsTrueFalse()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("truefalse"));
        }

        [Test]
        public void WritePluginData_WithInvalidQuestionType_ThrowsArgumentException()
        {
            // Arrange
            var invalidQuestion = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _strategy.WritePluginData(_mockXmlService, invalidQuestion));
            Assert.That(ex.Message, Does.Contain("Expected TrueFalseQuestion"));
        }

        [Test]
        public void WritePluginData_WritesRootStructure()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicTrueFalseQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("plugin_qtype_truefalse_question");
            _mockXmlService.Received().WriteElement("answers");
            _mockXmlService.Received().WriteElementWithAttributes("truefalse", Arg.Any<Dictionary<string, string>>());

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WithExplicitAnswers_WritesCorrectData()
        {
            // Arrange: Question with manually defined answers
            var question = new TrueFalseQuestion
            {
                Answers = new List<MultiChoiceAnswer>
                {
                    // Index 0 is strictly the "True" option in Moodle logic
                    new MultiChoiceAnswer
                    {
                        AnswerText = "True",
                        AnswerGrade = 1.0f,
                        AnswerFeedback = "Correct!",
                    },
                    // Index 1 is strictly the "False" option in Moodle logic
                    new MultiChoiceAnswer
                    {
                        AnswerText = "False",
                        AnswerGrade = 0.0f,
                        AnswerFeedback = "Wrong!",
                    },
                },
                CorrectAnswer = false,
                Category = "Unit Test",
                QuestionName = "UnitTestQuestion",
                QuestionText = "Will this test pass?",
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: First Answer (True Option - ID 1)
            _mockXmlService.Received().WriteElementWithAttributes("answer", Arg.Is<Dictionary<string, string>>(d => d["id"] == "1"));
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "True" &&
                d["fraction"] == "1.0000000" &&
                d["feedback"] == "Correct!"));

            // Assert: Second Answer (False Option - ID 2)
            _mockXmlService.Received().WriteElementWithAttributes("answer", Arg.Is<Dictionary<string, string>>(d => d["id"] == "2"));
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "False" &&
                d["fraction"] == "0.0000000" &&
                d["feedback"] == "Wrong!"));
        }

        [Test]
        public void WritePluginData_EmptyAnswers_CorrectIsTrue_AutoGeneratesCorrectFractions()
        {
            // Arrange: Empty answers list, CorrectAnswer = true
            var question = new TrueFalseQuestion
            {
                Category = "Unit Test",
                QuestionName = "UnitTestQuestion",
                QuestionText = "Will this test pass?",
                CorrectAnswer = true,
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            // Index 0 (True Option)
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "True" &&
                d["fraction"] == "1.0000000")); // Correct

            // Index 1 (False Option)
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "False" &&
                d["fraction"] == "0.0000000")); // Incorrect
        }

        [Test]
        public void WritePluginData_EmptyAnswers_CorrectIsFalse_AutoGeneratesCorrectFractions()
        {
            // Arrange: Empty answers list, CorrectAnswer = false
            var question = new TrueFalseQuestion
            {
                Category = "Unit Test",
                QuestionName = "UnitTestQuestion",
                QuestionText = "Will this test pass?",
                CorrectAnswer = false,
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            // Index 0 (True Option)
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "True" &&
                d["fraction"] == "0.0000000")); // Incorrect

            // Index 1 (False Option)
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "False" &&
                d["fraction"] == "1.0000000")); // Correct
        }

        [Test]
        public void WritePluginData_EmptyAnswers_WritesDefaultValues()
        {
            // Arrange
            var question = new TrueFalseQuestion
            {
                Category = "Unit Test",
                QuestionName = "UnitTestQuestion",
                QuestionText = "Will this test pass?",
                CorrectAnswer = true,
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: Check default texts
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d => d["answertext"] == "True"));
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d => d["answertext"] == "False"));
        }

        [Test]
        public void WritePluginData_AnswerCountWrong_WritesReviewableXmlAnyway()
        {
            // Arrange: 3 Answers. The loop in the strategy strictly runs i < 2.
            var question = new TrueFalseQuestion
            {
                Answers = new List<MultiChoiceAnswer>
                {
                    new MultiChoiceAnswer
                    {
                        AnswerText = "True",
                        AnswerGrade = 1.0f,
                        AnswerFeedback = "Correct!",
                    },
                    new MultiChoiceAnswer
                    {
                        AnswerText = "False",
                        AnswerGrade = 0.0f,
                        AnswerFeedback = "Wrong!",
                    },
                    new MultiChoiceAnswer // Should be ignored
                    {
                        AnswerText = "Oops, there's one too many!",
                        AnswerGrade = 0.0f,
                        AnswerFeedback = "Wrong!",
                    },
                },
                CorrectAnswer = false,
                Category = "Unit Test",
                QuestionName = "UnitTestQuestion",
                QuestionText = "Will this test pass?",
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: We should receive 2 write calls for answers, not 3
            _mockXmlService.Received(2).WriteElementWithAttributes("answer", Arg.Any<Dictionary<string, string>>());

            // Ensure "3" was never written
            _mockXmlService.DidNotReceive().WriteElements(Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("answertext") && d["answertext"] == "3"));
        }

        [Test]
        public void WritePluginData_OneAnswerOnly_FallsBackToDefaultsForSecond()
        {
            // Arrange: 1 Answer provided (Index 0 - True)
            var question = new TrueFalseQuestion
            {
                Answers = new List<MultiChoiceAnswer>
                {
                    new MultiChoiceAnswer
                    {
                        AnswerText = "CustomTrue", // <-- This will not work in Moodle, but it's fine for unit testing
                        AnswerGrade = 0.0f,
                        AnswerFeedback = "Wrong!",
                    },
                },
                CorrectAnswer = false,
                Category = "Unit Test",
                QuestionName = "UnitTestQuestion",
                QuestionText = "Will this test pass?",
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert: First iteration (Index 0): Uses provided object
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d => d["answertext"] == "CustomTrue"));

            // Second iteration (Index 1): Defaults because ElementAtOrDefault(1) returns null
            // Should fallback to "False" text and Grade 1.0 (because CorrectAnswer is false)
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["answertext"] == "False" &&
                d["fraction"] == "1.0000000"));
        }
    }
}
