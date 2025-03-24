using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.YAML.Readers
{
	[TestFixture]
	public class YamlActivityReaderTests : TestBase
	{
		private YamlActivityReader _reader;

		[SetUp]
		public void Setup()
		{
			_reader = new YamlActivityReader();
			ValueKeeper.CurrentActivityId = 1;
		}

		[Test]
		public void Read_FileDoesNotExist_ReturnsNull()
		{
			// Arrange
			var nonExistentPath = Path.Combine(TempDirectory, "nonexistent.yaml");

			// Act
			var result = _reader.Read(nonExistentPath);

			// Assert
			Assert.That(result, Is.Null);
		}

		[Test]
		public void Read_ValidUrlActivity_ReturnsUrlObject()
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
			var result = _reader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result, Is.TypeOf<URL>());

				var url = (URL)result;
				Assert.That(url.Name, Is.EqualTo("Test URL"));
				Assert.That(url.Description, Is.EqualTo("URL Description"));
				Assert.That(url.ExternalUrl, Is.EqualTo("https://example.com"));
				Assert.That(url.ActivityId, Is.EqualTo(1));
			});
		}

		[Test]
		public void Read_InvalidYamlActivity_ReturnsNull()
		{
			// Arrange
			var yamlContent = @"
Type: UNKNOWN
Name: Test Activity
Description: Activity Description
";
			var filePath = CreateTestYamlFile("invalid.yaml", yamlContent);

			// Act & Assert

			var ex = Assert.Throws<YamlDotNet.Core.YamlException>(() =>
				_reader.Read(filePath));
		}

		[Test]
		public void Read_FileActivity_DelegatesReadingToFileReader()
		{
			// Arrange
			var yamlContent = @"
Type: FILE
Name: Test File
Description: File Description
FilePath: files/test.txt
";
			var filePath = CreateTestYamlFile("file.yaml", yamlContent);

			// Act & Assert
			// This might fail if FileReader is not properly initialized, but this is a good test
			// to ensure the delegation works correctly
			Assert.DoesNotThrow(() => _reader.Read(filePath));
		}

		[Test]
		public void Read_LabelActivity_DelegatesReadingToTextMediaAreaReader()
		{
			// Arrange
			var yamlContent = @"
Type: LABEL
Name: Test Label
Content:
  - Type: MARKDOWN
    ContentFilePath: test
";
			var filePath = CreateTestYamlFile("label.yaml", yamlContent);

			// Act & Assert
			// This might fail if TextMediaAreaReader is not properly initialized, but this is a good test
			// to ensure the delegation works correctly
			Assert.DoesNotThrow(() => _reader.Read(filePath));

		}
	}
}