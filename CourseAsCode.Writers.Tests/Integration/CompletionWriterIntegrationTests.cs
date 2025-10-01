using System.Xml;
using System.Xml.Schema;

using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class CompletionWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for CompletionWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<CompletionWriter> _logger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "completion.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<CompletionWriter>.Instance; // Use NullLogger to avoid log output during tests

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC"
            };
        }

        [Test]
        public void WriteCompletion_GeneratesValidXml()
        {
            // Arrange
            var writer = new CompletionWriter(_xmlWriterFactory, _logger);

            // Act
            writer.WriteCompletion(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/completion.xml", XsdPath);
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



