using System.Xml;
using System.Xml.Schema;

using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Course;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class RoleAssignmentsWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for RoleAssignmentsWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<RoleAssignmentsWriter> _logger;
        private IGeneralHelper _generalHelper;

        [SetUp]
        public void SetUp()
        {

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "course", "roles.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<RoleAssignmentsWriter>.Instance; // Use NullLogger to avoid log output during tests
            _generalHelper = new GeneralHelper();

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
                            Id = 1,
                            RoleChange = DateTime.UtcNow
                        },
                        Role = new Role
                        {
                            ShortName = "student",
                            SystemDefaults = new Role.SystemDefaultsRole
                            {
                                RoleId = 1
                            }
                        }
                    }
                }
            };
        }

        [Test]
        public void WriteRoleAssignments_GeneratesValidXml()
        {
            // Arrange
            var writer = new RoleAssignmentsWriter(_xmlWriterFactory, _logger, _generalHelper);

            // Sicherstellen, dass das Verzeichnis existiert
            var rolesDirectory = Path.Combine(_outputPath, "course");
            if (!Directory.Exists(rolesDirectory))
            {
                Directory.CreateDirectory(rolesDirectory);
            }

            // Act
            writer.WriteRoleAssignments(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{rolesDirectory}/roles.xml", XsdPath);
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

