using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.Tests.YAML
{
    [TestFixture]
    public class ComplexYamlStructureTests : TestBase
    {
        private IYamlFileReaderHelper _yamlReaderHelper;

        [SetUp]
        public void Setup()
        {
            _yamlReaderHelper = new YamlFileReaderHelper();
        }
        [Test]
        public void DeserializeFromContent_DeepNestedStructure_SuccessfullyParses()
        {
            // Arrange - a valid but deeply nested structure
            var yamlContent = @"
SectionName: Deep Nested Section
VisibleToStudents: true
Activities:
  - ActivityFilePath: activity1.yaml
    Extra:
      Level1:
        Level2:
          Level3:
            Level4:
              Level5: deep value
  - ActivityFilePath: activity2.yaml
";

            // Act & Assert - since YamlSection doesn't have an Extra property, this should be ignored
            // due to the IgnoreUnmatchedProperties() configuration, but still parse successfully
            var result = _yamlReaderHelper.DeserializeFromContent<YamlSection>(yamlContent);
            
            Assert.Multiple(() => 
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SectionName, Is.EqualTo("Deep Nested Section"));
                Assert.That(result.VisibleToStudents, Is.True);
                Assert.That(result.Activities, Has.Count.EqualTo(2));
                Assert.That(result.Activities[0].ActivityFilePath, Is.EqualTo("activity1.yaml"));
                Assert.That(result.Activities[1].ActivityFilePath, Is.EqualTo("activity2.yaml"));
            });
        }

        [Test]
        public void DeserializeFromContent_AnchorAndAliasUsage_SuccessfullyParses()
        {
            // Arrange - using YAML anchors and aliases
            var yamlContent = @"
defaults: &defaults
  VisibleToStudents: true
  Summary: Default Summary

SectionName: Section with Anchors
<<: *defaults
Activities:
  - ActivityFilePath: activity1.yaml
  - ActivityFilePath: activity2.yaml
";

            // Act
            var result = _yamlReaderHelper.DeserializeFromContent<YamlSection>(yamlContent);
            
            // Assert
            Assert.Multiple(() => 
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SectionName, Is.EqualTo("Section with Anchors"));
                Assert.That(result.VisibleToStudents, Is.True);
                Assert.That(result.Summary, Is.EqualTo("Default Summary"));
                Assert.That(result.Activities, Has.Count.EqualTo(2));
            });
        }

        [Test]
        public void DeserializeFromContent_ComplexSequenceStructure_SuccessfullyParses()
        {
            // Arrange - complex sequence structure
            var yamlContent = @"
Sections:
  - SectionPath: Sections/Section1
  - SectionPath: Sections/Section2
";

            // Act
            var result = _yamlReaderHelper.DeserializeFromContent<YamlSectionsDefinition>(yamlContent);
            
            // Assert
            Assert.Multiple(() => 
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Sections, Has.Count.EqualTo(2));
                
                Assert.That(result.Sections[0].SectionPath, Is.EqualTo("Sections/Section1"));
                Assert.That(result.Sections[1].SectionPath, Is.EqualTo("Sections/Section2"));
            });
        }

        [Test]
        public void DeserializeFromContent_MultilineStringLiteral_SuccessfullyParses()
        {
            // Arrange - multiline string literal style
            var yamlContent = @"
SectionName: Section with Multiline Summary
VisibleToStudents: true
Summary: |
  This is a multiline summary.
  It has multiple paragraphs.
  
  And preserves line breaks exactly as written.
    Including indentation.
Activities:
  - ActivityFilePath: activity1.yaml
";

            // Act
            var result = _yamlReaderHelper.DeserializeFromContent<YamlSection>(yamlContent);
            
            // Assert
            Assert.Multiple(() => 
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SectionName, Is.EqualTo("Section with Multiline Summary"));
                Assert.That(result.Summary, Does.Contain("This is a multiline summary."));
                Assert.That(result.Summary, Does.Contain("Including indentation."));
                Assert.That(result.Activities, Has.Count.EqualTo(1));
            });
        }

        [Test]
        public void DeserializeFromContent_MultilineStringFolded_SuccessfullyParses()
        {
            // Arrange - multiline string folded style
            var yamlContent = @"
SectionName: Section with Folded Summary
VisibleToStudents: true
Summary: >
  This is a multiline summary.
  Line breaks are converted to spaces.
  
  Blank lines become line breaks.
    Indentation is preserved.
Activities:
  - ActivityFilePath: activity1.yaml
";

            // Act
            var result = _yamlReaderHelper.DeserializeFromContent<YamlSection>(yamlContent);
            
            // Assert
            Assert.Multiple(() => 
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SectionName, Is.EqualTo("Section with Folded Summary"));
                Assert.That(result.Summary, Does.Contain("This is a multiline summary. Line breaks are converted to spaces."));
                Assert.That(result.Activities, Has.Count.EqualTo(1));
            });
        }

        [Test]
        public void DeserializeFromContent_FlowMappingStyle_SuccessfullyParses()
        {
            // Arrange - using flow mapping style for activities
            var yamlContent = @"
SectionName: Section with Flow Style
VisibleToStudents: true
Activities: [
  {ActivityFilePath: activity1.yaml}, 
  {ActivityFilePath: activity2.yaml}
]
";

            // Act
            var result = _yamlReaderHelper.DeserializeFromContent<YamlSection>(yamlContent);
            
            // Assert
            Assert.Multiple(() => 
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SectionName, Is.EqualTo("Section with Flow Style"));
                Assert.That(result.Activities, Has.Count.EqualTo(2));
                Assert.That(result.Activities[0].ActivityFilePath, Is.EqualTo("activity1.yaml"));
                Assert.That(result.Activities[1].ActivityFilePath, Is.EqualTo("activity2.yaml"));
            });
        }

        [Test]
        public void DeserializeFromContent_ComplexEscapedCharacters_SuccessfullyParses()
        {
            // Arrange - using various escaped characters
            var yamlContent = @"
SectionName: ""Section with \""quoted\"" text and \\ backslashes""
VisibleToStudents: true
Summary: ""Unicode characters: \u2019 \u2018 and \t tab""
Activities:
  - ActivityFilePath: activity1.yaml
";

            // Act
            var result = _yamlReaderHelper.DeserializeFromContent<YamlSection>(yamlContent);
            
            // Assert
            Assert.Multiple(() => 
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SectionName, Is.EqualTo("Section with \"quoted\" text and \\ backslashes"));
                Assert.That(result.Summary, Does.Contain("Unicode characters:"));
                Assert.That(result.Activities, Has.Count.EqualTo(1));
            });
        }

        [Test]
        public void DeserializeFromContent_ExcessivelyNestedStructure_ThrowsYamlException()
        {
            // Create a YAML string with excessively deep nesting that exceeds the YamlDotNet depth limit
            var yamlBuilder = new System.Text.StringBuilder();
            yamlBuilder.AppendLine("SectionName: Too Deep");
            yamlBuilder.AppendLine("Level1:");
            
            // Create 200 levels of nesting, which should exceed YamlDotNet's capabilities
            for (int i = 2; i <= 200; i++)
            {
                yamlBuilder.AppendLine($"{new string(' ', i * 2)}Level{i}:");
            }
            
            // Finally add a leaf value
            yamlBuilder.AppendLine($"{new string(' ', 201 * 2)}Value: Too Deep");

            // Act & Assert - this should throw a YamlException due to exceeding max depth
            Assert.Throws<YamlException>(() => 
                _yamlReaderHelper.DeserializeFromContent<YamlSection>(yamlBuilder.ToString()));
        }

        [Test]
        public void DeserializeFromContent_YamlWithExtremeLargeContent_ThrowsException()
        {
            // Create large content - generates a string that's several MB in size
            var largeStringBuilder = new System.Text.StringBuilder();
            largeStringBuilder.AppendLine("SectionName: Large Content Test");
            largeStringBuilder.AppendLine("Summary: |");

            // Add approximately 5MB of content to trigger a memory issue
            for (int i = 0; i < 500000; i++)
            {
                largeStringBuilder.AppendLine($"  Line {i}: {Guid.NewGuid()}");
            }

            // Act & Assert - this should throw an exception due to excessive size
            Assert.Throws<Exception>(() => 
                _yamlReaderHelper.DeserializeFromContent<YamlSection>(largeStringBuilder.ToString()));
        }
    }
}