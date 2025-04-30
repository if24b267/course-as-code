using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
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

        [SetUp]
        public void SetUp()
        {
            // Arrange: Set up fake XML writer factory (to capture XML output) and logger
            _testFactory = new XmlWriterFactoryTest();
            _logger = Substitute.For<ILogger>();

            // Fake general helper with predictable outputs
            _fakeGeneralHelper = Substitute.For<IGeneralHelper>();
            _fakeGeneralHelper.EnumIntegerToString(Arg.Any<FormattingFormat>()).Returns("0");
            _fakeGeneralHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Instantiate QuestionsWriter with dependencies
            _questionsWriter = new QuestionsWriter(_testFactory, _logger, _fakeGeneralHelper);

            // Create a dummy course with three questions across two categories
            _course = new Model.Course();

            var q1 = new MultipleChoiceQuestion
            {
                QuestionId = 1,
                Category = "CatA",
                QuestionName = "Q1",
                QuestionText = "<p>T1</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                DefaultMark = 1,
                HintPenalty = 0,
                ChoiceNumbering = NumberingFormat.Numeric,
                ShuffleAnswers = false,
                SingleChoice = true,
                Answers = new List<MultiChoiceAnswer>
                {
                    new MultiChoiceAnswer
                    {
                        AnswerText = "<p>A1</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = 1f,
                    },
                    new MultiChoiceAnswer
                    {
                        AnswerText = "<p>A2</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = 0f,
                        AnswerFeedback = "<p>Incorrect</p>",
                        AnswerFeedbackFormat = FormattingFormat.HTML
                    }
                },
                Hints = null
            };
            var q2 = new MultipleChoiceQuestion
            {
                QuestionId = 2,
                Category = "CatB",
                QuestionName = "Q2",
                QuestionText = "<p>T2</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                DefaultMark = 1,
                HintPenalty = 0,
                ChoiceNumbering = NumberingFormat.Numeric,
                ShuffleAnswers = true,
                SingleChoice = false,
                Answers = new List<MultiChoiceAnswer>
                {
                    new MultiChoiceAnswer
                    {
                        AnswerText = "<p>B1</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = 0.5f,
                        AnswerFeedback = "<p>Good</p>",
                        AnswerFeedbackFormat = FormattingFormat.HTML
                    },
                    new MultiChoiceAnswer
                    {
                        AnswerText = "<p>B2</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = -0.25f,
                        AnswerFeedback = "<p>Penalized</p>",
                        AnswerFeedbackFormat = FormattingFormat.HTML
                    }
                },
                Hints = null
            };
            var q3 = new MultipleChoiceQuestion
            {
                QuestionId = 3,
                Category = "CatB",
                QuestionName = "Q3",
                QuestionText = "<p>T3</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                DefaultMark = 1,
                HintPenalty = 0,
                ChoiceNumbering = NumberingFormat.AlphabeticLower,
                ShuffleAnswers = false,
                SingleChoice = true,
                Answers = new List<MultiChoiceAnswer>
                {
                    new MultiChoiceAnswer
                    {
                        AnswerText = "<p>C1</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = 0f,
                        AnswerFeedback = null,
                        AnswerFeedbackFormat = FormattingFormat.HTML
                    }
                },
                Hints = null
            };

            _course.Questions = new List<Question> { q1, q2, q3 };
        }

        [Test]
        public void WriteQuestions_GeneratesValidXml()
        {
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
            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: There should be one top-level category plus one for each distinct course category
            int expectedCategories = 1 + _course.Questions.Select(q => q.Category).Distinct().Count();
            var categoryNodes = doc.SelectNodes("//question_category");
            Assert.That(categoryNodes?.Count, Is.EqualTo(expectedCategories));
        }

        [Test]
        public void WriteQuestions_CreatesCorrectNumberOfBankEntries()
        {
            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: The total number of question_bank_entry elements should match total questions (at least that's the case in our implementation)
            var entryNodes = doc.SelectNodes("//question_bank_entry");
            Assert.That(entryNodes?.Count, Is.EqualTo(_course.Questions.Count));
        }

        [Test]
        [Ignore("Changed the implementation to not auto increment - this test needs to be updated")]
        public void WriteQuestions_SetsIdsOnQuestionObjects()
        {
            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);

            // Assert: Category, Question and QuestionBankEntry IDs are set correctly for the three question instances
            var questions = _course.Questions;

            // First grouping "CatA" gets ID = DEFAULT_TOP_CATEGORY_ID+1 = 2
            Assert.That(questions[0].CategoryId, Is.EqualTo(2));
            // Next grouping "CatB" gets ID = 3
            Assert.That(questions[1].CategoryId, Is.EqualTo(3));
            Assert.That(questions[2].CategoryId, Is.EqualTo(3));

            // BankEntryId should increment starting at 1
            Assert.That(questions[0].BankEntryId, Is.EqualTo(1));
            Assert.That(questions[1].BankEntryId, Is.EqualTo(2));
            Assert.That(questions[2].BankEntryId, Is.EqualTo(3));

            // QuestionId should increment starting at 1
            Assert.That(questions[0].QuestionId, Is.EqualTo(1));
            Assert.That(questions[1].QuestionId, Is.EqualTo(2));
            Assert.That(questions[2].QuestionId, Is.EqualTo(3));
        }

        [Test]
        public void WriteQuestions_WritesAnswerElementsCorrectly()
        {
            // Act
            _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH);

            var doc = new XmlDocument();
            doc.LoadXml(_testFactory.StringWriter.ToString());

            // Assert: The <answer> elements should be written correctly

            // Question 1 should have two <answer> nodes under its <question> element
            var answersQ1 = doc.SelectNodes("//question[@id='1']//answer");
            Assert.That(answersQ1?.Count, Is.EqualTo(2));

            // Verify first answer text and fraction
            var firstAnswer = answersQ1[0];
            Assert.That(firstAnswer?.SelectSingleNode("answertext")?.InnerText, Is.EqualTo("<p>A1</p>"));
            Assert.That(firstAnswer.SelectSingleNode("fraction")?.InnerText, Is.EqualTo("1.0000000"));

            // Verify second answer feedback default and grade formatting
            var secondAnswer = answersQ1[1];
            Assert.That(secondAnswer?.SelectSingleNode("answertext")?.InnerText, Is.EqualTo("<p>A2</p>"));
            Assert.That(secondAnswer.SelectSingleNode("fraction")?.InnerText, Is.EqualTo("0.0000000"));
            Assert.That(secondAnswer.SelectSingleNode("feedback")?.InnerText, Is.EqualTo("<p>Incorrect</p>"));
        }

        [Test]
        public void WriteQuestions_LogsDebugOnSuccess()
        {
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

            // Act & Assert: Verify that an exception is thrown
            Assert.Throws<InvalidOperationException>(() =>
                _questionsWriter.WriteQuestions(_course, DEFAULT_DUMMY_OUTPUT_PATH));

            // Verify that an error log was recorded containing "questions.xml"
            _logger.Received().Error(Arg.Any<Exception>(), Arg.Is<string>(s => s.Contains("questions.xml")));
        }
    }
}
