using System.Globalization;
using System.Xml;

using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.Plugins;
using CourseAsCode.Writers.XML.Helper;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

using QWriter = CourseAsCode.Writers.XML.FileWriters.QuestionsWriter.QuestionsWriter;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class QuestionsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<QWriter> _questionsLogger;
        private ILogger<MultiChoicePluginWriter> _multiChoiceLogger;
        private ILogger<DropdownPluginWriter> _dropdownLogger;
        private ILogger<DragDropTextPluginWriter> _dragDropTextLogger;
        private ILogger<CodeRunnerPluginWriter> _codeRunnerLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private ISequentialIdGenerator _fakeIdGenerator;
        private IQuizHelper _fakeQuizHelper;
        private ITagsWriter _fakeTagsWriter;
        private QWriter _questionsWriter;
        private Model.Course _course;

        private const string MOCKED_MOODLE_AUTOFORMAT = "0";
        private const string MOCKED_HTML_FORMAT = "1";
        private const string MOCKED_PLAINTEXT_FORMAT = "2";
        private const string MOCKED_MARKDOWN_FORMAT = "4";

        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";
        private const string DEFAULT_FORMAT_MAPPED = "1";
        private const string DEFAULT_TIMESTAMP = "1234567890";

        [SetUp]
        public void SetUp()
        {
            // Arrange: Set up fake XML writer factory (to capture XML output) and logger
            _testFactory = new XmlWriterFactoryTest();
            _questionsLogger = Substitute.For<ILogger<QWriter>>();
            _multiChoiceLogger = Substitute.For<ILogger<MultiChoicePluginWriter>>();
            _dropdownLogger = Substitute.For<ILogger<DropdownPluginWriter>>();
            _dragDropTextLogger = Substitute.For<ILogger<DragDropTextPluginWriter>>();
            _codeRunnerLogger = Substitute.For<ILogger<CodeRunnerPluginWriter>>();

            // Fake general helper with predictable outputs
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.EnumIntegerToString(FormattingFormat.HTML).Returns(MOCKED_HTML_FORMAT);
            _fakeGeneralHelper.EnumIntegerToString(FormattingFormat.Markdown).Returns(MOCKED_MARKDOWN_FORMAT);
            _fakeGeneralHelper.EnumIntegerToString(FormattingFormat.PlainText).Returns(MOCKED_PLAINTEXT_FORMAT);
            _fakeGeneralHelper.EnumIntegerToString(FormattingFormat.MoodleAutoFormat).Returns(MOCKED_MOODLE_AUTOFORMAT);
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<FormattingFormat>()).Returns(MOCKED_HTML_FORMAT); // Fallback
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns(DEFAULT_TIMESTAMP);

            // Setup fake ID generator to return predictable sequential IDs for testing
            int categoryIdCounter = 2; // Start from 2 to avoid conflict with the top category
            int bankEntryIdCounter = 1;
            int answerIdCounter = 1;
            int hintIdCounter = 1;
            int codeRunnerOptionIdCounter = 1;
            int codeRunnerTestCaseIdCounter = 1;
            _fakeIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _fakeIdGenerator.GetNextCategoryId().Returns(_ => categoryIdCounter++);
            _fakeIdGenerator.GetNextBankEntryId().Returns(_ => bankEntryIdCounter++);
            _fakeIdGenerator.GetNextAnswerId().Returns(_ => answerIdCounter++);
            _fakeIdGenerator.GetNextHintId().Returns(_ => hintIdCounter++);
            _fakeIdGenerator.GetNextCodeRunnerOptionId().Returns(_ => codeRunnerOptionIdCounter++);
            _fakeIdGenerator.GetNextCodeRunnerTestCaseId().Returns(_ => codeRunnerTestCaseIdCounter++);

            // Setup fake quiz helper to always return "multichoice" for question type and "abc" for NumberingFormat
            _fakeQuizHelper = Substitute.For<IQuizHelper>();
            _fakeQuizHelper.MapQuestionType(Arg.Any<Question>()).Returns("multichoice");
            _fakeQuizHelper.MapNumberingFormat(Arg.Any<NumberingFormat>()).Returns("abc");

            // Setup fake tags writer
            _fakeTagsWriter = Substitute.For<ITagsWriter>();

            // Create instances of the plugin writer strategies
            var multiChoiceWriter = new MultiChoicePluginWriter(_testFactory, _multiChoiceLogger, _fakeGeneralHelper, _fakeIdGenerator, _fakeQuizHelper);
            var dropdownWriter = new DropdownPluginWriter(_testFactory, _dropdownLogger, _fakeGeneralHelper, _fakeIdGenerator);
            var dragDropTextWriter = new DragDropTextPluginWriter(_testFactory, _dragDropTextLogger, _fakeGeneralHelper, _fakeIdGenerator);
            var codeRunnerWriter = new CodeRunnerPluginWriter(_testFactory, _codeRunnerLogger, _fakeIdGenerator);
            var pluginWriters = new List<IQuestionPluginWriter>
            {
                multiChoiceWriter,
                dropdownWriter,
                dragDropTextWriter,
                codeRunnerWriter,
            };

            // Instantiate QuestionsWriter with dependencies
            _questionsWriter = new QWriter(_testFactory, _questionsLogger, _fakeGeneralHelper, _fakeIdGenerator, _fakeQuizHelper, _fakeTagsWriter, pluginWriters);

            // Create a dummy course - questions will be added per test
            _course = new Model.Course
            {
                Questions = [],
            };
        }

        [Test]
        public void WriteQuestions_GeneratesValidXml()
        {
            // Arrange
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA"));

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteQuestions_HandlesEmptyQuestionList()
        {
            // Arrange: Don't add questions, so the list is empty

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: Should still produce valid XML with root and top category
            var doc = new XmlDocument();
            Assert.DoesNotThrow(() => doc.LoadXml(output), "XML should be well-formed even with no questions");

            Assert.That(doc.DocumentElement?.Name, Is.EqualTo("question_categories"));
            var categoryNodes = doc.SelectNodes("//question_category");

            // Should only contain the 'top' category
            Assert.That(categoryNodes?.Count, Is.EqualTo(1), "Should only have the 'top' category");
            Assert.That(categoryNodes[0]?.SelectSingleNode("name")?.InnerText, Is.EqualTo("top"));

            // Bank entries within 'top' should be empty
            Assert.That(categoryNodes[0]?.SelectSingleNode("question_bank_entries")?.HasChildNodes, Is.False);
        }

        [Test]
        public void WriteQuestions_RootElementIsQuestionCategories()
        {
            // Arrange
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA"));

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The root element of the XML should be "question_categories"
            var doc = new XmlDocument();
            doc.LoadXml(output);

            Assert.That(doc.DocumentElement, Is.Not.Null, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("question_categories"));
        }

        [Test]
        public void WriteQuestions_CreatesCorrectNumberOfCategories()
        {
            // Arrange
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA"));
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(2, "CatB"));
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(3, "CatB")); // Same category as Q2

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: There should be one top-level category plus one for each distinct course category
            int expectedCategories = 1 + _course.Questions.Select(q => q.Category).Distinct().Count();
            var categoryNodes = doc.SelectNodes("//question_category");
            Assert.That(categoryNodes?.Count, Is.EqualTo(expectedCategories), "Incorrect number of categories");
        }

        [Test]
        public void WriteQuestions_CreatesCorrectNumberOfBankEntries()
        {
            // Arrange
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA"));
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(2, "CatB"));
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(3, "CatB")); // Same category as Q2

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The total number of question_bank_entry elements should match total questions (at least that's the case in our implementation)
            var entryNodes = doc.SelectNodes("//question_bank_entry");
            Assert.That(entryNodes?.Count, Is.EqualTo(_course.Questions.Count), "Incorrect number of bank entries");
        }

        [Test]
        public void WriteQuestions_WritesCodeRunnerBaseElementsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(60, "CodeRunnerCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check base question elements
            var questionNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']");
            Assert.That(questionNode, Is.Not.Null, $"Question node with ID {question.QuestionId} not found");

            Assert.That(questionNode.SelectSingleNode("name")?.InnerText, Is.EqualTo(question.QuestionName));
            Assert.That(questionNode.SelectSingleNode("questiontext")?.InnerText, Is.EqualTo(question.QuestionText));
            Assert.That(questionNode.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("generalfeedback")?.InnerText, Is.EqualTo(question.GeneralFeedback));
            Assert.That(questionNode.SelectSingleNode("generalfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("defaultmark")?.InnerText, Is.EqualTo(question.DefaultMark.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("penalty")?.InnerText, Is.EqualTo(question.HintPenalty.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("qtype")?.InnerText, Is.EqualTo("multichoice")); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timecreated")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timemodified")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
        }

        [Test]
        public void WriteQuestions_WritesCodeRunnerPluginStructureCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(61, "CodeRunnerStructCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Plugin structure
            var pluginNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/plugin_qtype_coderunner_question");
            Assert.That(pluginNode, Is.Not.Null, "CodeRunner node not found");

            var optionsNode = pluginNode.SelectSingleNode("coderunner_options/coderunner_option");
            Assert.That(optionsNode, Is.Not.Null, "CodeRunner options node not found");

            var testCasesNode = pluginNode.SelectSingleNode("coderunner_testcases");
            Assert.That(testCasesNode, Is.Not.Null, "CodeRunner testcases node not found");
            Assert.That(testCasesNode.SelectNodes("coderunner_testcase")?.Count, Is.EqualTo(question.TestCases.Count));
        }

        [Test]
        public void WriteQuestions_WritesCodeRunnerOptionsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(62, "CodeRunnerOptionsCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check options content
            var optionNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//coderunner_option");
            Assert.That(optionNode, Is.Not.Null);

            Assert.That(optionNode.SelectSingleNode("coderunnertype")?.InnerText, Is.EqualTo(question.CodeRunnerType));
            Assert.That(optionNode.SelectSingleNode("prototypetype")?.InnerText, Is.EqualTo(question.PrototypeType.ToString()));
            Assert.That(optionNode.SelectSingleNode("allornothing")?.InnerText, Is.EqualTo(question.AllOrNothing ? "1" : "0"));
            Assert.That(optionNode.SelectSingleNode("penaltyregime")?.InnerText, Is.EqualTo(question.PenaltyRegime));
            Assert.That(optionNode.SelectSingleNode("precheck")?.InnerText, Is.EqualTo(question.Precheck.ToString()));
            Assert.That(optionNode.SelectSingleNode("hidecheck")?.InnerText, Is.EqualTo(question.HideCheck.ToString()));
            Assert.That(optionNode.SelectSingleNode("showsource")?.InnerText, Is.EqualTo(question.ShowSource.ToString()));
            Assert.That(optionNode.SelectSingleNode("answerboxlines")?.InnerText, Is.EqualTo(question.AnswerBoxLines.ToString()));
            Assert.That(optionNode.SelectSingleNode("answerboxcolumns")?.InnerText, Is.EqualTo(question.AnswerBoxColumns.ToString()));
            Assert.That(optionNode.SelectSingleNode("answerpreload")?.InnerText, Is.EqualTo(question.AnswerPreload));
            Assert.That(optionNode.SelectSingleNode("globalextra")?.InnerText, Is.EqualTo(question.GlobalExtra));
            Assert.That(optionNode.SelectSingleNode("answer")?.InnerText.Replace("\r\n", "\n"), Is.EqualTo(question.Answer));
            Assert.That(optionNode.SelectSingleNode("validateonsave")?.InnerText, Is.EqualTo(question.ValidateOnSave ? "1" : "0"));
        }

        [Test]
        public void WriteQuestions_WritesCodeRunnerTestCasesCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(63, "CodeRunnerTestCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check test cases
            var testCaseNodes = doc.SelectNodes($"//question[@id='{question.QuestionId}']//coderunner_testcase");
            Assert.That(testCaseNodes, Is.Not.Null);
            Assert.That(testCaseNodes.Count, Is.EqualTo(question.TestCases.Count));

            // Check first test case
            var firstTestCase = question.TestCases[0];
            var firstTestCaseNode = testCaseNodes[0];
            Assert.That(firstTestCaseNode.SelectSingleNode("testcode").InnerText, Is.EqualTo(firstTestCase.TestCode));
            Assert.That(firstTestCaseNode.SelectSingleNode("expected").InnerText, Is.EqualTo(firstTestCase.Expected));
            Assert.That(firstTestCaseNode.SelectSingleNode("display").InnerText, Is.EqualTo(firstTestCase.Display));
            Assert.That(firstTestCaseNode.SelectSingleNode("mark").InnerText, Is.EqualTo(firstTestCase.Mark.ToString("0.000", CultureInfo.InvariantCulture)));

            // Check second test case
            var secondTestCase = question.TestCases[1];
            var secondTestCaseNode = testCaseNodes[1];
            Assert.That(secondTestCaseNode.SelectSingleNode("testcode").InnerText, Is.EqualTo(secondTestCase.TestCode));
            Assert.That(secondTestCaseNode.SelectSingleNode("expected").InnerText, Is.EqualTo(secondTestCase.Expected));
            Assert.That(secondTestCaseNode.SelectSingleNode("useasexample").InnerText, Is.EqualTo(secondTestCase.UseAsExample ? "1" : "0"));
            Assert.That(secondTestCaseNode.SelectSingleNode("hiderestiffail").InnerText, Is.EqualTo(secondTestCase.HideRestIfFail ? "1" : "0"));
            Assert.That(secondTestCaseNode.SelectSingleNode("mark").InnerText, Is.EqualTo(secondTestCase.Mark.ToString("0.000", CultureInfo.InvariantCulture)));
        }

        [Test]
        public void WriteQuestions_WritesCodeRunnerHintsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(64, "CodeRunnerHintsCat", withHints: true);
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Hints structure and content
            var hintsNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/question_hints");
            Assert.That(hintsNode, Is.Not.Null, "Hints node not found");

            var hintNodes = hintsNode.SelectNodes("question_hint");
            Assert.That(hintNodes?.Count, Is.EqualTo(question.Hints.Count));

            // Hint 1
            Assert.That(hintNodes[0]?.SelectSingleNode("hint")?.InnerText, Is.EqualTo(question.Hints[0].HintText));
            Assert.That(hintNodes[0]?.SelectSingleNode("hintformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(hintNodes[0]?.SelectSingleNode("shownumcorrect")?.InnerText, Is.EqualTo(question.Hints[0].ShowNumberOfCorrectAnswers ? "1" : "0"));
            Assert.That(hintNodes[0]?.SelectSingleNode("clearwrong")?.InnerText, Is.EqualTo(question.Hints[0].ClearIncorrectResponses ? "1" : "0"));

            // Hint 2
            Assert.That(hintNodes[1]?.SelectSingleNode("hint")?.InnerText, Is.EqualTo(question.Hints[1].HintText));
            Assert.That(hintNodes[1]?.SelectSingleNode("hintformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(hintNodes[1]?.SelectSingleNode("shownumcorrect")?.InnerText, Is.EqualTo(question.Hints[1].ShowNumberOfCorrectAnswers ? "1" : "0"));
            Assert.That(hintNodes[1]?.SelectSingleNode("clearwrong")?.InnerText, Is.EqualTo(question.Hints[1].ClearIncorrectResponses ? "1" : "0"));
        }

        [Test]
        public void WriteQuestions_HandlesCodeRunnerWithNoHintsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicCodeRunnerQuestion(65, "CodeRunnerNoHintsCat", withHints: false);
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Empty hints node
            var hintsNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/question_hints");
            Assert.That(hintsNode, Is.Not.Null, "Hints node should exist even if empty");
            Assert.That(hintsNode.ChildNodes.Count, Is.Zero, "Hints node should be empty");
        }

        [Test]
        public void WriteQuestions_WritesMultiChoiceBaseElementsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(10, "TestCat");
            _course.Questions.Add(question);

             // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

             // Assert: Check base question elements
            var questionNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']");
            Assert.That(questionNode, Is.Not.Null, $"Question node with ID {question.QuestionId} not found");

            Assert.That(questionNode.SelectSingleNode("name")?.InnerText, Is.EqualTo(question.QuestionName));
            Assert.That(questionNode.SelectSingleNode("questiontext")?.InnerText, Is.EqualTo(question.QuestionText));
            Assert.That(questionNode.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("generalfeedback")?.InnerText, Is.EqualTo(question.GeneralFeedback));
            Assert.That(questionNode.SelectSingleNode("generalfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(questionNode.SelectSingleNode("defaultmark")?.InnerText, Is.EqualTo(question.DefaultMark.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("penalty")?.InnerText, Is.EqualTo(question.HintPenalty.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("qtype")?.InnerText, Is.EqualTo("multichoice")); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timecreated")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timemodified")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
        }

        [Test]
        public void WriteQuestions_WritesMultiChoicePluginElementsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicMultiChoiceQuestion(11, "MCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check plugin_qtype_multichoice_question elements
            var pluginNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/plugin_qtype_multichoice_question");
            Assert.That(pluginNode, Is.Not.Null, $"MultiChoice plugin node not found");

            // Answers
            var answers = pluginNode.SelectNodes("answers/answer");
            Assert.That(answers?.Count, Is.EqualTo(question.Answers.Count));
            Assert.That(answers[0]?.SelectSingleNode("answertext")?.InnerText, Is.EqualTo(question.Answers[0].AnswerText));
            Assert.That(answers[0]?.SelectSingleNode("fraction")?.InnerText, Is.EqualTo(question.Answers[0].AnswerGrade.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(answers[0]?.SelectSingleNode("feedback")?.InnerText, Is.EqualTo(question.Answers[0].AnswerFeedback));
            Assert.That(answers[1]?.SelectSingleNode("answertext")?.InnerText, Is.EqualTo(question.Answers[1].AnswerText));
            Assert.That(answers[1]?.SelectSingleNode("fraction")?.InnerText, Is.EqualTo(question.Answers[1].AnswerGrade.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(answers[1]?.SelectSingleNode("feedback")?.InnerText, Is.EqualTo(question.Answers[1].AnswerFeedback));

            // Multichoice options
            var multiChoiceNode = pluginNode.SelectSingleNode("multichoice");
            Assert.That(multiChoiceNode, Is.Not.Null, $"Multichoice options node not found");
            Assert.That(multiChoiceNode.SelectSingleNode("single")?.InnerText, Is.EqualTo(question.SingleChoice ? "1" : "0"));
            Assert.That(multiChoiceNode.SelectSingleNode("shuffleanswers")?.InnerText, Is.EqualTo(question.ShuffleAnswers ? "1" : "0"));
            Assert.That(multiChoiceNode.SelectSingleNode("correctfeedback")?.InnerText, Is.EqualTo(question.CorrectResponseFeedback));
            Assert.That(multiChoiceNode.SelectSingleNode("partiallycorrectfeedback")?.InnerText, Is.EqualTo(question.PartiallyCorrectResponseFeedback));
            Assert.That(multiChoiceNode.SelectSingleNode("incorrectfeedback")?.InnerText, Is.EqualTo(question.IncorrectResponseFeedback));
            Assert.That(multiChoiceNode.SelectSingleNode("answernumbering")?.InnerText, Is.EqualTo("abc")); // Mapped value
            Assert.That(multiChoiceNode.SelectSingleNode("shownumcorrect")?.InnerText, Is.EqualTo("0")); // depends on number of hints, 0 in this case
            Assert.That(multiChoiceNode.SelectSingleNode("showstandardinstruction")?.InnerText, Is.EqualTo(question.ShowStandardInstructions ? "1" : "0"));
        }

        [Test]
        public void WriteQuestions_WritesDropdownBaseElementsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(20, "DropdownCat");
            _course.Questions.Add(question);

             // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

             // Assert: Check base question elements
            var questionNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']");
            Assert.That(questionNode, Is.Not.Null, $"Question node with ID {question.QuestionId} not found");

            Assert.That(questionNode.SelectSingleNode("name")?.InnerText, Is.EqualTo(question.QuestionName));
            Assert.That(questionNode.SelectSingleNode("questiontext")?.InnerText, Is.EqualTo(question.QuestionText));
            Assert.That(questionNode.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("generalfeedback")?.InnerText, Is.EqualTo(question.GeneralFeedback));
            Assert.That(questionNode.SelectSingleNode("generalfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(questionNode.SelectSingleNode("defaultmark")?.InnerText, Is.EqualTo(question.DefaultMark.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("penalty")?.InnerText, Is.EqualTo(question.HintPenalty.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("qtype")?.InnerText, Is.EqualTo("multichoice")); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timecreated")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timemodified")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
        }

        [Test]
        public void WriteQuestions_WritesDropdownPluginStructureCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(21, "DDCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Plugin structure
            var pluginNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/plugin_qtype_match_question");
            Assert.That(pluginNode, Is.Not.Null, "Match plugin node not found");

            var matchOptionsNode = pluginNode.SelectSingleNode("matchoptions");
            Assert.That(matchOptionsNode, Is.Not.Null, $"Match options node not found");
            Assert.That(matchOptionsNode.SelectSingleNode("shuffleanswers")?.InnerText, Is.EqualTo("1")); // Should always be 1 for dropdown

            var matchesNode = pluginNode.SelectSingleNode("matches");
            Assert.That(matchesNode, Is.Not.Null, $"Matches node not found");

            // Check number of matches: 2 sub-questions + 3 unique wrong answers (A, B, C) = 5 total matches
            var matchNodes = matchesNode.SelectNodes("match");
            var uniqueWrongAnswers = question.SubQuestions.SelectMany(a => a.WrongAnswers).Distinct().Count();
            Assert.That(matchNodes?.Count, Is.EqualTo(question.SubQuestions.Count + uniqueWrongAnswers));
        }

        [Test]
        public void WriteQuestions_WritesDropdownSubQuestionsAndAnswerPoolCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(22, "DDCatPool");
            _course.Questions.Add(question);

            var subQ1 = question.SubQuestions[0];
            var subQ2 = question.SubQuestions[1];

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Matches content
            var matchesNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//matches");
            Assert.That(matchesNode, Is.Not.Null);

            // SubQuestion 1 match
            var subQ1Match = matchesNode.SelectSingleNode($"match[questiontext='{subQ1.QuestionText}']");
            Assert.That(subQ1Match, Is.Not.Null, "SubQ1 match not found");
            Assert.That(subQ1Match.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(subQ1Match.SelectSingleNode("answertext")?.InnerText, Is.EqualTo(subQ1.CorrectAnswer));

            // SubQuestion 2 match
            var subQ2Match = matchesNode.SelectSingleNode($"match[questiontext='{subQ2.QuestionText}']");
            Assert.That(subQ2Match, Is.Not.Null, $"SubQ2 match not found");
            Assert.That(subQ2Match.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(subQ2Match.SelectSingleNode("answertext")?.InnerText, Is.EqualTo(subQ2.CorrectAnswer));

             // Pooled Wrong Answer A (only from SubQ1)
            var poolAMatch = matchesNode.SelectSingleNode("match[questiontext='' and answertext='WrongAnswerA']");
            Assert.That(poolAMatch, Is.Not.Null, $"Pooled Answer A match not found");

             // Pooled Wrong Answer B (from SubQ1 and SubQ2 - should appear once)
            var poolBMatch = matchesNode.SelectNodes("match[questiontext='' and answertext='WrongAnswerB']");
            Assert.That(poolBMatch?.Count, Is.EqualTo(1), "Pooled Answer B should appear only once");
            Assert.That(poolBMatch[0], Is.Not.Null, $"Pooled Answer B match not found");

             // Pooled Wrong Answer C (only from SubQ2)
            var poolCMatch = matchesNode.SelectSingleNode("match[questiontext='' and answertext='WrongAnswerC']");
            Assert.That(poolCMatch, Is.Not.Null, $"Pooled Answer C match not found");
        }

        [Test]
        public void WriteQuestions_WritesDropdownFeedbackCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(23, "DDCatFeedback", withFeedback: true);
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Feedback in matchoptions
            var matchOptionsNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//matchoptions");
            Assert.That(matchOptionsNode, Is.Not.Null);

            Assert.That(matchOptionsNode.SelectSingleNode("correctfeedback")?.InnerText, Is.EqualTo(question.CorrectResponseFeedback));
            Assert.That(matchOptionsNode.SelectSingleNode("correctfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked HTML
            Assert.That(matchOptionsNode.SelectSingleNode("partiallycorrectfeedback")?.InnerText, Is.EqualTo(question.PartiallyCorrectResponseFeedback));
            Assert.That(matchOptionsNode.SelectSingleNode("partiallycorrectfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(matchOptionsNode.SelectSingleNode("incorrectfeedback")?.InnerText, Is.EqualTo(question.IncorrectResponseFeedback));
            Assert.That(matchOptionsNode.SelectSingleNode("incorrectfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked HTML
        }

        [Test]
        public void WriteQuestions_WritesDropdownHintsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(24, "DDCatHints", withHints: true);
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Hints structure and content
            var hintsNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/question_hints");
            Assert.That(hintsNode, Is.Not.Null, $"Hints node not found");

            var hintNodes = hintsNode.SelectNodes("question_hint");
            Assert.That(hintNodes?.Count, Is.EqualTo(question.Hints.Count));

            // Hint 1
            Assert.That(hintNodes[0]?.SelectSingleNode("hint")?.InnerText, Is.EqualTo(question.Hints[0].HintText));
            Assert.That(hintNodes[0]?.SelectSingleNode("hintformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(hintNodes[0]?.SelectSingleNode("shownumcorrect")?.InnerText, Is.EqualTo(question.Hints[0].ShowNumberOfCorrectAnswers ? "1" : "0"));
            Assert.That(hintNodes[0]?.SelectSingleNode("clearwrong")?.InnerText, Is.EqualTo(question.Hints[0].ClearIncorrectResponses ? "1" : "0"));

             // Hint 2
            Assert.That(hintNodes[1]?.SelectSingleNode("hint")?.InnerText, Is.EqualTo(question.Hints[1].HintText));
            Assert.That(hintNodes[1]?.SelectSingleNode("hintformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(hintNodes[1]?.SelectSingleNode("shownumcorrect")?.InnerText, Is.EqualTo(question.Hints[1].ShowNumberOfCorrectAnswers ? "1" : "0"));
            Assert.That(hintNodes[1]?.SelectSingleNode("clearwrong")?.InnerText, Is.EqualTo(question.Hints[1].ClearIncorrectResponses ? "1" : "0"));
        }

        [Test]
        public void WriteQuestions_HandlesDropdownWithNoHintsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDropdownQuestion(25, "DDCatNoHints", withHints: false);
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Empty hints node
            var hintsNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/question_hints");
            Assert.That(hintsNode, Is.Not.Null, $"Hints node should exist even if empty");
            Assert.That(hintsNode.ChildNodes.Count, Is.Zero, "Hints node should be empty");
        }

        [Test]
        public void WriteQuestions_WritesDragDropTextBaseElementsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(50, "DDTBaseCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check base question elements
            var questionNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']");
            Assert.That(questionNode, Is.Not.Null, $"Question node with ID {question.QuestionId} not found");

            Assert.That(questionNode.SelectSingleNode("name")?.InnerText, Is.EqualTo(question.QuestionName));
            Assert.That(questionNode.SelectSingleNode("questiontext")?.InnerText, Is.EqualTo(question.QuestionText));
            Assert.That(questionNode.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("generalfeedback")?.InnerText, Is.EqualTo(question.GeneralFeedback));
            Assert.That(questionNode.SelectSingleNode("generalfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("defaultmark")?.InnerText, Is.EqualTo(question.DefaultMark.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("penalty")?.InnerText, Is.EqualTo(question.HintPenalty.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("qtype")?.InnerText, Is.EqualTo("multichoice")); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timecreated")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timemodified")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
        }

        [Test]
        public void WriteQuestions_WritesDragDropTextPluginStructureCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(51, "DDTStructCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Plugin structure
            var pluginNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/plugin_qtype_ddwtos_question");
            Assert.That(pluginNode, Is.Not.Null, $"ddwtos plugin node not found");

            var answersNode = pluginNode.SelectSingleNode("answers");
            Assert.That(answersNode, Is.Not.Null, $"Answers node not found");
            Assert.That(answersNode.SelectNodes("answer")?.Count, Is.EqualTo(question.Answers.Count), "Incorrect number of answer choices");

            var ddwtosNode = pluginNode.SelectSingleNode("ddwtos");
            Assert.That(ddwtosNode, Is.Not.Null, $"ddwtos options node not found");
        }

        [Test]
        public void WriteQuestions_WritesDragDropTextAnswersAndFeedbackCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(52, "DDTAnsCat");
            _course.Questions.Add(question);

            var choice1 = question.Answers.First(c => c.ID == 1); // Category A -> Group 1
            var choice2 = question.Answers.First(c => c.ID == 2); // Category B -> Group 2
            var choice4 = question.Answers.First(c => c.ID == 4); // Category A -> Group 1

            // Expected PHP feedback strings
            string expectedFeedback1 = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"1\";s:8:\"infinite\";b:0;}"; // Cat A
            string expectedFeedback2 = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"2\";s:8:\"infinite\";b:0;}"; // Cat B
            string expectedFeedback4 = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"1\";s:8:\"infinite\";b:0;}"; // Cat A

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check specific answer nodes
            var answersNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//plugin_qtype_ddwtos_question/answers");
            Assert.That(answersNode, Is.Not.Null);

            // Choice 1 (France, Cat A)
            var answer1Node = answersNode.SelectSingleNode($"answer[answertext='{choice1.Value}']");
            Assert.That(answer1Node, Is.Not.Null, $"Answer node for '{choice1.Value}' not found");
            Assert.That(answer1Node.SelectSingleNode("answerformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(answer1Node.SelectSingleNode("fraction")?.InnerText, Is.EqualTo("0.0000000"));
            Assert.That(answer1Node.SelectSingleNode("feedback")?.InnerText, Is.EqualTo(expectedFeedback1));
            Assert.That(answer1Node.SelectSingleNode("feedbackformat")?.InnerText, Is.EqualTo("0"));

            // Choice 2 (London, Cat B)
            var answer2Node = answersNode.SelectSingleNode($"answer[answertext='{choice2.Value}']");
            Assert.That(answer2Node, Is.Not.Null, $"Answer node for '{choice2.Value}' not found");
            Assert.That(answer2Node.SelectSingleNode("answerformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(answer2Node.SelectSingleNode("fraction")?.InnerText, Is.EqualTo("0.0000000"));
            Assert.That(answer2Node.SelectSingleNode("feedback")?.InnerText, Is.EqualTo(expectedFeedback2));
            Assert.That(answer2Node.SelectSingleNode("feedbackformat")?.InnerText, Is.EqualTo("0"));

             // Choice 4 (Germany, Cat A)
            var answer4Node = answersNode.SelectSingleNode($"answer[answertext='{choice4.Value}']");
            Assert.That(answer4Node, Is.Not.Null, $"Answer node for '{choice4.Value}' not found");
            Assert.That(answer4Node.SelectSingleNode("answerformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(answer4Node.SelectSingleNode("fraction")?.InnerText, Is.EqualTo("0.0000000"));
            Assert.That(answer4Node.SelectSingleNode("feedback")?.InnerText, Is.EqualTo(expectedFeedback4));
            Assert.That(answer4Node.SelectSingleNode("feedbackformat")?.InnerText, Is.EqualTo("0"));
        }

        [Test]
        public void WriteQuestions_HandlesDragDropTextWithNoCategoriesCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(56, "DDTNoCat");

            // Override answers to have no categories
            question.Answers = new List<TextChoice>
            {
                new TextChoice { ID = 1, Value = "Choice1", Category = null },
                new TextChoice { ID = 2, Value = "Choice2", Category = "" },
            };
            _course.Questions.Add(question);

            // Expected PHP feedback strings (all should map to group 1)
            string expectedFeedback = "O:8:\"stdClass\":2:{s:9:\"draggroup\";s:1:\"1\";s:8:\"infinite\";b:0;}";

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check feedback for both answers
            var answersNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//plugin_qtype_ddwtos_question/answers");
            Assert.That(answersNode, Is.Not.Null);

            var answer1Feedback = answersNode.SelectSingleNode("answer[answertext='Choice1']/feedback")?.InnerText;
            Assert.That(answer1Feedback, Is.EqualTo(expectedFeedback), "PHP Feedback string for Choice1 with null category is incorrect");

            var answer2Feedback = answersNode.SelectSingleNode("answer[answertext='Choice2']/feedback")?.InnerText;
            Assert.That(answer2Feedback, Is.EqualTo(expectedFeedback), "PHP Feedback string for Choice2 with empty category is incorrect");
        }

        [Test]
        public void WriteQuestions_WritesDragDropTextOptionsCorrectly()
        {
            // Arrange
            var question = FakeQuestionFactory.CreateBasicDragDropTextQuestion(53, "DDTOptionsCat", withFeedback: true, withHints: true);
            question.ShuffleAnswers = false; // Override default shuffle
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check ddwtos options node
            var ddwtosNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//ddwtos");
            Assert.That(ddwtosNode, Is.Not.Null, $"ddwtos options node not found");

            Assert.That(ddwtosNode.SelectSingleNode("shuffleanswers")?.InnerText, Is.EqualTo("0")); // Set to false in arrange
            Assert.That(ddwtosNode.SelectSingleNode("correctfeedback")?.InnerText, Is.EqualTo(question.CorrectResponseFeedback));
            Assert.That(ddwtosNode.SelectSingleNode("correctfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(ddwtosNode.SelectSingleNode("partiallycorrectfeedback")?.InnerText, Is.EqualTo(question.PartiallyCorrectResponseFeedback));
            Assert.That(ddwtosNode.SelectSingleNode("partiallycorrectfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(ddwtosNode.SelectSingleNode("incorrectfeedback")?.InnerText, Is.EqualTo(question.IncorrectResponseFeedback));
            Assert.That(ddwtosNode.SelectSingleNode("incorrectfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(ddwtosNode.SelectSingleNode("shownumcorrect")?.InnerText, Is.EqualTo("1")); // Hints were added
        }

        [Test]
        public void WriteQuestions_LogsDebugOnSuccess()
        {
            // Arrange
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA"));

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert
            _questionsLogger.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("questions.xml")),
                null,
                Arg.Any<Func<object, Exception, string>>()!);
        }

        [Test]
        public void WriteQuestions_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _questionsWriter = new QWriter(throwingFactory, _questionsLogger, _fakeGeneralHelper, _fakeIdGenerator, _fakeQuizHelper, _fakeTagsWriter, new List<IQuestionPluginWriter>());
            _course.Questions.Add(FakeQuestionFactory.CreateBasicMultiChoiceQuestion(1, "CatA"));

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH));

            // Verify that an error log was recorded containing "questions.xml"
            _questionsLogger.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("questions.xml")),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception, string>>()!);
        }
    }
}
