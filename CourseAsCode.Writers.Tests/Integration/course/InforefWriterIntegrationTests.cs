using System.Xml;
using System.Xml.Schema;

using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Course;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class InforefWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for InforefWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<InforefWriter> _logger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "course", "inforef.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<InforefWriter>.Instance; // Use NullLogger to avoid log output during tests

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                EnrolledUsers = new List<User>
                {
                    new User
                    {
                        Username = "user1",
                        SystemDefaults = new User.SystemDefaultsUser
                        {
                            Id = 1
                        }
                    }
                },
                Roles = new List<Role>
                {
                    new Role
                    {
                        ShortName = "student",
                        SystemDefaults = new Role.SystemDefaultsRole
                        {
                            RoleId = 1
                        }
                    }
                }
            };
        }

        [Test]
        public void WriteInforef_GeneratesValidXml()
        {
            // Arrange
            var writer = new InforefWriter(_xmlWriterFactory, _logger);

            // Sicherstellen, dass das Verzeichnis existiert
            var inforefDirectory = Path.Combine(_outputPath, "course");
            if (!Directory.Exists(inforefDirectory))
            {
                Directory.CreateDirectory(inforefDirectory);
            }

            // Act
            writer.WriteInforef(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{inforefDirectory}/inforef.xml", XsdPath);
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
