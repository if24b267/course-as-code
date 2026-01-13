using System.Xml;
using System.Xml.Schema;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class ScalesWriterIntegrationTests
    {
        private string XsdPath;
        private Model.Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for ScalesWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<ScalesWriter> _logger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "scale.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<ScalesWriter>.Instance; // Use NullLogger to avoid log output during tests

            _dummyCourse = new Model.Course
            {
                FullName = "Test Course",
                ShortName = "TC"
            };
        }

        [Test]
        public void WriteScales_GeneratesValidXml()
        {
            // Arrange
            var writer = new ScalesWriter(_xmlWriterFactory, _logger);

            // Act
            writer.WriteScales(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/scales.xml", XsdPath);
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


