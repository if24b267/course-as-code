using System.Xml;
using System.Xml.Schema;
using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Course;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration.course
{
    [TestFixture]
    public class CommentsWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for CommentsWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<CommentsWriter> _logger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "course", "comments.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<CommentsWriter>.Instance; // Use NullLogger to avoid log output during tests

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
        public void WriteComments_GeneratesValidXml()
        {
            // Arrange
            var writer = new CommentsWriter(_xmlWriterFactory, _logger);

            // Sicherstellen, dass das Verzeichnis existiert
            var commentsDirectory = Path.Combine(_outputPath, "course");
            if (!Directory.Exists(commentsDirectory))
            {
                Directory.CreateDirectory(commentsDirectory);
            }

            // Act
            writer.WriteComments(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{commentsDirectory}/comments.xml", XsdPath);
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
