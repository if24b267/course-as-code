using System.Xml;
using System.Xml.Schema;

using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
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
    public class CourseWriterIntegrationTests
    {
        private string XsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for CourseWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<CourseWriter> _logger;
        private IGeneralHelper _generalHelper;
        private ICourseHelper _courseHelper;

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            XsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "course", "course.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<CourseWriter>.Instance; // Use NullLogger to avoid log output during tests
            _generalHelper = new GeneralHelper();
            _courseHelper = new CourseHelper();

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                Summary = "This is a test course.",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                GroupMode = GroupMode.SeparateGroups,
                CourseLanguage = Language.English,
                Tags = new List<string> { "test", "course" },
                Sections = new List<Section>(),
                Roles = new List<Role>(),
                EnrolmentMethods = new List<EnrolmentMethod>(),
                EnrolledUsers = new List<User>(),
                SensibleDefaults = new Course.SensibleDefaultsCourse
                {
                    NumberOfAnnouncementsToDisplay = 5,
                    Format = CourseFormat.WeeklySections,
                    ShowGrades = true,
                    MaximumUploadSize = 1048576,
                    VisibleToStudents = true,
                    ForceGroupMode = false,
                    Theme = "default",
                    ShowActivityDates = true,
                    ShowActivityCompletionConditions = true,
                    EnableCompletionTracking = true,
                    CategoryName = "Test Category",
                    CategoryDescription = "This is a test category."
                },
                SystemDefaults = new Course.SystemDefaultsCourse
                {
                    Id = 1,
                    SummaryFormat = FormattingFormat.HTML,
                    UsesLegacyFiles = false,
                    TimeCreated = DateTime.UtcNow,
                    TimeModified = DateTime.UtcNow,
                    PdfExportFont = "Arial",
                    CustomFields = "Custom Field Data"
                }
            };
        }

        [Test]
        public void WriteCourse_GeneratesValidXml()
        {
            // Arrange
            var writer = new CourseWriter(_xmlWriterFactory, _logger, _courseHelper, _generalHelper);

            // Sicherstellen, dass das Verzeichnis existiert
            var courseDirectory = Path.Combine(_outputPath, "course");
            if (!Directory.Exists(courseDirectory))
            {
                Directory.CreateDirectory(courseDirectory);
            }

            // Act
            writer.WriteCourse(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{courseDirectory}/course.xml", XsdPath);
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
