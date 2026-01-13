using CourseAsCode.Model.Activities;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.YAML.Readers
{
    [TestFixture]
    public class YAMLSectionReaderTests : TestBase
    {
        private ISectionReader _sectionReader;
        private IActivityReader _activityReader;

        [SetUp]
        public void Setup()
        {
            _activityReader = new YamlActivityReader(new YamlFileReader(new YamlFileReaderHelper()),
                new YamlTextMediaAreaReader(), new YamlUrlReader(), new YamlFileReaderHelper(),
                new YamlQuizReader(new YamlFileReaderHelper()), new YamlAssignmentReader(new YamlFileReaderHelper()),
                new YamlWikiReader(new YamlFileReaderHelper(), new YamlWikiPageReader(new YamlFileReaderHelper())),
                new YamlPageReader(new YamlFileReaderHelper()), new YamlChoiceReader(new YamlFileReaderHelper()),
                new YamlFeedbackReader(new YamlFileReaderHelper()));
            _sectionReader = new YamlSectionReader(_activityReader, new YamlFileReaderHelper());
            ValueKeeper.CurrentSectionId = 1;
        }

        [Test]
        public void Read_FileDoesNotExist_ReturnsNull()
        {
            // Arrange
            var nonExistentPath = Path.Combine(TempDirectory, "nonexistent.yaml");

            // Act
            var result = _activityReader.Read(nonExistentPath);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Read_ValidSectionWithoutActivities_ReturnsSectionObject()
        {
            // Arrange
            var yamlContent = @"
SectionName: Test Section
Summary: Test section summary
VisibleToStudents: true
";
            var filePath = CreateTestYamlFile("section.yaml", yamlContent);

            // Act
            var result = _sectionReader.Read(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Test Section"));
                Assert.That(result.Summary, Is.EqualTo("Test section summary"));
                Assert.That(result.VisibleToStudents, Is.True);
                Assert.That(result.Activities, Is.Empty);
                Assert.That(result.SystemDefaults.SectionId, Is.EqualTo(1));
                Assert.That(result.SystemDefaults.PathToSection, Is.EqualTo("sections/section_1"));
            });
        }

        [Test]
        public void Read_ValidSection_SetsSystemDefaults()
        {
            // Arrange
            var yamlContent = @"
SectionName: Custom Section
Summary: Custom section summary
VisibleToStudents: false
";
            var filePath = CreateTestYamlFile("section.yaml", yamlContent);

            // Set section ID to verify it's used
            ValueKeeper.CurrentSectionId = 42;

            // Act
            var result = _sectionReader.Read(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SystemDefaults.SectionId, Is.EqualTo(42));
                Assert.That(result.SystemDefaults.PathToSection, Is.EqualTo("sections/section_42"));
            });
        }

        // Test activity class for mocking
        private class TestActivity : Activity
        {
            public TestActivity()
            {
                Name = "Default Test Activity";
            }
        }
    }
}
