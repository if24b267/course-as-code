using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Activities.CommonWriters;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityGradesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<ActivityGradesWriter> _testLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private ActivityGradesWriter _activityGradesWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<ActivityGradesWriter>>();

            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("70");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("123456789");

            _activityGradesWriter = new ActivityGradesWriter(_testFactory, _fakeGeneralHelper, _testLogger);
        }

        [Test]
        [Ignore("Needs rework for new Grading Implementation")]
        public void WriteGrade_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            // _activityGradesWriter.WriteGrade(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        [Ignore("Needs rework for new Grading Implementation")]
        public void WriteGrade_RootElementIsActivityGradebook()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            // _activityGradesWriter.WriteGrade(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "activity_gradebook"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("activity_gradebook"));
        }

        [Test]
        [Ignore("Needs rework for new Grading Implementation")]
        public void WriteGrade_ContainsGradeItems()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            // _activityGradesWriter.WriteGrade(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <grade_items> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeItems = doc.DocumentElement?.SelectSingleNode("grade_items");
            Assert.That(gradeItems, Is.Not.Null, "Missing <grade_items> element");
        }

        [Test]
        [Ignore("Needs rework for new Grading Implementation")]
        public void WriteGrade_ContainsGradeLetters()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            //_activityGradesWriter.WriteGrade(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <grade_letters> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeLetters = doc.DocumentElement?.SelectSingleNode("grade_letters");
            Assert.That(gradeLetters, Is.Not.Null, "Missing <grade_letters> element");
        }

        [Test]
        [Ignore("Needs rework for new Grading Implementation")]
        public void WriteGrade_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            // _activityGradesWriter.WriteGrade(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/grades.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("dummyPath/grades.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        [Ignore("Needs rework for new Grading Implementation")]
        public void WriteGrade_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _activityGradesWriter = new ActivityGradesWriter(throwingFactory, _fakeGeneralHelper, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            // Assert.Throws<InvalidOperationException>(() =>
            //    _activityGradesWriter.WriteGrade(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/grades.xml"
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("dummyPath/grades.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
