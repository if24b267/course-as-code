using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class CompletiondefaultsWriterIntegrationTests
    {
        private const string XsdPath = "..\\..\\..\\Schemas\\course\\completiondefaults.xsd";
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                Summary = "This is a test course.",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            };
            _outputPath = "..\\..\\..\\..\\Documentation\\out\\moodleCourse\\";
        }

        [Test]
        public void WriteCompletionDefaults_GeneratesValidXml()
        {
            // Arrange
            var writer = new CompletiondefaultsWriter();

            // Act
            writer.WriteCompletionDefaults(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/course/completiondefaults.xml", XsdPath);
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

