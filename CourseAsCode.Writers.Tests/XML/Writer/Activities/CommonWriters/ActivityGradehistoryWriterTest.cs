using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityGradehistoryWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<ActivityGradehistoryWriter> _testLogger;
        private ActivityGradehistoryWriter _activityGradehistoryWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<ActivityGradehistoryWriter>>();
            _activityGradehistoryWriter = new ActivityGradehistoryWriter(_testFactory, _testLogger);
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteGradeHistory_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradehistoryWriter.WriteGradeHistory(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteGradeHistory_RootElementIsGradeHistory()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradehistoryWriter.WriteGradeHistory(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "grade_history"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("grade_history"));
        }

        [Test]
        public void WriteGradeHistory_ContainsGradeGradesSection()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradehistoryWriter.WriteGradeHistory(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <grade_grades> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeGradesNode = doc.DocumentElement?.SelectSingleNode("grade_grades");
            Assert.That(gradeGradesNode, Is.Not.Null, "The <grade_grades> element is missing");
        }

        [Test]
        public void WriteGradeHistory_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityGradehistoryWriter.WriteGradeHistory(_dummyCourse, dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/grade_history.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("dummyPath/grade_history.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteGradeHistory_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _activityGradehistoryWriter = new ActivityGradehistoryWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _activityGradehistoryWriter.WriteGradeHistory(_dummyCourse, dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/grade_history.xml"
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("dummyPath/grade_history.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
