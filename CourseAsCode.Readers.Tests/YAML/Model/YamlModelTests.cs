using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.Tests.YAML.Model
{
    [TestFixture]
    public class YamlModelTests : TestBase
    {
        private IYamlFileReaderHelper _yamlReader;

        [SetUp]
        public void Setup()
        {
            _yamlReader = new YamlFileReaderHelper();
        }

        [Test]
        public void YamlCourse_DeserializesCorrectly()
        {
            // Arrange
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Summary: Test course summary
Description: Test course description
Visible: true
Category: TestCategory
CategoryId: 1
CategoryDescription: Test category description
StartDate: 01-01-2024 00:00
EndDate: 31-12-2024 23:59
IdNumber: 12345
ShowGrades: true
GroupMode: 0
ForceGroupMode: false
EnableCompletionTracking: true
NumberOfAnnouncements: 5
ForcedLanguage: en
ShowActivityDates: true
ShowCompletionConditions: true
";
            var filePath = CreateTestYamlFile("course.yaml", yamlContent);

            // Act
            var result = _yamlReader.DeserializeFromFile<YamlCourse>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.CourseName, Is.EqualTo("Test Course"));
                Assert.That(result.ShortName, Is.EqualTo("TC"));
                Assert.That(result.Summary, Is.EqualTo("Test course summary"));
                Assert.That(result.Description, Is.EqualTo("Test course description"));
                Assert.That(result.Visible, Is.True);
                Assert.That(result.Category, Is.EqualTo("TestCategory"));
                Assert.That(result.CategoryId, Is.EqualTo(1));
                Assert.That(result.CategoryDescription, Is.EqualTo("Test category description"));
                Assert.That(result.StartDate.Date, Is.EqualTo(new DateTime(2024, 1, 1).Date));
                Assert.That(result.EndDate.Date, Is.EqualTo(new DateTime(2024, 12, 31).Date));
                Assert.That(result.IdNumber, Is.EqualTo(12345));
                Assert.That(result.ShowGrades, Is.True);
                Assert.That(result.GroupMode, Is.EqualTo(CourseAsCode.Model.Enums.GroupMode.NoGroups));
                Assert.That(result.ForceGroupMode, Is.False);
                Assert.That(result.EnableCompletionTracking, Is.True);
                Assert.That(result.NumberOfAnnouncements, Is.EqualTo(5));
                Assert.That(result.ForcedLanguage, Is.EqualTo("en"));
                Assert.That(result.ShowActivityDates, Is.True);
                Assert.That(result.ShowCompletionConditions, Is.True);
            });
        }

        [Test]
        public void YamlSectionsDefinition_DeserializesCorrectly()
        {
            // Arrange
            var yamlContent = @"
Sections:
  - SectionPath: Sections/Section1
  - SectionPath: Sections/Section2
";
            var filePath = CreateTestYamlFile("sections.yaml", yamlContent);

            // Act
            var result = _yamlReader.DeserializeFromFile<YamlSectionsDefinition>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Sections, Has.Count.EqualTo(2));
                Assert.That(result.Sections[0].SectionPath, Is.EqualTo("Sections/Section1"));
                Assert.That(result.Sections[1].SectionPath, Is.EqualTo("Sections/Section2"));
            });
        }

        [Test]
        public void YamlSection_DeserializesCorrectly()
        {
            // Arrange
            var yamlContent = @"
SectionName: Test Section
Summary: Test section summary
VisibleToStudents: true
Activities:
  - ActivityFilePath: Activities/url1.yaml
  - ActivityFilePath: Activities/file1.yaml
";
            var filePath = CreateTestYamlFile("section.yaml", yamlContent);

            // Act
            var result = _yamlReader.DeserializeFromFile<YamlSection>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SectionName, Is.EqualTo("Test Section"));
                Assert.That(result.Summary, Is.EqualTo("Test section summary"));
                Assert.That(result.VisibleToStudents, Is.True);
                Assert.That(result.Activities, Has.Count.EqualTo(2));
                Assert.That(result.Activities[0].ActivityFilePath, Is.EqualTo("Activities/url1.yaml"));
                Assert.That(result.Activities[1].ActivityFilePath, Is.EqualTo("Activities/file1.yaml"));
            });
        }

        [Test]
        public void YamlActivity_DeserializesCorrectly()
        {
            // Arrange
            var yamlContent = @"
Type: URL
Name: Test URL
Description: URL Description
URL: https://example.com
";
            var filePath = CreateTestYamlFile("url.yaml", yamlContent);

            // Act
            var result = _yamlReader.DeserializeFromFile<YamlActivity>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Type, Is.EqualTo(ActivityType.URL));
                Assert.That(result.Name, Is.EqualTo("Test URL"));
                Assert.That(result.Description, Is.EqualTo("URL Description"));
                //Assert.That(result.URL, Is.EqualTo("https://example.com"));
            });
        }

        [Test]
        public void YamlUrl_DeserializesCorrectly()
        {
            // Arrange
            var yamlContent = @"
Name: Test URL
Description: URL Description
URL: https://example.com
";
            var filePath = CreateTestYamlFile("url.yaml", yamlContent);

            // Act
            var result = _yamlReader.DeserializeFromFile<YamlUrl>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Test URL"));
                Assert.That(result.Description, Is.EqualTo("URL Description"));
                Assert.That(result.URL, Is.EqualTo("https://example.com"));
            });
        }

        [Test]
        public void YamlFile_DeserializesCorrectly()
        {
            // Arrange
            var yamlContent = @"
Name: Test File
Description: File Description
FilePath: files/test.txt
";
            var filePath = CreateTestYamlFile("file.yaml", yamlContent);

            // Act
            var result = _yamlReader.DeserializeFromFile<YamlFile>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Test File"));
                Assert.That(result.Description, Is.EqualTo("File Description"));
                Assert.That(result.FilePath, Is.EqualTo("files/test.txt"));
            });
        }

        [Test]
        public void YamlTextMediaArea_DeserializesCorrectly()
        {
            // Arrange
            var yamlContent = @"
Name: Test Label
Description: Label Description
Content:
  - Type: MARKDOWN
    ContentFilePath: This is a test
";
            var filePath = CreateTestYamlFile("label.yaml", yamlContent);

            // Act
            var result = _yamlReader.DeserializeFromFile<YamlTextMediaArea>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Test Label"));
                Assert.That(result.Description, Is.EqualTo("Label Description"));
                Assert.That(result.Content, Has.Count.EqualTo(1));

                var content1 = result.Content[0];
                Assert.That(content1.ContentFilePath, Is.EqualTo("This is a test"));
                Assert.That(content1.Type, Is.EqualTo(TextMediaContentType.MARKDOWN));
            });
        }
    }
}
