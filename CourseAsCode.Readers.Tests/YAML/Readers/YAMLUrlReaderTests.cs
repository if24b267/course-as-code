using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.YAML.Readers
{
	[TestFixture]
	public class YAMLUrlReaderTests : TestBase
	{
		private YamlUrlReader _reader;

		[SetUp]
		public void Setup()
		{
			var mockYamlFileReader = Substitute.For<IYamlFileReaderHelper>();
			mockYamlFileReader.DeserializeFromFile<YamlUrl>(Arg.Any<string>())
				.Returns(callInfo =>
				{
					var path = callInfo.Arg<string>();
					if (!File.Exists(path)) return null;

					var content = File.ReadAllText(path);
					if (content.Contains("URL:"))
					{
						return new YamlUrl
						{
							Name = "Test URL",
							Description = "URL Description",
							URL = "https://example.com"
						};
					}

					// Throw exception for invalid files
					throw new YamlDotNet.Core.YamlException("Required property missing");
				});

			mockYamlFileReader.ValidateObject(Arg.Any<object>())
				.Returns(new List<System.ComponentModel.DataAnnotations.ValidationResult>());

			_reader = new YamlUrlReader(mockYamlFileReader);
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
		public void Read_ValidUrlYaml_ReturnsUrlObject()
		{
			// Arrange
			var yamlContent = @"
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

				var url = result;
				Assert.That(url.Name, Is.EqualTo("Test URL"));
				Assert.That(url.Description, Is.EqualTo("URL Description"));
				Assert.That(url.ExternalUrl, Is.EqualTo("https://example.com"));
				Assert.That(url.ActivityId, Is.EqualTo(1));
			});
		}

		[Test]
		public void Read_MissingUrlField_ReturnsNull()
		{
			// Arrange
			var yamlContent = @"
Name: Test URL
Description: URL Description
";
			var filePath = CreateTestYamlFile("url.yaml", yamlContent);

			var read = _reader.Read(filePath);
			Assert.That(read, Is.Null);
		}

		[Test]
		public void Read_UsesCurrentActivityId()
		{
			// Arrange
			var yamlContent = @"
Name: Test URL
URL: https://example.com
";
			var filePath = CreateTestYamlFile("url.yaml", yamlContent);

			// Set custom activity ID
			ValueKeeper.CurrentActivityId = 42;

			// Act
			var result = _reader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.ActivityId, Is.EqualTo(42));
			});
		}

		[Test]
		[Ignore("Currently we are not verifying the yaml indentation/format")]
		public void Read_MalformedYaml_ReturnsNull()
		{
			// Arrange
			var yamlContent = @"
Name: Test URL
Description: URL Description
URL: https://example.com
  - Invalid indentation
";
			var filePath = CreateTestYamlFile("url.yaml", yamlContent);

			// Act & Assert
			Assert.Throws<YamlDotNet.Core.YamlException>(() => _reader.Read(filePath));
		}
	}
}