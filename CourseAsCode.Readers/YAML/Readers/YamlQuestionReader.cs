using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Readers.Enums;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlQuestionReader(IYamlFileReaderHelper fileReaderHelper) : IQuestionReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public List<Question>? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            var baseDir = Path.GetDirectoryName(modelObjectPath);

            var questionList = fileReaderHelper.DeserializeFromFile<YamlQuestionList>(modelObjectPath);

            var finalQuestionList = new List<Question>();

            foreach (YamlQuestion yamlQuestion in questionList.Questions)
            {
                if (yamlQuestion.QuestionType == QuestionType.CODERUNNER)
                {
                    yamlQuestion.QuestionText = PathUtils.InlineIfFile(yamlQuestion.QuestionText, baseDir);
                    yamlQuestion.AnswerPreload = PathUtils.InlineIfFile(yamlQuestion.AnswerPreload, baseDir);
                    yamlQuestion.Answer = PathUtils.InlineIfFile(yamlQuestion.Answer, baseDir);

                    foreach (YamlCodeRunnerTestCase yamlQuestionCodeRunnerTestCase in yamlQuestion.CodeRunnerTestCases)
                    {
                        if (yamlQuestion.CodeRunnerTestCases == null)
                        {
                            continue;
                        }

                        yamlQuestionCodeRunnerTestCase.Expected =
                            PathUtils.InlineIfFile(yamlQuestionCodeRunnerTestCase.Expected, baseDir);
                        yamlQuestionCodeRunnerTestCase.StdIn =
                            PathUtils.InlineIfFile(yamlQuestionCodeRunnerTestCase.StdIn, baseDir);
                    }
                }
                var question = ConvertYamlQuestionToMoodleQuestion(yamlQuestion);
                if (finalQuestionList.Any(questionFromList => questionFromList.QuestionId == question.QuestionId))
                {
                    _logger.Error($"Question with ID {question.QuestionId} already exists in the list.");
                    // skip if the question id already exists
                    continue;
                }
                finalQuestionList.Add(question);
            }

            return finalQuestionList;
        }

        private Question? ConvertYamlQuestionToMoodleQuestion(YamlQuestion yamlQuestion)
        {
            // TODO: add some kind of verification that all required properties are set for that question type (e.g. coderunner -> coderunnertype)
            switch (yamlQuestion.QuestionType)
            {
                case QuestionType.SINGLECHOICE:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to SingleChoiceQuestion");
                    return ConvertYamlQuestionToSingleChoiceQuestion(yamlQuestion);
                case QuestionType.MULTIPLECHOICE:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to MultipleChoiceQuestion");
                    return ConvertYamlQuestionToMultipleChoiceQuestion(yamlQuestion);
                case QuestionType.DROPDOWN:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to DropdownQuestion");
                    return ConvertYamlQuestionToDropdownQuestion(yamlQuestion);
                case QuestionType.DRAGANDDROPTEXT:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to DragAndDropTextQuestion");
                    return ConvertYamlQuestionToDragAndDropTextQuestion(yamlQuestion);
                case QuestionType.CODERUNNER:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to DragAndDropTextQuestion");
                    return ConvertYamlQuestionToCodeRunnerQuestion(yamlQuestion);
                default:
                    _logger.Error($"Question type {yamlQuestion.QuestionType} is not supported.");
                    break;
            }
            return null;
        }

        private CodeRunnerQuestion ConvertYamlQuestionToCodeRunnerQuestion(YamlQuestion yamlQuestion)
        {
            var question = new CodeRunnerQuestion
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                Category = yamlQuestion.Category,
                QuestionId = yamlQuestion.QuestionId,
                DefaultMark = yamlQuestion.DefaultMark,
                CodeRunnerType = yamlQuestion.CodeRunnerType,
                AllOrNothing = yamlQuestion.AllOrNothing,
                Precheck = yamlQuestion.Precheck,
                HideCheck = yamlQuestion.HideCheck ? 1 : 0,
                ShowSource = yamlQuestion.ShowSource ? 1 : 0,
                AnswerBoxLines = yamlQuestion.AnswerBoxLines,
                AnswerBoxColumns = yamlQuestion.AnswerBoxColumns,
                AnswerPreload = yamlQuestion.AnswerPreload,
                Answer = yamlQuestion.Answer,
                ValidateOnSave = yamlQuestion.ValidateOnSave,
                DisplayFeedback = yamlQuestion.DisplayFeedback,
                GiveUpAllowed = yamlQuestion.GiveUpAllowed,
                TestCases = ConvertYamlCodeRunnerTestCasesToCodeRunnerTestCases(yamlQuestion.CodeRunnerTestCases)
            };

            return question;
        }

        public List<CodeRunnerTestCase> ConvertYamlCodeRunnerTestCasesToCodeRunnerTestCases(List<YamlCodeRunnerTestCase> yamlCodeRunnerTestCases)
        {
            return yamlCodeRunnerTestCases.Select(yamlTestCase => new CodeRunnerTestCase()
            {
                Id = yamlTestCase.Id,
                TestCode = yamlTestCase.TestCode,
                TestType = yamlTestCase.TestType,
                Expected = yamlTestCase.Expected,
                UseAsExample = yamlTestCase.UseAsExample,
                Display = yamlTestCase.Display,
                HideRestIfFail = yamlTestCase.HideRestIfFail,
                Mark = yamlTestCase.Mark,
                StdIn = yamlTestCase.StdIn,
                Extra = yamlTestCase.Extra
            }).ToList();
        }

        private DragAndDropTextQuestion ConvertYamlQuestionToDragAndDropTextQuestion(YamlQuestion yamlQuestion)
        {
            var question = new DragAndDropTextQuestion
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                Category = yamlQuestion.Category,
                Answers = ConvertYamlDragDropAnswerToMoodleTextChoice(yamlQuestion.DragDropAnswers),
                QuestionId = yamlQuestion.QuestionId,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                DefaultMark = yamlQuestion.DefaultMark,
                QuestionReady = yamlQuestion.QuestionReady,
                HintPenalty = yamlQuestion.HintPenalty,
                ShuffleAnswers = yamlQuestion.ShuffleAnswers,
                CorrectResponseFeedback = yamlQuestion.CorrectResponseFeedback,
                CorrectResponseFeedbackFormat = yamlQuestion.CorrectResponseFeedbackFormat,
                PartiallyCorrectResponseFeedback = yamlQuestion.PartiallyCorrectResponseFeedback,
                PartiallyCorrectResponseFeedbackFormat = yamlQuestion.PartiallyCorrectResponseFeedbackFormat,
                IncorrectResponseFeedback = yamlQuestion.IncorrectResponseFeedback,
                IncorrectResponseFeedbackFormat = yamlQuestion.IncorrectResponseFeedbackFormat,
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints)
            };

            return question;
        }

        private List<TextChoice> ConvertYamlDragDropAnswerToMoodleTextChoice(List<YamlDragDropAnswerText> yamlDragDropAnswers)
        {
            return yamlDragDropAnswers.Select(yamlDragDropAnswer => new TextChoice()
            {
                ID = yamlDragDropAnswer.Id,
                Value = yamlDragDropAnswer.Text,
                ValueFormat = yamlDragDropAnswer.TextFormat ?? FormattingFormat.PlainText,
                Category = yamlDragDropAnswer.Category,
            })
                .ToList();
        }

        private DropdownQuestion ConvertYamlQuestionToDropdownQuestion(YamlQuestion yamlQuestion)
        {
            var question = new DropdownQuestion
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                Category = yamlQuestion.Category,
                QuestionId = yamlQuestion.QuestionId,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                DefaultMark = yamlQuestion.DefaultMark,
                QuestionReady = yamlQuestion.QuestionReady,
                HintPenalty = yamlQuestion.HintPenalty,
                CorrectResponseFeedback = yamlQuestion.CorrectResponseFeedback,
                CorrectResponseFeedbackFormat = yamlQuestion.CorrectResponseFeedbackFormat,
                PartiallyCorrectResponseFeedback = yamlQuestion.PartiallyCorrectResponseFeedback,
                PartiallyCorrectResponseFeedbackFormat = yamlQuestion.PartiallyCorrectResponseFeedbackFormat,
                IncorrectResponseFeedback = yamlQuestion.IncorrectResponseFeedback,
                IncorrectResponseFeedbackFormat = yamlQuestion.IncorrectResponseFeedbackFormat,
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints),
                SubQuestions = ConvertYamlSubQuestionListToMoodleSubQuestionList(yamlQuestion.SubQuestions)
            };

            return question;
        }

        private List<DropdownSubQuestion> ConvertYamlSubQuestionListToMoodleSubQuestionList(List<YamlSubQuestion> yamlSubQuestions)
        {
            return yamlSubQuestions.Select(yamlSubQuestion => new DropdownSubQuestion()
                {
                    QuestionText = yamlSubQuestion.SubQuestionText, QuestionTextFormat = yamlSubQuestion.SubQuestionTextFormat, CorrectAnswer = yamlSubQuestion.CorrectAnswer, WrongAnswers = yamlSubQuestion.WrongAnswers ?? [],
                })
                .ToList();
        }

        private SingleChoiceQuestion ConvertYamlQuestionToSingleChoiceQuestion(YamlQuestion yamlQuestion)
        {
            var question = new SingleChoiceQuestion
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                Category = yamlQuestion.Category,
                QuestionId = yamlQuestion.QuestionId,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                ShuffleAnswers = yamlQuestion.ShuffleAnswers,
                DefaultMark = yamlQuestion.DefaultMark,
                QuestionReady = yamlQuestion.QuestionReady,
                Answers = GetMoodleAnswersFromYamlAnswers(yamlQuestion.Answers),
                HintPenalty = yamlQuestion.HintPenalty,
                CorrectResponseFeedback = yamlQuestion.CorrectResponseFeedback,
                CorrectResponseFeedbackFormat = yamlQuestion.CorrectResponseFeedbackFormat,
                PartiallyCorrectResponseFeedback = yamlQuestion.PartiallyCorrectResponseFeedback,
                PartiallyCorrectResponseFeedbackFormat = yamlQuestion.PartiallyCorrectResponseFeedbackFormat,
                IncorrectResponseFeedback = yamlQuestion.IncorrectResponseFeedback,
                IncorrectResponseFeedbackFormat = yamlQuestion.IncorrectResponseFeedbackFormat,
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints)
            };

            return question;
        }

        private MultipleChoiceQuestion ConvertYamlQuestionToMultipleChoiceQuestion(YamlQuestion yamlQuestion)
        {
            var question = new MultipleChoiceQuestion
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                Category = yamlQuestion.Category,
                QuestionId = yamlQuestion.QuestionId,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                ShuffleAnswers = yamlQuestion.ShuffleAnswers,
                DefaultMark = yamlQuestion.DefaultMark,
                QuestionReady = yamlQuestion.QuestionReady,
                Answers = GetMoodleAnswersFromYamlAnswers(yamlQuestion.Answers),
                HintPenalty = yamlQuestion.HintPenalty,
                CorrectResponseFeedback = yamlQuestion.CorrectResponseFeedback,
                CorrectResponseFeedbackFormat = yamlQuestion.CorrectResponseFeedbackFormat,
                PartiallyCorrectResponseFeedback = yamlQuestion.PartiallyCorrectResponseFeedback,
                PartiallyCorrectResponseFeedbackFormat = yamlQuestion.PartiallyCorrectResponseFeedbackFormat,
                IncorrectResponseFeedback = yamlQuestion.IncorrectResponseFeedback,
                IncorrectResponseFeedbackFormat = yamlQuestion.IncorrectResponseFeedbackFormat,
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints)
            };

            return question;
        }

        private List<Hint> ConvertYamlHintListToMoodleHintList(List<YamlHint> yamlHints)
        {
            List<Hint> hints = new List<Hint>();
            foreach (YamlHint yamlHint in yamlHints)
            {
                hints.Add(new Hint()
                {
                    ClearIncorrectResponses = yamlHint.ClearIncorrectResponses,
                    HintText = yamlHint.HintText,
                    HintTextFormat = yamlHint.HintTextFormat,
                    ShowNumberOfCorrectAnswers = yamlHint.ShowNumberOfCorrectAnswers,
                });
            }

            return hints;
        }

        private List<MultiChoiceAnswer> GetMoodleAnswersFromYamlAnswers(List<YamlQuestionAnswer> yamlAnswers)
        {
            return yamlAnswers.Select(yamlAnswer => ConvertYamlAnswerToMoodleAnswer(yamlAnswer)).ToList();
        }

        private MultiChoiceAnswer ConvertYamlAnswerToMoodleAnswer(YamlQuestionAnswer questionAnswer)
        {
            return new MultiChoiceAnswer()
            {
                AnswerText = questionAnswer.AnswerText,
                AnswerTextFormat = questionAnswer.AnswerTextFormat,
                AnswerGrade = questionAnswer.AnswerGrade,
                AnswerFeedback = questionAnswer.Feedback,
                AnswerFeedbackFormat = questionAnswer.FeedbackFormat
            };
        }
    }
}
