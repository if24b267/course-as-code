using System.Net.Mail;
using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TimeZone = CourseAsCode.Model.Enums.TimeZone;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class UsersWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<UsersWriter> _testLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private IUserHelper _fakeUserHelper;
        private UsersWriter _usersWriter;
        private Model.Course _dummyCourse;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger<UsersWriter>>();

            // Create substitutes for the helper dependencies
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeUserHelper = Substitute.For<IUserHelper>();

            // Configure the fake general helper to return predictable values
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _fakeGeneralHelper.GetLanguageFromEnum(Arg.Any<Language>()).Returns("en");
            _fakeGeneralHelper.GetTimeZoneFromEnum(Arg.Any<TimeZone>()).Returns("Europe/Vienna");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Configure the fake user helper
            _fakeUserHelper.GetCountryFromEnum(Arg.Any<Country>()).Returns("AT");
            _fakeUserHelper.GetAuthMethodFromEnum(Arg.Any<AuthenticationMethod>()).Returns("manual");

            // Instantiate the UsersWriter with injected dependencies
            _usersWriter = new UsersWriter(_testFactory, _testLogger, _fakeGeneralHelper, _fakeUserHelper);

            // Create a dummy course with one enrolled user
            _dummyCourse = FakeCourseFactory.CreateCourseWithUser();
        }

        [Test]
        public void WriteUsers_GeneratesValidXml()
        {
            // Act
            _usersWriter.WriteUsers(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteUsers_RootElementIsUsers()
        {
            // Act
            _usersWriter.WriteUsers(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert that the root element is <users>.
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("users"));
        }

        [Test]
        public void WriteUsers_ContainsUserElementWithExpectedContent()
        {
            // Arrange: Create a dummy course with one enrolled user
            _dummyCourse = FakeCourseFactory.CreateCourseWithUser();

            // Act
            _usersWriter.WriteUsers(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <user> element exists with the expected attribute
            var doc = new XmlDocument();
            doc.LoadXml(output);

            var userElement = doc.SelectSingleNode($"/users/user[@id='{_dummyCourse.EnrolledUsers[0].SystemDefaults.Id}']") as XmlElement;
            Assert.That(userElement, Is.Not.Null, $"User element with id '{_dummyCourse.EnrolledUsers[0].SystemDefaults.Id}' not found");

            // Check some of the inner elements (full schema validation should be done in the integration tests)
            // The expected values come partly from the dummy user and partly from the fake helpers defined in the setup
            Assert.That(userElement["username"]?.InnerText, Is.EqualTo(_dummyCourse.EnrolledUsers[0].Username));
            Assert.That(userElement["email"]?.InnerText, Is.EqualTo(_dummyCourse.EnrolledUsers[0].EMail.ToString()));

            // Fields that rely on the helpers
            Assert.That(userElement["country"]?.InnerText, Is.EqualTo("AT"));
            Assert.That(userElement["auth"]?.InnerText, Is.EqualTo("manual"));
            Assert.That(userElement["descriptionformat"]?.InnerText, Is.EqualTo("0")); // from fake EnumIntegerToString
            Assert.That(userElement["lang"]?.InnerText, Is.EqualTo("en"));
            Assert.That(userElement["timezone"]?.InnerText, Is.EqualTo("Europe/Vienna"));
            Assert.That(userElement["firstaccess"]?.InnerText, Is.EqualTo("1234567890"));
        }

        [Test]
        public void WriteUsers_ContainsSubsections()
        {
            // Act
            _usersWriter.WriteUsers(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that for the user, the correct subsections are present
            var doc = new XmlDocument();
            doc.LoadXml(output);

            var userElement = doc.SelectSingleNode($"/users/user[@id='{_dummyCourse.EnrolledUsers[0].SystemDefaults.Id}']") as XmlElement;
            Assert.That(userElement, Is.Not.Null, "User element not found");

            Assert.That(userElement.SelectSingleNode("custom_fields"), Is.Not.Null, "custom_fields section missing.");
            Assert.That(userElement.SelectSingleNode("tags"), Is.Not.Null, "tags section missing.");
            Assert.That(userElement.SelectSingleNode("preferences"), Is.Not.Null, "preferences section missing.");
            Assert.That(userElement.SelectSingleNode("roles"), Is.Not.Null, "roles section missing.");
        }

        [Test]
        public void WriteUsers_LogsDebugOnSuccess()
        {
            // Act
            _usersWriter.WriteUsers(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert: Verify that a debug log was recorded containing "users.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("users.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteUsers_LogsErrorOnException()
        {
            // Arrange: Use a factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _usersWriter = new UsersWriter(throwingFactory, _testLogger, _fakeGeneralHelper, _fakeUserHelper);

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _usersWriter.WriteUsers(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH));

            // Assert: Verify that an error log was recorded containing "users.xml"
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("users.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }

    /// <summary>
    /// Factory for creating a course with a single user
    /// </summary>
    internal static partial class FakeCourseFactory
    {
        public static Model.Course CreateCourseWithUser()
        {
            var course = new Model.Course
            {
                EnrolledUsers = new List<User>(),
            };

            // Create a dummy user
            var dummyUser = new User
            {
                Username = "jdoe",
                EMail = new MailAddress("jdoe@example.com"),
                SystemDefaults = new User.SystemDefaultsUser()
                {
                    FirstAccess = DateTime.Now,
                },
            };

            course.EnrolledUsers.Add(dummyUser);
            return course;
        }
    }
}
