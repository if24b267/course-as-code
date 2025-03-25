using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class BadgesWriterIntegrationTests
    {
        private const string XsdPath = "..\\..\\..\\Schemas\\badges.xsd";
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            _outputPath = "..\\..\\..\\..\\Documentation\\out\\moodleCourse\\";
        }

        [Test]
        public void WriteBadges_GeneratesValidXml()
        {
            // Arrange
            var writer = new BadgesWriter();

            // Act
            writer.WriteBadges(_outputPath);

            // Assert
            ValidateXml($"{_outputPath}/badges.xml", XsdPath);
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




