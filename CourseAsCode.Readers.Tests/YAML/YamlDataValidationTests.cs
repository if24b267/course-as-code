using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Tests.YAML
{
    [TestFixture]
    public class YamlDataValidationTests : TestBase
    {
        private IYamlFileReaderHelper _yamlReader;

        [SetUp]
        public void Setup()
        {
            _yamlReader = new YamlFileReaderHelper();
        }

        [Test]
        public void DeserializeFromContent_TypeMismatch_DateField_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: true
StartDate: not-a-date
";

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_TypeMismatch_BooleanField_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: not-a-boolean
StartDate: 01-01-2024 00:00
";

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_TypeMismatch_IntegerField_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: true
IdNumber: not-an-integer
";

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_TypeMismatch_EnumField_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: true
GroupMode: not-a-valid-group-mode
";

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_InvalidDateFormat_ThrowsYamlException()
        {
            // Arrange - date in wrong format
            var yamlContent = @"
CourseName: Test Course
ShortName: TC
Visible: true
StartDate: 2024/01/01 00:00
";

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_MultipleRequiredFieldsMissing_ContainsAllErrorMessages()
        {
            // Arrange
            var yamlContent = @"
ShortName: TC
Visible: true
";

            // Act
            var ex = Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));

            // Assert
            Assert.That(ex.Message, Does.Contain("The CourseName field is required"));
        }

        [Test]
        public void DeserializeFromContent_ScalarWhenSequenceExpected_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
SectionName: Test Section
VisibleToStudents: true
Activities: not-a-list
";

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlSection>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_SequenceWhenScalarExpected_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
SectionName: 
  - Item1
  - Item2
VisibleToStudents: true
";

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlSection>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_MappingWhenScalarExpected_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
SectionName: 
  Key1: Value1
  Key2: Value2
VisibleToStudents: true
";

            // Act & Assert
            Assert.Throws<YamlException>(() =>
                 _yamlReader.DeserializeFromContent<YamlSection>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_UnclosedQuotes_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
CourseName: ""Test Course
ShortName: TC
";

            // Act & Assert
            Assert.Throws<SyntaxErrorException>(() =>
                 _yamlReader.DeserializeFromContent<YamlCourse>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_CircularAlias_ThrowsYamlException()
        {
            // Arrange
            var yamlContent = @"
Course1: &course1
  Name: *course2
Course2: &course2
  Name: *course1
";

            // Act & Assert
            Assert.Throws<AnchorNotFoundException>(() =>
                 _yamlReader.DeserializeFromContent<object>(yamlContent));
        }

        [Test]
        public void DeserializeFromContent_VeryDeepNesting_ThrowsYamlException()
        {
            // Create a YAML string with extremely deep nesting
            var yamlBuilder = new System.Text.StringBuilder();
            yamlBuilder.AppendLine("CourseName: Deep Nesting Test");
            yamlBuilder.AppendLine("Level1:");

            // Create 100 levels of nesting
            for (int i = 2; i <= 100; i++)
            {
                yamlBuilder.AppendLine($"{new string(' ', i * 2)}Level{i}:");
            }

            // Final leaf value
            yamlBuilder.AppendLine($"{new string(' ', 101 * 2)}Value: too deep");

            // Act & Assert
            Assert.DoesNotThrow(() =>
                 _yamlReader.DeserializeFromContent<object>(yamlBuilder.ToString()));
        }
    }
}
