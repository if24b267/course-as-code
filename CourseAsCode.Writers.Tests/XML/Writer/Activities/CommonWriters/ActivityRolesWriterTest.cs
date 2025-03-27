using System.Xml;
using CourseAsCode.Writers.UnitTests.Fakes;
using CourseAsCode.Writers.XML.Writer.Activities.CommonWriters;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.CommonWriters
{
    [TestFixture]
    public class ActivityRolesWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private ActivityRolesWriter _activityRolesWriter;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // Create a fake XML writer factory that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _activityRolesWriter = new ActivityRolesWriter(_testFactory, _testLogger);
        }

        [Test]
        public void WriteRoles_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityRolesWriter.WriteRoles(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteRoles_RootElementIsRoles()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityRolesWriter.WriteRoles(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "roles"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("roles"));
        }

        [Test]
        public void WriteRoles_ContainsRoleOverrides()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityRolesWriter.WriteRoles(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <role_overrides> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var roleOverrides = doc.DocumentElement?.SelectSingleNode("role_overrides");
            Assert.IsNotNull(roleOverrides, "Missing <role_overrides> element");
        }

        [Test]
        public void WriteRoles_ContainsRoleAssignments()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityRolesWriter.WriteRoles(dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <role_assignments> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var roleAssignments = doc.DocumentElement?.SelectSingleNode("role_assignments");
            Assert.IsNotNull(roleAssignments, "Missing <role_assignments> element");
        }

        [Test]
        public void WriteRoles_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _activityRolesWriter.WriteRoles(dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "dummyPath/roles.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("dummyPath/roles.xml")));
        }

        [Test]
        public void WriteRoles_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _activityRolesWriter = new ActivityRolesWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _activityRolesWriter.WriteRoles(dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "dummyPath/roles.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("dummyPath/roles.xml")));
        }
    }
}
