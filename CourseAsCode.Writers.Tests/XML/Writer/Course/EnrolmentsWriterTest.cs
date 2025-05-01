using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Enrolments;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.Course;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Course
{
    [TestFixture]
    public class EnrolmentsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<EnrolmentsWriter> _testLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private EnrolmentsWriter _enrolmentsWriter;
        private Model.Course _dummyCourse;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger<EnrolmentsWriter>>();

            // Create a substitute for the general helper
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Instantiate the EnrolmentsWriter with dependency injection
            _enrolmentsWriter = new EnrolmentsWriter(_testFactory, _testLogger, _fakeGeneralHelper);

            // Create a dummy course with enrolment methods and enrolled users
            _dummyCourse = FakeCourseFactory.CreateCourseWithEnrolments();
        }

        [Test]
        public void WriteEnrolment_GeneratesValidXml()
        {
            // Act
            _enrolmentsWriter.WriteEnrolment(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteEnrolment_RootStructureIsCorrect()
        {
            // Act
            _enrolmentsWriter.WriteEnrolment(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The root element is <enrolments> and it contains a single <enrols> child.
            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("enrolments"));
            var enrolsNode = doc.DocumentElement.SelectSingleNode("enrols");
            Assert.IsNotNull(enrolsNode, "<enrols> element is missing");
        }

        [TestCase("1", "manual")]
        [TestCase("2", "guest")]
        [TestCase("3", "self")]
        public void WriteEnrolment_EnrolmentSection_ContainsEnrolSectionForMethod(string id, string expectedValue)
        {
            // Arrange: Create a dummy course with enrolment methods and enrolled users
            _dummyCourse = FakeCourseFactory.CreateCourseWithEnrolments();

            // Act
            _enrolmentsWriter.WriteEnrolment(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Verify that the <enrol> element with the expected ID and value exists
            var enrolNode = doc.SelectSingleNode($"/enrolments/enrols/enrol[@id='{id}']") as XmlElement;
            Assert.IsNotNull(enrolNode, $"Enrol section for {expectedValue} Enrolment missing");

            // Check a key element inside the section | Full Schema validation should be done in integration tests
            Assert.That(enrolNode.SelectSingleNode("enrol")?.InnerText, Is.EqualTo(expectedValue));

            // Verify that the section contains a <user_enrolments> element
            Assert.IsNotNull(enrolNode.SelectSingleNode("user_enrolments"), "User enrolments section missing");
        }

        [TestCase("1", "101")]
        [TestCase("2", "102")]
        [TestCase("3", "103")]
        public void WriteEnrolment_EnrolmentSection_ContainsUserEntryWithCorrectId(string id, string expectedUserId)
        {
            // Arrange: Create a dummy course with enrolment methods and enrolled users
            _dummyCourse = FakeCourseFactory.CreateCourseWithEnrolments();

            // Act
            _enrolmentsWriter.WriteEnrolment(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            var enrolNode = doc.SelectSingleNode($"/enrolments/enrols/enrol[@id='{id}']") as XmlElement;

            // Verify that for each enrolled user, a corresponding <enrolment> element exists
            var userEnrolment = enrolNode?.SelectSingleNode("user_enrolments/enrolment");
            Assert.IsNotNull(userEnrolment, "User enrolment entry missing");

            // Check that the <userid> element in the enrolment matches the expected user ID
            var userId = userEnrolment.SelectSingleNode("userid")?.InnerText;
            Assert.That(userId, Is.EqualTo(expectedUserId));
        }

        [Test]
        public void WriteEnrolment_LogsDebugOnSuccess()
        {
            // Act
            _enrolmentsWriter.WriteEnrolment(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert: Verify that a debug log was recorded containing the enrolments file name
            _testLogger.Received().LogDebug(Arg.Is<string>(s => s.Contains("course/enrolments.xml")));
        }

        [Test]
        public void WriteEnrolment_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _enrolmentsWriter = new EnrolmentsWriter(throwingFactory, _testLogger, _fakeGeneralHelper);

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _enrolmentsWriter.WriteEnrolment(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH));

            // Assert: Verify that an error log was recorded containing "course/enrolments.xml"
            _testLogger.Received().LogError(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("course/enrolments.xml")));
        }
    }

    /// <summary>
    /// Factory for creating a dummy course with enrolment methods and enrolled users
    /// </summary>
    internal static partial class FakeCourseFactory
    {
        public static Model.Course CreateCourseWithEnrolments()
        {
            var course = new Model.Course();

            // Create dummy enrolment methods
            var manual = new ManualEnrolment();
            var guest = new GuestEnrolment();
            var self = new SelfEnrolment();

            course.EnrolmentMethods = new List<Model.EnrolmentMethod> { manual, guest, self };

            course.EnrolledUsers = new List<Model.User>();

            // User for Manual Enrolment
            var userManual = new Model.User
            {
                Username = "manualUser",
                SystemDefaults = new User.SystemDefaultsUser() { Id = 101, EnrolmentSuspended = false },
                SensibleDefaults = new User.SensibleDefaultsUser()
                {
                    EnrolmentStartDate = DateTime.Now.AddDays(-2),
                    EnrolmentEndDate = DateTime.Now.AddDays(10),
                },
                EnrolmentMethod = manual,
            };

            // User for Guest Enrolment
            var userGuest = new Model.User
            {
                Username = "guestUser",
                SystemDefaults = new User.SystemDefaultsUser() { Id = 102, EnrolmentSuspended = true },
                SensibleDefaults = new User.SensibleDefaultsUser()
                {
                    EnrolmentStartDate = DateTime.Now.AddDays(-3),
                    EnrolmentEndDate = DateTime.Now.AddDays(5),
                },
                EnrolmentMethod = guest,
            };

            // User for Self Enrolment
            var userSelf = new Model.User
            {
                Username = "selfUser",
                SystemDefaults = new User.SystemDefaultsUser() { Id = 103, EnrolmentSuspended = false },
                SensibleDefaults = new User.SensibleDefaultsUser()
                {
                    EnrolmentStartDate = DateTime.Now.AddDays(-1),
                    EnrolmentEndDate = DateTime.Now.AddDays(15),
                },
                EnrolmentMethod = self,
            };

            course.EnrolledUsers.Add(userManual);
            course.EnrolledUsers.Add(userGuest);
            course.EnrolledUsers.Add(userSelf);

            return course;
        }
    }
}
