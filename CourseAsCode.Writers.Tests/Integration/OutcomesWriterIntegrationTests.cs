using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Model;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class OutcomesWriterIntegrationTests
    {
        private const string XsdPath = "..\\..\\..\\Schemas\\outcomes.xsd";
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC"
            };
            _outputPath = "..\\..\\..\\..\\Documentation\\out\\moodleCourse\\";
        }

        [Test]
        public void WriteOutcomes_GeneratesValidXml()
        {
            // Arrange
            var writer = new OutcomesWriter();

            // Act
            writer.WriteOutcomes(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/outcomes.xml", XsdPath);
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

