using System.Xml;
using System.Xml.Schema;
using CourseAsCode.Model;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Plugins;
using QWriter = CourseAsCode.Writers.XML.Writer.QuestionsWriter.QuestionsWriter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class QuestionsWriterIntegrationTests
    {
        private string _xsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        // Dependencies needed for QuestionsWriter
        private IXmlWriterFactory _xmlWriterFactory;
        private ILogger<QWriter> _logger;
        private ILogger<TagsWriter> _tagsLogger;
        private ILogger<MultiChoicePluginWriter> _multiChoicePluginWriterLogger;
        private ILogger<DragDropTextPluginWriter> _dragDropTextPluginWriterLogger;
        private ILogger<DropdownPluginWriter> _dropdownPluginWriterLogger;
        private IGeneralHelper _generalHelper;
        private ISequentialIdGenerator _sequentialIdGenerator;
        private ITagsWriter _tagsWriter;
        private readonly List<IQuestionPluginWriter> _pluginWriters = new List<IQuestionPluginWriter>();

        // Dependencies for GeneralHelper
        private ILogger<GeneralHelper> _generalHelperLogger;


        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            _xsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "questions.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _xmlWriterFactory = new DefaultXmlWriterFactory();
            _logger = NullLogger<QWriter>.Instance; // Use NullLogger to avoid log output during tests
            _tagsLogger = NullLogger<TagsWriter>.Instance; // Use NullLogger to avoid log output during tests
            _multiChoicePluginWriterLogger = NullLogger<MultiChoicePluginWriter>.Instance; // Use NullLogger to avoid log output during tests
            _dropdownPluginWriterLogger = NullLogger<DropdownPluginWriter>.Instance; // Use NullLogger to avoid log output during tests
            _dragDropTextPluginWriterLogger = NullLogger<DragDropTextPluginWriter>.Instance; // Use NullLogger to avoid log output during tests
            _generalHelper = new GeneralHelper(_generalHelperLogger);
            _sequentialIdGenerator = new SequentialIdGenerator();
            _tagsWriter = new TagsWriter(_tagsLogger);

            // PluginWriters
            _pluginWriters.Add(new DragDropTextPluginWriter(_xmlWriterFactory, _dragDropTextPluginWriterLogger));
            _pluginWriters.Add(new MultiChoicePluginWriter(_xmlWriterFactory, _multiChoicePluginWriterLogger));
            _pluginWriters.Add(new DropdownPluginWriter(_xmlWriterFactory, _dropdownPluginWriterLogger));

            _dummyCourse = new Course
            {
                FullName = "Test Course",
                ShortName = "TC",
            };
        }

        [Test]
        public void WriteQuestions_GeneratesValidXml()
        {
            // Arrange
            var writer = new QWriter(_xmlWriterFactory, _logger, _generalHelper, _sequentialIdGenerator, _tagsWriter, _pluginWriters);

            // Act
            writer.WriteQuestions(_dummyCourse, _outputPath);

            // Assert
            ValidateXml($"{_outputPath}/questions.xml", _xsdPath);
        }

        private void ValidateXml(string xmlPath, string xsdPath)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdPath);
            settings.ValidationType = ValidationType.Schema;

            using (var reader = XmlReader.Create(xmlPath, settings))
            {
                var document = new XmlDocument();
                document.Load(reader);
                document.Validate((sender, e) =>
                {
                    if (e.Severity == XmlSeverityType.Error)
                    {
                        Assert.Fail($"XML Validation Error: {e.Message}");
                    }
                });
            }
        }
    }
}

