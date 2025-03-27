using System.Xml;
using CourseAsCode.Model;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.Sections;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Sections
{
    [TestFixture]
    public class SectionWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private IFileService _fakeFileService;
        private SectionWriter _sectionWriter;
        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";
        
        // Dummy section
        private Section _dummySection = new Section
        {
            Name = "Test Section",
            Summary = "This is a test section"
        };

        [SetUp]
        public void SetUp()
        {
            // Use the fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger>();

            // Create a substitute for the IFileService
            _fakeFileService = Substitute.For<IFileService>();

            // Instantiate the SectionWriter with injected dependencies
            _sectionWriter = new SectionWriter(_testFactory, _testLogger, _fakeFileService);
        }

        [Test]
        public void WriteSections_CreatesValidXml()
        {
            // Arrange: Create sections list (of course) with a dummy section
            var sections = new List<Section> { _dummySection };

            // Act: Write sections
            _sectionWriter.WriteSections(DEFAULT_DUMMY_OUTPUT_PATH, sections);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }
        
        [Test]
        public void WriteSections_ContainsSectionElementWithExpectedAttributes()
        {
            // Arrange: Create sections list (of course) with a dummy section
            var sections = new List<Section> { _dummySection };

            // Act: Write sections
            _sectionWriter.WriteSections(DEFAULT_DUMMY_OUTPUT_PATH, sections);
            string output = _testFactory.StringWriter.ToString();
            
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: Verify that the output XML is well formed and contains the expected <section> element
            var sectionElement = xmlDoc.DocumentElement;
            Assert.IsNotNull(sectionElement, "No root element found");
            Assert.That(sectionElement.Name, Is.EqualTo("section"));
            Assert.That(sectionElement.GetAttribute("id"), Is.EqualTo(_dummySection.SystemDefaults.SectionId.ToString()));
        }
        
        [Test]
        public void WriteSections_SectionElementContainsExpectedElements()
        {
            // Arrange: Create sections list (of course) with a dummy section
            var sections = new List<Section> { _dummySection };

            // Act: Write sections
            _sectionWriter.WriteSections(DEFAULT_DUMMY_OUTPUT_PATH, sections);
            string output = _testFactory.StringWriter.ToString();
            
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: Verify that some of the child elements are present
            // Full Schema Validation should be done in integration tests
            var sectionElement = xmlDoc.DocumentElement;

            Assert.That(sectionElement?.SelectSingleNode("name")?.InnerText, Is.EqualTo(_dummySection.Name));
            Assert.That(sectionElement?.SelectSingleNode("summary")?.InnerText, Is.EqualTo(_dummySection.Summary));
        }
        
        [Test]
        public void WriteSections_LogsDebugOnSuccess()
        {
            // Arrange: Create sections list (of course) with a dummy section
            var sections = new List<Section> { _dummySection };

            // Act: Write sections
            _sectionWriter.WriteSections(DEFAULT_DUMMY_OUTPUT_PATH, sections);

            // Verify that a debug log was recorded for successful creation
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains($"section_{_dummySection.SystemDefaults.SectionId}/section.xml")));
        }

        [Test]
        public void WriteSections_DirectoryCreationFails_LogsErrorAndSkipsSection()
        {
            // Arrange: Create sections list (of course) with a dummy section
            var sections = new List<Section> { _dummySection };

            // Simulate directory creation failure
            _fakeFileService
                .When(x => x.CreateDirectory(Arg.Any<string>()))
                .Do(x => throw new InvalidOperationException("Test directory failure"));

            // Act: Call WriteSections. The exception during directory creation should be caught and logged,
            // and this section should be skipped (i.e. no XML file is created).
            Assert.DoesNotThrow(() => _sectionWriter.WriteSections(DEFAULT_DUMMY_OUTPUT_PATH, sections));
            string output = _testFactory.StringWriter.ToString();
            
            // Assert: Verify that the output is empty because the section was skipped
            Assert.IsEmpty(output, "Output should be empty because the section was skipped.");

            // Verify that error logs were recorded
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("Error creating directory for section")));
            _testLogger.Received().Error(Arg.Is<string>(s => s.Contains("Skipping section")));
        }
    }
}
