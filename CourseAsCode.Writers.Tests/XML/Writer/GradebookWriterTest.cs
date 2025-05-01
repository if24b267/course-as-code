using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class GradebookWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<GradebookWriter> _testLogger;
        private GradebookWriter _gradebookWriter;
        private Model.Course _dummyCourse;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<GradebookWriter>>();
            _gradebookWriter = new GradebookWriter(_testFactory, _testLogger);
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteGradebook_GeneratesValidXml()
        {
            // Act
            _gradebookWriter.WriteGradebook(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteGradebook_RootElementIsGradebook()
        {
            // Act
            _gradebookWriter.WriteGradebook(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "gradebook"
            var doc = new XmlDocument();
            doc.LoadXml(output);

            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("gradebook"));
        }

        [Test]
        public void WriteGradebook_ContainsRequiredElements()
        {
            // Act
            _gradebookWriter.WriteGradebook(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Check that the required child elements exist in the gradebook XML
            var doc = new XmlDocument();
            doc.LoadXml(output);

            var attributesNode = doc.DocumentElement?.SelectSingleNode("attributes");
            var gradeCategoriesNode = doc.DocumentElement?.SelectSingleNode("grade_categories");
            var gradeItemsNode = doc.DocumentElement?.SelectSingleNode("grade_items");
            var gradeLettersNode = doc.DocumentElement?.SelectSingleNode("grade_letters");
            var gradeSettingsNode = doc.DocumentElement?.SelectSingleNode("grade_settings");

            Assert.IsNotNull(attributesNode, "The <attributes> element is missing");
            Assert.IsNotNull(gradeCategoriesNode, "The <grade_categories> element is missing");
            Assert.IsNotNull(gradeItemsNode, "The <grade_items> element is missing");
            Assert.IsNotNull(gradeLettersNode, "The <grade_letters> element is missing");
            Assert.IsNotNull(gradeSettingsNode, "The <grade_settings> element is missing");
        }

        [Test]
        public void WriteGradebook_LogsDebugOnSuccess()
        {
            // Act
            _gradebookWriter.WriteGradebook(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert: Verify that a debug log was recorded containing the gradebook file name
            _testLogger.Received().LogDebug(Arg.Is<string>(s => s.Contains("gradebook.xml")));
        }

        [Test]
        public void WriteGradebook_LogsErrorOnException()
        {
            // Arrange: Use a fake XML writer factory that throws an exception when creating the writer
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _gradebookWriter = new GradebookWriter(throwingFactory, _testLogger);

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _gradebookWriter.WriteGradebook(_dummyCourse, DEFAULT_DUMMY_OUTPUT_PATH));

            // Assert: Verify that an error log was recorded with a message containing "gradebook.xml"
            _testLogger.Received().LogError(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("gradebook.xml")));
        }
    }
}
