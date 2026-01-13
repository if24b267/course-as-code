using System.Xml;

using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using Microsoft.Extensions.Logging;
using NSubstitute;

using QuizModel = CourseAsCode.Model.Activities.Resources.Quiz;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.Quiz
{
    [TestFixture]
    public class QuizWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<QuizWriter> _logger;
        private IGeneralHelper _generalHelper;
        private IQuizHelper _quizHelper;
        private QuizWriter _quizWriter;
        private const string DEFAULT_DEFAULT_OUTPUT_PATH = "dummyDir";

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory to capture XML output
            _testFactory = new XmlWriterFactoryTest();

            // Mock the logger and general helper dependencies and configure them to return consistent values
            _logger = Substitute.For<ILogger<QuizWriter>>();
            _generalHelper = Substitute.For<IGeneralHelper>();
            _generalHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Mock the quiz helper
            _quizHelper = Substitute.For<IQuizHelper>();

            // Instantiate the QuizWriter with the fake dependencies
            _quizWriter = new QuizWriter(_testFactory, _logger, _generalHelper, _quizHelper);
        }

        [Test]
        public void WriteQuiz_CreatesValidXml()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteQuiz_RootElementIsActivity_HasCorrectAttributes()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The root element is <activity> with correct attributes
            var root = doc.DocumentElement;
            Assert.That(root, Is.Not.Null, "The XML document should have a root element");
            Assert.That(root.Name, Is.EqualTo("activity"));
            Assert.That(root.GetAttribute("id"), Is.EqualTo(dummyQuiz.ActivityId.ToString()));
            Assert.That(root.GetAttribute("moduleid"), Is.EqualTo(dummyQuiz.ModuleId.ToString()));
            Assert.That(root.GetAttribute("modulename"), Is.EqualTo(dummyQuiz.ModuleName));
            Assert.That(root.GetAttribute("contextid"), Is.EqualTo("383")); // hardcoded in the QuizWriter
        }

        [Test]
        public void WriteQuiz_IncludesQuizElement_WithCorrectId()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The quiz element should be <quiz> with correct id attribute
            var quizNode = doc.DocumentElement?.SelectSingleNode("quiz");
            Assert.That(quizNode, Is.Not.Null, "Missing <quiz> element");
            Assert.That(quizNode.Attributes?["id"]?.Value, Is.EqualTo(dummyQuiz.ActivityId.ToString()));
        }

        [Test]
        public void WriteQuiz_WritesNameAndDescriptionElements()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Name and description elements should be present and have correct values
            var nameNode = doc.DocumentElement?.SelectSingleNode("quiz/name");
            Assert.That(nameNode, Is.Not.Null, "Missing <name>");
            Assert.That(nameNode.InnerText, Is.EqualTo(dummyQuiz.Name));

            var introNode = doc.DocumentElement?.SelectSingleNode("quiz/intro");
            Assert.That(introNode, Is.Not.Null, "Missing <intro>");
            Assert.That(introNode.InnerText, Is.EqualTo(dummyQuiz.Description));
        }

        [Test]
        public void WriteQuiz_IncludesQuestionElementsWithCorrectIds()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The question elements should be <question_instance> with correct id attributes
            var questionNodes = doc.DocumentElement?.SelectNodes("//quiz//question_instance");
            Assert.That(questionNodes, Is.Not.Null, "Missing <question> elements");
            Assert.That(questionNodes.Count, Is.EqualTo(dummyQuiz.Questions.Count));

            // Assert: The IDs of the <question_instance> elements should match the IDs of the questions
            for (int i = 0; i < questionNodes.Count; i++)
            {
                var questionNode = questionNodes[i];
                Assert.That(questionNode?.Attributes?["id"]?.Value, Is.EqualTo(dummyQuiz.Questions[i].QuestionId.ToString()));
            }
        }

        [Test]
        public void WriteQuiz_IncludesQuestionReferenceElementsWithCorrectIds()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The question reference elements should be <question_reference> with correct id attributes
            var questionRefNodes = doc.DocumentElement?.SelectNodes("//quiz//question_instance//question_reference");
            Assert.That(questionRefNodes, Is.Not.Null, "Missing <question_reference> elements");
            Assert.That(questionRefNodes.Count, Is.EqualTo(dummyQuiz.Questions.Count));

            // Assert: The IDs of the <question_reference> elements should match the IDs of the questions
            for (int i = 0; i < questionRefNodes.Count; i++)
            {
                var questionRefNode = questionRefNodes[i];
                Assert.That(questionRefNode?.Attributes?["id"]?.Value, Is.EqualTo(dummyQuiz.Questions[i].QuestionId.ToString()));
            }
        }

        [Test]
        public void WriteQuiz_QuestionReferenceReferencesCorrectQuestionBank()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The <questionbankentryid> elements inside the <question_reference> elements should reference the correct question bank
            var questionBankRefNodes = doc.DocumentElement?.SelectNodes("//quiz//question_instance//question_reference//questionbankentryid");
            Assert.That(questionBankRefNodes, Is.Not.Null, "Missing <questionbankentryid> elements");
            Assert.That(questionBankRefNodes.Count, Is.EqualTo(dummyQuiz.Questions.Count));

            // Assert: The IDs (= value) of the <questionbankentryid> elements should match the QuestionBank IDs stored in the questions
            for (int i = 0; i < questionBankRefNodes.Count; i++)
            {
                var questionBankRefNode = questionBankRefNodes[i];
                Assert.That(questionBankRefNode?.InnerText, Is.EqualTo(dummyQuiz.Questions[i].BankEntryId.ToString()));
            }
        }

        [Test]
        public void WriteQuiz_SetsTimeOpenAndTimeCloseToZero_WhenNull()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Arrange
            dummyQuiz.OpenTime = null;
            dummyQuiz.CloseTime = null;

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: timeopen and timeclose should be set to 0
            var openNode = doc.DocumentElement?.SelectSingleNode("quiz/timeopen");
            var closeNode = doc.DocumentElement?.SelectSingleNode("quiz/timeclose");

            Assert.That(openNode?.InnerText, Is.EqualTo("0"), "timeopen should default to 0");
            Assert.That(closeNode?.InnerText, Is.EqualTo("0"), "timeclose should default to 0");
        }

        [Test]
        public void WriteQuiz_SetsTimeOpenAndTimeCloseToUnixTimestamp_WhenNotNull()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Arrange
            dummyQuiz.OpenTime = new DateTime(2025, 4, 20, 9, 0, 0);
            dummyQuiz.CloseTime = new DateTime(2025, 4, 20, 9, 30, 0);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: timeopen and timeclose should be set to the correct Unix timestamp
            var openNode = doc.DocumentElement?.SelectSingleNode("quiz/timeopen");
            var closeNode = doc.DocumentElement?.SelectSingleNode("quiz/timeclose");

            Assert.That(openNode?.InnerText, Is.EqualTo("1234567890"), "timeopen should be set to 1234567890"); // from mocked DateTimeToUnixTimestamp
            Assert.That(closeNode?.InnerText, Is.EqualTo("1234567890"), "timeclose should be set to 1234567890"); // from mocked DateTimeToUnixTimestamp
        }

        [Test]
        public void WriteQuiz_LogsDebugOnSuccess()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz);

            // Assert: The logger should have been called with a debug message
            _logger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("quiz.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteQuiz_LogsErrorAndRethrows_OnException()
        {
            // Arrange
            QuizModel dummyQuiz = FakeQuizFactory.CreateBasicQuiz(1);

            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            var writer = new QuizWriter(throwingFactory, _logger, _generalHelper, _quizHelper);

            // Act and assert: The logger should have been called with an error message and the exception should be rethrown
            Assert.Throws<InvalidOperationException>(() => writer.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, dummyQuiz));
            _logger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("quiz.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
