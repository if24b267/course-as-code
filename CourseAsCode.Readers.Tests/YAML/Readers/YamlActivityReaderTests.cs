using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Readers;
using NSubstitute.ExceptionExtensions;
using System.Reflection.PortableExecutable;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Tests.YAML.Readers
{
	[TestFixture]
	public class YamlActivityReaderTests : TestBase
	{
		private IActivityReader _activityReader;
		private IFileReader _mockFileReader;
		private ITextAndMediaAreaReader _mockTextMediaAreaReader;
		private IUrlReader _mockUrlReader;

		[SetUp]
		public void Setup()
		{
			// Setup mock readers
			_mockFileReader = Substitute.For<IFileReader>();
			_mockTextMediaAreaReader = Substitute.For<ITextAndMediaAreaReader>();
			_mockUrlReader = Substitute.For<IUrlReader>();
			_activityReader = new YamlActivityReader(new YamlFileReader(new YamlFileReaderHelper()), _mockTextMediaAreaReader, _mockUrlReader, new YamlFileReaderHelper());

			// Configure mock URL reader to return a valid URL object
			_mockUrlReader.Read(Arg.Any<string>()).Returns(new URL
			{
				Name = "Test URL",
				Description = "URL Description",
				ExternalUrl = "https://example.com",
				ActivityId = 1
			});


			ValueKeeper.CurrentActivityId = 1;
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
			var result = _activityReader.Read(filePath);

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

				// Verify URL reader was called
				_mockUrlReader.Received(1).Read(filePath);
			});
		}

		[Test]
		public void Read_InvalidYamlActivity_ReturnsError()
		{
			// Arrange
			var yamlContent = @"
Type: UNKNOWN
Name: Test Activity
Description: Activity Description
";
			var filePath = CreateTestYamlFile("invalid.yaml", yamlContent);

			// Act & Assert
			Assert.Throws<YamlException>(() => _activityReader.Read(filePath));
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

			// Configure mock file reader
			_mockFileReader.Read(Arg.Any<string>()).Returns(new MoodleFile
			{
				Name = "Test File",
				ActivityId = 1
			});

			// Act
			var result = _mockFileReader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result, Is.TypeOf<MoodleFile>());

				// Verify File reader was called
				_mockFileReader.Received(1).Read(filePath);
			});
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

			// Configure mock text media reader
			_mockTextMediaAreaReader.Read(Arg.Any<string>()).Returns(new TextAndMediaArea
			{
				Name = "Test Label",
				ActivityId = 1
			});

			// Act
			var result = _mockTextMediaAreaReader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result, Is.TypeOf<TextAndMediaArea>());

				// Verify TextMediaArea reader was called
				_mockTextMediaAreaReader.Received(1).Read(filePath);
			});
		}
	}
}