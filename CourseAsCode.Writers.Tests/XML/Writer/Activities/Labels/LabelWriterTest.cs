using System.Xml;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Labels;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.Labels
{
    [TestFixture]
    public class LabelWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private LabelWriter _labelWriter;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        // Dummy label activity
        private TextAndMediaArea _dummyLabel = new TextAndMediaArea
        {
            ActivityId = 1,
            Name = "Test Label",
            Content = "This is a test label content",
            FormattingFormat = FormattingFormat.Markdown,
            SystemDefaults = new TextAndMediaArea.SystemDefaultsTextAndMediaArea()
            {
                ModuleId = 101, 
                ModuleName = "label"
            },
        };

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory to capture XML output
            _testFactory = new XmlWriterFactoryTest();

            // Set up a substitute logger
            _testLogger = Substitute.For<ILogger>();

            // Create a substitute for the general helper and configure its methods
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("4");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Instantiate LabelWriter with injected dependencies
            _labelWriter = new LabelWriter(_testFactory, _testLogger, _fakeGeneralHelper);
        }

        [Test]
        public void WriteLabel_CreatesValidXml()
        {
            // Act
            _labelWriter.WriteLabel(DEFAULT_DUMMY_OUTPUT_PATH, _dummyLabel);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteLabel_RootElementIsActivity_HasCorrectAttributes()
        {
            // Act
            _labelWriter.WriteLabel(DEFAULT_DUMMY_OUTPUT_PATH, _dummyLabel);
            string output = _testFactory.StringWriter.ToString();
            
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: The root element is <activity> with correct attributes
            var root = xmlDoc.DocumentElement;
            Assert.IsNotNull(root, "The XML document should have a root element");
            Assert.That(root.Name, Is.EqualTo("activity"));
            Assert.That(root.GetAttribute("id"), Is.EqualTo(_dummyLabel.ActivityId.ToString()));
            Assert.That(root.GetAttribute("moduleid"), Is.EqualTo(_dummyLabel.SystemDefaults.ModuleId.ToString()));
            Assert.That(root.GetAttribute("modulename"), Is.EqualTo(_dummyLabel.SystemDefaults.ModuleName));
            Assert.That(root.GetAttribute("contextid"), Is.EqualTo("4")); // from mocked EnumIntegerToString
        }

        [Test]
        public void WriteLabel_ContainsLabelElementWithRequiredElements()
        {
            // Act
            _labelWriter.WriteLabel(DEFAULT_DUMMY_OUTPUT_PATH, _dummyLabel);
            string output = _testFactory.StringWriter.ToString();
            
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: Verify that the <label> element exists with the expected child elements.
            var labelElement = xmlDoc.DocumentElement?.SelectSingleNode("label") as XmlElement;
            Assert.IsNotNull(labelElement, "The <label> element is missing");
            Assert.That(labelElement.GetAttribute("id"), Is.EqualTo(_dummyLabel.ActivityId.ToString()));

            // Check that the child elements are written as expected.
            Assert.That(labelElement["name"]?.InnerText, Is.EqualTo(_dummyLabel.Name));
            Assert.That(labelElement["intro"]?.InnerText, Is.EqualTo(_dummyLabel.Content ?? string.Empty));
            Assert.That(labelElement["introformat"]?.InnerText, Is.EqualTo("4")); // from mocked EnumIntegerToString
            Assert.That(labelElement["timemodified"]?.InnerText, Is.EqualTo("1234567890")); // from mocked general helper
        }

        [Test]
        public void WriteLabel_LogsDebugOnSuccess()
        {
            // Act
            _labelWriter.WriteLabel(DEFAULT_DUMMY_OUTPUT_PATH, _dummyLabel);

            // Assert: Verify that a debug log was recorded.
            _testLogger.Received().Debug(Arg.Is<string>(s =>
                s.Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) &&
                s.Contains("label.xml")));
        }

        [Test]
        public void WriteLabel_LogsErrorOnException()
        {
            // Arrange: Use a factory that throws an exception to simulate a failure.
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _labelWriter = new LabelWriter(throwingFactory, _testLogger, _fakeGeneralHelper);

            // Act & Assert: Verify that an exception is thrown and that an error log is recorded.
            Assert.Throws<InvalidOperationException>(() => 
                _labelWriter.WriteLabel(DEFAULT_DUMMY_OUTPUT_PATH, _dummyLabel));

            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s =>
                s.Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) &&
                s.Contains("label.xml")));
        }
    }
}
