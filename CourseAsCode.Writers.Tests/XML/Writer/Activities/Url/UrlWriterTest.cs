using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Url;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.Url
{
    [TestFixture]
    public class UrlWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<UrlWriter> _testLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private UrlWriter _urlWriter;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger<UrlWriter>>();

            // Create a substitute for the general helper and configure its methods
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Instantiate the UrlWriter with injected dependencies
            _urlWriter = new UrlWriter(_testFactory, _testLogger, _fakeGeneralHelper);
        }

        [Test]
        public void WriteUrl_CreatesValidXml()
        {
            // Arrange
            URL dummyUrl = FakeUrlFactory.CreateBasicUrl(1);

            // Act
            _urlWriter.WriteUrl(DEFAULT_DUMMY_OUTPUT_PATH, dummyUrl);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteUrl_RootElementIsActivity_HasCorrectAttributes()
        {
            // Arrange
            URL dummyUrl = FakeUrlFactory.CreateBasicUrl(1);

            // Act
            _urlWriter.WriteUrl(DEFAULT_DUMMY_OUTPUT_PATH, dummyUrl);
            string output = _testFactory.StringWriter.ToString();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: The root element is <activity> and has expected attributes
            var activityElement = xmlDoc.DocumentElement;
            Assert.That(activityElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(activityElement.Name, Is.EqualTo("activity"));
            Assert.That(activityElement.GetAttribute("id"), Is.EqualTo(dummyUrl.ActivityId.ToString()));
            Assert.That(activityElement.GetAttribute("moduleid"), Is.EqualTo(dummyUrl.ModuleId.ToString()));
            Assert.That(activityElement.GetAttribute("modulename"), Is.EqualTo(dummyUrl.ModuleName));
            Assert.That(activityElement.GetAttribute("contextid"), Is.EqualTo("0")); // from mocked EnumIntegerToString
        }

        [Test]
        public void WriteUrl_ContainsUrlElementWithRequiredElements()
        {
            // Arrange
            URL dummyUrl = FakeUrlFactory.CreateBasicUrl(1);

            // Act
            _urlWriter.WriteUrl(DEFAULT_DUMMY_OUTPUT_PATH, dummyUrl);
            string output = _testFactory.StringWriter.ToString();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: Verify that <url> element exists and contains its child elements
            var urlElement = xmlDoc.DocumentElement?.SelectSingleNode("url") as XmlElement;
            Assert.That(urlElement, Is.Not.Null, "The <url> element is missing");
            Assert.That(urlElement.GetAttribute("id"), Is.EqualTo(dummyUrl.ActivityId.ToString()));

            // Check some of the child elements for expected content
            // Full Schema Validation should be done in integration tests
            Assert.That(urlElement["name"]?.InnerText, Is.EqualTo(dummyUrl.Name));
            Assert.That(urlElement["intro"]?.InnerText, Is.EqualTo(dummyUrl.Description ?? string.Empty));
            Assert.That(urlElement["externalurl"]?.InnerText, Is.EqualTo(dummyUrl.ExternalUrl));
        }

        [Test]
        public void WriteUrl_LogsDebugOnSuccess()
        {
            // Arrange
            URL dummyUrl = FakeUrlFactory.CreateBasicUrl(1);

            // Act
            _urlWriter.WriteUrl(DEFAULT_DUMMY_OUTPUT_PATH, dummyUrl);

            // Assert: Verify that a debug log was recorded
            _testLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) && o.ToString().Contains("url.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteUrl_LogsErrorOnException()
        {
            // Arrange
            URL dummyUrl = FakeUrlFactory.CreateBasicUrl(1);

            // Arrange: Use a throwing XML writer factory to simulate an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _urlWriter = new UrlWriter(throwingFactory, _testLogger, _fakeGeneralHelper);

            // Act & Assert: Verify that an exception is thrown and error log is recorded
            Assert.Throws<InvalidOperationException>(() =>
                _urlWriter.WriteUrl(DEFAULT_DUMMY_OUTPUT_PATH, dummyUrl));

            _testLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) && o.ToString().Contains("url.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
