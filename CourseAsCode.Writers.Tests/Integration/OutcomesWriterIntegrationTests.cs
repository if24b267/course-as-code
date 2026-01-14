using System.Xml;
using System.Xml.Schema;
using CourseAsCode.Model;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class OutcomesWriterIntegrationTests
    {
        private string _xsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for OutcomesWriter
        private IXmlServiceFactory _xmlServiceFactory;
        private ILogger<OutcomesWriter> _logger;

        [SetUp]
        [Ignore("Needs to be rewritten to work with new refactored implementation")]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            _xsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "outcomes.xsd"));
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
            _dummyCourse = CourseDefaults.GetDefaultCourse();
            _logger = NullLogger<OutcomesWriter>.Instance;
        }

        [Test]
        public void WriteOutcomes_GeneratesValidXml()
        {
            // Arrange
            var writer = new OutcomesWriter(_xmlServiceFactory, _logger);

            // Act
            writer.WriteOutcomes(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/outcomes.xml", _xsdPath);
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

