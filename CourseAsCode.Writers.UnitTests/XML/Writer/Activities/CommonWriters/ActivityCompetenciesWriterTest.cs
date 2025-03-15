using System.Xml;
using NSubstitute;
using NLog;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using CourseAsCode.Writers.UnitTests.Fakes;

namespace CourseAsCode.Writers.UnitTests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityCompetenciesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private ActivityCompetenciesWriter _competenciesWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _competenciesWriter = new ActivityCompetenciesWriter(_testFactory, _testLogger);
        }

        [Test]
        public void WriteCompetencies_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteCompetencies_RootElementIsCourseModuleCompetencies()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the root element is "course_module_competencies"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("course_module_competencies"));
        }

        [Test]
        public void WriteCompetencies_ContainsCompetenciesElement()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a nested <competencies> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var competenciesNode = doc.DocumentElement?.SelectSingleNode("competencies");
            Assert.IsNotNull(competenciesNode, "The <competencies> element is missing");
        }

        [Test]
        public void WriteCompetencies_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/competencies.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("dummyPath/competencies.xml")));
        }

        [Test]
        public void WriteCompetencies_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _competenciesWriter = new ActivityCompetenciesWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _competenciesWriter.WriteCompetencies(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/competencies.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("dummyPath/competencies.xml")));
        }
    }
}
