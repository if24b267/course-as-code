using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityXapistateWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<ActivityXapistateWriter> _testLogger;
        private ActivityXapistateWriter _xapistateWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // Fake XML writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<ActivityXapistateWriter>>();
            _xapistateWriter = new ActivityXapistateWriter(_testFactory, _testLogger);
        }

        [Test]
        public void WriteState_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _xapistateWriter.WriteState(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteState_RootElementIsStates()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _xapistateWriter.WriteState(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "states"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("states"));
        }

        [Test]
        public void WriteState_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _xapistateWriter.WriteState(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/xapistate.xml"
            _testLogger.Received().LogDebug(Arg.Is<string>(s => s.Contains("dummyPath/xapistate.xml")));
        }

        [Test]
        public void WriteState_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _xapistateWriter = new ActivityXapistateWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() => _xapistateWriter.WriteState(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/xapistate.xml"
            _testLogger.Received().LogError(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("dummyPath/xapistate.xml")));
        }
    }
}
