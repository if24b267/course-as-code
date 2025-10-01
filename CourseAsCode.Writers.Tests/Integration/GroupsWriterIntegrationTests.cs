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
    public class GroupsWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for GroupsWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<GroupsWriter> _logger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "groups.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<GroupsWriter>.Instance; // Use NullLogger to avoid log output during tests

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC"
            };
        }

        [Test]
        public void WriteGroups_GeneratesValidXml()
        {
            // Arrange
            var writer = new GroupsWriter(_xmlWriterFactory, _logger);

            // Act
            writer.WriteGroups(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/groups.xml", XsdPath);
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
