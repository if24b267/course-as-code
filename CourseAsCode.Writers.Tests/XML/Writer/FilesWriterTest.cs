using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class FilesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private IFileService _fileService;
        private ILogger _testLogger;
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
            _testLogger = Substitute.For<ILogger>();
            
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
                Name = "TestSection"
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
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
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
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("files.xml")));
        }

        [Test]
        public void WriteFiles_SkipsNonexistentFile()
        {
            // Arrange: Create a MoodleFile activity
            var moodleFile = new MoodleFile
            {
                Name = "TestFile",
                FilePath = "nonexistent.txt",
                FileName = "nonexistent.txt"
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
            Assert.IsNull(fileNode, "No <file> element should be written if file does not exist");
        }
        
        [Test]
        public void WriteFiles_SkippingNonexistentFile_LogsWarning()
        {
            // Arrange: Create a MoodleFile activity
            var moodleFile = new MoodleFile
            {
                Name = "TestFile",
                FilePath = "nonexistent.txt",
                FileName = "nonexistent.txt"
            };
            _dummyCourse.Sections[0].Activities.Add(moodleFile);

            // Configure IFileService so that the file does not exist
            _fileService.Exists(moodleFile.FilePath).Returns(false);

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);

            // Assert: Verify that a warning is logged
            _testLogger.Received().Warn(Arg.Is<string>(s => s.Contains("nonexistent.txt")));
        }

        [Test]
        public void WriteFiles_FileOpenThrows_SkipsFile()
        {
            // Arrange: Create a MoodleFile activity
            var moodleFile = new MoodleFile
            {
                Name = "TestFile",
                FilePath = "throwOnOpen.txt",
                FileName = "throwOnOpen.txt"
            };
            _dummyCourse.Sections[0].Activities.Add(moodleFile);
            
            // Configure IFileService: Exists returns true, but OpenRead throws.
            _fileService.Exists(moodleFile.FilePath).Returns(true);
            _fileService.OpenRead(moodleFile.FilePath).Returns(x => throw new IOException("Simulated OpenRead failure"));
            
            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            
            // Assert: No <file> element is written.
            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            var fileNode = doc.DocumentElement?.SelectSingleNode("file");
            Assert.IsNull(fileNode, "No <file> element should be written if opening the file fails");
        }
        
        [Test]
        public void WriteFiles_FileOpenThrows_LogsError()
        {
            // Arrange: Create a MoodleFile activity
            var moodleFile = new MoodleFile
            {
                Name = "TestFile",
                FilePath = "throwOnOpen.txt",
                FileName = "throwOnOpen.txt"
            };
            _dummyCourse.Sections[0].Activities.Add(moodleFile);
            
            // Configure IFileService: Exists returns true, but OpenRead throws.
            _fileService.Exists(moodleFile.FilePath).Returns(true);
            _fileService.OpenRead(moodleFile.FilePath).Returns(x => throw new IOException("Simulated OpenRead failure"));

            // Act
            _filesWriter.WriteFiles(_dummyCourse, DEFAULT_OUTPUT_PATH);

            // Assert: Verify that an error is logged
            _testLogger.Received().Error(Arg.Any<IOException>(), Arg.Is<string>(s => s.Contains("throwOnOpen.txt")));
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
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("files.xml")));
        }
    }
}
