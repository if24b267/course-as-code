using System.Xml;
using System.Xml.Schema;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class RolesWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for RolesWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<RolesWriter> _logger;
        private IRolesHelper _RolesHelper;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "role.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<RolesWriter>.Instance; // Use NullLogger to avoid log output during tests
            _RolesHelper = new RolesHelper();

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                Roles = new List<Role>
        {
            new Role
            {
                ShortName = "student",
                LocalName = "Student",
                Description = "A student role",
                Archetype = Archetype.Student,
                SystemDefaults = new Role.SystemDefaultsRole
                {
                    RoleId = 1
                }
            },
            new Role
            {
                ShortName = "teacher",
                LocalName = "Teacher",
                Description = "A teacher role",
                Archetype = Archetype.EditingTeacher,
                SystemDefaults = new Role.SystemDefaultsRole
                {
                    RoleId = 2
                }
            }
        }
            };
        }


        [Test]
        public void WriteRoles_GeneratesValidXml()
        {
            // Arrange
            var writer = new RolesWriter(_xmlWriterFactory, _logger, _RolesHelper);

            // Act
            writer.WriteRoles(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/roles.xml", XsdPath);
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
