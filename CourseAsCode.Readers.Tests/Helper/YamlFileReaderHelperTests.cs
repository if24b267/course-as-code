using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.Tests.Helper
{
	[TestFixture]
	public class YamlFileReaderHelperTests : TestBase
	{
		[Test]
		public void DeserializeFromContent_ValidYaml_ReturnsDeserializedObject()
		{
			// Arrange
			var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: true
StartDate: 01-01-2024 00:00
";

			// Act
			var result = YamlFileReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent);

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
		public void DeserializeFromContent_EmptyContent_ThrowsArgumentException()
		{
			// Arrange
			var yamlContent = string.Empty;

			// Act & Assert
			var ex = Assert.Throws<ArgumentException>(() =>
				 YamlFileReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent));

			Assert.That(ex.Message, Does.Contain("YAML content cannot be null or empty"));
		}

		[Test]
		public void DeserializeFromContent_NullContent_ThrowsArgumentException()
		{
			// Act & Assert
			var ex = Assert.Throws<ArgumentException>(() =>
				 YamlFileReaderHelper.DeserializeFromContent<YamlCourse>(null));

			Assert.That(ex.Message, Does.Contain("YAML content cannot be null or empty"));
		}

		[Test]
		public void DeserializeFromContent_InvalidYaml_ThrowsException()
		{
			// Arrange
			var yamlContent = "Invalid: yaml: content: - broken";

			// Act & Assert
			Assert.Throws<YamlDotNet.Core.SemanticErrorException>(() =>
				 YamlFileReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_MissingRequiredProperty_ThrowsValidationException()
		{
			// Arrange
			var yamlContent = @"
ShortName: TC
Visible: true
StartDate: 01-01-2024 00:00
";

			// Act & Assert
			var ex = Assert.Throws<YamlDotNet.Core.YamlException>(() =>
				 YamlFileReaderHelper.DeserializeFromContent<YamlCourse>(yamlContent));

			Assert.That(ex.Message, Does.Contain("The CourseName field is required"));
		}

		[Test]
		public void DeserializeFromFile_ValidFile_ReturnsDeserializedObject()
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
			var result = YamlFileReaderHelper.DeserializeFromFile<YamlCourse>(filePath);

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
		public void DeserializeFromFile_EmptyFilePath_ThrowsArgumentException()
		{
			// Act & Assert
			var ex = Assert.Throws<ArgumentException>(() =>
				 YamlFileReaderHelper.DeserializeFromFile<YamlCourse>(string.Empty));

			Assert.That(ex.Message, Does.Contain("File path cannot be null or empty"));
		}

		[Test]
		public void DeserializeFromFile_NullFilePath_ThrowsArgumentException()
		{
			// Act & Assert
			var ex = Assert.Throws<ArgumentException>(() =>
				 YamlFileReaderHelper.DeserializeFromFile<YamlCourse>(null));

			Assert.That(ex.Message, Does.Contain("File path cannot be null or empty"));
		}

		[Test]
		public void DeserializeFromFile_FileNotFound_ThrowsFileNotFoundException()
		{
			// Arrange
			var nonExistentFilePath = Path.Combine(TempDirectory, "nonexistent.yaml");

			// Act & Assert
			Assert.Throws<FileNotFoundException>(() =>
				 YamlFileReaderHelper.DeserializeFromFile<YamlCourse>(nonExistentFilePath));
		}
	}
}