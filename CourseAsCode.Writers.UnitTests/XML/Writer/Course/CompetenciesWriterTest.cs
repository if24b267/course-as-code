using System.Xml;
using NSubstitute;
using NLog;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Writers.UnitTests.Fakes;

namespace CourseAsCode.Writers.UnitTests.XML.Writer.Course
{
    [TestFixture]
    public class CompetenciesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private CompetenciesWriter _competenciesWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _competenciesWriter = new CompetenciesWriter(_testFactory, _testLogger); // Dependency Injection
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteCompetencies_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteCompetencies_RootElementIsCourseCompetencies()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "course_competencies".
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("course_competencies"));
        }

        [Test]
        public void WriteCompetencies_ContainsCompetenciesSections()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Check that the <competencies> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var competenciesNode = doc.DocumentElement?.SelectSingleNode("competencies");
            Assert.IsNotNull(competenciesNode, "The <competencies> element is missing");
        }
        
        [Test]
        public void WriteCompetencies_ContainsUserCompetenciesSections()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();
            
            // Assert: Check that the <user_competencies> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var userCompetenciesNode = doc.DocumentElement?.SelectSingleNode("user_competencies");
            Assert.IsNotNull(userCompetenciesNode, "The <user_competencies> element is missing");
        }

        [Test]
        public void WriteCompetencies_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _competenciesWriter.WriteCompetencies(_dummyCourse, dummyOutputPath);

            // Assert: Verify that a debug log was recorded with a message containing "course/competencies.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("course/competencies.xml")));
        }

        [Test]
        public void WriteCompetencies_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _competenciesWriter = new CompetenciesWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _competenciesWriter.WriteCompetencies(_dummyCourse, dummyOutputPath));

            // Assert: Verify that an error log was recorded with a message containing "course/competencies.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("course/competencies.xml")));
        }
    }
}
