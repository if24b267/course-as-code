using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.Quiz
{
    [TestFixture]
    public class ModuleWriterQuizTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<ModuleWriterQuiz> _fakeLogger;
        private IMoodleVersionHelper _fakeMoodleVersionHelper;
        private IGeneralHelper _fakeGeneralHelper;
        private ITagsWriter _fakeTagsWriter;
        private ModuleWriterQuiz _moduleWriterQuiz;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        private Model.Course _dummyCourse;
        private Section _dummySection;
        private Model.Activities.Resources.Quiz _dummyQuiz;

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory (for capturing the XML output) and logger substitute
            _testFactory = new XmlWriterFactoryTest();
            _fakeLogger = Substitute.For<ILogger<ModuleWriterQuiz>>();
            _fakeTagsWriter = Substitute.For<ITagsWriter>();

            // Create substitutes for helper dependencies
            _fakeMoodleVersionHelper = Substitute.For<IMoodleVersionHelper>();
            _fakeGeneralHelper       = Substitute.For<IGeneralHelper>();

            // Configure the fake helpers to return fixed values
            _fakeMoodleVersionHelper.GetMoodleVersionStringFromEnum(Arg.Any<MoodleVersion>()).Returns("2024100700");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _fakeGeneralHelper.GetLanguageFromEnum(Arg.Any<Language>()).Returns("en");

            // Instantiate the ModuleWriterQuiz using the fake dependencies
            _moduleWriterQuiz = new ModuleWriterQuiz(_testFactory, _fakeLogger, _fakeMoodleVersionHelper, _fakeGeneralHelper, _fakeTagsWriter);

            // Create a dummy course with backup metadata and one section
            _dummyCourse = new Model.Course
            {
                BackupMetadata = new BackupMetadata { MoodleVersion = MoodleVersion.Moodle450 },
                Sections = new List<Section>()
            };

            _dummySection = new Section
            {
                Name = "Test Section",
                SystemDefaults = new Section.SystemDefaultsSection
                {
                    SectionId = 7
                }
            };
            _dummyCourse.Sections.Add(_dummySection);

            // Create a dummy quiz activity
            _dummyQuiz = new Model.Activities.Resources.Quiz
            {
                ActivityId = 42,
                Name = "Test Quiz",
                Description = "Test quiz description",
                TimeCreated = new DateTime(2025, 4, 20, 8, 0, 0),
                VisibleToStudents = true,
                DisplayDescription = true,
                GroupMode = GroupMode.NoGroups,
                Language = Language.English,
                Tags = new List<string> { "alpha", "beta" },
                ModuleId = 101,
                ModuleName = "quiz",
            };
        }

        [Test]
        public void WriteModule_CreatesValidXml()
        {
            // Act
            _moduleWriterQuiz.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

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
            _moduleWriterQuiz.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

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
            _moduleWriterQuiz.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);
            var moduleEl = doc.DocumentElement!;

            // Assert: "id" attribute is set to the quiz's module id and "version" attribute via the moodle version helper'
            Assert.That(moduleEl.GetAttribute("id"),    Is.EqualTo(_dummyQuiz.ModuleId.ToString()));
            Assert.That(moduleEl.GetAttribute("version"), Is.EqualTo("2024100700"));
        }

        [Test]
        public void WriteModule_ModuleElementHasExpectedChildElements()
        {
            // Act
            _moduleWriterQuiz.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that (some of) the expected child elements are present in the <module> element
            // Full Schema Validation should be done in integration tests
            var moduleElement = doc.DocumentElement!;

            // Verify inner elements from the dictionary
            Assert.That(moduleElement["modulename"]?.InnerText,  Is.EqualTo(_dummyQuiz.ModuleName));
            Assert.That(moduleElement["sectionid"]?.InnerText,   Is.EqualTo(_dummySection.SystemDefaults.SectionId.ToString()));
            Assert.That(moduleElement["sectionnumber"]?.InnerText, Is.EqualTo(_dummyCourse.Sections.IndexOf(_dummySection).ToString()));

            // Inner elements from the mocked helper methods
            Assert.That(moduleElement["added"]?.InnerText, Is.EqualTo("1234567890"));
            Assert.That(moduleElement["lang"]?.InnerText, Is.EqualTo("en"));
        }

        [Test]
        public void WriteModule_ThrowsInvalidCastException_ForNonQuizActivity()
        {
            // Arrange: Create a dummy activity that is not a Quiz.
            var notQuiz = new URL
            {
                ActivityId = 77,
                Name = "not a quiz",
                ModuleId = 102,
                ModuleName = "url",
            };

            // Act & Assert: Passing a non-Quiz activity should throw an InvalidCastException.
            Assert.Throws<InvalidCastException>(() =>
                _moduleWriterQuiz.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, notQuiz));
        }

        [Test]
        public void WriteModule_LogsDebugOnSuccess()
        {
            // Act
            _moduleWriterQuiz.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyQuiz);

            // Assert: Verify that the logger was called with the expected message
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
            // Arrange: Use a throwing XML writer factory to simulate an exception
            IXmlWriterFactory throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _moduleWriterQuiz = new ModuleWriterQuiz(throwingFactory, _fakeLogger, _fakeMoodleVersionHelper, _fakeGeneralHelper, _fakeTagsWriter);

            // Act & Assert: Verify that an exception is thrown and the logger is called with the expected message
            Assert.Throws<InvalidOperationException>(() =>
                _moduleWriterQuiz.WriteModule(DEFAULT_DUMMY_OUTPUT_PATH, _dummyCourse, _dummySection, _dummyQuiz));

            _fakeLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) && o.ToString().Contains("module.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
