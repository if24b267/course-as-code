using CourseAsCode.Model.Defaults;
using CourseAsCode.Writers.XML.FileWriters;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class BadgesWriterTests
    {
        private IXmlServiceFactory _mockXmlFactory;
        private IXmlService _mockXmlService;
        private ILogger<BadgesWriter> _mockLogger;
        private BadgesWriter _badgesWriter;
        private CourseAsCode.Model.Course _dummyCourse;

        [SetUp]
        public void SetUp()
        {
            // Arrange Mocks
            _mockXmlFactory = Substitute.For<IXmlServiceFactory>();
            _mockXmlService = Substitute.For<IXmlService>();
            _mockLogger = Substitute.For<ILogger<BadgesWriter>>();

            // Setup the factory to return the mock service when Create is called
            _mockXmlFactory.Create(Arg.Any<string>()).Returns(_mockXmlService);

            // Create dummy data
            _dummyCourse = CourseDefaults.GetDefaultCourse();

            // Instantiate System Under Test (SUT)
            _badgesWriter = new BadgesWriter(_mockXmlFactory, _mockLogger);
        }

        [TearDown]
        public void Dispose()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void WriteBadges_RootElementIsCorrect()
        {
            // Arrange
            string dummyOutputPath = "dummyPath";

            // Act
            _badgesWriter.WriteBadges(_dummyCourse, dummyOutputPath);

            // Assert: The root element should be "badges"
            _mockXmlService.Received(1).WriteElement("badges");

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "WriteElement" || call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }
    }
}
