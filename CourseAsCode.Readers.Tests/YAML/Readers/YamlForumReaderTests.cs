using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using CourseAsCode.Readers.YAML.Readers;

namespace CourseAsCode.Readers.Tests.YAML.Readers
{
    [TestFixture]
    public class YamlForumReaderTests : TestBase
    {
        private YamlForumReader _reader;
        private IYamlFileReaderHelper _mockYamlFileReader;

        [SetUp]
        public void Setup()
        {
            var mockYamlFileReader = Substitute.For<IYamlFileReaderHelper>();
            mockYamlFileReader.DeserializeFromFile<YamlForum>(Arg.Any<string>())
                .Returns(callInfo =>
                {
                    var path = callInfo.Arg<string>();
                    if (!File.Exists(path))
                    {
                        return null;
                    }

                    var content = File.ReadAllText(path);
                    if (content.Contains("Name:"))
                    {
                        return new YamlForum
                        {
                            Name = "Test Forum",
                            ForumType = CourseAsCode.Model.Enums.Forums.ForumType.General,
                            Intro = "Forum Testing",
                            IntroFormat = 0,
                            DueDate = 0,
                            CutoffDate = 0,
                            Assessed = false,
                            AssessTimeStart = 0,
                            AssessTimeFinish = 0
                        };
                    }

                    // Throw exception for invalid files
                    throw new YamlDotNet.Core.YamlException("Required property missing");
                });

            mockYamlFileReader.ValidateObject(Arg.Any<object>())
                .Returns(new List<System.ComponentModel.DataAnnotations.ValidationResult>());

            _reader = new YamlForumReader(mockYamlFileReader);
            ValueKeeper.CurrentActivityId = 1;
        }

        [Test]
        public void Read_FileDoesNotExist_ReturnsNull()
        {
            var nonExistentPath = Path.Combine(TempDirectory, "nonexistent.yaml");

            var result = _reader.Read(nonExistentPath);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Read_ValidFile_ReturnsForum()
        {
            string testYamlContent = @"
                                        Name: Test Forum
                                        ForumType: General
                                        Intro: Forum Testing
                                        IntroFormat: 0
                                        DueDate: 0
                                        CutoffDate: 0
                                        Assessed: false
                                        AssessTimeStart: 0
                                        AssessTimeFinish: 0
                                      ";

            var validFilePath = CreateTestYamlFile("valid_forum.yaml", testYamlContent);

            var result = _reader.Read(validFilePath);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo("Test Forum"));
                Assert.That(result.ForumType, Is.EqualTo(CourseAsCode.Model.Enums.Forums.ForumType.General));
                Assert.That(result.Intro, Is.EqualTo("Forum Testing"));
            });
        }

        [Test]
        public void Read_MissingNameField_ReturnsNull()
        {
            string testYamlContent = @"
                                        ForumType: General
                                        Intro: Forum Testing
                                        IntroFormat: 0
                                        DueDate: 0
                                        CutoffDate: 0
                                        Assessed: false
                                        AssessTimeStart: 0
                                        AssessTimeFinish: 0
                                      ";
            var invalidFilePath = CreateTestYamlFile("invalid_forum.yaml", testYamlContent);

            var result = _reader.Read(invalidFilePath);

            Assert.That(result, Is.Null);
        }
    }
}
