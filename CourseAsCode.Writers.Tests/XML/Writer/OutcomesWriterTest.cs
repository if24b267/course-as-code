using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class OutcomesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<OutcomesWriter> _testLogger;
        private OutcomesWriter _outcomesWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<OutcomesWriter>>();
            _outcomesWriter = new OutcomesWriter(_testFactory, _testLogger); // Dependency Injection
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteOutcomes_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _outcomesWriter.WriteOutcomes(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteOutcomes_RootElementIsOutcomesDefinition()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _outcomesWriter.WriteOutcomes(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the root element is "outcomes_definition"
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("outcomes_definition"));
        }

        [Test]
        public void WriteOutcomes_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _outcomesWriter.WriteOutcomes(_dummyCourse, dummyOutputPath);

            // Assert: Verify that a debug log was written with a message containing "outcomes.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("outcomes.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteOutcomes_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _outcomesWriter = new OutcomesWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: An exception should be thrown
            Assert.Throws<InvalidOperationException>(() =>
                _outcomesWriter.WriteOutcomes(_dummyCourse, dummyOutputPath));

            // Verify that an error log was written containing "outcomes.xml"
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("outcomes.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
