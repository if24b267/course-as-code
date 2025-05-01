using System.Net.Mail;
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
    public class UsersWriterIntegrationTests
    {
        private string _xsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for UsersWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<UsersWriter> _logger;
        private IGeneralHelper _generalHelper;
        private IUserHelper _userHelper;

        [SetUp]
        public void SetUp()
        {
            // --- Path Setup ---
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            _xsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "users.xsd"));

            if (!File.Exists(_xsdPath))
            {
                Assert.Inconclusive($"Schema file not found at expected path: {_xsdPath}. Adjust the relative path.");
            }

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _generalHelper = GeneralHelper.Instance; // Assuming GeneralHelper is a Singleton
            _userHelper = new UserHelper(); // Assuming UserHelper can be instantiated directly
            // Use NullLogger to avoid log output during tests
            _logger = NullLogger<UsersWriter>.Instance;

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
                EnrolledUsers = new List<User>
                {
                    new User
                    {
                        Username = "testuser",
                        SystemDefaults = new User.SystemDefaultsUser
                        {
                            Id = 1,
                            ExternalId = 12345,
                            LastIP = System.Net.IPAddress.Parse("127.0.0.1"),
                            DescriptionFormat = FormattingFormat.HTML,
                            IsConfirmed = true,
                            AcceptedPolicy = true,
                            IsDeleted = false,
                            TimeCreated = DateTimeOffset.UtcNow.DateTime,
                            TimeModified = DateTimeOffset.UtcNow.DateTime,
                        },
                        EMail = new MailAddress("testuser@example.com"),
                        FirstName = "Test",
                        LastName = "User",
                        City = "Test City",
                        SensibleDefaults = new User.SensibleDefaultsUser
                        {
                            PhoneNumber = "123456789",
                            MobilePhoneNumber = "987654321",
                            Department = "Test Department",
                            Institution = "Test Institution",
                            Address = "Test Address",
                            FirstNamePhonetic = "T",
                            LastNamePhonetic = "U",
                            MiddleName = "Middle",
                            AlternateName = "Alt",
                        },
                        UserPreferences = new User.UserPreferencesClass
                        {
                            PreferredLanguage = Language.English,
                            EMailDigestType = EMailDigestType.NoDigest,
                            EMailVisibility = EMailVisibility.VisibleToEveryone,
                            AutoSubscribeToForums = true,
                            TrackReadPosts = true,
                        },
                    },
                },
            };
        }

        [Test]
        public void WriteUsers_GeneratesValidXml()
        {
            // Arrange: Instantiate UsersWriter with the real dependencies prepared in SetUp
            var writer = new UsersWriter(_xmlWriterFactory, _logger, _generalHelper, _userHelper);

            // Act
            writer.WriteUsers(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/users.xml", _xsdPath);
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
