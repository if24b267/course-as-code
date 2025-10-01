using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.File
{
    [TestFixture]
    public class ModuleWriterFileTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<ModuleWriterFile> _fakeLogger;
        private IMoodleVersionHelper _fakeMoodleVersionHelper;
        private IGeneralHelper _fakeGeneralHelper;
        private ITagsWriter _fakeTagsWriter;
        private ModuleWriterFile _moduleWriterFile;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        private Model.Course _dummyCourse;
        private Section _dummySection;
        private MoodleFile _dummyFile; // File (resource) activity

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory and logger substitute
            _testFactory = new XmlWriterFactoryTest();
            _fakeLogger = Substitute.For<ILogger<ModuleWriterFile>>();
            _fakeTagsWriter = Substitute.For<ITagsWriter>();

            // Create substitutes for helper dependencies
            _fakeMoodleVersionHelper = Substitute.For<IMoodleVersionHelper>();
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();

            // Configure the fake helpers to return fixed values
            _fakeMoodleVersionHelper.GetMoodleVersionStringFromEnum(Arg.Any<MoodleVersion>()).Returns("2024100700");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _fakeGeneralHelper.GetLanguageFromEnum(Arg.Any<Language>()).Returns("en");

            // Instantiate the ModuleWriterFile using Dependency Injection
            _moduleWriterFile = new ModuleWriterFile(_testFactory, _fakeMoodleVersionHelper, _fakeGeneralHelper, _fakeLogger, _fakeTagsWriter);

            // Create a dummy course with backup metadata and one section.
            _dummyCourse = new Model.Course
            {
                BackupMetadata = new BackupMetadata { MoodleVersion = MoodleVersion.Moodle450 },
                Sections = new List<Section>(),
            };

            _dummySection = new Section
            {
                Name = "Test Section",
                SystemDefaults = new Section.SystemDefaultsSection()
                {
                    SectionId = 5,
                },
            };
            _dummyCourse.Sections.Add(_dummySection);

            // Create a dummy file activity with some metadata
            _dummyFile = new MoodleFile
            {
                ActivityId = 10,
                Name = "Test File",
                Description = "Test file description",
                TimeCreated = new DateTime(2020, 1, 1),
                VisibleToStudents = true,
                GroupMode = GroupMode.NoGroups,
                DisplayDescription = true,
                Language = Language.English,
                Tags = new List<string> { "filetag1", "filetag2" },
                ModuleId = 1,
                ModuleName = "resource",
            };
        }

        [Test]
        public void WriteModule_CreatesValidXml()
        {
            // Act
            _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyFile);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (i.e. XML is well-formed)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteModule_RootElementIsModule()
        {
            // Act
            _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyFile);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The XML root element is <module>
            Assert.IsNotNull(doc.DocumentElement, "The XML document should have a root element.");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("module"));
        }

        [Test]
        public void WriteModule_RootElementHasExpectedAttributes()
        {
            // Act
            _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyFile);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);
            var moduleElement = doc.DocumentElement;

            // Assert: "id" attribute is set to the file's module id and "version" attribute via the moodle version helper
            Assert.That(moduleElement?.GetAttribute("id"), Is.EqualTo(_dummyFile.ModuleId.ToString()));
            Assert.That(moduleElement.GetAttribute("version"), Is.EqualTo("2024100700"));
        }

        [Test]
        public void WriteModule_ModuleElementHasExpectedChildElements()
        {
            // Act
            _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyFile);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that (some of) the expected child elements are present in the <module> element
            // Full Schema Validation should be done in integration tests
            var moduleElement = doc.DocumentElement;

            // Verify inner elements from the dictionary
            Assert.That(moduleElement?["modulename"]?.InnerText, Is.EqualTo(_dummyFile.ModuleName));
            Assert.That(moduleElement?["sectionid"]?.InnerText, Is.EqualTo(_dummySection.SystemDefaults.SectionId.ToString()));
            Assert.That(moduleElement?["sectionnumber"]?.InnerText, Is.EqualTo(_dummyCourse.Sections.IndexOf(_dummySection).ToString()));

            // Inner elements from the mocked helper methods
            Assert.That(moduleElement?["added"]?.InnerText, Is.EqualTo("1234567890"));
            Assert.That(moduleElement?["lang"]?.InnerText, Is.EqualTo("en"));
        }

        [Test]
        public void WriteModule_ThrowsInvalidCastException_ForNonFileActivity()
        {
            // Arrange: Create a dummy activity that is not a MoodleFile.
            Activity nonFileActivity = new TextAndMediaArea
            {
                ActivityId = 20,
                Name = "Not a File Activity",
                ModuleId = 2,
                ModuleName = "label",
            };

            // Act & Assert: Passing a non-MoodleFile activity should throw an InvalidCastException.
            Assert.Throws<InvalidCastException>(() =>
                _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, nonFileActivity));
        }

        [Test]
        public void WriteModule_LogsDebugOnSuccess()
        {
            // Act
            _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyFile);

            // Assert: Verify that a debug log was recorded containing "module.xml"
            _fakeLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) && o.ToString().Contains("module.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteModule_LogsErrorOnException()
        {
            // Arrange: Use a throwing XML writer factory to simulate an exception.
            IXmlWriterFactory throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _moduleWriterFile = new ModuleWriterFile(throwingFactory, _fakeMoodleVersionHelper, _fakeGeneralHelper, _fakeLogger, _fakeTagsWriter);

            // Act & Assert: Verify that an exception is thrown and an error is logged
            Assert.Throws<InvalidOperationException>(() =>
                _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyFile));

            _fakeLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) && o.ToString().Contains("module.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
