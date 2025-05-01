using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Writer.Course;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityCompletionWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<ActivityCompletionWriter> _testLogger;
        private ActivityCompletionWriter _activityCompletionWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<ActivityCompletionWriter>>();
            _activityCompletionWriter = new ActivityCompletionWriter(_testFactory, _testLogger);
        }

        [Test]
        public void WriteCompletion_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCompletionWriter.WriteCompletion(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteCompletion_RootElementIsCompletions()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCompletionWriter.WriteCompletion(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The XML root element should be "completions"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("completions"));
        }

        [Test]
        public void WriteCompletion_ContainsCompletionViewsElement()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCompletionWriter.WriteCompletion(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <completionviews> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var completionViewsNode = doc.DocumentElement?.SelectSingleNode("completionviews");
            Assert.IsNotNull(completionViewsNode, "The <completionviews> element is missing");
        }

        [Test]
        public void WriteCompletion_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCompletionWriter.WriteCompletion(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/completion.xml"
            _testLogger.Received().LogDebug(Arg.Is<string>(s => s.Contains("dummyPath/completion.xml")));
        }

        [Test]
        public void WriteCompletion_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _activityCompletionWriter = new ActivityCompletionWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: An exception should be thrown
            Assert.Throws<InvalidOperationException>(() =>
                _activityCompletionWriter.WriteCompletion(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/completion.xml"
            _testLogger.Received().LogError(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("dummyPath/completion.xml")));
        }
    }
}
