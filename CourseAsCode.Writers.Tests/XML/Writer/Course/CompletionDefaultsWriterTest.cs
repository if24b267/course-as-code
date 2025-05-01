using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer.Course;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Course
{
    [TestFixture]
    public class CompletiondefaultsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<CompletiondefaultsWriter> _testLogger;
        private CompletiondefaultsWriter _completiondefaultsWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger<CompletiondefaultsWriter>>();
            _completiondefaultsWriter = new CompletiondefaultsWriter(_testFactory, _testLogger);
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteCompletionDefaults_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _completiondefaultsWriter.WriteCompletionDefaults(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteCompletionDefaults_RootElementIsCourseCompletionDefaults()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _completiondefaultsWriter.WriteCompletionDefaults(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Verify that the XML root element is "course_completion_defaults"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("course_completion_defaults"));
        }

        [Test]
        public void WriteCompletionDefaults_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _completiondefaultsWriter.WriteCompletionDefaults(_dummyCourse, dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "course/completiondefaults.xml"
            _testLogger.Received().LogDebug(Arg.Is<string>(s => s.Contains("course/completiondefaults.xml")));
        }

        [Test]
        public void WriteCompletionDefaults_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _completiondefaultsWriter = new CompletiondefaultsWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _completiondefaultsWriter.WriteCompletionDefaults(_dummyCourse, dummyOutputPath));

            // Assert: Verify that logger.Error was called with a message containing "course/completiondefaults.xml"
            _testLogger.Received().LogError(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("course/completiondefaults.xml")));
        }
    }
}
