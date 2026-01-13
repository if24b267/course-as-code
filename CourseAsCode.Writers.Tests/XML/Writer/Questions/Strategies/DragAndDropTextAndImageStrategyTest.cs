using CourseAsCode.Model.Questions.Types.DragAndDropTextAndImage;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Strategies;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions.Strategies
{
    [TestFixture]
    public class DragAndDropTextAndImageStrategyTests
    {
        private ILogger<DragAndDropTextAndImageStrategy> _mockLogger;
        private IGeneralHelper _mockGeneralHelper;
        private ISequentialIdGenerator _mockIdGenerator;
        private IQuizHelper _mockQuizHelper;
        private IXmlService _mockXmlService;
        private DragAndDropTextAndImageStrategy _strategy;

        private const string MOCKED_FORMAT_STRING = "1";

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<ILogger<DragAndDropTextAndImageStrategy>>();
            _mockGeneralHelper = Substitute.For<IGeneralHelper>();
            _mockIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _mockQuizHelper = Substitute.For<IQuizHelper>();
            _mockXmlService = Substitute.For<IXmlService>();

            var idCounter = 100;
            _mockIdGenerator.GetNextAnswerId().Returns(_ => idCounter++);
            _mockGeneralHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            _strategy = new DragAndDropTextAndImageStrategy(_mockLogger, _mockGeneralHelper, _mockIdGenerator, _mockQuizHelper);
        }

        [TearDown]
        public void TearDown()
        {
            _mockXmlService.Dispose();
        }

        [Test]
        public void GetQuestionTypeString_ReturnsDdimageortext()
        {
            Assert.That(_strategy.GetQuestionTypeString(), Is.EqualTo("ddimageortext"));
        }

        [Test]
        public void WritePluginData_WithInvalidQuestionType_ThrowsArgumentException()
        {
            // Arrange
            var invalidQuestion = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _strategy.WritePluginData(_mockXmlService, invalidQuestion));
            Assert.That(ex.Message, Does.Contain("Expected DragAndDropTextAndImageQuestion"));
        }

        [Test]
        public void WritePluginData_WritesRootStructure()
        {
            // Arrange
            var question = new DragAndDropTextAndImageQuestion { QuestionName = "TestName", QuestionText = "TestText", Category = "TestCategorie", Drags = new(), Drops = new() };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElement("plugin_qtype_ddimageortext_question");
            _mockXmlService.Received().WriteElementWithAttributes("ddimageortext", Arg.Any<Dictionary<string, string>>());
            _mockXmlService.Received().WriteElement("drags");
            _mockXmlService.Received().WriteElement("drops");

            int openTags = _mockXmlService.ReceivedCalls().Count(call =>
                call.GetMethodInfo().Name == "WriteElement" ||
                call.GetMethodInfo().Name == "WriteElementWithAttributes");
            _mockXmlService.Received(openTags).CloseElement();
        }

        [Test]
        public void WritePluginData_WritesGeneralSettingsCorrectly()
        {
            // Arrange
            var question = new DragAndDropTextAndImageQuestion
            {
                QuestionName = "TestName",
                QuestionText = "TestText",
                Category = "TestCategorie",
                ShuffleAnswers = true,
                DropZoneVisibility = true,
                CorrectResponseFeedback = "Correct!",
                Drags = new(),
                Drops = new(),
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["shuffleanswers"] == "1" &&
                d["dropzonevisibility"] == "1" &&
                d["correctfeedback"] == "Correct!" &&
                d["correctfeedbackformat"] == MOCKED_FORMAT_STRING));
        }

        [Test]
        public void WritePluginData_WritesDragsWithCorrectData()
        {
            // Arrange
            var question = new DragAndDropTextAndImageQuestion
            {
                QuestionName = "TestName",
                QuestionText = "TestText",
                Category = "TestCategorie",
                Drags = new List<TextAndImageDrag>
                {
                    new TextAndImageDrag { ID = 55, Number = 1, DragGroup = 1, Label = "DragMe", Infinite = true },
                },
                Drops = new(),
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert 
            _mockXmlService.Received().WriteElementWithAttributes("drag", Arg.Is<Dictionary<string, string>>(d => d["id"] == "55"));
            _mockXmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["no"] == "1" &&
                d["draggroup"] == "1" &&
                d["infinite"] == "1" &&
                d["label"] == "DragMe"));
        }

        [Test]
        public void WritePluginData_GeneratesIdIfMissing()
        {
            // Arrange
            var question = new DragAndDropTextAndImageQuestion
            {
                QuestionName = "TestName",
                QuestionText = "TestText",
                Category = "TestCategorie",
                Drags = new List<TextAndImageDrag> { new TextAndImageDrag { ID = 0 } },
                Drops = new(),
            };
            _mockIdGenerator.GetNextAnswerId().Returns(777);

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes("drag", Arg.Do<Dictionary<string, string>>(d =>
            {
                Assert.That(d["id"], Is.EqualTo("777"), "The dictionary should contain key 'id' with value '777'");
            }));
        }

        [Test]
        public void WritePluginData_WritesDropsWithCorrectData()
        {
            // Arrange
            var question = new DragAndDropTextAndImageQuestion
            {
                QuestionName = "TestName",
                QuestionText = "TestText",
                Category = "TestCategorie",
                Drags = new(),
                Drops = new List<TextAndImageDrop>
        {
            new TextAndImageDrop { ID = 99, Number = 1, XLeft = 10, YTop = 20, Choice = 1, Label = "Target" },
        },
            };

            // Act
            _strategy.WritePluginData(_mockXmlService, question);

            // Assert
            _mockXmlService.Received().WriteElementWithAttributes("drop", Arg.Do<Dictionary<string, string>>(d =>
            {
                Assert.That(d["id"], Is.EqualTo("99"));
            }));

            _mockXmlService.Received().WriteElements(Arg.Do<Dictionary<string, string>>(d =>
            {
                Assert.That(d["no"], Is.EqualTo("1"));
                Assert.That(d["xleft"], Is.EqualTo("10"));
                Assert.That(d["ytop"], Is.EqualTo("20"));
                Assert.That(d["choice"], Is.EqualTo("1"));
                Assert.That(d["label"], Is.EqualTo("Target"));
            }));
        }
    }
}
