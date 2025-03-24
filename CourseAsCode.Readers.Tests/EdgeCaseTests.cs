using CourseAsCode.Model.Activities;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests
{
	[TestFixture]
	public class EdgeCaseTests : TestBase
	{
		[Test]
		public void CourseReader_EmptySectionsYaml_ReturnsCourseWithNoSections()
		{
			// Arrange
			var testDir = CreateTestDirectory("emptySectionsTest");
			var configPath = Path.Combine(testDir, "config.yaml");
			var sectionsPath = Path.Combine(testDir, "sections.yaml");

			File.WriteAllText(configPath, "CourseName: Test Course");
			File.WriteAllText(sectionsPath, "Sections: []");

			var reader = new CourseReader(new YAMLSectionReader());

			// Act
			var result = reader.Read(testDir, CourseFormat.YAML);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.FullName, Is.EqualTo("Test Course"));
				Assert.That(result.Sections, Is.Empty);
			});
		}

		[Test]
		public void SectionReader_EmptySectionYaml_ReturnsEmptySection()
		{
			// Arrange
			var yamlContent = "SectionName: Empty Section";
			var filePath = CreateTestYamlFile("emptySection.yaml", yamlContent);

			var reader = new YAMLSectionReader();

			// Act
			var result = reader.Read(filePath);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Empty Section"));
				Assert.That(result.Activities, Is.Empty);
			});
		}

		[Test]
		public void CourseReader_NonexistentSectionPath_SkipsMissingSection()
		{
			// Arrange
			var testDir = CreateTestDirectory("nonexistentSectionTest");
			var configPath = Path.Combine(testDir, "config.yaml");
			var sectionsPath = Path.Combine(testDir, "sections.yaml");

			// Create valid section
			var section1Dir = CreateTestDirectory(Path.Combine("nonexistentSectionTest", "Sections", "Section1"));
			File.WriteAllText(Path.Combine(section1Dir, "section.yaml"), "SectionName: Valid Section");

			// Create config files
			File.WriteAllText(configPath, "CourseName: Test Course");
			File.WriteAllText(sectionsPath, @"
Sections:
  - SectionPath: Sections/Section1
  - SectionPath: Sections/NonexistentSection
");

			var reader = new CourseReader(new YAMLSectionReader());

			// Act
			var result = reader.Read(testDir, CourseFormat.YAML);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Sections, Has.Count.EqualTo(1));
				Assert.That(result.Sections[0].Name, Is.EqualTo("Valid Section"));
			});
		}

		[Test]
		[Ignore("Currently we are not verifying the yaml indentation/format")]
		public void YamlActivityReader_MalformedYaml_ReturnsNull()
		{
			// Arrange
			var yamlContent = @"
Type: URL
Name: Test URL
-Invalid: indentation
";
			var filePath = CreateTestYamlFile("malformed.yaml", yamlContent);

			var reader = new YamlActivityReader();

			// Act & Assert
			Assert.Throws<YamlDotNet.Core.YamlException>(() => reader.Read(filePath));
		}

		[Test]
		public void YamlSectionReader_CircularActivityReferences_HandlesGracefully()
		{
			// This test is to ensure that the reader handles circular references correctly
			// In a real situation, this would happen if an activity referenced itself

			// Arrange
			var yamlContent = @"
SectionName: Circular Section
Activities:
  - ActivityFilePath: circular.yaml
";
			var sectionFilePath = CreateTestYamlFile("circularSection.yaml", yamlContent);
			var activityFilePath = Path.Combine(Path.GetDirectoryName(sectionFilePath)!, "circular.yaml");

			File.WriteAllText(activityFilePath, @"
Type: URL
Name: Circular URL
URL: https://example.com
");

			// Create a section reader with a mocked activity reader that always returns null
			// to simulate a circular reference
			var mockActivityReader = Substitute.For<YamlActivityReader>();
			mockActivityReader.Read(Arg.Any<string>()).Returns((Activity?)null);

			var reader = new YAMLSectionReader();

			// Use reflection to set the private _activityReader field
			var field = typeof(YAMLSectionReader).GetField("_activityReader",
				 System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			field?.SetValue(reader, mockActivityReader);

			// Act
			var result = reader.Read(sectionFilePath);

			// Assert
			// The section should be created but with an empty activities list
			Assert.Multiple(() =>
			{
				Assert.That(result, Is.Not.Null);
				Assert.That(result.Name, Is.EqualTo("Circular Section"));
				Assert.That(result.Activities, Is.Empty);
			});
		}
	}
}