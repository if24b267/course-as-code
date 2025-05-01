using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class CompetenciesWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for CompetenciesWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<CompetenciesWriter> _logger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "course", "competencies.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<CompetenciesWriter>.Instance; // Use NullLogger to avoid log output during tests

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                Summary = "This is a test course.",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            };
        }

        [Test]
        public void WriteCompetencies_GeneratesValidXml()
        {
            // Arrange
            var writer = new CompetenciesWriter(_xmlWriterFactory, _logger);

            // Sicherstellen, dass das Verzeichnis existiert
            var competenciesDirectory = Path.Combine(_outputPath, "course");
            if (!Directory.Exists(competenciesDirectory))
            {
                Directory.CreateDirectory(competenciesDirectory);
            }

            // Act
            writer.WriteCompetencies(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{competenciesDirectory}/competencies.xml", XsdPath);
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


