using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class CompletionWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private CompletionWriter _testCompletionWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _testCompletionWriter = new CompletionWriter(_testFactory, _testLogger); // Inject dependencies
        }

        [Test]
        public void WriteCompletion_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";
            var dummyCourse = new Model.Course();

            // Act
            _testCompletionWriter.WriteCompletion(dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteCompletion_RootElementIsCorrect()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";
            var dummyCourse = new Model.Course();

            // Act
            _testCompletionWriter.WriteCompletion(dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The root element should be "course_completion"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement);
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("course_completion"));
        }

        [Test]
        public void WriteCompletion_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";
            var dummyCourse = new Model.Course();

            // Act
            _testCompletionWriter.WriteCompletion(dummyCourse, dummyOutputPath);

            // Assert: Verify that logger.Debug is called with a message containing "completion.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("completion.xml")));
        }

        [Test]
        public void WriteCompletion_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception.
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _testCompletionWriter = new CompletionWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";
            var dummyCourse = new Model.Course();

            // Act & Assert: An exception should be thrown
            Assert.Throws<InvalidOperationException>(() => _testCompletionWriter.WriteCompletion(dummyCourse, dummyOutputPath));

            // Verify that logger.Error is called with a message containing "completion.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("completion.xml")));
        }
    }
}
