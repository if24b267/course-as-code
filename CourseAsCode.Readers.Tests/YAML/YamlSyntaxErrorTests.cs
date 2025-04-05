using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Tests.YAML
{
	[TestFixture]
	[Ignore("YamlDotNet Doesn't support this i think -> check back later")]
	public class YamlSyntaxErrorTests : TestBase
	{
		private IYamlFileReaderHelper _yamlReader;

		[SetUp]
		public void Setup()
		{
			_yamlReader = new YamlFileReaderHelper();
		}

		[Test]
		public void DeserializeFromContent_InvalidIndentation_ThrowsYamlException()
		{
			// Arrange
			var yamlContent = @"
CourseName: Test Course
ShortName: TC
  Visible: true # Incorrect indentation
StartDate: 01-01-2024 00:00
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_MissingKeyValueSeparator_ThrowsYamlException()
		{
			// Arrange
			var yamlContent = @"
CourseName: Test Course
ShortName TC # Missing colon
Visible: true
StartDate: 01-01-2024 00:00
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_IncompleteBlockStructure_ThrowsYamlException()
		{
			// Arrange
			var yamlContent = @"
CourseName: Test Course
Activities:
  - ActivityFilePath: activity1.yaml
  - ActivityFilePath activity2.yaml # Missing colon in nested structure
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlSectionsDefinition>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_UnescapedSpecialCharacters_ThrowsYamlException()
		{
			// Arrange
			var yamlContent = @"
CourseName: Test Course: With Colon # Unescaped colon in string
ShortName: TC
Visible: true
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_DuplicateKeys_ThrowsYamlException()
		{
			// Arrange
			var yamlContent = @"
CourseName: Test Course
ShortName: TC
CourseName: Duplicate Course Name # Duplicate key
Visible: true
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_InvalidTabIndentation_ThrowsYamlException()
		{
			// Arrange - using literal tab character in indentation
			var yamlContent = "CourseName: Test Course\n\tShortName: TC\nVisible: true";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_UnterminatedFlowSequence_ThrowsYamlException()
		{
			// Arrange - missing closing bracket in sequence
			var yamlContent = @"
CourseName: Test Course
ShortName: TC
Tags: [tag1, tag2
Visible: true
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_IncorrectBlockScalarIndicator_ThrowsYamlException()
		{
			// Arrange - incorrect block scalar indicator
			var yamlContent = @"
CourseName: Test Course
ShortName: TC
Summary: |+  
  This is a summary
  spanning multiple lines
 with incorrect indentation on this line
Visible: true
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_InvalidAnchorName_ThrowsYamlException()
		{
			// Arrange - invalid characters in anchor name
			var yamlContent = @"
Course: &course-name@ Test Course  # Invalid anchor name with @
ShortName: *course-name
Visible: true
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}

		[Test]
		public void DeserializeFromContent_InvalidEscapeSequence_ThrowsYamlException()
		{
			// Arrange - invalid escape sequence
			var yamlContent = @"
CourseName: ""Test Course with \z invalid escape sequence""
ShortName: TC
Visible: true
";

			// Act & Assert
			Assert.Throws<YamlException>(() =>
				 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
		}
	}
}