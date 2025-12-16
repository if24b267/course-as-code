using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.Questions.Interfaces;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Questions
{
    [TestFixture]
    public class QuestionsWriterTests
    {
        private IXmlServiceFactory _xmlServiceFactory;
        private IXmlService _xmlService;
        private ILogger<Writers.XML.FileWriters.Questions.QuestionsWriter> _logger;
        private IGeneralHelper _generalHelper;
        private ISequentialIdGenerator _sequentialIdGenerator;
        private IQuizHelper _quizHelper;
        private ITagsWriter _tagsWriter;
        private IQuestionStrategyResolver _strategyResolver;
        private IQuestionStrategy _mockStrategy;

        private Writers.XML.FileWriters.Questions.QuestionsWriter _questionsWriter;

        private const string MOCKED_TIMESTAMP = "1234567890";
        private const string MOCKED_FORMAT_STRING = "1";

        [SetUp]
        public void SetUp()
        {
            // Create Mocks
            _xmlServiceFactory = Substitute.For<IXmlServiceFactory>();
            _xmlService = Substitute.For<IXmlService>();
            _logger = Substitute.For<ILogger<Writers.XML.FileWriters.Questions.QuestionsWriter>>();
            _generalHelper = Substitute.For<IGeneralHelper>();
            _sequentialIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _quizHelper = Substitute.For<IQuizHelper>();
            _tagsWriter = Substitute.For<ITagsWriter>();
            _strategyResolver = Substitute.For<IQuestionStrategyResolver>();
            _mockStrategy = Substitute.For<IQuestionStrategy>();

            // Configure Mock Behaviors
            _xmlServiceFactory.Create(Arg.Any<string>()).Returns(_xmlService);

            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns(MOCKED_TIMESTAMP);
            _generalHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns(MOCKED_FORMAT_STRING);

            // Setup IDs to return predictable strings for Dictionary checks
            _sequentialIdGenerator.GetNextCategoryId().Returns(2, 3, 4);
            _sequentialIdGenerator.GetNextBankEntryId().Returns(100, 101);
            _sequentialIdGenerator.GetNextHintId().Returns(500, 501);

            // Setup Strategy Resolver to always return a valid mock strategy
            _strategyResolver.ResolveStrategy(Arg.Any<Model.Questions.Question>()).Returns((IQuestionStrategy)null);
            _quizHelper.MapQuestionType(Arg.Any<Model.Questions.Question>()).Returns("multichoice");

            // Initialize System Under Test
            _questionsWriter = new Writers.XML.FileWriters.Questions.QuestionsWriter(
                _xmlServiceFactory,
                _logger,
                _generalHelper,
                _sequentialIdGenerator,
                _quizHelper,
                _tagsWriter,
                _strategyResolver);
        }

        [TearDown]
        public void TearDown()
        {
            _xmlService.Dispose();
        }

        [Test]
        public void WriteQuestions_WritesRootElementQuestionCategories()
        {
            // Arrange
            var course = new Model.Course();
            var outputPath = "dummy";

            // Act
            _questionsWriter.WriteQuestions(course, outputPath);

            // Assert
            _xmlService.Received(1).WriteElement("question_categories");
        }

        [Test]
        public void WriteQuestions_WritesTopCategory()
        {
            // Arrange
            var course = new Model.Course();
            var outputPath = "dummy";

            // Act
            _questionsWriter.WriteQuestions(course, outputPath);

            // Assert
            _xmlService.Received(1).WriteElementWithAttributes(
                "question_category",
                Arg.Is<Dictionary<string, string>>(d => d["id"] == "1")); // Default Top ID

            _xmlService.Received(1).WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("name") && d["name"] == "top" &&
                d.ContainsKey("parent") && d["parent"] == "0"));
        }

        [Test]
        public void WriteQuestions_CreatesCorrectNumberOfCategories()
        {
            // Arrange
            var course = new Model.Course();
            var q1 = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            var q2 = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(2, "CatA"); // Same category
            var q3 = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(3, "CatB"); // Different category
            course.Questions.AddRange(new[] { q1, q2, q3 });

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert: Should write 2 specific categories (CatA and CatB) plus the Top category (checked in different tests)
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("name") && d["name"] == "CatA"));
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d => d.ContainsKey("name") && d["name"] == "CatB"));
        }

        [Test]
        public void WriteQuestions_WritesQuestionBankSection()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            course.Questions.Add(q);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            _xmlService.Received().WriteElement("question_bank_entries");
            _xmlService.Received().WriteElementWithAttributes("question_bank_entry", Arg.Any<Dictionary<string, string>>());
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("questioncategoryid") &&
                d.ContainsKey("ownerid")));
        }

        [Test]
        public void WriteQuestions_CreatesCorrectNumberOfBankEntries()
        {
            // Arrange
            var course = new Model.Course();
            course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA"));
            course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(2, "CatA"));
            course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(3, "CatB"));

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert: The total number of question_bank_entry elements should match total questions (at least that's the case in our implementation)
            _xmlService.Received().WriteElement("question_bank_entries");
            _xmlService.Received(3).WriteElementWithAttributes("question_bank_entry", Arg.Any<Dictionary<string, string>>());
        }

        [Test]
        public void WriteQuestions_WritesQuestionVersion()
        {
            // Arrange
            var course = new Model.Course();
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            _xmlService.Received().WriteElement("question_version");
            _xmlService.Received().WriteElementWithAttributes("question_versions", Arg.Is<Dictionary<string,string>>(d => d["id"] == question.QuestionId.ToString()));
            _xmlService.Received().WriteElements(elements: Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("version") && d["version"] == "1"));
        }

        [Test]
        public void WriteQuestions_WritesQuestionStatus()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            q.QuestionReady = true;
            course.Questions.Add(q);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            _xmlService.Received().WriteElement("question_version");
            _xmlService.Received().WriteElementWithAttributes("question_versions", Arg.Is<Dictionary<string,string>>(d => d["id"] == q.QuestionId.ToString()));
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d.ContainsKey("status") && d["status"] == "ready"));
        }

        [Test]
        public void WriteQuestions_WritesBaseQuestionElements()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(99, "CatA");
            q.QuestionName = "My Test Question";
            q.QuestionText = "What is 2+2?";
            q.DefaultMark = 5.0f;
            course.Questions.Add(q);

            _quizHelper.MapQuestionType(q).Returns("multichoice");

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            _xmlService.Received().WriteElementWithAttributes("question", Arg.Is<Dictionary<string,string>>(d => d["id"] == q.QuestionId.ToString()));

            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["name"] == "My Test Question" &&
                d["questiontext"] == "What is 2+2?" &&
                d["defaultmark"] == "5.0000000" &&
                d["qtype"] == "multichoice" &&
                d.ContainsKey("stamp") &&
                d.ContainsKey("timecreated")));
        }

        [Test]
        public void WriteQuestions_DelegatesToStrategyResolverAndStrategy()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            course.Questions.Add(q);

            // Pre-configured mock strategy from SetUp
            var specificStrategy = Substitute.For<IQuestionStrategy>();
            _strategyResolver.ResolveStrategy(q).Returns(specificStrategy);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            // 1. Resolver called with the question
            _strategyResolver.Received(1).ResolveStrategy(q);

            // 2. Strategy called to write data to the xml service
            specificStrategy.Received(1).WritePluginData(_xmlService, q);
        }

        [Test]
        public void WriteQuestions_WritesPluginSection()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            course.Questions.Add(q);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            // Comments
            _xmlService.Received().WriteElement("plugin_qbank_comment_question");
            _xmlService.Received().WriteElement("comments");

            // Custom Fields
            _xmlService.Received().WriteElement("plugin_qbank_customfields_question");
            _xmlService.Received().WriteElement("customfields");
        }

        [Test]
        public void WriteQuestions_WritesQuestionTags()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            q.Tags = new List<string> { "Tag1", "Tag2" };
            course.Questions.Add(q);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            _tagsWriter.Received(1).WriteTags(_xmlService, q.Tags);
        }

        [Test]
        public void WriteQuestions_WritesHints_ForMultiChoice()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            q.Hints = new List<Hint>
            {
                new Hint { HintText = "Try harder", ShowNumberOfCorrectAnswers = true },
            };
            course.Questions.Add(q);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            _xmlService.Received().WriteElement("question_hints");
            _xmlService.Received().WriteElementWithAttributes("question_hint", Arg.Any<Dictionary<string,string>>());

            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["hint"] == "Try harder" &&
                d["shownumcorrect"] == "1"
            ));
        }

        [Test]
        public void WriteQuestions_WritesHints_ForCodeRunner()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(2, "CodeCat");
            q.Hints = new List<Hint>
            {
                new Hint { HintText = "Code Hint", ClearIncorrectResponses = true },
            };
            course.Questions.Add(q);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert (Hints logic is inside QuestionsWriter, generic for specific types)
            _xmlService.Received().WriteElement("question_hints");
            _xmlService.Received().WriteElements(Arg.Is<Dictionary<string, string>>(d =>
                d["hint"] == "Code Hint" &&
                d["clearwrong"] == "1"
            ));
        }

        [Test]
        public void WriteQuestions_WithNoHints_WritesEmptyHintsElement()
        {
            // Arrange
            var course = new Model.Course();
            var q = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA");
            q.Hints = null;
            course.Questions.Add(q);

            // Act
            _questionsWriter.WriteQuestions(course, "dummy");

            // Assert
            _xmlService.Received().WriteElement("question_hints");
            _xmlService.Received().CloseElement(); // Close question_hints immediately
            _xmlService.DidNotReceive().WriteElementWithAttributes("question_hint", Arg.Any<Dictionary<string, string>>());
        }
    }
}
