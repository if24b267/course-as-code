using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types.CalculatedSimple;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Model.Questions.Types.Essay;
using CourseAsCode.Model.Questions.Types.MultipleChoice;
using CourseAsCode.Model.Questions.Types.ShortAnswer;
using CourseAsCode.Model.Questions.Types.TrueFalse;

namespace CourseAsCode.Writers.Tests.Fakes
{
    public static class FakeQuestionFactory
    {
        /// <summary>
        /// Creates a basic multiple-choice question with predefined parameters and structure
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <returns>A <see cref="MultipleChoiceQuestion"/> object initialized with default values and structure</returns>
        public static MultipleChoiceQuestion CreateBasicMultiChoiceQuestion(int id, string category)
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
        public static DropdownQuestion CreateBasicDropdownQuestion(int id, string category, bool withHints = false, bool withFeedback = false)
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
                SubQuestions = new List<DropdownSubQuestion>
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
        /// Creates a basic CodeRunner question with predefined parameters and structure
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <param name="withHints">Indicates whether the question should include hints</param>
        /// <returns>A <see cref="CodeRunnerQuestion"/> object initialized with default values and structure</returns>
        public static CodeRunnerQuestion CreateBasicCodeRunnerQuestion(int id, string category, bool withHints = false)
        {
            var crq = new CodeRunnerQuestion
            {
                QuestionId = id,
                Category = category,
                QuestionName = $"CodeRunner_{id}",
                QuestionText = "<p>Write a program that outputs \"Hello, World!\"</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                GeneralFeedback = "<p>Remember to include the appropriate main method.</p>",
                GeneralFeedbackFormat = FormattingFormat.HTML,
                DefaultMark = 1.0f,
                HintPenalty = 0.1f,
                QuestionReady = true,
                CodeRunnerType = "java_program",
                PrototypeType = 0,
                AllOrNothing = true,
                PenaltyRegime = "10, 20, ...",
                Precheck = 0,
                HideCheck = 0,
                ShowSource = 0,
                AnswerBoxLines = 18,
                AnswerBoxColumns = 100,
                AnswerPreload = "",
                GlobalExtra = "",
                Answer = "public class Main {\n    public static void main(String[] args) {\n        System.out.println(\"Hello, World!\");\n    }\n}",
                ValidateOnSave = true,
                TemplateParams = "",
                HoistTemplateParams = true,
                ExtractCodeFromJson = true,
                TemplateParamsLang = "None",
                TemplateParamsEvalPerTry = false,
                TemplateParamsEvald = "{}",
                TwigAll = false,
                UiParameters = "",
                Attachments = 0,
                AttachmentsRequired = 0,
                MaxFileSize = 10240,
                FilenamesRegex = "",
                FilenamesExplain = "",
                DisplayFeedback = true,
                GiveUpAllowed = false,
                TestCases = new List<CodeRunnerTestCase>
                {
                    new CodeRunnerTestCase
                    {
                        Id = 1,
                        TestCode = "Hello World Test",
                        TestType = 0,
                        Expected = "Hello, World!",
                        UseAsExample = false,
                        Display = "SHOW",
                        HideRestIfFail = false,
                        Mark = 1.000m,
                        StdIn = "",
                        Extra = "",
                    },
                    new CodeRunnerTestCase
                    {
                        Id = 2,
                        TestCode = "Empty Input Test",
                        TestType = 0,
                        Expected = "Hello, World!",
                        UseAsExample = true,
                        Display = "HIDE_IF_FAIL",
                        HideRestIfFail = true,
                        Mark = 0.500m,
                        StdIn = "",
                        Extra = "Some extra data",
                    },
                },
                Hints = null,
                Tags = new List<string> { $"Tag_CR_{id}" },
            };

            if (withHints)
            {
                crq.Hints = new List<Hint>
                {
                    new Hint
                    {
                        HintText = "CodeRunner Hint 1",
                        HintTextFormat = FormattingFormat.HTML,
                        ShowNumberOfCorrectAnswers = true,
                        ClearIncorrectResponses = false,
                    },
                    new Hint
                    {
                        HintText = "CodeRunner Hint 2",
                        HintTextFormat = FormattingFormat.PlainText,
                        ShowNumberOfCorrectAnswers = false,
                        ClearIncorrectResponses = true,
                    },
                };
            }

            return crq;
        }


        /// <summary>
        /// Creates a basic drag-and-drop text question with specified parameters
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <param name="withHints">Indicates whether the question should include hints</param>
        /// <param name="withFeedback">Indicates whether the question should include feedback for responses</param>
        /// <returns>A <see cref="DragAndDropTextQuestion"/> object initialized with specified settings and default structure</returns>
        public static DragAndDropTextQuestion CreateBasicDragDropTextQuestion(int id, string category, bool withHints = false, bool withFeedback = false)
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


