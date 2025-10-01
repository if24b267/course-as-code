using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Tests.Helper
{
    [TestFixture]
    public class YamlFileReaderHelperTests : TestBase
    {
        private IYamlFileReaderHelper _yamlReaderHelper;

        [SetUp]
        public void Setup()
        {
            _yamlReaderHelper = new YamlFileReaderHelper();
        }

        [Test]
        public void Deserialize_ValidYaml_ReturnsDeserializedObject()
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
        public void Deserialize_EmptyContent_ThrowsArgumentException()
        {
            // Arrange
            var yamlContent = string.Empty;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                 _yamlReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent));

            Assert.That(ex.Message, Does.Contain("YAML content cannot be null or empty"));
        }

        [Test]
        public void Deserialize_NullContent_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                 _yamlReaderHelper.DeserializeFromContent<YamlCourse>(null));

            Assert.That(ex.Message, Does.Contain("YAML content cannot be null or empty"));
        }

        [Test]
        public void Deserialize_InvalidYaml_ThrowsException()
        {
            // Arrange
            var yamlContent = "Invalid: yaml: content: - broken";

            // Act & Assert
            Assert.Throws<YamlDotNet.Core.SemanticErrorException>(() =>
                 _yamlReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent));
        }

        [Test]
        public void Deserialize_MissingRequiredProperty_ThrowsValidationException()
        {
            // Arrange
            var yamlContent = @"
ShortName: TC
Visible: true
StartDate: 01-01-2024 00:00
";

            // Act & Assert
            var ex = Assert.Throws<YamlException>(() =>
                 _yamlReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent));

            Assert.That(ex.Message, Does.Contain("The CourseName field is required"));
        }

        [Test]
        public void DeserializeFile_ValidFile_ReturnsDeserializedObject()
        {
            // Arrange
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: true
StartDate: 01-01-2024 00:00
";
            var filePath = CreateTestYamlFile("test.yaml", yamlContent);

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

        [Test]
        public void DeserializeFile_EmptyFilePath_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(string.Empty));

            Assert.That(ex.Message, Does.Contain("File path cannot be null or empty"));
        }

        [Test]
        public void DeserializeFile_NullFilePath_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(null));

            Assert.That(ex.Message, Does.Contain("File path cannot be null or empty"));
        }

        [Test]
        public void DeserializeFile_FileNotFound_ThrowsFileNotFoundException()
        {
            // Arrange
            var nonExistentFilePath = Path.Combine(TempDirectory, "nonexistent.yaml");

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() =>
                 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(nonExistentFilePath));
        }

        [Test]
        public void ValidateObject_ValidObject_ReturnsNoErrors()
        {
            // Arrange
            var yamlContent = @"
Name: Test URL
URL: https://example.com
";
            var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

            // Act
            var validationResults = _yamlReaderHelper.ValidateObject(result);

            // Assert
            Assert.That(validationResults, Is.Empty);
        }

        [Test]
        public void ValidateObject_InvalidUrl_ReturnsValidationErrors()
        {
            // Arrange
            var yamlContent = @"
Name: Test URL
URL: not-a-valid-url
";
            var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

            // Act
            var validationResults = _yamlReaderHelper.ValidateObject(result);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(validationResults, Is.Not.Empty);
                Assert.That(validationResults[0].ErrorMessage, Does.Contain("invalid URL format"));
            });
        }

        [Test]
        public void ValidateObjectAndThrow_InvalidObject_ThrowsExceptionOnWindows()
        {
            // Arrange
            var yamlContent = @"
Name: Test File
FilePath: invalid*chars?in:path
";
            var result = _yamlReaderHelper.DeserializeFromContent<YamlFile>(yamlContent);

            if (OperatingSystem.IsWindows())
            {
                // Act & Assert
                var ex = Assert.Throws<YamlException>(() => _yamlReaderHelper.ValidateObjectAndThrow(result));
                Assert.That(ex.Message, Does.Contain("invalid characters"));
            }
        }

        [Test]
        public void DeserializeWithEnhancedValidation_InvalidUrl_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
Name: Test URL
URL: not-a-valid-url
";

            var deserialize = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

            // Act & Assert
            var ex = Assert.Throws<YamlException>(() =>
                 _yamlReaderHelper.ValidateObjectAndThrow(deserialize));

            Assert.That(ex.Message, Does.Contain("invalid URL format"));
        }

        [Test]
        public void DeserializeFileWithEnhancedValidation_ValidYaml_SuccessfullyDeserializes()
        {
            // Arrange
            var yamlContent = @"
Name: Valid URL
URL: https://example.com
";
            var filePath = CreateTestYamlFile("valid_url.yaml", yamlContent);

            // Act
            var result = _yamlReaderHelper.DeserializeFromFile<YamlUrl>(filePath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Valid URL"));
                Assert.That(result.URL, Is.EqualTo("https://example.com"));
            });
        }
    }
}
