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
    public class GradeHistoryWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for GradehistoryWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<GradehistoryWriter> _logger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "gradeHistory.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<GradehistoryWriter>.Instance; // Use NullLogger to avoid log output during tests

            _dummyCourse = new Model.Course
            {
                FullName = "Test Course",
                ShortName = "TC"
            };
        }

        [Test]
        public void WriteGradehistory_GeneratesValidXml()
        {
            // Arrange
            var writer = new GradehistoryWriter(_xmlWriterFactory, _logger);

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
