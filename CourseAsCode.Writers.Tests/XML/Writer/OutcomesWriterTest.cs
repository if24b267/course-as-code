using System.Xml;
using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class OutcomesWriterTests
    {
        private IXmlServiceFactory _mockXmlFactory;
        private IXmlService _mockXmlService;
        private ILogger<OutcomesWriter> _mockLogger;
        private OutcomesWriter _outcomesWriter;
        private CourseAsCode.Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            // Arrange Mocks
            _mockXmlFactory = Substitute.For<IXmlServiceFactory>();
            _mockXmlService = Substitute.For<IXmlService>();
            _mockLogger = Substitute.For<ILogger<OutcomesWriter>>();

            // Setup the factory to return the mock service when Create is called
            _mockXmlFactory.Create(Arg.Any<string>()).Returns(_mockXmlService);

            // Create dummy data
            _dummyCourse = CourseDefaults.GetDefaultCourse();

            // Instantiate System Under Test (SUT)
            _outcomesWriter = new OutcomesWriter(_mockXmlFactory, _mockLogger);
        }

        [TearDown]
        public void Dispose()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void WriteOutcomes_RootElementIsOutcomesDefinition()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _outcomesWriter.WriteOutcomes(_dummyCourse, dummyOutputPath);

            // Assert: Verify that the root element is "outcomes_definition"
            _mockXmlService.Received(1).WriteElement("outcomes_definition");

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }
    }
}
