using CourseAsCode.Model.Activities;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.YAML.Readers
{
	[TestFixture]
	public class YAMLSectionReaderTests : TestBase
	{
		private YamlSectionReader _reader;
		private IActivityReader _mockActivityReader;

		[SetUp]
		public void Setup()
		{
			// Setup a mock activity reader that returns null or a simple activity
			_mockActivityReader = Substitute.For<IActivityReader>();
			var mockYamlFileReader = Substitute.For<IYamlFileReaderHelper>();
				_reader = new YamlSectionReader(_mockActivityReader, mockYamlFileReader);

			ValueKeeper.CurrentSectionId = 1;
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
		public void Read_ValidSectionWithoutActivities_ReturnsSectionObject()
		{
			// Arrange
			var yamlContent = @"
SectionName: Test Section
Summary: Test section summary
VisibleToStudents: true
";
			var filePath = CreateTestYamlFile("section.yaml", yamlContent);

			// Act
			var result = _reader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Test Section"));
				Assert.That(result.Summary, Is.EqualTo("Test section summary"));
				Assert.That(result.VisibleToStudents, Is.True);
				Assert.That(result.Activities, Is.Empty);
				Assert.That(result.SystemDefaults.SectionId, Is.EqualTo(1));
				Assert.That(result.SystemDefaults.PathToSection, Is.EqualTo("sections/section_1"));
			});
		}

		[Test]
		public void Read_SectionWithActivities_CallsActivityReaderForEachActivity()
		{
			// Arrange
			var testActivity = new TestActivity
			{
				Name = "Test Activity",
				ActivityId = 2
			};

			_mockActivityReader.Read(Arg.Any<string>()).Returns(testActivity);

			var yamlContent = @"
SectionName: Test Section
Summary: Test section summary
VisibleToStudents: true
Activities:
  - ActivityFilePath: activity1.yaml
  - ActivityFilePath: activity2.yaml
";
			var filePath = CreateTestYamlFile("section.yaml", yamlContent);

			// Create the activity files referenced in section.yaml
			CreateTestYamlFile("activity1.yaml", "Type: URL");
			CreateTestYamlFile("activity2.yaml", "Type: URL");

			// Act
			var result = _reader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Activities, Has.Count.EqualTo(2));

				// Verify activity reader was called twice
				_mockActivityReader.Received(2).Read(Arg.Any<string>());
			});
		}

		[Test]
		public void Read_ValidSection_IncrementsActivityIdPerActivity()
		{
			// Arrange
			var testActivity1 = new TestActivity { Name = "Test Activity 1" };
			var testActivity2 = new TestActivity { Name = "Test Activity 2" };

			int callCount = 0;
			_mockActivityReader.Read(Arg.Any<string>())
				 .Returns(_ =>
				 {
					 callCount++;
					 return callCount == 1 ? testActivity1 : testActivity2;
				 });

			var yamlContent = @"
SectionName: Test Section
Summary: Test section summary
VisibleToStudents: true
Activities:
  - ActivityFilePath: activity1.yaml
  - ActivityFilePath: activity2.yaml
";
			var filePath = CreateTestYamlFile("section.yaml", yamlContent);

			// Set initial value to verify it's incremented
			ValueKeeper.CurrentActivityId = 1;

			// Act
			var result = _reader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Activities, Has.Count.EqualTo(2));
				Assert.That(ValueKeeper.CurrentActivityId, Is.EqualTo(3)); // 1 + 2 increments
			});
		}

		[Test]
		public void Read_ValidSection_SetsSystemDefaults()
		{
			// Arrange
			var yamlContent = @"
SectionName: Custom Section
Summary: Custom section summary
VisibleToStudents: false
";
			var filePath = CreateTestYamlFile("section.yaml", yamlContent);

			// Set section ID to verify it's used
			ValueKeeper.CurrentSectionId = 42;

			// Act
			var result = _reader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.SystemDefaults.SectionId, Is.EqualTo(42));
				Assert.That(result.SystemDefaults.PathToSection, Is.EqualTo("sections/section_42"));
			});
		}

		// Test activity class for mocking
		private class TestActivity : Activity
		{
			public TestActivity()
			{
				Name = "Default Test Activity";
			}
		}
	}
}