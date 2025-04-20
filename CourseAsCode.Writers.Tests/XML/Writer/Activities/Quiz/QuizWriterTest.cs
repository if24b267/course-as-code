using System.Xml;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
using QuizModel = CourseAsCode.Model.Activities.Resources.Quiz;
using CourseAsCode.Writers.XML.Helper.Interfaces;
using CourseAsCode.Writers.XML.Writer.Activities.Quiz;
using CourseAsCode.Writers.Tests.Fakes;
using NLog;
using NSubstitute;

namespace CourseAsCode.Writers.Tests.XML.Writer.Activities.Quiz
{
    [TestFixture]
    public class QuizWriterTests
    {
        private XmlWriterFactoryTest _testFactory;
        private ILogger _logger;
        private IGeneralHelper _generalHelper;
        private QuizWriter _quizWriter;
        private QuizModel _dummyQuiz;
        private const string DEFAULT_DEFAULT_OUTPUT_PATH = "dummyDir";

        [SetUp]
        public void SetUp()
        {
            // Set up the fake XML writer factory to capture XML output
            _testFactory = new XmlWriterFactoryTest();

            // Mock the logger and general helper dependencies and configure them to return consistent values
            _logger = Substitute.For<ILogger>();
            _generalHelper = Substitute.For<IGeneralHelper>();
            _generalHelper.EnumIntegerToString(Arg.Any<Enum>()).Returns("0");
            _generalHelper.DateTimeToUnixTimestamp(Arg.Any<DateTime>()).Returns("1234567890");

            // Instantiate the QuizWriter with the fake dependencies
            _quizWriter = new QuizWriter(_testFactory, _logger, _generalHelper);

            // Create dummy questions
            var q1 = new MultipleChoiceQuestion 
            {
                QuestionId          = 1, // Normally set by `QuestionsWriter.cs`, simulate it here
                BankEntryId         = 1, // Normally set by `QuestionsWriter.cs`, simulate it here
                Category            = "Default category",
                QuestionName        = "Shamir’s Theorem",
                QuestionText        = "<p>Which equality does Shamir’s Theorem prove?</p>",
                QuestionTextFormat  = FormattingFormat.HTML,
                DefaultMark         = 1,
                HintPenalty         = 0.3333333f,
                ChoiceNumbering     = NumberingFormat.AlphabeticLower,
                ShuffleAnswers      = true,
                SingleChoice        = true,
                Answers = new List<Answer>
                {
                    new()
                    {
                        AnswerText  = "<p>IP = NP</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = 0,
                        AnswerFeedback =
                            "<p>Just seeing an NP somewhere doesn’t make it right.</p>",
                        AnswerFeedbackFormat = FormattingFormat.HTML
                    },
                    new()
                    {
                        AnswerText  = "<p>IP = PSPACE</p>",
                        AnswerTextFormat = FormattingFormat.HTML,
                        AnswerGrade = 1
                    }
                },
                Hints = new List<Hint>
                {
                    new()
                    {
                        HintText =
                            "<p>Recall that IP refers to interactive proofs …</p>",
                        HintTextFormat = FormattingFormat.HTML
                    }
                }
            }; 
            
            var q2 = new MultipleChoiceQuestion
            {
                QuestionId          = 5, // Normally set by `QuestionsWriter.cs`, simulate it here
                BankEntryId         = 8, // Normally set by `QuestionsWriter.cs`, simulate it here
                Category            = "Linux basics",
                QuestionName        = "Linux navigation commands",
                QuestionText        = "<p>Select the navigation-related commands.</p>",
                QuestionTextFormat  = FormattingFormat.HTML,
                DefaultMark         = 1,
                ChoiceNumbering     = NumberingFormat.Numeric,
                ShuffleAnswers      = true,
                SingleChoice        = false,
                Answers = new List<Answer>
                {
                    new() { AnswerText = "<p>ls</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = 1f / 3f },
                    new() { AnswerText = "<p>cd</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = 1f / 3f },
                    new() { AnswerText = "<p>pwd</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = 1f / 3f },
                    new() { AnswerText = "<p>grep</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = -1f / 3f }
                }
            };

            var q3 = new SingleChoiceQuestion
            {
                QuestionId = 8, // Normally set by `QuestionsWriter.cs`, simulate it here
                BankEntryId = 24, // Normally set by `QuestionsWriter.cs`, simulate it here
                Category = "Linux basics",
                QuestionName = "Linux filesystem",
                QuestionText = "What does the `/etc` directory in the Linux file system hierarchy typically contain?",
                QuestionTextFormat = FormattingFormat.Markdown,
                DefaultMark = 1,
                ChoiceNumbering = NumberingFormat.AlphabeticLower,
                ShuffleAnswers = true,
                SingleChoice = true,
                Answers = new List<Answer>
                {
                    new() { AnswerText = "<p>User home directories</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = -1f / 4f, AnswerFeedback = "<p>Hahaha, are you stupid?</p>", AnswerFeedbackFormat = FormattingFormat.HTML},
                    new() { AnswerText = "<p>System configuration files</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = -1f / 4f },
                    new() { AnswerText = "<p>Essential command binaries</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = -1f / 4f },
                    new() { AnswerText = "<p>Device files</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = -1f / 4f },
                    new() { AnswerText = "<p>System log files</p>", AnswerTextFormat = FormattingFormat.HTML, AnswerGrade = 1f }
                }
            };
            
            // Create a dummy quiz with three questions across two categories and three feedbacks
            _dummyQuiz = new QuizModel
            {
                ActivityId = 10,
                Name = "Unit Test Quiz",
                Description = "A quiz for testing",
                DescriptionFormat = FormattingFormat.HTML,
                OpenTime = null,
                CloseTime = null,
                TimeLimit = 15,
                TimeCreated = new DateTime(2025, 4, 20, 9, 0, 0),
                TimeModified = new DateTime(2025, 4, 20, 9, 30, 0),
                ShowPicture = PictureOptions.NoImage,
                Questions = new List<Question> { q1, q2, q3 },
                Feedback                = new List<QuizFeedback>()
                {
                    new QuizFeedback()
                    {
                        FeedbackText = "<p>You did it!</p>",
                        GradeBoundary = 100f,
                        OverallFeedbackFormat = FormattingFormat.HTML
                    },
                    new QuizFeedback()
                    {
                        FeedbackText  = "<p>Probably should go back to kindergarten</p>",
                        GradeBoundary = 50f,
                        OverallFeedbackFormat = FormattingFormat.HTML
                    },
                    new QuizFeedback()
                    {
                        FeedbackText = "<p>Oh, wow that's just sad</p>",
                        GradeBoundary = 0f,
                        OverallFeedbackFormat = FormattingFormat.HTML
                    }
                }
            };
        }

        [Test]
        public void WriteQuiz_CreatesValidXml()
        {
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            string output = _testFactory.StringWriter.ToString();
            
            // Assert: Loading the XML should not throw an exception (well-formed XML)
            Assert.DoesNotThrow(() =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(output);
            });
        }

        [Test]
        public void WriteQuiz_RootElementIsActivity_HasCorrectAttributes()
        {
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            string output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            // Assert: The root element is <activity> with correct attributes
            var root = doc.DocumentElement;
            Assert.IsNotNull(root, "The XML document should have a root element");
            Assert.That(root.Name, Is.EqualTo("activity"));
            Assert.That(root.GetAttribute("id"), Is.EqualTo(_dummyQuiz.ActivityId.ToString()));
            Assert.That(root.GetAttribute("moduleid"), Is.EqualTo(_dummyQuiz.SystemDefaults.ModuleId.ToString()));
            Assert.That(root.GetAttribute("modulename"), Is.EqualTo(_dummyQuiz.SystemDefaults.ModuleName));
            Assert.That(root.GetAttribute("contextid"), Is.EqualTo("383")); // hardcoded in the QuizWriter
        }

        [Test]
        public void WriteQuiz_IncludesQuizElement_WithCorrectId()
        {
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            // Assert: The quiz element should be <quiz> with correct id attribute
            var quizNode = doc.DocumentElement?.SelectSingleNode("quiz");
            Assert.IsNotNull(quizNode, "Missing <quiz> element");
            Assert.That(quizNode.Attributes?["id"]?.Value, Is.EqualTo(_dummyQuiz.ActivityId.ToString()));
        }

        [Test]
        public void WriteQuiz_WritesNameAndDescriptionElements()
        {
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);

            // Assert: Name and description elements should be present and have correct values
            var nameNode = doc.DocumentElement?.SelectSingleNode("quiz/name");
            Assert.IsNotNull(nameNode, "Missing <name>");
            Assert.That(nameNode.InnerText, Is.EqualTo(_dummyQuiz.Name));

            var introNode = doc.DocumentElement?.SelectSingleNode("quiz/intro");
            Assert.IsNotNull(introNode, "Missing <intro>");
            Assert.That(introNode.InnerText, Is.EqualTo(_dummyQuiz.Description));
        }

        [Test]
        public void WriteQuiz_IncludesQuestionElementsWithCorrectIds()
        {
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();
            
            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            // Assert: The question elements should be <question_instance> with correct id attributes
            var questionNodes = doc.DocumentElement?.SelectNodes("//quiz//question_instance");
            Assert.IsNotNull(questionNodes, "Missing <question> elements");
            Assert.That(questionNodes.Count, Is.EqualTo(_dummyQuiz.Questions.Count));
            
            // Assert: The IDs of the <question_instance> elements should match the IDs of the questions
            for (int i = 0; i < questionNodes.Count; i++)
            {
                var questionNode = questionNodes[i];
                Assert.That(questionNode?.Attributes?["id"]?.Value, Is.EqualTo(_dummyQuiz.Questions[i].QuestionId.ToString()));
            }
        }

        [Test]
        public void WriteQuiz_IncludesQuestionReferenceElementsWithCorrectIds()
        {
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();
            
            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            // Assert: The question reference elements should be <question_reference> with correct id attributes
            var questionRefNodes = doc.DocumentElement?.SelectNodes("//quiz//question_instance//question_reference");
            Assert.IsNotNull(questionRefNodes, "Missing <question_reference> elements");
            Assert.That(questionRefNodes.Count, Is.EqualTo(_dummyQuiz.Questions.Count));
            
            // Assert: The IDs of the <question_reference> elements should match the IDs of the questions
            for (int i = 0; i < questionRefNodes.Count; i++)
            {
                var questionRefNode = questionRefNodes[i];
                Assert.That(questionRefNode?.Attributes?["id"]?.Value, Is.EqualTo(_dummyQuiz.Questions[i].QuestionId.ToString()));
            }
        }

        [Test]
        public void WriteQuiz_QuestionReferenceReferencesCorrectQuestionBank()
        {
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();
            
            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            // Assert: The <questionbankentryid> elements inside the <question_reference> elements should reference the correct question bank
            var questionBankRefNodes = doc.DocumentElement?.SelectNodes("//quiz//question_instance//question_reference//questionbankentryid");
            Assert.IsNotNull(questionBankRefNodes, "Missing <questionbankentryid> elements");
            Assert.That(questionBankRefNodes.Count, Is.EqualTo(_dummyQuiz.Questions.Count));
            
            // Assert: The IDs (= value) of the <questionbankentryid> elements should match the QuestionBank IDs stored in the questions
            for (int i = 0; i < questionBankRefNodes.Count; i++)
            {
                var questionBankRefNode = questionBankRefNodes[i];
                Assert.That(questionBankRefNode?.InnerText, Is.EqualTo(_dummyQuiz.Questions[i].BankEntryId.ToString()));
            }
        }

        [Test]
        public void WriteQuiz_SetsTimeOpenAndTimeCloseToZero_WhenNull()
        {
            // Arrange
            _dummyQuiz.OpenTime = null;
            _dummyQuiz.CloseTime = null;

            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            // Assert: timeopen and timeclose should be set to 0
            var openNode  = doc.DocumentElement?.SelectSingleNode("quiz/timeopen");
            var closeNode = doc.DocumentElement?.SelectSingleNode("quiz/timeclose");

            Assert.That(openNode?.InnerText, Is.EqualTo("0"),  "timeopen should default to 0");
            Assert.That(closeNode?.InnerText, Is.EqualTo("0"), "timeclose should default to 0");
        }

        [Test]
        public void WriteQuiz_SetsTimeOpenAndTimeCloseToUnixTimestamp_WhenNotNull()
        {
            // Arrange
            _dummyQuiz.OpenTime = new DateTime(2025, 4, 20, 9, 0, 0);
            _dummyQuiz.CloseTime = new DateTime(2025, 4, 20, 9, 30, 0);
            
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            var output = _testFactory.StringWriter.ToString();
            
            var doc = new XmlDocument();
            doc.LoadXml(output);
            
            // Assert: timeopen and timeclose should be set to the correct Unix timestamp
            var openNode  = doc.DocumentElement?.SelectSingleNode("quiz/timeopen");
            var closeNode = doc.DocumentElement?.SelectSingleNode("quiz/timeclose");
            
            Assert.That(openNode?.InnerText, Is.EqualTo("1234567890"),  "timeopen should be set to 1234567890"); // from mocked DateTimeToUnixTimestamp
            Assert.That(closeNode?.InnerText, Is.EqualTo("1234567890"), "timeclose should be set to 1234567890"); // from mocked DateTimeToUnixTimestamp
        }

        [Test]
        public void WriteQuiz_LogsDebugOnSuccess()
        {
            // Act
            _quizWriter.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz);
            
            // Assert: The logger should have been called with a debug message
            _logger.Received().Debug(Arg.Is<string>(msg => msg.Contains("quiz.xml")));
        }

        [Test]
        public void WriteQuiz_LogsErrorAndRethrows_OnException()
        {
            var throwingFactory = new ExceptionThrowingXmlWriterFactory();
            var writer          = new QuizWriter(throwingFactory, _logger, _generalHelper);

            // Act and assert: The logger should have been called with an error message and the exception should be rethrown
            Assert.Throws<InvalidOperationException>(() => writer.WriteQuiz(DEFAULT_DEFAULT_OUTPUT_PATH, _dummyQuiz));
            _logger.Received()
                   .Error(Arg.Any<Exception>(), Arg.Is<string>(msg => msg.Contains("quiz.xml")));
        }
    }
}
