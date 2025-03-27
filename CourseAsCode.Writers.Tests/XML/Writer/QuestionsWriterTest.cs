using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class QuestionsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private QuestionsWriter _questionsWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _questionsWriter = new QuestionsWriter(_testFactory, _testLogger); // Dependency Injection
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteQuestions_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _questionsWriter.WriteQuestions(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteQuestions_RootElementIsQuestionCategories()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _questionsWriter.WriteQuestions(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The root element of the XML should be "question_categories"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("question_categories"));
        }

        [Test]
        public void WriteQuestions_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _questionsWriter.WriteQuestions(_dummyCourse, dummyOutputPath);

            // Assert: Verify that a debug log was recorded with a message containing "questions.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("questions.xml")));
        }

        [Test]
        public void WriteQuestions_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _questionsWriter = new QuestionsWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _questionsWriter.WriteQuestions(_dummyCourse, dummyOutputPath));

            // Verify that an error log was recorded containing "questions.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("questions.xml")));
        }
    }
}
