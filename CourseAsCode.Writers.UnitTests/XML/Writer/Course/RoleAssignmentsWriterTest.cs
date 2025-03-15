using System.Xml;
using CourseAsCode.Model;
using NLog;
using NSubstitute;
using CourseAsCode.Writers.XML.Writer.Course;
using CourseAsCode.Writers.UnitTests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;

namespace CourseAsCode.Writers.UnitTests.XML.Writer.Course
{
    [TestFixture]
    public class RoleAssignmentsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private RoleAssignmentsWriter _roleAssignmentsWriter;
        private Model.Course _dummyCourse;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger>();

            // Create a substitute for the general helper
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Instantiate the RoleAssignmentsWriter with injected dependency
            _roleAssignmentsWriter = new RoleAssignmentsWriter(_testFactory, _testLogger, _fakeGeneralHelper);

            // Create a dummy course with enrolled users using a fake course factory
            _dummyCourse = FakeCourseFactory.CreateCourseWithRoleAssignments();
        }

        [Test]
        public void WriteRoleAssignments_GeneratesValidXml()
        {
            // Act
            _roleAssignmentsWriter.WriteRoleAssignments(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteRoleAssignments_RootStructureIsCorrect()
        {
            // Act
            _roleAssignmentsWriter.WriteRoleAssignments(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Root element is <roles> with <role_overrides> and <role_assignments> children
            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("roles"));

            var overridesNode = doc.DocumentElement.SelectSingleNode("role_overrides");
            var assignmentsNode = doc.DocumentElement.SelectSingleNode("role_assignments");

            Assert.IsNotNull(overridesNode, "<role_overrides> section is missing");
            Assert.IsNotNull(assignmentsNode, "<role_assignments> section is missing");
        }

        [Test]
        public void WriteRoleAssignments_ContainsAssignmentElements()
        {
            // Arrange: Create a dummy course with enrolled users who have been assigned roles
            _dummyCourse = FakeCourseFactory.CreateCourseWithRoleAssignments();
            
            // Act
            _roleAssignmentsWriter.WriteRoleAssignments(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            // Assert: Verify that for each enrolled user, an <assignment> element is created inside <role_assignments>.
            var assignmentNodes = doc.DocumentElement?.SelectNodes("role_assignments/assignment");
            Assert.IsNotNull(assignmentNodes, "No <assignment> elements found");
            Assert.That(assignmentNodes.Count, Is.EqualTo(_dummyCourse.EnrolledUsers.Count));
        }
        
        [Test]
        public void WriteRoleAssignments_AssignmentElementsContainExpectedValues()
        {
            // Arrange: Create a dummy course with enrolled users who have been assigned roles
            _dummyCourse = FakeCourseFactory.CreateCourseWithRoleAssignments();
            
            // Act
            _roleAssignmentsWriter.WriteRoleAssignments(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();
            var doc = new XmlDocument();
            doc.LoadXml(output);

            // For each user, there is an <assignment> element. For each assignment element, check (some of the) expected child elements
            // Full Schema Validation should be done in Integration tests
            for (int i = 0; i < _dummyCourse.EnrolledUsers.Count; i++)
            {
                var user = _dummyCourse.EnrolledUsers[i];
                var xpath = $"/roles/role_assignments/assignment[@id='{i}']";
                var assignmentElement = doc.SelectSingleNode(xpath) as XmlElement;
                Assert.IsNotNull(assignmentElement, $"Assignment element with id '{i}' not found.");

                Assert.That(assignmentElement["roleid"]?.InnerText, Is.EqualTo(user.Role.SystemDefaults.RoleId.ToString()));
                Assert.That(assignmentElement["userid"]?.InnerText, Is.EqualTo(user.SystemDefaults.Id.ToString()));
                Assert.That(assignmentElement["timemodified"]?.InnerText, Is.EqualTo("1234567890")); // is returned by the fake helper
            }
        }

        [Test]
        public void WriteRoleAssignments_LogsDebugOnSuccess()
        {
            // Act
            _roleAssignmentsWriter.WriteRoleAssignments(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert: Verify that a debug log was recorded containing the role assignments file path.
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("course/roles.xml")));
        }

        [Test]
        public void WriteRoleAssignments_LogsErrorOnException()
        {
            // Arrange: Use a throwing XML writer factory.
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _roleAssignmentsWriter = new RoleAssignmentsWriter(throwingFactory, _testLogger, _fakeGeneralHelper);

            // Act & Assert: Verify that an exception is thrown.
            Assert.Throws<InvalidOperationException>(() =>
                _roleAssignmentsWriter.WriteRoleAssignments(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH));

            // Assert: Verify that an error log was recorded containing "course/roles.xml".
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("course/roles.xml")));
        }
    }

    #region Fake Course Factory
    /// <summary>
    /// Factory for creating a dummy course with enrolled users who have been assigned roles
    /// </summary>
    internal static partial class FakeCourseFactory
    {
        public static Model.Course CreateCourseWithRoleAssignments()
        {
            var course = new Model.Course
            {
                EnrolledUsers = new System.Collections.Generic.List<Model.User>()
            };

            // Create dummy users with assigned roles
            var user1 = new Model.User
            {
                Username = "user1",
                SystemDefaults = new User.SystemDefaultsUser() { Id = 201 },
                Role = new Role
                {
                    ShortName = "Test Role 1",
                    SystemDefaults = new Role.SystemDefaultsRole()
                    {
                        RoleId = 10
                    }
                }
            };

            var user2 = new Model.User
            {
                Username = "user2",
                SystemDefaults = new User.SystemDefaultsUser() { Id = 202 },
                Role = new Role
                {
                    ShortName = "Test Role 2",
                    SystemDefaults = new Role.SystemDefaultsRole()
                    {
                        RoleId = 20
                    }
                }
            };

            course.EnrolledUsers.Add(user1);
            course.EnrolledUsers.Add(user2);
            return course;
        }
    }

    #endregion
}
