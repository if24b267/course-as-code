using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class FilesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private IFileService _fileService;
        private ILogger<FilesWriter> _testLogger;
        private IFileHelper _fakeFileHelper;
        private IGeneralHelper _fakeGeneralHelper;
        private FilesWriter _filesWriter;
        private Model.Course _dummyCourse;
        private const string DEFAULT_OUTPUT_PATH = "dummyOutput";

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _fileService = Substitute.For<IFileService>();
            _testLogger = Substitute.For<ILogger<FilesWriter>>();

            _fakeFileHelper = Substitute.For<IFileHelper>();
            _fakeFileHelper.GetComponentFromEnum(Arg.Any<Component>()).Returns("course");
            _fakeFileHelper.GetFileAreaFromEnum(Arg.Any<FileArea>()).Returns("content");
            _fakeFileHelper.GetLicenseFromEnum(Arg.Any<License>()).Returns("unknown");
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("70");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("123456789");


            _filesWriter = new FilesWriter(_testFactory, _testLogger, _fakeFileHelper, _fakeGeneralHelper, _fileService); // Inject dependencies

            // Create a dummy course with one section
            _dummyCourse = new Model.Course();
            var section = new Section
            {
                Activities = new List<Activity>(),
                Name = "TestSection",
            };
            _dummyCourse.Sections = new List<Section> { section };
        }

        [Test]
        public void WriteFiles_CreatesValidXml_WithEmptyCourse()
        {
            // Arrange: Course with no activities
            _dummyCourse.Sections[0].Activities = new List<Activity>();

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteFiles_RootElementIsFiles()
        {
            // Arrange: Course with no activities
            _dummyCourse.Sections[0].Activities = new List<Activity>();

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The XML root element should be "files"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("files"));
        }

        [Test]
        public void WriteFiles_LogsDebugOnSuccess()
        {
            // Arrange: Course with no activities
            _dummyCourse.Sections[0].Activities = new List<Activity>();

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);

            // Assert: Verify that logger.Debug was called with a message containing "files.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("files.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteFiles_SkipsNonexistentFile()
        {
            // Arrange: Create a MoodleFile activity
            var moodleFile = new MoodleFile
            {
                Name = "TestFile",
                FilePath = "nonexistent.txt",
                FileName = "nonexistent.txt",
                ModuleId = 1,
                ModuleName = "resource",
            };
            _dummyCourse.Sections[0].Activities.Add(moodleFile);

            // Configure IFileService so that the file does not exist
            _fileService.Exists(moodleFile.FilePath).Returns(false);

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The produced XML should not contain a <file> element
            var doc = new XmlDocument();
            doc.LoadXml(output);

            var fileNode = doc.DocumentElement?.SelectSingleNode("file");
            Assert.That(fileNode, Is.Null, "No <file> element should be written if file does not exist");
        }

        [Test]
        public void WriteFiles_SkippingNonexistentFile_LogsWarning()
        {
            // Arrange: Create a MoodleFile activity
            var moodleFile = new MoodleFile
            {
                Name = "TestFile",
                FilePath = "nonexistent.txt",
                FileName = "nonexistent.txt",
                ModuleId = 1,
                ModuleName = "resource",
            };
            _dummyCourse.Sections[0].Activities.Add(moodleFile);

            // Configure IFileService so that the file does not exist
            _fileService.Exists(moodleFile.FilePath).Returns(false);

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);

            // Assert: Verify that a warning is logged
            _testLogger.Received().Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("nonexistent.txt")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteFiles_FileOpenThrows_SkipsFile()
        {
            // Arrange: Create a MoodleFile activity
            var moodleFile = new MoodleFile
            {
                Name = "TestFile",
                FilePath = "throwOnOpen.txt",
                FileName = "throwOnOpen.txt",
                ModuleId = 1,
                ModuleName = "resource",
            };
            _dummyCourse.Sections[0].Activities.Add(moodleFile);

            // Configure IFileService: Exists returns true, but OpenRead throws.
            _fileService.Exists(moodleFile.FilePath).Returns(true);
            _fileService.OpenRead(moodleFile.FilePath).Returns(_ => throw new IOException("Simulated OpenRead failure"));

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: No <file> element is written.
            var doc = new XmlDocument();
            doc.LoadXml(output);

            var fileNode = doc.DocumentElement?.SelectSingleNode("file");
            Assert.That(fileNode, Is.Null, "No <file> element should be written if opening the file fails");
        }

        [Test]
        public void WriteFiles_FileOpenThrows_LogsError()
        {
            // Arrange: Create a MoodleFile activity
            var moodleFile = new MoodleFile
            {
                Name = "TestFile",
                FilePath = "throwOnOpen.txt",
                FileName = "throwOnOpen.txt",
                ModuleId = 1,
                ModuleName = "resource",
            };
            _dummyCourse.Sections[0].Activities.Add(moodleFile);

            // Configure IFileService: Exists returns true, but OpenRead throws.
            _fileService.Exists(moodleFile.FilePath).Returns(true);
            _fileService.OpenRead(moodleFile.FilePath).Returns(_ => throw new IOException("Simulated OpenRead failure"));

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);

            // Assert: Verify that an error is logged
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("throwOnOpen.txt")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteFiles_ExceptionInXmlWriter_LogsError()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _filesWriter = new FilesWriter(throwingFactory, _testLogger, _fakeFileHelper, _fakeGeneralHelper, _fileService); // Inject dependencies

            // Act & Assert: An exception should be thrown
            Assert.Throws<InvalidOperationException>(() =>
                _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH));

            // Assert: Verify that logger.Error was called with a message containing "files.xml"
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("files.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
