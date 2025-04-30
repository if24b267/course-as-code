using System.Globalization;
using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Writers.Tests.Fakes;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer
{
    [TestFixture]
    public class QuestionsWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _logger;
        private IGeneralHelper _fakeGeneralHelper;
        private QuestionsWriter _questionsWriter;
        private Model.Course _course;

        private const string DEFAULT_DUMMY_OUTPUT_PATH = "dummyPath";
        private const string DEFAULT_FORMAT_MAPPED = "1";
        private const string DEFAULT_TIMESTAMP = "1234567890";

        [SetUp]
        public void SetUp()
        {
            // Arrange: Set up fake XML writer factory (to capture XML output) and logger
            _testFactory = new XmlWriterFactoryTest();
            _logger = Substitute.For<ILogger>();

            // Fake general helper with predictable outputs
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<FormattingFormat>()).Returns(DEFAULT_FORMAT_MAPPED); // Default to HTML format "1"
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns(DEFAULT_TIMESTAMP);

            // Instantiate QuestionsWriter with dependencies
            _questionsWriter = new QuestionsWriter(_testFactory, _logger, _fakeGeneralHelper);

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
        public void WriteQuestions_LogsDebugOnSuccess()
        {
            // Arrange
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(1, "CatA"));

            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert
            _logger.Received().Debug(Arg.Is<string>(s => s.Contains("questions.xml")));
        }

        [Test]
        public void WriteQuestions_LogsErrorOnException()
        {
            // Arrange: Use an XML writer factory that throws an exception
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            _questionsWriter = new QuestionsWriter(throwingFactory, _logger, _fakeGeneralHelper);
            _course.Questions.Add(CreateBasicMultiChoiceQuestion(1, "CatA"));

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH));

            // Verify that an error log was recorded containing "questions.xml"
            _logger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("questions.xml")));
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
                Hints = null, // Add hints separately
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
    }
}
