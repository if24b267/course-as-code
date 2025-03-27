using System.Xml;
using CourseAsCode.Writers.UnitTests.Fakes;
using CourseAsCode.Writers.XML.Writer;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class GroupsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private GroupsWriter _groupsWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _groupsWriter = new GroupsWriter(_testFactory, _testLogger); // Inject the fake factory and logger
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteGroups_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _groupsWriter.WriteGroups(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteGroups_RootElementIsGroups()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _groupsWriter.WriteGroups(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The root element should be "groups"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("groups"));
        }

        [Test]
        public void WriteGroups_HasGroupCustomFieldsSection()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _groupsWriter.WriteGroups(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <groupcustomfields> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var groupCustomFieldsNode = doc.DocumentElement?.SelectSingleNode("groupcustomfields");
            Assert.IsNotNull(groupCustomFieldsNode, "The <groupcustomfields> element is missing");
        }

        [Test]
        public void WriteGroups_HasGroupingsSection()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _groupsWriter.WriteGroups(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that a <groupings> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var groupingsNode = doc.DocumentElement?.SelectSingleNode("groupings");
            Assert.IsNotNull(groupingsNode, "The <groupings> element is missing");
        }
        
        [Test]
        public void WriteGroups_GroupingsSectionContainsGroupingCustomFields()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _groupsWriter.WriteGroups(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Within the <groupings> element, verify that a <groupingcustomfields> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var groupingsNode = doc.DocumentElement?.SelectSingleNode("groupings");
            var groupingCustomFieldsNode = groupingsNode?.SelectSingleNode("groupingcustomfields");
            Assert.IsNotNull(groupingCustomFieldsNode, "The <groupingcustomfields> element is missing within <groupings>");
        }

        [Test]
        public void WriteGroups_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _groupsWriter.WriteGroups(_dummyCourse, dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "groups.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("groups.xml")));
        }

        [Test]
        public void WriteGroups_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _groupsWriter = new GroupsWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _groupsWriter.WriteGroups(_dummyCourse, dummyOutputPath));

            // Verify that logger.Error was called with a message containing "groups.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("groups.xml")));
        }
    }
}
