using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Tests.YAML
{
	[TestFixture]
	public class YamlFileParsingErrorTests : TestBase
	{
		private IYamlFileReaderHelper _yamlReaderHelper;

		[SetUp]
		public void Setup()
		{
			_yamlReaderHelper = new YamlFileReaderHelper();
		}

		[Test]
		public void DeserializeFile_EmptyFile_ThrowsArgumentException()
		{
			// Arrange
			var filePath = CreateTestYamlFile("empty.yaml", "");

			// Act & Assert
			var ex = Assert.Throws<ArgumentException>(() =>
				 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(filePath));

			Assert.That(ex.Message, Does.Contain("YAML content cannot be null or empty"));
		}

		[Test]
		public void DeserializeFile_WhitespaceOnlyFile_ThrowsArgumentException()
		{
			// Arrange
			var filePath = CreateTestYamlFile("whitespace.yaml", "   \n\t   \n   ");

			// Act & Assert
			var ex = Assert.Throws<ArgumentException>(() =>
				 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(filePath));

			Assert.That(ex.Message, Does.Contain("YAML content cannot be null or empty"));
		}

		[Test]
		public void DeserializeFile_BinaryFile_ThrowsYamlException()
		{
			// Arrange - create binary-like content
			byte[] binaryData = new byte[100];
			for (int i = 0; i < binaryData.Length; i++)
			{
				binaryData[i] = (byte)(i % 256);
			}

			string tempFilePath = Path.Combine(TempDirectory, "binary.yaml");
			File.WriteAllBytes(tempFilePath, binaryData);

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(tempFilePath));
		}

		[Test]
		public void DeserializeFile_InvalidYamlStructure_ThrowsYamlException()
		{
			// Arrange - complex invalid structure
			var yamlContent = @"
CourseName: Test Course
Sections:
  - SectionName: Section 1
    Activities:
      - {ActivityFilePath: activity1.yaml
      - ActivityFilePath: activity2.yaml}
";

			var filePath = CreateTestYamlFile("invalid_structure.yaml", yamlContent);

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReaderHelper.DeserializeFromFile<YamlSectionsDefinition>(filePath));
		}

		[Test]
		public void DeserializeFile_NonYamlFormatFile_ThrowsYamlException()
		{
			// Arrange - XML content in a YAML file
			var xmlContent = @"
<course>
    <courseName>Test Course</courseName>
    <shortName>TC</shortName>
    <visible>true</visible>
</course>";

			var filePath = CreateTestYamlFile("not_yaml.yaml", xmlContent);

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(filePath));
		}

		[Test]
		public void DeserializeFile_BrokenUnicodeSequence_ThrowsException()
		{
			// Arrange - broken Unicode sequence
			var yamlContent = "CourseName: \"Test course with broken unicode: \\uD83D\"";

			var filePath = CreateTestYamlFile("broken_unicode.yaml", yamlContent);

			// Act & Assert
			Assert.Throws<SyntaxErrorException>(() =>
				 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(filePath));
		}

		[Test]
		public void DeserializeFile_OverlappingAliasUsage_ThrowsYamlException()
		{
			// Arrange - invalid YAML with overlapping alias usage
			var yamlContent = @"
defaults: &defaults
  CourseName: Default Course
  
course:
  <<: *defaults
  <<: *defaults  # Overlapping merge key usage
  ShortName: TC
";

			var filePath = CreateTestYamlFile("overlapping_alias.yaml", yamlContent);

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReaderHelper.DeserializeFromFile<YamlCourse>(filePath));
		}

		// Tests for the new validation behavior

		[Test]
		public void DeserializeContent_WithInvalidUrl_DeserializesSuccessfully()
		{
			// Arrange
			var yamlContent = @"
Name: Test URL
URL: not-a-valid-url
";

			// Act - Deserialization should succeed
			var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Test URL"));
				Assert.That(result.URL, Is.EqualTo("not-a-valid-url"));
			});
		}

		[Test]
		public void ValidateObject_WithInvalidUrl_ReturnsValidationErrors()
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
		public void ValidateObjectAndThrow_WithInvalidUrl_ThrowsYamlException()
		{
			// Arrange
			var yamlContent = @"
Name: Test URL
URL: not-a-valid-url
";
			var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

			// Act & Assert
			var ex = Assert.Throws<YamlException>(() => _yamlReaderHelper.ValidateObjectAndThrow(result));
			Assert.That(ex.Message, Does.Contain("invalid URL format"));
		}

		[Test]
		public void DeserializeContent_WithInvalidCharsInFilePath_DeserializesSuccessfully()
		{
			// Arrange
			var yamlContent = @"
Name: Test File
FilePath: invalid*chars?in:path
";

			// Act - Deserialization should succeed
			var result = _yamlReaderHelper.DeserializeFromContent<YamlFile>(yamlContent);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Test File"));
				Assert.That(result.FilePath, Is.EqualTo("invalid*chars?in:path"));
			});
		}

		[Test]
		public void ValidateObject_WithInvalidCharsInFilePath_ReturnsValidationErrorsOnWindows()
		{
			// Arrange
			var yamlContent = @"
Name: Test File
FilePath: invalid*chars?in:path
";
			var result = _yamlReaderHelper.DeserializeFromContent<YamlFile>(yamlContent);

			// Act
			var validationResults = _yamlReaderHelper.ValidateObject(result);

			if (OperatingSystem.IsWindows())
			{
				// Assert
				Assert.Multiple(() =>
				{
					Assert.That(validationResults, Is.Not.Empty);
					Assert.That(validationResults[0].ErrorMessage, Does.Contain("invalid characters"));
				});
			}
		}

		[Test]
		public void ValidateObjectAndThrow_WithInvalidCharsInFilePath_ThrowsYamlExceptionOnWindows()
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
		public void Deserialize_WithMissingRequiredProperty_ThrowsYamlException()
		{
			// Arrange - missing Name property which is [Required]
			var yamlContent = @"
URL: https://example.com
";

			// Act & Assert
			// This should throw during deserialization, not during validation
			var ex = Assert.Throws<YamlException>(() =>
				 _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent));

			Assert.That(ex.Message, Does.Contain("Name"));
		}

		[Test]
		public void DeserializeWithEnhancedValidation_InvalidValues_ThrowsYamlException()
		{
			// Arrange - this has missing required field AND invalid URL
			var yamlContent = @"
URL: not-a-valid-url
";

			// Act & Assert - enhanced validation should catch all errors
			var ex = Assert.Throws<YamlException>(() =>
				 _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent));

			// It should first fail on the required field
			Assert.That(ex.Message, Does.Contain("Name"));
		}
	}
}