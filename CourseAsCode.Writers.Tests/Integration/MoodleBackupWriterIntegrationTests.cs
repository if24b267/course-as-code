using System.Xml;
using System.Xml.Schema;

using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class MoodleBackupWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for MoodleBackupWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<MoodleBackupWriter> _logger;
        private IGeneralHelper _generalHelper;
        private IMoodleVersionHelper _moodleVersionHelper;
        private ICourseHelper _courseHelper;

        // Dependencies for GeneralHelper
        private ILogger<GeneralHelper> _generalHelperLogger;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "moodleBackup.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<MoodleBackupWriter>.Instance; // Use NullLogger to avoid log output during tests
            _generalHelper = new GeneralHelper(_generalHelperLogger);
            _moodleVersionHelper = new MoodleVersionHelper();
            _courseHelper = new CourseHelper();

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                BackupMetadata = new BackupMetadata
                {
                    Name = "Test Backup",
                    MoodleVersion = MoodleVersion.Moodle440,
                    MoodleUrl = new Uri("https://example.com"),
                    SensibleDefaults = new BackupMetadata.SensibleDefaultsBackupMetadata
                    {
                        FilesIncluded = true
                    },
                    SystemDefaults = new BackupMetadata.SystemDefaultsBackupMetadata
                    {
                        CreationDate = DateTime.UtcNow,
                        MNetRemoteUsers = 0,
                        IncludeFileReferencesToExternalContent = false,
                        BackupId = Guid.NewGuid().ToString(),
                        WholeSiteBackup = false,
                        UsesNewBackupFormat = true
                    },
                    BackupSettings = new BackupMetadata.BackupSettingsClass
                    {
                        IncludeEnrolledUsers = true,
                        IncludeActivitiesAndResources = true,
                        IncludeFiles = true
                    }
                }
            };
        }

        [Test]
        public void WriteMoodleBackup_GeneratesValidXml()
        {
            // Arrange
            var writer = new MoodleBackupWriter(_xmlWriterFactory, _logger, _generalHelper, _moodleVersionHelper, _courseHelper);

            // Act
            writer.WriteMoodleBackup(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/moodle_backup.xml", XsdPath);
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
