using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class GradeHistoryWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            XsdPath = PathHelper.GetAbsolutePath("..\\..\\..\\Schemas\\gradeHistory.xsd");
            _outputPath = PathHelper.EnsureDirectoryExists("..\\..\\..\\..\\Documentation\\out\\moodleCourse");

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC"
            };
        }

        [Test]
        public void WriteGradehistory_GeneratesValidXml()
        {
            // Arrange
            var writer = new GradehistoryWriter();

            // Act
            writer.WriteGradehistory(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/gradehistory.xml", XsdPath);
        }

        private void ValidateXml(string xmlPath, string xsdPath)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdPath);
            settings.ValidationType = ValidationType.Schema;

            using (var reader = XmlReader.Create(xmlPath, settings))
            {
                var document = new XmlDocument();
                document.Load(reader);
                document.Validate((sender, e) =>
                {
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        Assert.Fail($"XML Validation Error: {e.Message}");
                    }
                });
            }
        }
    }
}