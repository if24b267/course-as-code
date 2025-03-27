using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer.Course;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Course
{
    [TestFixture]
    public class FiltersWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private FiltersWriter _filtersWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _filtersWriter = new FiltersWriter(_testFactory, _testLogger);
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteFilters_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _filtersWriter.WriteFilters(_dummyCourse, dummyOutputPath);
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
            _filtersWriter.WriteFilters(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "filters"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("filters"));
        }

        [Test]
        public void WriteFilters_ContainsFilterActives()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _filtersWriter.WriteFilters(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the <filter_actives> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var filterActives = doc.DocumentElement?.SelectSingleNode("filter_actives");
            Assert.IsNotNull(filterActives, "The <filter_actives> element is missing");
        }
        
        [Test]
        public void WriteFilters_ContainsFilterConfigs()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _filtersWriter.WriteFilters(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();
            
            // Assert: Verify that the <filter_configs> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var filterConfigs = doc.DocumentElement?.SelectSingleNode("filter_configs");
            Assert.IsNotNull(filterConfigs, "The <filter_configs> element is missing");
        }

        [Test]
        public void WriteFilters_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _filtersWriter.WriteFilters(_dummyCourse, dummyOutputPath);

            // Assert: Verify that a debug log was recorded with a message containing "course/filters.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("course/filters.xml")));
        }

        [Test]
        public void WriteFilters_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _filtersWriter = new FiltersWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _filtersWriter.WriteFilters(_dummyCourse, dummyOutputPath));

            // Assert: Verify that an error log was recorded with a message containing "course/filters.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("course/filters.xml")));
        }
    }
}
