using System.Xml;
using CourseAsCode.Writers.UnitTests.Fakes;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityGradesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private ActivityGradesWriter _activityGradesWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _activityGradesWriter = new ActivityGradesWriter(_testFactory, _testLogger);
        }

        [Test]
        public void WriteGrade_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteGrade_RootElementIsActivityGradebook()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "activity_gradebook"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("activity_gradebook"));
        }

        [Test]
        public void WriteGrade_ContainsGradeItems()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <grade_items> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeItems = doc.DocumentElement?.SelectSingleNode("grade_items");
            Assert.IsNotNull(gradeItems, "Missing <grade_items> element");
        }

        [Test]
        public void WriteGrade_ContainsGradeLetters()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <grade_letters> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeLetters = doc.DocumentElement?.SelectSingleNode("grade_letters");
            Assert.IsNotNull(gradeLetters, "Missing <grade_letters> element");
        }

        [Test]
        public void WriteGrade_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradesWriter.WriteGrade(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/grades.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("dummyPath/grades.xml")));
        }

        [Test]
        public void WriteGrade_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _activityGradesWriter = new ActivityGradesWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _activityGradesWriter.WriteGrade(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/grades.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("dummyPath/grades.xml")));
        }
    }
}
