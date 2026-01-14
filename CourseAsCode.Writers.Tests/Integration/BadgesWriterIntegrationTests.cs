using System.Xml;
using System.Xml.Schema;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class BadgesWriterIntegrationTests
    {
        private string _xsdPath;
        private string _outputPath;

        // Dependencies needed for BadgesWriter
        private IXmlServiceFactory _xmlServiceFactory;
        private ILogger<BadgesWriter> _logger;

        [SetUp]
        [Ignore("Needs to be rewritten to work with new refactored implementation")]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            _xsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "badges.xsd"));
            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            if (!File.Exists(_xsdPath))
            {
                Assert.Inconclusive($"Integration test skipped: XSD not found at {_xsdPath}");
            }

            if (Directory.Exists(_outputPath))
            {
                Directory.Delete(_outputPath, true);
            }
            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlServiceFactory = new XmlServiceFactory(NullLogger<XmlServiceFactory>.Instance, NullLogger<XmlService>.Instance);
            _logger = NullLogger<BadgesWriter>.Instance; // Use NullLogger to silence logs
        }

        [Test]
        public void WriteBadges_GeneratesValidXml()
        {
            // Arrange
            var writer = new BadgesWriter(_xmlServiceFactory, _logger);
            var dummyCourse = CourseDefaults.GetDefaultCourse();

            // Act
            writer.WriteBadges(dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/badges.xml", _xsdPath);
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
