using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class ScalesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<ScalesWriter> _testLogger;
        private ScalesWriter _scalesWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<ScalesWriter>>();
            _scalesWriter = new ScalesWriter(_testFactory, _testLogger); // Use DI to inject the test factory and logger
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteScales_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _scalesWriter.WriteScales(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteScales_RootElementIsScalesDefinition()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _scalesWriter.WriteScales(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the root element is "scales_definition"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("scales_definition"));
        }

        [Test]
        public void WriteScales_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _scalesWriter.WriteScales(_dummyCourse, dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "scales.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("scales.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteScales_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _scalesWriter = new ScalesWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _scalesWriter.WriteScales(_dummyCourse, dummyOutputPath));

            // Verify that logger.Error was called with a message containing "scales.xml"
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("scales.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