        /// <summary>
        /// Creates a basic true-false question with specified parameters
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <param name="withFeedback">Indicates whether the question should include feedback for responses</param>
        /// <returns>A <see cref="TrueFalseQuestion"/> object initialized with specified settings and default structure</returns>
        public static TrueFalseQuestion CreateBasicTrueFalseQuestion(int id, string category, bool withFeedback = false)
        {
            var trueFalseQuestion = new TrueFalseQuestion
            {
                QuestionId = id,
                QuestionName = "DavidsGreatness",
                QuestionText = "Is David the greatest person to have ever lived?",
                QuestionTextFormat = FormattingFormat.PlainText,
                DefaultMark = 1,
                CorrectAnswer = true,
                Answers = new List<MultiChoiceAnswer>
                {
                    new MultiChoiceAnswer
                    {
                        AnswerText = "True",
                        AnswerTextFormat = FormattingFormat.PlainText,
                        AnswerGrade = 1.0f,
                        AnswerFeedback = withFeedback ? "Correct! David is indeed the greatest person to have ever lived." : string.Empty,
                        AnswerFeedbackFormat = FormattingFormat.PlainText,
                    },
                    new MultiChoiceAnswer
                    {
                        AnswerText = "False",
                        AnswerTextFormat = FormattingFormat.PlainText,
                        AnswerGrade = 0.0f,
                        AnswerFeedback = withFeedback ? "Incorrect! David is indeed the greatest person to have ever lived." : string.Empty,
                        AnswerFeedbackFormat = FormattingFormat.PlainText,
                    },
                },
                Category = category,
            };

            return trueFalseQuestion;
        }

        /// <summary>
        /// Creates a basic short answer question with predefined structure and configuration
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <param name="withHints">Indicates whether the question includes hints</param>
        /// <param name="withFeedback">Indicates whether the question provides general feedback</param>
        /// <returns>A <see cref="ShortAnswerQuestion"/> object with initialized values and structure</returns>
        public static ShortAnswerQuestion CreateBasicShortAnswerQuestion(int id, string category, bool withHints = false)
        {
            var saq = new ShortAnswerQuestion
            {
                QuestionId = id,
                Category = category,
                QuestionName = $"ShortAnswer_{id}",
                QuestionText = $"<p>Short Answer Text {id}</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                GeneralFeedback = $"General Feedback SA {id}",
                GeneralFeedbackFormat = FormattingFormat.HTML,
                DefaultMark = 1.0f,
                HintPenalty = 0.33f,
                UseCase = false,
                QuestionReady = true,
                Answers = new List<ShortAnswerOptions>
                {
                    new ShortAnswerOptions
                    {
                        AnswerText = "Correct Answer",
                        AnswerTextFormat = FormattingFormat.PlainText,
                        AnswerGrade = 1.0f,
                        AnswerFeedback = "Excellent!",
                        AnswerFeedbackFormat = FormattingFormat.PlainText,
                    },
                    new ShortAnswerOptions
                    {
                        AnswerText = "Almost Correct",
                        AnswerTextFormat = FormattingFormat.PlainText,
                        AnswerGrade = 0.0f,
                        AnswerFeedback = "Close, but not perfect.",
                        AnswerFeedbackFormat = FormattingFormat.PlainText,
                    },
                    new ShortAnswerOptions
                    {
                        AnswerText = "*", 
                        AnswerTextFormat = FormattingFormat.PlainText,
                        AnswerGrade = 0.0f,
                        AnswerFeedback = "Wrong answer.",
                        AnswerFeedbackFormat = FormattingFormat.PlainText,
                    },
                },
                Hints = null, // Hints are added below
                Tags = new List<string> { $"TagSA{id}" },
            };

            if (withHints)
            {
                saq.Hints = new List<Hint>
                {
                    new Hint
                    {
                        HintText = "SA Hint 1",
                        HintTextFormat = FormattingFormat.HTML,
                        ShowNumberOfCorrectAnswers = true,
                        ClearIncorrectResponses = false,
                    },
                    new Hint
                    {
                        HintText = "SA Hint 2",
                        HintTextFormat = FormattingFormat.PlainText,
                        ShowNumberOfCorrectAnswers = false,
                        ClearIncorrectResponses = true,
                    },
                };
            }

            return saq;
        }

