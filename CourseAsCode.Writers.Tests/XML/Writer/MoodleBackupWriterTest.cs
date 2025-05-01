using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class MoodleBackupWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<MoodleBackupWriter> _testLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private IMoodleVersionHelper _fakeMoodleVersionHelper;
        private ICourseHelper _fakeCourseHelper;
        private MoodleBackupWriter _moodleBackupWriter;
        private Model.Course _dummyCourse;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger<MoodleBackupWriter>>();

            // Use NSubstitute to create fake helper implementations
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeMoodleVersionHelper = Substitute.For<IMoodleVersionHelper>();
            _fakeCourseHelper = Substitute.For<ICourseHelper>();

            // Configure the fake helpers with predictable return values
            _fakeGeneralHelper.GenerateBackupName(Arg.Any<Model.Course>()).Returns("backup-moodle2-course-2-empco-20241225-0920.mbz");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");
            _fakeMoodleVersionHelper.GetMoodleVersionStringFromEnum(Arg.Any<MoodleVersion>()).Returns("2024100701");
            _fakeMoodleVersionHelper.GetMoodleReleaseStringFromEnum(Arg.Any<MoodleVersion>()).Returns("4.5.1 (Build: 2024100701)");
            _fakeCourseHelper.GetCourseFormatFromEnum(Arg.Any<CourseFormat>()).Returns("topics");

            // Instantiate the MoodleBackupWriter with injected dependencies
            _moodleBackupWriter = new MoodleBackupWriter(_testFactory, _testLogger, _fakeGeneralHelper, _fakeMoodleVersionHelper, _fakeCourseHelper);

            // Create a dummy course
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteMoodleBackup_GeneratesValidXml()
        {
            // Act
            _moodleBackupWriter.WriteMoodleBackup(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteMoodleBackup_RootElementIsMoodleBackup()
        {
            // Act
            _moodleBackupWriter.WriteMoodleBackup(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert that the root element is "moodle_backup"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("moodle_backup"));
        }

        [Test]
        public void WriteMoodleBackup_ContainsRequiredSections()
        {
            // Act
            _moodleBackupWriter.WriteMoodleBackup(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert that the required sections exist.
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var informationNode = doc.DocumentElement?.SelectSingleNode("information");
            Assert.IsNotNull(informationNode, "The <information> element is missing");

            var detailsNode = informationNode.SelectSingleNode("details");
            var contentsNode = informationNode.SelectSingleNode("contents");
            var settingsNode = informationNode.SelectSingleNode("settings");

            Assert.IsNotNull(detailsNode, "The <details> element is missing");
            Assert.IsNotNull(contentsNode, "The <contents> element is missing");
            Assert.IsNotNull(settingsNode, "The <settings> element is missing");
        }

        [Test]
        public void WriteMoodleBackup_LogsDebugOnSuccess()
        {
            // Act
            _moodleBackupWriter.WriteMoodleBackup(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert that a debug log is recorded containing "moodle_backup.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("moodle_backup.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteMoodleBackup_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _moodleBackupWriter = new MoodleBackupWriter(throwingFactory, _testLogger, _fakeGeneralHelper, _fakeMoodleVersionHelper, _fakeCourseHelper);

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _moodleBackupWriter.WriteMoodleBackup(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH));

            // Assert that an error log was recorded containing "moodle_backup.xml"
            _testLogger.Received().LogError(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("moodle_backup.xml")));
        }
    }
}
