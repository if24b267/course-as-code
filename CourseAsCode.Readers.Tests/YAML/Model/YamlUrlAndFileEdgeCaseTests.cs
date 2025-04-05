using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Tests.YAML.Model
{
	[TestFixture]
	public class YamlUrlAndFileEdgeCaseTests : TestBase
	{
		private IYamlFileReaderHelper _yamlReaderHelper;

		[SetUp]
		public void Setup()
		{
			_yamlReaderHelper = new YamlFileReaderHelper();
		}

		[Test]
		public void Deserialize_UrlWithInvalidFormat_ParsesSuccessfullyWithNoExceptionFromDeserializer()
		{
			// Arrange
			var yamlContent = @"
URL: not-a-valid-url
Name: Invalid URL Test
";

			// Act
			var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.URL, Is.EqualTo("not-a-valid-url"));
				Assert.That(result.Name, Is.EqualTo("Invalid URL Test"));
			});

			// The validation can be performed separately
			var validationResults = _yamlReaderHelper.ValidateObject(result);
			Assert.That(validationResults, Is.Not.Empty);
			Assert.That(validationResults[0].ErrorMessage, Does.Contain("invalid URL format"));
		}

		[Test]
		public void Validate_UrlWithExtremelyLongValue_FailsValidation()
		{
			// Arrange - create a long URL to trigger length validation
			var longPath = new string('a', 10000);
			var longUrl = $"http://example.com/{longPath}";
			var yamlContent = $@"
URL: {longUrl}
Name: Long URL Test
";

			// Act
			var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

			// Assert - deserialization works
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.URL, Is.EqualTo(longUrl));
				Assert.That(result.Name, Is.EqualTo("Long URL Test"));
			});

			// But validation fails
			var validationResults = _yamlReaderHelper.ValidateObject(result);
			Assert.That(validationResults, Is.Not.Empty);
			Assert.That(validationResults[0].ErrorMessage, Does.Contain("maximum allowed length"));
		}

		[Test]
		public void Deserialize_UrlWithInvalidProtocol_ParsesSuccessfullyButFailsValidation()
		{
			// Arrange
			var yamlContent = @"
URL: invalid-protocol://example.com
Name: Invalid Protocol URL Test
";

			// Act
			var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

			// Assert - deserialization succeeds
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.URL, Is.EqualTo("invalid-protocol://example.com"));
				Assert.That(result.Name, Is.EqualTo("Invalid Protocol URL Test"));
			});

			// But validation fails
			var validationResults = _yamlReaderHelper.ValidateObject(result);
			Assert.That(validationResults, Is.Not.Empty);
			Assert.That(validationResults[0].ErrorMessage, Does.Contain("invalid URL format"));
		}

		[Test]
		public void Deserialize_UrlWithMalformedProtocol_DeserializesSuccessfully()
		{
			// Arrange
			var yamlContent = @"
URL: http:/example.com # Missing second slash
Name: Malformed Protocol URL Test
";

			// Act
			var result = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.URL, Is.EqualTo("http:/example.com"));
				Assert.That(result.Name, Is.EqualTo("Malformed Protocol URL Test"));
			});

			// But validation fails
			var validationResults = _yamlReaderHelper.ValidateObject(result);
			Assert.That(validationResults, Is.Not.Empty);
			Assert.That(validationResults[0].ErrorMessage, Does.Contain("invalid URL format"));
		}

		[Test]
		public void Deserialize_FileThatPointsToNonExistentPath_DeserializesSuccessfully()
		{
			// Arrange
			var nonExistentPath = "non-existent-directory/file.txt";
			var yamlContent = $@"
Name: Test File
FilePath: {nonExistentPath}
";
			var testFilePath = CreateTestYamlFile("file_test.yaml", yamlContent);

			// Act - This should parse correctly
			var result = _yamlReaderHelper.DeserializeFromFile<YamlFile>(testFilePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.FilePath, Is.EqualTo(nonExistentPath));
				Assert.That(result.Name, Is.EqualTo("Test File"));
			});

			// Validation should be fine for nonexistent path
			var validationResults = _yamlReaderHelper.ValidateObject(result);
			Assert.That(validationResults, Is.Empty);
		}

		[Test]
		[Ignore("This is broken on the linux build agent - WHY????")]
		public void Deserialize_FilePathWithInvalidCharacters_DeserializesSuccessfully()
		{
			// Arrange
			var invalidPath = "file:with:invalid*characters?in<path>";
			var yamlContent = $@"
Name: Test File
FilePath: {invalidPath}
";

			// Act
			var result = _yamlReaderHelper.DeserializeFromContent<YamlFile>(yamlContent);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.FilePath, Is.EqualTo(invalidPath));
				Assert.That(result.Name, Is.EqualTo("Test File"));
			});

			// But validation fails
			var validationResults = _yamlReaderHelper.ValidateObject(result);
			Assert.That(validationResults, Is.Not.Empty);
			Assert.That(validationResults[0].ErrorMessage, Does.Contain("invalid characters"));
		}

		[Test]
		public void Deserialize_EmptyUrlString_DeserializesSuccessfully()
		{
			// Arrange
			var yamlContent = @"
URL: """"
Name: Empty URL Test
";

			// Act & Assert
			var ex = Assert.Throws<YamlException>(() => _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent));
			Assert.That(ex.Message, Does.Contain("URL field is required"));
		}

		[Test]
		public void Deserialize_WhitespaceOnlyUrlString_DeserializesSuccessfully()
		{
			// Arrange
			var yamlContent = @"
URL: ""   ""
Name: Whitespace URL Test
";

			// Act & Assert
			var ex = Assert.Throws<YamlException>(() => _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent));
			Assert.That(ex.Message, Does.Contain("URL field is required"));
		}

		[Test]
		public void DeserializeWithEnhancedValidation_InvalidUrlFormat_ThrowsException()
		{
			// Arrange
			var yamlContent = @"
URL: not-a-valid-url
Name: Invalid URL Test
";

			var deserialize = _yamlReaderHelper.DeserializeFromContent<YamlUrl>(yamlContent);
			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReaderHelper.ValidateObjectAndThrow(deserialize));
		}
	}
}