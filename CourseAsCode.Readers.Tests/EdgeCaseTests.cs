using CourseAsCode.Model.Activities;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Readers;
using Microsoft.Extensions.DependencyInjection;

namespace CourseAsCode.Readers.Tests
{
    [TestFixture]
    public class EdgeCaseTests : TestBase
    {
        private ServiceProvider _serviceProvider;
        private IActivityReader _mockActivityReader;
        private IFileReader _mockFileReader;
        private ITextAndMediaAreaReader _mockTextMediaReader;
        private IUrlReader _mockUrlReader;
        private ISectionReader _mockSectionReader;
        private ICourseReader _courseReader;

        [SetUp]
        public void Setup()
        {
            // Create mocks for all dependencies
            _mockActivityReader = Substitute.For<IActivityReader>();
            _mockFileReader = Substitute.For<IFileReader>();
            _mockTextMediaReader = Substitute.For<ITextAndMediaAreaReader>();
            _mockUrlReader = Substitute.For<IUrlReader>();
            _mockSectionReader = Substitute.For<ISectionReader>();

            // Create service collection and register mocks
            var services = new ServiceCollection();
            services.AddSingleton(_mockFileReader);
            services.AddSingleton(_mockTextMediaReader);
            services.AddSingleton(_mockUrlReader);
            services.AddSingleton(_mockActivityReader);
            services.AddSingleton(_mockSectionReader);

            var mockServiceProvider = Substitute.For<IServiceProvider>();
            mockServiceProvider.GetService(typeof(ISectionReader)).Returns(_mockSectionReader);

            _courseReader = new CourseReader(_mockSectionReader, mockServiceProvider);

            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider?.Dispose();
        }

        [Test]
        public void CourseReader_EmptySectionsYaml_ReturnsCourseWithNoSections()
        {
            // Arrange
            var testDir = CreateTestDirectory("emptySectionsTest");
            var configPath = Path.Combine(testDir, "config.yaml");
            var sectionsPath = Path.Combine(testDir, "sections.yaml");

            File.WriteAllText(configPath, "CourseName: Test Course");
            File.WriteAllText(sectionsPath, "Sections: []");

            // Act
            var result = _courseReader.Read(testDir, CourseFormat.YAML);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.FullName, Is.EqualTo("Test Course"));
                Assert.That(result.Sections, Is.Empty);
            });
        }

        [Test]
        public void SectionReader_EmptySectionYaml_ReturnsEmptySection()
        {
            // Arrange
            var yamlContent = "SectionName: Empty Section";
            var filePath = CreateTestYamlFile("emptySection.yaml", yamlContent);

            var reader = new YamlSectionReader(_mockActivityReader, new YamlFileReaderHelper());

            // Act
            var result = reader.Read(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Empty Section"));
                Assert.That(result.Activities, Is.Empty);
            });
        }

        [Test]
        public void CourseReader_NonexistentSectionPath_SkipsMissingSection()
        {
            // Arrange
            var testDir = CreateTestDirectory("nonexistentSectionTest");
            var configPath = Path.Combine(testDir, "config.yaml");
            var sectionsPath = Path.Combine(testDir, "sections.yaml");

            // Create valid section
            var section1Dir = CreateTestDirectory(Path.Combine("nonexistentSectionTest", "Sections", "Section1"));
            File.WriteAllText(Path.Combine(section1Dir, "section.yaml"), "SectionName: Valid Section");

            // Create config files
            File.WriteAllText(configPath, "CourseName: Test Course");
            File.WriteAllText(sectionsPath, @"
Sections:
  - SectionPath: Sections/Section1
  - SectionPath: Sections/NonexistentSection
");

            // Configure mock section reader to return a valid section only for the first path
            _mockSectionReader.Read(Arg.Is<string>(s => s.Contains("Section1")))
                .Returns(new Model.Section { Name = "Valid Section" });
            _mockSectionReader.Read(Arg.Is<string>(s => s.Contains("NonexistentSection")))
                .Returns((Model.Section)null);

            // Act
            var result = _courseReader.Read(testDir, CourseFormat.YAML);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Sections, Has.Count.EqualTo(1));
                Assert.That(result.Sections[0].Name, Is.EqualTo("Valid Section"));
            });
        }

        [Test]
        [Ignore("Currently we are not verifying the yaml indentation/format")]
        public void YamlActivityReader_MalformedYaml_ReturnsNull()
        {
            // Arrange
            var yamlContent = @"
Type: URL
Name: Test URL
-Invalid: indentation
";
            var filePath = CreateTestYamlFile("malformed.yaml", yamlContent);

            var reader = new YamlActivityReader(_mockFileReader, _mockTextMediaReader, _mockUrlReader, new YamlFileReaderHelper());

            // Act & Assert
            Assert.Throws<YamlDotNet.Core.YamlException>(() => reader.Read(filePath));
        }

        [Test]
        public void YamlSectionReader_CircularActivityReferences_HandlesGracefully()
        {
            // This test is to ensure that the reader handles circular references correctly
            // In a real situation, this would happen if an activity referenced itself

            // Arrange
            var yamlContent = @"
SectionName: Circular Section
Activities:
  - ActivityFilePath: circular.yaml
";
            var sectionFilePath = CreateTestYamlFile("circularSection.yaml", yamlContent);
            var activityFilePath = Path.Combine(Path.GetDirectoryName(sectionFilePath)!, "circular.yaml");

            File.WriteAllText(activityFilePath, @"
Type: URL
Name: Circular URL
URL: https://example.com
");

            // Create a section reader with a mocked activity reader that always returns null
            // to simulate a circular reference
            var mockCircularActivityReader = Substitute.For<IActivityReader>();
            mockCircularActivityReader.Read(Arg.Any<string>()).Returns((Activity?)null);

            var reader = new YamlSectionReader(mockCircularActivityReader, new YamlFileReaderHelper());

            // Act
            var result = reader.Read(sectionFilePath);

            // Assert
            // The section should be created but with an empty activities list
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Circular Section"));
                Assert.That(result.Activities, Is.Empty);
            });
        }
    }
}
