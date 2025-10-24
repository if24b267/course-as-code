using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer.Course;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Course
{
    [TestFixture]
    public class InforefWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<InforefWriter> _testLogger;
        private InforefWriter _inforefWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<InforefWriter>>();
            _inforefWriter = new InforefWriter(_testFactory, _testLogger);
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteInforef_GeneratesValidXmlAndCorrectStructure()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _inforefWriter.WriteInforef(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            var doc = new XmlDocument();
            Assert.DoesNotThrow(() => doc.LoadXml(output));
        }

        [Test]
        public void WriteInforef_RootElementIsInforef()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _inforefWriter.WriteInforef(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the root element is "inforef".
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.That(doc.DocumentElement.Name, Is.Not.Null, "The root element should not be null");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("inforef"));
        }

        [Test]
        public void WriteInforef_ContainsUserref()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _inforefWriter.WriteInforef(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the <userref> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var userref = doc.DocumentElement.SelectSingleNode("userref");
            Assert.That(userref, Is.Not.Null, "Missing <userref> element");
        }

        [Test]
        public void WriteInforef_ContainsRoleref()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _inforefWriter.WriteInforef(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the <roleref> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var roleref = doc.DocumentElement.SelectSingleNode("roleref");
            Assert.That(roleref, Is.Not.Null, "Missing <roleref> element");
        }

        [Test]
        public void WriteInforef_ContainsFileref()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _inforefWriter.WriteInforef(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the <fileref> element exists
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var roleref = doc.DocumentElement.SelectSingleNode("fileref");
            Assert.That(roleref, Is.Not.Null, "Missing <fileref> element");
        }

        [Test]
        public void WriteInforef_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _inforefWriter.WriteInforef(_dummyCourse, dummyOutputPath);

            // Assert: Verify that a debug log was written with a message containing "course/inforef.xml"
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("course/inforef.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteInforef_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _inforefWriter = new InforefWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() => _inforefWriter.WriteInforef(_dummyCourse, dummyOutputPath));

            // Verify that an error log was written with a message containing "course/inforef.xml"
            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("course/inforef.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