        /// <summary>
        /// Creates a basic essay question with predefined structure and configuration
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <param name="withHints">Indicates whether the question includes hints</param>
        /// <param name="withFeedback">Indicates whether the question provides general feedback</param>
        /// <returns>A <see cref="EssayaQuestion"/> object with initialized values and structure</returns>
        public static EssayQuestion CreateBasicEssayQuestion(int id, string category, bool withHints = false)
        {
            var eq = new EssayQuestion
            {
                QuestionId = id,
                Category = category,
                QuestionName = $"Essay_{id}",
                QuestionText = $"<p>Essay Text {id}</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                GeneralFeedback = $"General Feedback SA {id}",
                GeneralFeedbackFormat = FormattingFormat.HTML,
                DefaultMark = 1.0f,
                HintPenalty = 0.33f,
                QuestionReady = true,
                ResponseFormat = "editor",
                ResponseRequired = true,
                ResponseFieldLines = 20,
                MinWordLimit = 3,
                MaxWordLimit = 300,
                AttachmentsAllowed = 1,
                AttachmentsRequired = 0,
                FileTypesList = ".pdf",
                MaxBytes = 0,
                GraderInfo = "Make sure that the person is passionate",
                GraderInfoFormat = FormattingFormat.Markdown,
                ResponseTemplate = "Start your essay here...",
                ResponseTemplateFormat = FormattingFormat.Markdown,
                Tags = new List<string> { $"TagSA{id}" },
            };

            return eq;
        }

        /// <summary>
        /// Creates a basic simple calculated question with predefined datasets, units and tolerance settings.
        /// </summary>
        /// <param name="id">The unique identifier for the question</param>
        /// <param name="category">The category to which the question belongs</param>
        /// <returns>A <see cref="SimpleCalculatedQuestion"/> object initialized with default values</returns>
        public static SimpleCalculatedQuestion CreateBasicSimpleCalculatedQuestion(int id, string category)
        {
            var scq = new SimpleCalculatedQuestion
            {
                QuestionId = id,
                Category = category,
                QuestionName = $"SimpleCalc_{id}",
                QuestionText = "<p>Calculate the sum of {a} and {b}.</p>",
                QuestionTextFormat = FormattingFormat.HTML,
                GeneralFeedback = "General Feedback",
                GeneralFeedbackFormat = FormattingFormat.HTML,
                DefaultMark = 1.0f,
                QuestionReady = true,

                NumericalOptions = new CalculatedHelper.NumericalOption
                {
                    ShowUnits = 3,
                    UnitsLeft = false,
                    UnitGradingType = 1,
                    UnitPenalty = 0.1f,
                },

                NumericalUnits = new List<CalculatedHelper.NumericalUnit>
                {
                    new CalculatedHelper.NumericalUnit { UnitName = "cm", Multiplier = 1.0f },
                    new CalculatedHelper.NumericalUnit { UnitName = "m", Multiplier = 0.01f },
                },

                DatasetDefinitions = new List<CalculatedHelper.DatasetDefinition>
                {
                    new CalculatedHelper.DatasetDefinition
                    {
                        Name = "a",
                        Category = 0,
                        Type = 1,
                        Options = "uniform:1.0:10.0:1",
                        Items = new List<CalculatedHelper.DatasetItem>
                        {
                            new CalculatedHelper.DatasetItem { Number = 1, Value = 5.0f },
                            new CalculatedHelper.DatasetItem { Number = 2, Value = 3.5f },
                        },
                    },
                    new CalculatedHelper.DatasetDefinition
                    {
                        Name = "b",
                        Category = 0,
                        Type = 1,
                        Options = "uniform:1.0:10.0:1",
                        Items = new List<CalculatedHelper.DatasetItem>
                        {
                            new CalculatedHelper.DatasetItem { Number = 1, Value = 2.0f },
                            new CalculatedHelper.DatasetItem { Number = 2, Value = 6.1f },
                        },
                    },
                },

                Answers = new List<SimpleCalculatedAnswer>
                {
                    new SimpleCalculatedAnswer
                    {
                        AnswerText = "{a} + {b}",
                        Fraction = 1.0f,
                        AnswerTextFormat = FormattingFormat.PlainText,
                        Feedback = "Well done!",
                        FeedbackFormat = FormattingFormat.HTML,
                    },
                },

                CalculatedRecords = new List<CalculatedHelper.CalculatedRecord>
                {
                    new CalculatedHelper.CalculatedRecord
                    {
                        Tolerance = 0.01f,
                        ToleranceType = 1,
                        CorrectAnswerLength = 2,
                        CorrectAnswerFormat = 1,
                    },
                },

                Tags = new List<string> { $"TagSimpleCalc{id}" },
            };

            return scq;
        }
    }
}
