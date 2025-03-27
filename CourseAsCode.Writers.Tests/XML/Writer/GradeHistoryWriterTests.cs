using System.Xml;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Writer;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class GradehistoryWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private GradehistoryWriter _gradehistoryWriter;
        private Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            _testFactory = new XmlWriterFactoryTest(); // fake XML Writer factory that creates an XML writer that writes to an in-memory StringWriter instead
            _testLogger = Substitute.For<ILogger>();
            _gradehistoryWriter = new GradehistoryWriter(_testFactory, _testLogger); // Use DI to inject the test factory and logger
            _dummyCourse = new Model.Course();
        }

        [Test]
        public void WriteGradehistory_GeneratesValidXml()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _gradehistoryWriter.WriteGradehistory(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteGradehistory_RootElementIsGradeHistory()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _gradehistoryWriter.WriteGradehistory(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The root element should be "grade_history"
            var doc = new XmlDocument();
            doc.LoadXml(output);
            Assert.IsNotNull(doc.DocumentElement, "XML should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("grade_history"));
        }

        [Test]
        public void WriteGradehistory_HasGradeGradesSection()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _gradehistoryWriter.WriteGradehistory(_dummyCourse, dummyOutputPath);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The produced XML should include a <grade_grades> element
            var doc = new XmlDocument();
            doc.LoadXml(output);
            var gradeGradesNode = doc.DocumentElement?.SelectSingleNode("grade_grades");
            Assert.IsNotNull(gradeGradesNode, "The <grade_grades> element is missing.");
        }

        [Test]
        public void WriteGradehistory_LogsDebugOnSuccess()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _gradehistoryWriter.WriteGradehistory(_dummyCourse, dummyOutputPath);

            // Assert: Verify that logger.Debug was called with a message containing "gradehistory.xml"
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains("gradehistory.xml")));
        }

        [Test]
        public void WriteGradehistory_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _gradehistoryWriter = new GradehistoryWriter(throwingFactory, _testLogger);
            string dummyOutputPath = "dummyPath";

            // Act & Assert: Verify that an exception is thrown.
            Assert.Throws<InvalidOperationException>(() => 
                _gradehistoryWriter.WriteGradehistory(_dummyCourse, dummyOutputPath));

            // Verify that logger.Error was called with a message containing "gradehistory.xml"
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("gradehistory.xml")));
        }
    }
}
