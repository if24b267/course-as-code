using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.UnitTests.Fakes;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.File
{
    [TestFixture]
    public class ModuleWriterFileTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _fakeLogger;
        private IMoodleVersionHelper _fakeMoodleVersionHelper;
        private IGeneralHelper _fakeGeneralHelper;
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
            _fakeLogger = Substitute.For<ILogger>();

            // Create substitutes for helper dependencies
            _fakeMoodleVersionHelper = Substitute.For<IMoodleVersionHelper>();
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();

            // Configure the fake helpers to return fixed values
            _fakeMoodleVersionHelper.GetMoodleVersionStringFromEnum(Arg.Any<MoodleVersion>()).Returns("2024100700");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _fakeGeneralHelper.GetLanguageFromEnum(Arg.Any<Language>()).Returns("en");

            // Instantiate the ModuleWriterFile using Dependency Injection
            _moduleWriterFile = new ModuleWriterFile(_testFactory, _fakeMoodleVersionHelper, _fakeGeneralHelper, _fakeLogger);

            // Create a dummy course with backup metadata and one section.
            _dummyCourse = new Model.Course
            {
                BackupMetadata = new BackupMetadata { MoodleVersion = MoodleVersion.Moodle450 },
                Sections = new List<Section>()
            };

            _dummySection = new Section
            {
                Name = "Test Section",
                SystemDefaults = new Section.SystemDefaultsSection()
                {
                    SectionId = 5
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
                Tags = new List<string> { "filetag1", "filetag2" }
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
            Assert.That(moduleElement?.GetAttribute("id"), Is.EqualTo(_dummyFile.SystemDefaults.ModuleId.ToString()));
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
            Assert.That(moduleElement?["modulename"]?.InnerText, Is.EqualTo(_dummyFile.SystemDefaults.ModuleName));
            Assert.That(moduleElement?["sectionid"]?.InnerText, Is.EqualTo(_dummySection.SystemDefaults.SectionId.ToString()));
            Assert.That(moduleElement?["sectionnumber"]?.InnerText, Is.EqualTo(_dummyCourse.Sections.IndexOf(_dummySection).ToString()));
            
            // Inner elements from the mocked helper methods
            Assert.That(moduleElement?["added"]?.InnerText, Is.EqualTo("1234567890"));
            Assert.That(moduleElement?["lang"]?.InnerText, Is.EqualTo("en"));
        }

        [Test]
        public void WriteModule_WritesTagsSectionCorrectly()
        {
            // Act
            _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyFile);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that a <tags> element exists and contains the expected number <tag> elements
            var tagsElement = doc.DocumentElement?.SelectSingleNode("tags") as XmlElement;
            Assert.IsNotNull(tagsElement, "The <tags> element is missing");

            var tagNodes = tagsElement.SelectNodes("tag");
            Assert.IsNotNull(tagNodes, "No <tag> elements found despite having tags");
            Assert.That(tagNodes.Count, Is.EqualTo(_dummyFile.Tags!.Count));

            // Check details of the first tag
            var firstTag = tagNodes[0] as XmlElement;
            Assert.That(firstTag?.GetAttribute("id"), Is.EqualTo("0"));
            Assert.That(firstTag?["name"]?.InnerText, Is.EqualTo("filetag1"));
            Assert.That(firstTag?["rawname"]?.InnerText, Is.EqualTo("filetag1"));
        }

        [Test]
        public void WriteModule_ThrowsInvalidCastException_ForNonFileActivity()
        {
            // Arrange: Create a dummy activity that is not a MoodleFile.
            Activity nonFileActivity = new TextAndMediaArea
            {
                ActivityId = 20,
                Name = "Not a File Activity"
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
            _fakeLogger.Received().Debug(Arg.Is<string>(s =>
                s.Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) &&
                s.Contains("module.xml")));
        }

        [Test]
        public void WriteModule_LogsErrorOnException()
        {
            // Arrange: Use a throwing XML writer factory to simulate an exception.
            IXmlWriterFactory throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _moduleWriterFile = new ModuleWriterFile(throwingFactory, _fakeMoodleVersionHelper, _fakeGeneralHelper, _fakeLogger);

            // Act & Assert: Verify that an exception is thrown and an error is logged
            Assert.Throws<InvalidOperationException>(() =>
                _moduleWriterFile.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyFile));

            _fakeLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s =>
                s.Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) &&
                s.Contains("module.xml")));
        }
    }
}