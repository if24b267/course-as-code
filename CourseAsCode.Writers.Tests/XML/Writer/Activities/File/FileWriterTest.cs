using System.Xml;
using NLog;
using NSubstitute;
using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Enums;
using CourseAsCode.Writers.XML.Writer.Activities.File;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.UnitTests.Fakes;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.File
{
    [TestFixture]
    public class FileWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private FileWriter _fileWriter;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";

        // Dummy MoodleFile activity
        private MoodleFile _dummyFile = new MoodleFile()
        {
            Name = "TestFile",
            ActivityId = 2,
            FileName = "TestFile.txt",
            Description = "Test file description",
            DescriptionFormat = FormattingFormat.Markdown,
            TimeModified = new DateTime(2020, 1, 1),
            SystemDefaults = new MoodleFile.SystemDefaultsFile()
            {
                ModuleId = 202,
                ModuleName = "resource"
            },
        };

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory to capture XML output.
            _testFactory = new XmlWriterFactoryTest();

            // Set up a substitute logger.
            _testLogger = Substitute.For<ILogger>();

            // Create a substitute for the general helper and configure its methods
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("4");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("9876543210");

            // Instantiate FileWriter with injected dependencies
            _fileWriter = new FileWriter(_testFactory, _testLogger, _fakeGeneralHelper);
        }

        [Test]
        public void WriteFile_CreatesValidXml()
        {
            // Act
            _fileWriter.WriteFile(DEFAULT_DUMMY_OUTPUT_PATH, _dummyFile);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteFile_RootElementIsActivity_HasCorrectAttributes()
        {
            // Act
            _fileWriter.WriteFile(DEFAULT_DUMMY_OUTPUT_PATH, _dummyFile);
            string output = _testFactory.StringWriter.ToString();
            
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: The root element is <activity> and its attributes match the expected values.
            var root = xmlDoc.DocumentElement;
            Assert.IsNotNull(root, "The XML document should have a root element");
            Assert.That(root.Name, Is.EqualTo("activity"));
            Assert.That(root.GetAttribute("id"), Is.EqualTo(_dummyFile.ActivityId.ToString()));
            Assert.That(root.GetAttribute("moduleid"), Is.EqualTo(_dummyFile.SystemDefaults.ModuleId.ToString()));
            Assert.That(root.GetAttribute("modulename"), Is.EqualTo(_dummyFile.SystemDefaults.ModuleName));
            Assert.That(root.GetAttribute("contextid"), Is.EqualTo("4")); // from mocked EnumIntegerToString
        }
        
        [Test]
        public void WriteFile_ContainsResourceElement()
        {
            // Act
            _fileWriter.WriteFile(DEFAULT_DUMMY_OUTPUT_PATH, _dummyFile);
            string output = _testFactory.StringWriter.ToString();
    
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: Verify that the <resource> element exists and has the correct "id" attribute.
            var resourceElement = xmlDoc.DocumentElement?.SelectSingleNode("resource") as XmlElement;
            Assert.IsNotNull(resourceElement, "The <resource> element is missing");
            Assert.That(resourceElement.GetAttribute("id"), Is.EqualTo(_dummyFile.ActivityId.ToString()));
        }

        [Test]
        public void WriteFile_ResourceElementContainsChildElements()
        {
            // Act
            _fileWriter.WriteFile(DEFAULT_DUMMY_OUTPUT_PATH, _dummyFile);
            string output = _testFactory.StringWriter.ToString();
    
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);
            var resourceElement = xmlDoc.DocumentElement?.SelectSingleNode("resource") as XmlElement;
    
            // Check some of the child elements of <resource> for expected content
            // Full Schema Validation should be done in integration tests
            Assert.That(resourceElement?["name"]?.InnerText, Is.EqualTo(_dummyFile.FileName));
            Assert.That(resourceElement["intro"]?.InnerText, Is.EqualTo(_dummyFile.Description ?? ""));
            Assert.That(resourceElement["introformat"]?.InnerText, Is.EqualTo("4")); // from mocked EnumIntegerToString
        }

        [Test]
        public void WriteFile_LogsDebugOnSuccess()
        {
            // Act
            _fileWriter.WriteFile(DEFAULT_DUMMY_OUTPUT_PATH, _dummyFile);

            // Assert: Verify that a debug log was recorded indicating the file creation.
            _testLogger.Received().Debug(Arg.Is<string>(s =>
                s.Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) &&
                s.Contains("resource.xml")));
        }

        [Test]
        public void WriteFile_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception.
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _fileWriter = new FileWriter(throwingFactory, _testLogger, _fakeGeneralHelper);

            // Act & Assert: Ensure that an exception is thrown and that an error log is recorded.
            Assert.Throws<InvalidOperationException>(() =>
                _fileWriter.WriteFile(DEFAULT_DUMMY_OUTPUT_PATH, _dummyFile));

            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s =>
                s.Contains(Path.GetFileName(DEFAULT_DUMMY_OUTPUT_PATH)) &&
                s.Contains("resource.xml")));
        }
    }
}
