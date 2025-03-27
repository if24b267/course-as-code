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
    public class InforefSectionsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _testLogger;
        private IFileService _fakeFileService;
        private InforefSectionsWriter _inforefSectionsWriter;
        private List<Section> _sections = new List<Section>();
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
            // Set up the fake XML writer factory and logger
            _testFactory = new XmlWriterFactoryTest();
            _testLogger = Substitute.For<ILogger>();

            // Create a substitute for the IFileService
            _fakeFileService = Substitute.For<IFileService>();

            // Instantiate the InforefSectionsWriter with injected dependencies
            _inforefSectionsWriter = new InforefSectionsWriter(_testFactory, _testLogger, _fakeFileService);
        }

        [Test]
        public void WriteInforefSections_CreatesValidXml()
        {
            // Arrange: Create a list of sections
            _sections = new List<Section> { _dummySection };
            
            // Act
            _inforefSectionsWriter.WriteInforefSections(DEFAULT_DUMMY_OUTPUT_PATH, _sections);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }
        
        [Test]
        public void WriteInforefSections_ContainsRequiredElements()
        {
            // Arrange: Create a list of sections
            _sections = new List<Section> { _dummySection };
            
            // Act
            _inforefSectionsWriter.WriteInforefSections(DEFAULT_DUMMY_OUTPUT_PATH, _sections);
            string output = _testFactory.StringWriter.ToString();
            
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(output);

            // Assert: Verify that the XML contains the required elements
            Assert.IsNotNull(xmlDoc.DocumentElement, "The XML document should have a root element");
            Assert.That(xmlDoc.DocumentElement.Name, Is.EqualTo("inforef"));
        }
        
        [Test]
        public void WriteInforefSections_LogsDebugOnSuccess()
        {
            // Arrange: Create a list of sections
            _sections = new List<Section> { _dummySection };
            
            // Act
            _inforefSectionsWriter.WriteInforefSections(DEFAULT_DUMMY_OUTPUT_PATH, _sections);
            
            // Assert: Verify that a debug log was recorded
            _testLogger.Received().Debug(Arg.Is<string>(s => s.Contains($"section_{_dummySection.SystemDefaults.SectionId}/inforef.xml")));
        }

        [Test]
        public void WriteInforefSections_DirectoryCreationFails_SkipsSectionAndLogsError()
        {
            // Arrange: Configure the fake file service to throw an exception when CreateDirectory is called.
            _fakeFileService
                .When(fs => fs.CreateDirectory(Arg.Any<string>()))
                .Do(x => { throw new InvalidOperationException("Test directory failure"); });
            
            // Create a list of sections
            _sections = new List<Section> { _dummySection };

            // Act
            _inforefSectionsWriter.WriteInforefSections(DEFAULT_DUMMY_OUTPUT_PATH, _sections);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Since directory creation failed, no XML should be written.
            Assert.IsEmpty(output, "Output should be empty because the section was skipped.");

            // Verify that error logs were recorded.
            _testLogger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("Error creating directory for section")));
            _testLogger.Received().Error(Arg.Is<string>(s => s.Contains("Skipping section")));
        }
    }
}
