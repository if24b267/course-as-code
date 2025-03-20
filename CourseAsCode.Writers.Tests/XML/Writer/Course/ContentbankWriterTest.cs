using System.Xml;
using NSubstitute;
using NLog;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Writers.UnitTests.Fakes;

namespace CourseAsCode.Writers.UnitTests.XML.Writer.Course
{
    [TestFixture]
    public class ContentbankWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private ContentbankWriter _contentbankWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _contentbankWriter = new ContentbankWriter(_testFactory, _testLogger);
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteContentbank_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _contentbankWriter.WriteContentbank(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteContentbank_RootElementIsContents()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _contentbankWriter.WriteContentbank(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the root element is "contents"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("contents"));
        }
        
        [Test]
        public void WriteContentbank_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _contentbankWriter.WriteContentbank(_dummyCourse, dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "course/contentbank.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("course/contentbank.xml")));
        }

        [Test]
        public void WriteContentbank_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _contentbankWriter = new ContentbankWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _contentbankWriter.WriteContentbank(_dummyCourse, dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "course/contentbank.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("course/contentbank.xml")));
        }
    }
}
