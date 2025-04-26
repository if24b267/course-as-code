using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Tests.Helper
{
    [TestFixture]
    public class EnhancedYamlFileReaderHelperTests : TestBase
    {
        private IYamlFileReaderHelper _yamlReaderHelper;

        [SetUp]
        public void Setup()
        {
            _yamlReaderHelper = new YamlFileReaderHelper();
        }

        [Test]
        public void DeserializeWithEnhancedValidation_ValidYaml_ReturnsDeserializedObject()
        {
            // Arrange
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: true
StartDate: 01-01-2024 00:00
";

            // Act
            var result = _yamlReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.CourseName, Is.EqualTo("Test Course"));
                Assert.That(result.ShortName, Is.EqualTo("TC"));
                Assert.That(result.Visible, Is.True);
                Assert.That(result.StartDate.Date, Is.EqualTo(new DateTime(2024, 1, 1).Date));
            });
        }

        [Test]
        public void DeserializeWithEnhancedValidation_InvalidUrl_ThrowsYamlException()
        {
            // Arrange - an invalid URL format
            var yamlContent = @"
Name: Test Name
URL: not-a-valid-url
";

            // Act & Assert
            var deserialize = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);
            var ex = Assert.Throws<YamlException>(() =>
                _yamlReaderHelper.ValidateObjectAndThrow(deserialize));

            Assert.That(ex.Message, Does.Contain("invalid URL format"));
        }

        [Test]
        public void DeserializeWithEnhancedValidation_ExtremelyLongString_ThrowsYamlException()
        {
            // Arrange - create very long content that should be rejected
            var longName = new string('X', 11000);
            var yamlContent = $@"
CourseName: {longName}
ShortName: TC
";

            var deserialize = _yamlReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent);

            // Act & Assert
            var ex = Assert.Throws<YamlException>(() =>
                _yamlReaderHelper.ValidateObjectAndThrow(deserialize));
            Assert.That(ex.Message, Does.Contain("exceeds the maximum allowed length"));
        }

        [Test]

        // [Ignore("Broken because of linux paths not being validated")]
        public void DeserializeWithEnhancedValidation_InvalidFilePath_ThrowsYamlExceptionOnWindows()
        {
            // Arrange - invalid file path with prohibited characters
            var yamlContent = @"
Name: Test File
FilePath: file:with:invalid*characters?in<path>
";

            // Act & Assert
            var deserialize = _yamlReaderHelper.DeserializeFromContent<YamlFile>(yamlContent);
            if (OperatingSystem.IsWindows())
            {
                var ex = Assert.Throws<YamlException>(() =>
                    _yamlReaderHelper.ValidateObjectAndThrow(deserialize));
                Assert.That(ex.Message, Does.Contain("invalid characters"));
            }
        }

        [Test]
        [Ignore("We/Yamldotnet does not support merged keys properly")]
        public void DeserializeWithEnhancedValidation_ValidButComplexYaml_DeserializesCorrectly()
        {
            // Arrange - complex but valid YAML with anchors, aliases, and multiline strings
            var yamlContent = @"
defaults: &defaults
  VisibleToStudents: true
  Summary: |
    This is a multiline summary
    with multiple paragraphs.

SectionName: Complex Valid Section
<<: *defaults
Activities:
  - ActivityFilePath: activity1.yaml
  - ActivityFilePath: activity2.yaml
";

            // Act
            var result = _yamlReaderHelper.DeserializeFromContent<YamlSection>(yamlContent);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SectionName, Is.EqualTo("Complex Valid Section"));
                Assert.That(result.VisibleToStudents, Is.True);
                Assert.That(result.Summary, Does.Contain("This is a multiline summary"));
                Assert.That(result.Activities, Has.Count.EqualTo(2));
            });
        }

        [Test]
        public void Deserialize_ThenValidate_CatchesValidationErrors()
        {
            // Arrange - YAML with invalid URL will parse but fail validation
            var yamlContent = @"
Name: Test URL
URL: not-a-valid-url
";

            // Act - First deserialize (should succeed)
            var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

            // Then validate (should return errors)
            var validationResults = _yamlReaderHelper.ValidateObject(result);

            // Assert
            Assert.Multiple(() =>
            {
                // Deserialization worked
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Test URL"));
                Assert.That(result.URL, Is.EqualTo("not-a-valid-url"));

                // But validation found errors
                Assert.That(validationResults, Is.Not.Empty);
                Assert.That(validationResults[0].ErrorMessage, Does.Contain("invalid URL format"));
            });
        }

        [Test]
        public void DeserializeFileWithEnhancedValidation_ValidFile_ReturnsDeserializedObject()
        {
            // Arrange
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: true
StartDate: 01-01-2024 00:00
";
            var filePath = CreateTestYamlFile("valid_course.yaml", yamlContent);

            // Act
            var result = _yamlReaderHelper.DeserializeFromFile<YamlCourse>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.CourseName, Is.EqualTo("Test Course"));
                Assert.That(result.ShortName, Is.EqualTo("TC"));
                Assert.That(result.Visible, Is.True);
                Assert.That(result.StartDate.Date, Is.EqualTo(new DateTime(2024, 1, 1).Date));
            });
        }
    }
}
