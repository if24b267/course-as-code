using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityFiltersWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private ActivityFiltersWriter _activityFiltersWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _activityFiltersWriter = new ActivityFiltersWriter(_testFactory, _testLogger);
        }

        [Test]
        public void WriteFilters_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityFiltersWriter.WriteFilters(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteFilters_RootElementIsFilters()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityFiltersWriter.WriteFilters(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "filters"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("filters"));
        }

        [Test]
        public void WriteFilters_ContainsFilterActives()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityFiltersWriter.WriteFilters(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <filter_actives> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var filterActives = doc.DocumentElement?.SelectSingleNode("filter_actives");
            Assert.IsNotNull(filterActives, "Missing <filter_actives> element.");
        }

        [Test]
        public void WriteFilters_ContainsFilterConfigs()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityFiltersWriter.WriteFilters(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <filter_configs> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var filterConfigs = doc.DocumentElement?.SelectSingleNode("filter_configs");
            Assert.IsNotNull(filterConfigs, "Missing <filter_configs> element.");
        }

        [Test]
        public void WriteFilters_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityFiltersWriter.WriteFilters(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/filters.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("dummyPath/filters.xml")));
        }

        [Test]
        public void WriteFilters_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _activityFiltersWriter = new ActivityFiltersWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _activityFiltersWriter.WriteFilters(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/filters.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("dummyPath/filters.xml")));
        }
    }
}
