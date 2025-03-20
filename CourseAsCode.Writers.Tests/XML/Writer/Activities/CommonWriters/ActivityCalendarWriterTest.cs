using System.Xml;
using NSubstitute;
using NLog;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.UnitTests.Fakes;

namespace CourseAsCode.Writers.UnitTests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityCalendarWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private ActivityCalendarWriter _activityCalendarWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _activityCalendarWriter = new ActivityCalendarWriter(_testFactory, _testLogger);
        }

        [Test]
        public void WriteCalendar_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCalendarWriter.WriteCalendar(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteCalendar_RootElementIsEvents()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCalendarWriter.WriteCalendar(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "events"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("events"));
        }

        [Test]
        public void WriteCalendar_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityCalendarWriter.WriteCalendar(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/calendar.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("dummyPath/calendar.xml")));
        }

        [Test]
        public void WriteCalendar_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _activityCalendarWriter = new ActivityCalendarWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _activityCalendarWriter.WriteCalendar(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/calendar.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("dummyPath/calendar.xml")));
        }
    }
}
