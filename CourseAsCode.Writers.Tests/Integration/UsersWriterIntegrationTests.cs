using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
using System.Net.Mail;
using NLog.Config;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class UsersWriterIntegrationTests
    {
        private string XsdPath; 
        private Course _dummyCourse;
        private string _outputPath;

        [SetUp]
        public void SetUp()
        {
            XsdPath = PathHelper.GetAbsolutePath("..\\..\\..\\Schemas\\users.xsd");

            _outputPath = PathHelper.EnsureDirectoryExists("..\\..\\..\\..\\Documentation\\out\\moodleCourse");

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
                    TimeModified = DateTimeOffset.UtcNow.DateTime
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
                    AlternateName = "Alt"
                },
                UserPreferences = new User.UserPreferencesClass
                {
                    PreferredLanguage = Language.English,
                    EMailDigestType = EMailDigestType.NoDigest,
                    EMailVisibility = EMailVisibility.VisibleToEveryone,
                    AutoSubscribeToForums = true,
                    TrackReadPosts = true
                }
            }
        }
            };
        }

        [Test]
        public void WriteUsers_GeneratesValidXml()
        {
            // Arrange
            var writer = new UsersWriter();

            // Act
            writer.WriteUsers(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/users.xml", XsdPath);
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