using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityCommentsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<ActivityCommentsWriter> _testLogger;
        private ActivityCommentsWriter _activityCommentsWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<ActivityCommentsWriter>>();
            _activityCommentsWriter = new ActivityCommentsWriter(_testFactory, _testLogger);
        }

        [Test]
        public void WriteComments_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCommentsWriter.WriteComments(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteComments_RootElementIsComments()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCommentsWriter.WriteComments(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the root element is "comments"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("comments"));
        }

        [Test]
        public void WriteComments_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCommentsWriter.WriteComments(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/comments.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("dummyPath/comments.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteComments_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _activityCommentsWriter = new ActivityCommentsWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() => _activityCommentsWriter.WriteComments(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/comments.xml"
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("dummyPath/comments.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
