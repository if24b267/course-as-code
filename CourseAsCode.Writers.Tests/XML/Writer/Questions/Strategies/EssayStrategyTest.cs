using CourseAsCode.Model.Questions.Types.Essay;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class EssayStrategyTests
    {
        private ILogger<EssayStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IXmlService _mockXmlService;
        private EssayStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";

        [SetUp]
        public void SetUp()
        {
            // Create Mocks
            _mockLogger = Substitute.For<ILogger<EssayStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockXmlService = Substitute.For<IXmlService>();

            // Configure default mock behaviors
            var idCounter = 1;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => idCounter++);

            // Mock the enum conversion (e.g. HTML -> "1")
            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            // Initialize System Under Test
            _strategy = new EssayStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsEssay()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("essay"));
        }

        [Test]
        public void WritePluginData_WithInvalidQuestionType_ThrowsArgumentException()
        {
            // Arrange - We intentionally create a wrong question type
            var invalidQuestion = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _strategy.WritePluginData(_mockXmlService, invalidQuestion));
            Assert.That(ex.Message, Does.Contain("Expected EssayQuestion"));
        }

        [Test]
        public void WritePluginData_WritesRootStructure()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicEssayQuestion(1, "CatA");

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert call order
            _mockXmlService.Received().WriteElement("plugin_qtype_essay_question");
            _mockXmlService.Received().WriteElementWithAttributes("essay", Arg.Any<Dictionary<string, string>>());

            // Check if all opened tags were closed
            int openTags = _mockXmlService.ReceivedCalls().Count(call =>
                call.GetMethodInfo().Name == "WriteElement" ||
                call.GetMethodInfo().Name == "WriteElementWithAttributes");

            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesCorrectConfigurationData()
        {
            // Arrange
            var question = new EssayQuestion
            {
                QuestionName = "essay test",
                QuestionText = "just testing essays",
                Category = "essay",
                ResponseFormat = "editor",
                ResponseRequired = true,
                ResponseFieldLines = 25,
                MinWordLimit = 100,
                MaxWordLimit = 1000,
                AttachmentsAllowed = 2,
                AttachmentsRequired = 1,
                FileTypesList = ".pdf",
                ResponseTemplate = "Please start your analysis here...",
                GraderInfo = "Check for logical consistency and citations."
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["responseformat"] == "editor" &&
                d["responserequired"] == "1" &&
                d["responsefieldlines"] == "25" &&
                d["minwordlimit"] == "100" &&
                d["maxwordlimit"] == "1000" &&
                d["attachments"] == "2" &&
                d["attachmentsrequired"] == "1" &&
                d["filetypeslist"] == ".pdf" &&
                d["responsetemplate"] == "Please start your analysis here..." &&
                d["graderinfo"] == "Check for logical consistency and citations."
            ));
        }

        [Test]
        public void WritePluginData_WithNullableWordLimits_WritesEmptyStrings()
        {
            // Arrange
            var question = new EssayQuestion
            {
                Category = "TestCategory",
                QuestionName = "TestName",
                QuestionText = "TestText",

                MinWordLimit = null,
                MaxWordLimit = null
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["minwordlimit"] == string.Empty &&
                d["maxwordlimit"] == string.Empty
            ));
        }
    }
}
