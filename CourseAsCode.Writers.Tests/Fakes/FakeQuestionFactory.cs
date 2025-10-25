using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Enums.Quizzes;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;

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
    }
}
