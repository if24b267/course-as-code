using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Model.Enums;
using NLog;
using NSubstitute;
using CourseAsCode.Writers.XML.Writer;
using CourseAsCode.Writers.UnitTests.Fakes;
using CourseAsCode.Writers.XML.Helper;

namespace CourseAsCode.Writers.UnitTests.XML.Writer
{
    [TestFixture]
    public class RolesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private IRolesHelper _fakeRolesHelper;
        private RolesWriter _rolesWriter;
        private Model.Course _dummyCourse;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        [SetUp]
        public void SetUp()
        {
            // Set up fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger>();

            // Create a substitute for the roles helper and configure to return a predictable value
            _fakeRolesHelper = Substitute.For<IRolesHelper>();
            _fakeRolesHelper.GetArchetypeFromEnum(Arg.Any<Archetype>()).Returns("editingteacher");

            // Instantiate the RolesWriter with injected dependencies
            _rolesWriter = new RolesWriter(_testFactory, _testLogger, _fakeRolesHelper);

            // Create a dummy course with one role
            _dummyCourse = FakeCourseFactory.CreateCourseWithRole();
        }

        [Test]
        public void WriteRoles_GeneratesValidXml()
        {
            // Act
            _rolesWriter.WriteRoles(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteRoles_RootElementIsRolesDefinition()
        {
            // Act
            _rolesWriter.WriteRoles(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The root element should be <roles_definition>
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("roles_definition"));
        }

        [Test]
        public void WriteRoles_ContainsRoleElementsWithExpectedContent()
        {
            // Arrange: Create a dummy course with one role
            _dummyCourse = FakeCourseFactory.CreateCourseWithRole();
            
            // Act
            _rolesWriter.WriteRoles(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the expected role elements are present and contain the expected content
            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            var roleNodes = doc.DocumentElement?.SelectNodes("role");
            Assert.IsNotNull(roleNodes, "No <role> elements found");
            Assert.That(roleNodes.Count, Is.EqualTo(_dummyCourse.Roles.Count));

            // For each role, verify that the attribute and child elements are present and correct
            foreach (Role role in _dummyCourse.Roles)
            {
                // Find the <role> element with the expected id attribute
                var xpath = $"/roles_definition/role[@id='{role.SystemDefaults.RoleId}']";
                var roleElement = doc.SelectSingleNode(xpath) as XmlElement;
                Assert.IsNotNull(roleElement, $"Role element with id '{role.SystemDefaults.RoleId}' was not found");

                // Check the inner elements
                Assert.That(roleElement["name"]?.InnerText, Is.EqualTo(role.Name ?? string.Empty));
                Assert.That(roleElement["shortname"]?.InnerText, Is.EqualTo(role.ShortName));
                Assert.That(roleElement["nameincourse"]?.InnerText, Is.EqualTo(role.LocalName ?? "$@NULL@$"));
                Assert.That(roleElement["description"]?.InnerText, Is.EqualTo(role.Description ?? string.Empty));
                Assert.That(roleElement["sortorder"]?.InnerText, Is.EqualTo(role.SystemDefaults.RoleId.ToString()));
                Assert.That(roleElement["archetype"]?.InnerText, Is.EqualTo("editingteacher"));
            }
        }

        [Test]
        public void WriteRoles_LogsDebugOnSuccess()
        {
            // Act
            _rolesWriter.WriteRoles(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert: Verify that a debug log was recorded containing "roles.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("roles.xml")));
        }

        [Test]
        public void WriteRoles_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _rolesWriter = new RolesWriter(throwingFactory, _testLogger, _fakeRolesHelper);

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _rolesWriter.WriteRoles(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH));

            // Assert: Verify that an error log was recorded containing "roles.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("roles.xml")));
        }
    }

    #region Fake Course Factory
    /// <summary>
    /// Factory for creating a dummy course with a role
    /// </summary>
    internal static partial class FakeCourseFactory
    {
        public static Model.Course CreateCourseWithRole()
        {
            var course = new Model.Course
            {
                Roles = new List<Role>()
            };

            // Create a dummy role
            var dummyRole = new Role
            {
                Name = "Teacher",
                ShortName = "teacher",
                LocalName = "Course Teacher",
                Description = "Teacher role",
                Archetype = Archetype.EditingTeacher,
            };

            course.Roles.Add(dummyRole);
            return course;
        }
    }
    #endregion
}
