using System.Globalization;
using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Interfaces;
using CourseAsCode.Writers.XML.Writer.QuestionsWriter.Plugins;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class QuestionsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger<QuestionsWriter> _questionsLogger;
        private ILogger<MultiChoicePluginWriter> _multiChoiceLogger;
        private ILogger<DropdownPluginWriter> _dropdownLogger;
        private ILogger<DragDropTextPluginWriter> _dragDropTextLogger;
        private IGeneralHelper _fakeGeneralHelper;
        private ISequentialIdGenerator _fakeIdGenerator;
        private ITagsWriter _fakeTagsWriter;
        private QuestionsWriter _questionsWriter;
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
            _questionsLogger = Substitute.For<ILogger<QuestionsWriter>>();
            _multiChoiceLogger = Substitute.For<ILogger<MultiChoicePluginWriter>>();
            _dropdownLogger = Substitute.For<ILogger<DropdownPluginWriter>>();
            _dragDropTextLogger = Substitute.For<ILogger<DragDropTextPluginWriter>>();

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
            _fakeIdGenerator = Substitute.For<ISequentialIdGenerator>();
            _fakeIdGenerator.GetNextCategoryId().Returns(_ => categoryIdCounter++);
            _fakeIdGenerator.GetNextBankEntryId().Returns(_ => bankEntryIdCounter++);
            _fakeIdGenerator.GetNextAnswerId().Returns(_ => answerIdCounter++);
            _fakeIdGenerator.GetNextHintId().Returns(_ => hintIdCounter++);

            // Setup fake tags writer
            _fakeTagsWriter = Substitute.For<ITagsWriter>();

            // Create instances of the plugin writer strategies
            var multiChoiceWriter = new MultiChoicePluginWriter(_testFactory, _multiChoiceLogger);
            var dropdownWriter = new DropdownPluginWriter(_testFactory, _dropdownLogger);
            var dragDropTextWriter = new DragDropTextPluginWriter(_testFactory, _dragDropTextLogger);
            var pluginWriters = new List<IQuestionPluginWriter>
            {
                multiChoiceWriter,
                dropdownWriter,
                dragDropTextWriter,
            };

            // Instantiate QuestionsWriter with dependencies
            _questionsWriter = new QuestionsWriter(_testFactory, _questionsLogger, _fakeGeneralHelper, _fakeIdGenerator, _fakeTagsWriter, pluginWriters);

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
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(1, "CatA"));

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
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(1, "CatA"));

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            // Assert: The root element of the XML should be "question_categories"
            var doc = new XmlDocument();
            doc.LoadXml(output);

            Assert.IsNotNull(doc.DocumentElement, "The XML document should have a root element");
            Assert.That(doc.DocumentElement.Name, Is.EqualTo("question_categories"));
        }

        [Test]
        public void WriteQuestions_CreatesCorrectNumberOfCategories()
        {
            // Arrange
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(1, "CatA"));
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(2, "CatB"));
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(3, "CatB")); // Same category as Q2

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
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(1, "CatA"));
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(2, "CatB"));
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(3, "CatB")); // Same category as Q2

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
        public void WriteQuestions_WritesMultiChoiceBaseElementsCorrectly()
        {
            // Arrange
            var question = CreateBasicMultiChoiceQuestion(10, "TestCat");
            _course.Questions.Add(question);

             // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

             // Assert: Check base question elements
            var questionNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']");
            Assert.IsNotNull(questionNode, $"Question node with ID {question.QuestionId} not found");

            Assert.That(questionNode.SelectSingleNode("name")?.InnerText, Is.EqualTo(question.QuestionName));
            Assert.That(questionNode.SelectSingleNode("questiontext")?.InnerText, Is.EqualTo(question.QuestionText));
            Assert.That(questionNode.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("generalfeedback")?.InnerText, Is.EqualTo(question.GeneralFeedback));
            Assert.That(questionNode.SelectSingleNode("generalfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(questionNode.SelectSingleNode("defaultmark")?.InnerText, Is.EqualTo(question.DefaultMark.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("penalty")?.InnerText, Is.EqualTo(question.HintPenalty.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("qtype")?.InnerText, Is.EqualTo("multichoice")); // Mapped value for multi-choice
            Assert.That(questionNode.SelectSingleNode("timecreated")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timemodified")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
        }

        [Test]
        public void WriteQuestions_WritesMultiChoicePluginElementsCorrectly()
        {
            // Arrange
            var question = CreateBasicMultiChoiceQuestion(11, "MCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check plugin_qtype_multichoice_question elements
            var pluginNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/plugin_qtype_multichoice_question");
            Assert.IsNotNull(pluginNode, "MultiChoice plugin node not found");

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
            Assert.IsNotNull(multiChoiceNode, "Multichoice options node not found");
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
            var question = CreateBasicDropdownQuestion(20, "DropdownCat");
            _course.Questions.Add(question);

             // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

             // Assert: Check base question elements
            var questionNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']");
            Assert.IsNotNull(questionNode, $"Question node with ID {question.QuestionId} not found");

            Assert.That(questionNode.SelectSingleNode("name")?.InnerText, Is.EqualTo(question.QuestionName));
            Assert.That(questionNode.SelectSingleNode("questiontext")?.InnerText, Is.EqualTo(question.QuestionText));
            Assert.That(questionNode.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("generalfeedback")?.InnerText, Is.EqualTo(question.GeneralFeedback));
            Assert.That(questionNode.SelectSingleNode("generalfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(questionNode.SelectSingleNode("defaultmark")?.InnerText, Is.EqualTo(question.DefaultMark.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("penalty")?.InnerText, Is.EqualTo(question.HintPenalty.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("qtype")?.InnerText, Is.EqualTo("match")); // Mapped value for Dropdown
            Assert.That(questionNode.SelectSingleNode("timecreated")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timemodified")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
        }

        [Test]
        public void WriteQuestions_WritesDropdownPluginStructureCorrectly()
        {
            // Arrange
            var question = CreateBasicDropdownQuestion(21, "DDCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Plugin structure
            var pluginNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/plugin_qtype_match_question");
            Assert.IsNotNull(pluginNode, "Match plugin node not found");

            var matchOptionsNode = pluginNode.SelectSingleNode("matchoptions");
            Assert.IsNotNull(matchOptionsNode, "Match options node not found");
            Assert.That(matchOptionsNode.SelectSingleNode("shuffleanswers")?.InnerText, Is.EqualTo("1")); // Should always be 1 for dropdown

            var matchesNode = pluginNode.SelectSingleNode("matches");
            Assert.IsNotNull(matchesNode, "Matches node not found");

            // Check number of matches: 2 sub-questions + 3 unique wrong answers (A, B, C) = 5 total matches
            var matchNodes = matchesNode.SelectNodes("match");
            var uniqueWrongAnswers = question.Answers.SelectMany(a => a.WrongAnswers).Distinct().Count();
            Assert.That(matchNodes?.Count, Is.EqualTo(question.Answers.Count + uniqueWrongAnswers));
        }

        [Test]
        public void WriteQuestions_WritesDropdownSubQuestionsAndAnswerPoolCorrectly()
        {
            // Arrange
            var question = CreateBasicDropdownQuestion(22, "DDCatPool");
            _course.Questions.Add(question);

            var subQ1 = question.Answers[0];
            var subQ2 = question.Answers[1];

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Matches content
            var matchesNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//matches");
            Assert.IsNotNull(matchesNode);

            // SubQuestion 1 match
            var subQ1Match = matchesNode.SelectSingleNode($"match[questiontext='{subQ1.QuestionText}']");
            Assert.IsNotNull(subQ1Match, "SubQ1 match not found");
            Assert.That(subQ1Match.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(subQ1Match.SelectSingleNode("answertext")?.InnerText, Is.EqualTo(subQ1.CorrectAnswer));

            // SubQuestion 2 match
            var subQ2Match = matchesNode.SelectSingleNode($"match[questiontext='{subQ2.QuestionText}']");
            Assert.IsNotNull(subQ2Match, "SubQ2 match not found");
            Assert.That(subQ2Match.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(subQ2Match.SelectSingleNode("answertext")?.InnerText, Is.EqualTo(subQ2.CorrectAnswer));

             // Pooled Wrong Answer A (only from SubQ1)
            var poolAMatch = matchesNode.SelectSingleNode("match[questiontext='' and answertext='WrongAnswerA']");
            Assert.IsNotNull(poolAMatch, "Pooled Answer A match not found");

             // Pooled Wrong Answer B (from SubQ1 and SubQ2 - should appear once)
            var poolBMatch = matchesNode.SelectNodes("match[questiontext='' and answertext='WrongAnswerB']");
            Assert.That(poolBMatch?.Count, Is.EqualTo(1), "Pooled Answer B should appear only once");
            Assert.IsNotNull(poolBMatch[0], "Pooled Answer B match not found");

             // Pooled Wrong Answer C (only from SubQ2)
            var poolCMatch = matchesNode.SelectSingleNode("match[questiontext='' and answertext='WrongAnswerC']");
            Assert.IsNotNull(poolCMatch, "Pooled Answer C match not found");
        }

        [Test]
        public void WriteQuestions_WritesDropdownFeedbackCorrectly()
        {
            // Arrange
            var question = CreateBasicDropdownQuestion(23, "DDCatFeedback", withFeedback: true);
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Feedback in matchoptions
            var matchOptionsNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//matchoptions");
            Assert.IsNotNull(matchOptionsNode);

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
            var question = CreateBasicDropdownQuestion(24, "DDCatHints", withHints: true);
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Hints structure and content
            var hintsNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/question_hints");
            Assert.IsNotNull(hintsNode, "Hints node not found");

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
            var question = CreateBasicDropdownQuestion(25, "DDCatNoHints", withHints: false);
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Empty hints node
            var hintsNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/question_hints");
            Assert.IsNotNull(hintsNode, "Hints node should exist even if empty");
            Assert.That(hintsNode.ChildNodes.Count, Is.EqualTo(0), "Hints node should be empty");
        }

        [Test]
        public void WriteQuestions_WritesDragDropTextBaseElementsCorrectly()
        {
            // Arrange
            var question = CreateBasicDragDropTextQuestion(50, "DDTBaseCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check base question elements
            var questionNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']");
            Assert.IsNotNull(questionNode, $"Question node with ID {question.QuestionId} not found");

            Assert.That(questionNode.SelectSingleNode("name")?.InnerText, Is.EqualTo(question.QuestionName));
            Assert.That(questionNode.SelectSingleNode("questiontext")?.InnerText, Is.EqualTo(question.QuestionText));
            Assert.That(questionNode.SelectSingleNode("questiontextformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("generalfeedback")?.InnerText, Is.EqualTo(question.GeneralFeedback));
            Assert.That(questionNode.SelectSingleNode("generalfeedbackformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("defaultmark")?.InnerText, Is.EqualTo(question.DefaultMark.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("penalty")?.InnerText, Is.EqualTo(question.HintPenalty.ToString("0.0000000", CultureInfo.InvariantCulture)));
            Assert.That(questionNode.SelectSingleNode("qtype")?.InnerText, Is.EqualTo("ddwtos")); // Mapped value for DragDropText
            Assert.That(questionNode.SelectSingleNode("timecreated")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
            Assert.That(questionNode.SelectSingleNode("timemodified")?.InnerText, Is.EqualTo(DEFAULT_TIMESTAMP)); // Mocked value
        }

        [Test]
        public void WriteQuestions_WritesDragDropTextPluginStructureCorrectly()
        {
            // Arrange
            var question = CreateBasicDragDropTextQuestion(51, "DDTStructCat");
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Plugin structure
            var pluginNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']/plugin_qtype_ddwtos_question");
            Assert.IsNotNull(pluginNode, "ddwtos plugin node not found");

            var answersNode = pluginNode.SelectSingleNode("answers");
            Assert.IsNotNull(answersNode, "Answers node not found");
            Assert.That(answersNode.SelectNodes("answer")?.Count, Is.EqualTo(question.Answers.Count), "Incorrect number of answer choices");

            var ddwtosNode = pluginNode.SelectSingleNode("ddwtos");
            Assert.IsNotNull(ddwtosNode, "ddwtos options node not found");
        }

        [Test]
        public void WriteQuestions_WritesDragDropTextAnswersAndFeedbackCorrectly()
        {
            // Arrange
            var question = CreateBasicDragDropTextQuestion(52, "DDTAnsCat");
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
            Assert.IsNotNull(answersNode);

            // Choice 1 (France, Cat A)
            var answer1Node = answersNode.SelectSingleNode($"answer[answertext='{choice1.Value}']");
            Assert.IsNotNull(answer1Node, $"Answer node for '{choice1.Value}' not found");
            Assert.That(answer1Node.SelectSingleNode("answerformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(answer1Node.SelectSingleNode("fraction")?.InnerText, Is.EqualTo("0.0000000"));
            Assert.That(answer1Node.SelectSingleNode("feedback")?.InnerText, Is.EqualTo(expectedFeedback1));
            Assert.That(answer1Node.SelectSingleNode("feedbackformat")?.InnerText, Is.EqualTo("0"));

            // Choice 2 (London, Cat B)
            var answer2Node = answersNode.SelectSingleNode($"answer[answertext='{choice2.Value}']");
            Assert.IsNotNull(answer2Node, $"Answer node for '{choice2.Value}' not found");
            Assert.That(answer2Node.SelectSingleNode("answerformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(answer2Node.SelectSingleNode("fraction")?.InnerText, Is.EqualTo("0.0000000"));
            Assert.That(answer2Node.SelectSingleNode("feedback")?.InnerText, Is.EqualTo(expectedFeedback2));
            Assert.That(answer2Node.SelectSingleNode("feedbackformat")?.InnerText, Is.EqualTo("0"));

             // Choice 4 (Germany, Cat A)
            var answer4Node = answersNode.SelectSingleNode($"answer[answertext='{choice4.Value}']");
            Assert.IsNotNull(answer4Node, $"Answer node for '{choice4.Value}' not found");
            Assert.That(answer4Node.SelectSingleNode("answerformat")?.InnerText, Is.EqualTo(DEFAULT_FORMAT_MAPPED));
            Assert.That(answer4Node.SelectSingleNode("fraction")?.InnerText, Is.EqualTo("0.0000000"));
            Assert.That(answer4Node.SelectSingleNode("feedback")?.InnerText, Is.EqualTo(expectedFeedback4));
            Assert.That(answer4Node.SelectSingleNode("feedbackformat")?.InnerText, Is.EqualTo("0"));
        }

        [Test]
        public void WriteQuestions_HandlesDragDropTextWithNoCategoriesCorrectly()
        {
            // Arrange
            var question = CreateBasicDragDropTextQuestion(56, "DDTNoCat");

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
            Assert.IsNotNull(answersNode);

            var answer1Feedback = answersNode.SelectSingleNode("answer[answertext='Choice1']/feedback")?.InnerText;
            Assert.That(answer1Feedback, Is.EqualTo(expectedFeedback), "PHP Feedback string for Choice1 with null category is incorrect");

            var answer2Feedback = answersNode.SelectSingleNode("answer[answertext='Choice2']/feedback")?.InnerText;
            Assert.That(answer2Feedback, Is.EqualTo(expectedFeedback), "PHP Feedback string for Choice2 with empty category is incorrect");
        }

        [Test]
        public void WriteQuestions_WritesDragDropTextOptionsCorrectly()
        {
            // Arrange
            var question = CreateBasicDragDropTextQuestion(53, "DDTOptionsCat", withFeedback: true, withHints: true);
            question.ShuffleAnswers = false; // Override default shuffle
            _course.Questions.Add(question);

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: Check ddwtos options node
            var ddwtosNode = doc.SelectSingleNode($"//question[@id='{question.QuestionId}']//ddwtos");
            Assert.IsNotNull(ddwtosNode, "ddwtos options node not found");

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
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(1, "CatA"));

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
            _questionsWriter = new QuestionsWriter(throwingFactory, _questionsLogger, _fakeGeneralHelper, _fakeIdGenerator, _fakeTagsWriter, new List<IQuestionPluginWriter>());
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(1, "CatA"));

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


        /// <summary>
        /// Creates a basic multiple-choice question with predefined parameters and structure
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <returns>A <see cref="MultipleChoiceQuestion"/> object initialized with default values and structure</returns>
        private MultipleChoiceQuestion CreateBasicMultiChoiceQuestion(int id, string category)
        {
            return new MultipleChoiceQuestion
            {
                QuestionId = id,
                Category = category,
                QuestionName = $"MCQ_{id}",
                QuestionText = $"<p>MCQ Text {id}</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                GeneralFeedback = $"General Feedback {id}",
                GeneralFeedbackFormat = FormattingFormat.HTML,
                DefaultMark = 1.5f,
                HintPenalty = 0.1f,
                ChoiceNumbering = NumberingFormat.AlphabeticLower,
                ShuffleAnswers = true,
                SingleChoice = true,
                CorrectResponseFeedback = "Correct MCQ",
                CorrectResponseFeedbackFormat = FormattingFormat.HTML,
                PartiallyCorrectResponseFeedback = "Partially Correct MCQ",
                PartiallyCorrectResponseFeedbackFormat = FormattingFormat.HTML,
                IncorrectResponseFeedback = "Incorrect MCQ",
                IncorrectResponseFeedbackFormat = FormattingFormat.HTML,
                ShowStandardInstructions = true,
                QuestionReady = true,
                Answers = new List<MultiChoiceAnswer>
                {
                    new MultiChoiceAnswer
                    {
                        AnswerText = $"<p>Answer {id}-1 Correct</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = 1f,
                        AnswerFeedback = "Feedback Correct",
                        AnswerFeedbackFormat = FormattingFormat.HTML,
                    },
                    new MultiChoiceAnswer
                    {
                        AnswerText = $"<p>Answer {id}-2 Wrong</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = 0f,
                        AnswerFeedback = "Feedback Wrong",
                        AnswerFeedbackFormat = FormattingFormat.HTML,
                    },
                },
                Hints = null, // TODO: Add support for hints as feedback as in CreateBasicDropdownQuestion
                Tags = new List<string> { $"Tag{id}" },
            };
        }


        /// <summary>
        /// Creates a basic dropdown question with predefined structure and configuration
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <param name="withHints">Indicates whether the question includes hints</param>
        /// <param name="withFeedback">Indicates whether the question provides general feedback</param>
        /// <returns>A <see cref="DropdownQuestion"/> object with initialized values and structure</returns>
        private DropdownQuestion CreateBasicDropdownQuestion(int id, string category, bool withHints = false, bool withFeedback = false)
        {
            var dq = new DropdownQuestion
            {
                QuestionId = id,
                Category = category,
                QuestionName = $"Dropdown_{id}",
                QuestionText = $"<p>Dropdown Text {id}</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                GeneralFeedback = $"General Feedback DD {id}",
                GeneralFeedbackFormat = FormattingFormat.HTML,
                DefaultMark = 2.0f,
                HintPenalty = 0.25f,
                QuestionReady = true,
                Answers = new List<DropdownSubQuestion>
                {
                    new DropdownSubQuestion
                    {
                        QuestionText = "SubQ 1 Text",
                        QuestionTextFormat = FormattingFormat.PlainText,
                        CorrectAnswer = "CorrectAnswer1",
                        WrongAnswers = ["WrongAnswerA", "WrongAnswerB"],
                    },
                    new DropdownSubQuestion
                    {
                        QuestionText = "SubQ 2 Text",
                        QuestionTextFormat = FormattingFormat.HTML,
                        CorrectAnswer = "CorrectAnswer2",
                        WrongAnswers = ["WrongAnswerB", "WrongAnswerC"], // "WrongAnswerB" is shared
                    },
                },
                Hints = null, // Hints are added below
                Tags = new List<string> { $"TagDD{id}" },
            };

            if (withFeedback)
            {
                dq.CorrectResponseFeedback = "DD Correct";
                dq.CorrectResponseFeedbackFormat = FormattingFormat.HTML;
                dq.PartiallyCorrectResponseFeedback = "DD Partial";
                dq.PartiallyCorrectResponseFeedbackFormat = FormattingFormat.PlainText;
                dq.IncorrectResponseFeedback = "DD Incorrect";
                dq.IncorrectResponseFeedbackFormat = FormattingFormat.HTML;
            }

            if (withHints)
            {
                dq.Hints = new List<Hint>
                {
                    new Hint
                    {
                        HintText = "Hint 1 Text",
                        HintTextFormat = FormattingFormat.HTML,
                        ShowNumberOfCorrectAnswers = true,
                        ClearIncorrectResponses = false,
                    },
                    new Hint
                    {
                        HintText = "Hint 2 Text",
                        HintTextFormat = FormattingFormat.PlainText,
                        ShowNumberOfCorrectAnswers = false,
                        ClearIncorrectResponses = true,
                    },
                };
            }

            return dq;
        }


        /// <summary>
        /// Creates a basic drag-and-drop text question with specified parameters
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <param name="withHints">Indicates whether the question should include hints</param>
        /// <param name="withFeedback">Indicates whether the question should include feedback for responses</param>
        /// <param name="questionReady">Indicates whether the question is considered ready for use</param>
        /// <returns>A <see cref="DragAndDropTextQuestion"/> object initialized with specified settings and default structure</returns>
        private DragAndDropTextQuestion CreateBasicDragDropTextQuestion(int id, string category, bool withHints = false, bool withFeedback = false)
        {
            var ddtq = new DragAndDropTextQuestion
            {
                QuestionId = id,
                Category = category,
                QuestionName = $"DragDrop_{id}",
                QuestionText = "The capital of [[1]] is Paris. The capital of the UK is [[2]]. The capital of Austria is [[3]].",
                QuestionTextFormat = FormattingFormat.HTML,
                GeneralFeedback = $"General Feedback DDT {id}",
                GeneralFeedbackFormat = FormattingFormat.HTML,
                DefaultMark = 3.0f,
                HintPenalty = 0.3333333f,
                QuestionReady = true,
                ShuffleAnswers = true,
                Answers = new List<TextChoice>
                {
                    new TextChoice { ID = 1, Value = "France", Category = "A", ValueFormat = FormattingFormat.PlainText },
                    new TextChoice { ID = 2, Value = "London", Category = "B", ValueFormat = FormattingFormat.HTML },
                    new TextChoice { ID = 3, Value = "Vienna", Category = "A", ValueFormat = FormattingFormat.PlainText },
                    new TextChoice { ID = 4, Value = "Germany", Category = "A", ValueFormat = FormattingFormat.HTML }, // Distractor in group A
                    new TextChoice { ID = 5, Value = "Madrid", Category = "B", ValueFormat = FormattingFormat.PlainText }, // Distractor in group B
                },
                Hints = null, // Hints are added below
                Tags = new List<string> { $"TagDDT{id}" },
            };

            if (withFeedback)
            {
                ddtq.CorrectResponseFeedback = "DDT Correct";
                ddtq.CorrectResponseFeedbackFormat = FormattingFormat.HTML;
                ddtq.PartiallyCorrectResponseFeedback = "DDT Partial";
                ddtq.PartiallyCorrectResponseFeedbackFormat = FormattingFormat.PlainText;
                ddtq.IncorrectResponseFeedback = "DDT Incorrect";
                ddtq.IncorrectResponseFeedbackFormat = FormattingFormat.Markdown;
            }

            if (withHints)
            {
                ddtq.Hints = new List<Hint>
                {
                    new Hint
                    {
                        HintText = "DDT Hint 1",
                        HintTextFormat = FormattingFormat.HTML,
                        ShowNumberOfCorrectAnswers = true,
                        ClearIncorrectResponses = false,
                    },
                    new Hint
                    {
                        HintText = "DDT Hint 2",
                        HintTextFormat = FormattingFormat.PlainText,
                        ShowNumberOfCorrectAnswers = false,
                        ClearIncorrectResponses = true,
                    },
                };
            }

            return ddtq;
        }
    }
}
