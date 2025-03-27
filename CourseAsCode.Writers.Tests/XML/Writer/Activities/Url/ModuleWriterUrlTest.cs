using System.Text.RegularExpressions;
using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.UnitTests.Fakes;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.Url
{
    [TestFixture]
    public class ModuleWriterUrlTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private IMoodleVersionHelper _fakeMoodleVersionHelper;
        private IGeneralHelper _fakeGeneralHelper;
        private ModuleWriterUrl _moduleWriterUrl;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";
        
        private Model.Course _dummyCourse;
        private Section _dummySection;
        private URL _dummyUrl;

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger>();

            // Create substitutes for helper dependencies
            _fakeMoodleVersionHelper = Substitute.For<IMoodleVersionHelper>();
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();

            // Configure the fake helpers to return fixed values
            _fakeMoodleVersionHelper.GetMoodleVersionStringFromEnum(Arg.Any<MoodleVersion>()).Returns("2024100700");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _fakeGeneralHelper.GetLanguageFromEnum(Arg.Any<Language>()).Returns("en");

            // Instantiate the ModuleWriterUrl with injected dependencies
            _moduleWriterUrl = new ModuleWriterUrl(_testFactory, _testLogger, _fakeMoodleVersionHelper, _fakeGeneralHelper);

            // Create a dummy course with backup metadata and one section
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

            // Create a dummy URL activity with required properties and tags.
            _dummyUrl = new URL
            {
                ActivityId = 10,
                Name = "Test URL",
                Description = "This is a test URL description",
                ExternalUrl = "https://example.com",
                TimeCreated = DateTime.Now,
                VisibleToStudents = true,
                GroupMode = GroupMode.NoGroups,
                DisplayDescription = true,
                Language = Language.English,
                Tags = new List<string> { "tag1", "tag2" }
            };
        }

        [Test]
        public void WriteModule_CreatesValidXml()
        {
            // Act
            _moduleWriterUrl.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyUrl);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
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
            _moduleWriterUrl.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyUrl);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The root element should be <module>
            Assert.IsNotNull(doc.DocumentElement, "The XML document should have a root element.");
            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("module"));
        }

        [Test]
        public void WriteModule_RootElementHasExpectedAttributes()
        {
            // Act
            _moduleWriterUrl.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyUrl);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: "id" attribute is set to the current module id (initially "0") and "version" attribute is set via the moodle version helper
            var moduleElement = doc.DocumentElement;

            Assert.That(moduleElement?.GetAttribute("id"), Is.EqualTo(_dummyUrl.SystemDefaults.ModuleId.ToString()));
            Assert.That(moduleElement?.GetAttribute("version"), Is.EqualTo("2024100700"));
        }
        
        [Test]
        public void WriteModule_ModuleElementHasExpectedChildElements()
        {
            // Act
            _moduleWriterUrl.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyUrl);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that (some of) the expected child elements are present in the <module> element
            // Full Schema Validation should be done in integration tests
            var moduleElement = doc.DocumentElement;

            // Verify inner elements from the dictionary
            Assert.That(moduleElement?["modulename"]?.InnerText, Is.EqualTo(_dummyUrl.SystemDefaults.ModuleName));
            Assert.That(moduleElement["sectionid"]?.InnerText, Is.EqualTo(_dummySection.SystemDefaults.SectionId.ToString()));
            
            // Inner elements from the mocked helper methods
            Assert.That(moduleElement["added"]?.InnerText, Is.EqualTo("1234567890"));
            Assert.That(moduleElement["lang"]?.InnerText, Is.EqualTo("en"));
        }

        [Test]
        public void WriteModule_WritesTagsSectionCorrectly()
        {
            // Act
            _moduleWriterUrl.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyUrl);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that a <tags> element exists and contains the expected number of <tag> elements
            var tagsElement = doc.DocumentElement?.SelectSingleNode("tags") as XmlElement;
            Assert.IsNotNull(tagsElement, "The <tags> element is missing");

            var tagNodes = tagsElement.SelectNodes("tag");
            Assert.IsNotNull(tagNodes, "No <tag> elements found despite having tags");
            Assert.That(tagNodes.Count, Is.EqualTo(_dummyUrl.Tags!.Count));

            // Check details of the first tag.
            var firstTag = tagNodes[0] as XmlElement;
            Assert.That(firstTag?.GetAttribute("id"), Is.EqualTo("0"));
            Assert.That(firstTag["name"]?.InnerText, Is.EqualTo("tag1"));
            Assert.That(firstTag["rawname"]?.InnerText, Is.EqualTo("tag1"));
        }

        [Test]
        public void WriteModule_ThrowsInvalidCastException_ForNonUrlActivity()
        {
            // Arrange: Create a dummy activity that is not a URL.
            Activity nonUrlActivity = new TextAndMediaArea()
            {
                Name = "This activity is not an URL",
                ActivityId = 20
            };

            // Act & Assert: Verify that passing a non-URL activity throws an InvalidCastException.
            Assert.Throws<InvalidCastException>(() =>
                _moduleWriterUrl.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, nonUrlActivity));
        }

        [Test]
        public void WriteModule_LogsDebugOnSuccess()
        {
            // Act
            _moduleWriterUrl.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyUrl);

            // Assert: Verify that a debug log was recorded containing "module.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) &&
                                                               s.Contains("module.xml")));
        }

        [Test]
        public void WriteModule_LogsErrorOnException()
        {
            // Arrange: Use a throwing XML writer factory to simulate an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _moduleWriterUrl = new ModuleWriterUrl(throwingFactory, _testLogger, _fakeMoodleVersionHelper, _fakeGeneralHelper);
            
            // Act & Assert: Verify that an exception is thrown and an error is logged
            Assert.Throws<InvalidOperationException>(() =>
                _moduleWriterUrl.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyUrl));
            
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) &&
                                                                                   s.Contains("module.xml")));
        }
    }
}
