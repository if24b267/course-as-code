using System.Xml;
using System.Xml.Schema;

using CourseAsCode.Model;
using CourseAsCode.Writers.Tests.XML.Writer;
using CourseAsCode.Writers.XML.FileWriters.Questions;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using QWriter = CourseAsCode.Writers.XML.FileWriters.Questions.QuestionsWriter;

namespace CourseAsCode.Writers.Tests.Integration
{
    [TestFixture]
    public class QuestionsWriterIntegrationTests
    {
        private string _xsdPath;
        private Course _dummyCourse;
        private string _outputPath;

        private ServiceProvider _serviceProvider;

        // Dependencies needed for QuestionsWriter
        private IXmlServiceFactory _xmlServiceFactory;
        private ILogger<XmlServiceFactory> _xmlServiceFactoryLogger;
        private ILogger<XmlService> _xmlServiceLogger;

        private IQuestionStrategyResolver _questionStrategyResolver;

        private ILogger<QWriter> _logger;
        private ILogger<TagsWriter> _tagsLogger;
        private ILogger<MultiChoiceStrategy> _multiChoicePluginWriterLogger;
        private ILogger<DragDropTextStrategy> _dragDropTextPluginWriterLogger;
        private ILogger<DropdownStrategy> _dropdownPluginWriterLogger;
        private ILogger<QuestionStrategyResolver> _questionStrategyResolverLogger;
        private IGeneralHelper _generalHelper;
        private ISequentialIdGenerator _sequentialIdGenerator;
        private IQuizHelper _quizHelper;
        private ITagsWriter _tagsWriter;

        // Dependencies for GeneralHelper
        private ILogger<GeneralHelper> _generalHelperLogger;

        // Dependency for QuizHelper
        private ILogger<QuizHelper> _quizHelperLogger;

        [TearDown]
        public void TearDown()
        {
            _serviceProvider.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            _xsdPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "Schemas", "questions.xsd"));

            _outputPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", "..", "Documentation", "out", "moodleCourse"));

            Directory.CreateDirectory(_outputPath);

            // --- Initialize Dependencies ---
            // Use real implementations for integration testing
            _logger = NullLogger<QWriter>.Instance;
            _tagsLogger = NullLogger<TagsWriter>.Instance;
            _multiChoicePluginWriterLogger = NullLogger<MultiChoiceStrategy>.Instance;
            _dropdownPluginWriterLogger = NullLogger<DropdownStrategy>.Instance;
            _dragDropTextPluginWriterLogger = NullLogger<DragDropTextStrategy>.Instance;
            _questionStrategyResolverLogger = NullLogger<QuestionStrategyResolver>.Instance;
            _xmlServiceFactoryLogger = NullLogger<XmlServiceFactory>.Instance;
            _generalHelperLogger = NullLogger<GeneralHelper>.Instance;
            _quizHelperLogger = NullLogger<QuizHelper>.Instance;
            _generalHelper = new GeneralHelper(_generalHelperLogger);
            _sequentialIdGenerator = new SequentialIdGenerator();
            _quizHelper = new QuizHelper(_quizHelperLogger);
            _tagsWriter = new TagsWriter(_tagsLogger);

            _xmlServiceFactory = new XmlServiceFactory(_xmlServiceFactoryLogger, _xmlServiceLogger);

            // --- QuestionStrategyResolver ---
            // requires a ServiceCollection to be injected, but we don't have one here, so we need to create one manually
            var services = new ServiceCollection();

            // Register the Strategies
            services.AddTransient<CodeRunnerStrategy>();
            services.AddTransient<DragDropTextStrategy>();
            services.AddTransient<DropdownStrategy>();
            services.AddTransient<MultiChoiceStrategy>();

            // Register the Resolver itself
            services.AddTransient<IQuestionStrategyResolver, QuestionStrategyResolver>();

            // Build the ServiceProvider
            _serviceProvider = services.BuildServiceProvider();

            // Instantiate the QuestionStrategyResolver
            _questionStrategyResolver = new QuestionStrategyResolver(_serviceProvider, _questionStrategyResolverLogger);

            _dummyCourse = FakeCourseFactory.CreateCourseWithRole();
        }

        [Test]
        [Ignore("Needs rework due to refactoring")]
        public void WriteQuestions_GeneratesValidXml()
        {
            // Arrange
            var writer = new QWriter(_xmlServiceFactory, _logger, _generalHelper, _sequentialIdGenerator, _quizHelper, _tagsWriter, _questionStrategyResolver);

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

