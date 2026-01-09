using System.ComponentModel;
using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.Calculated;
using CourseAsCode.Model.Questions.Types.CalculatedSimple;
using CourseAsCode.Model.Questions.Types.CodeRunner;
using CourseAsCode.Model.Questions.Types.Description;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Model.Questions.Types.Essay;
using CourseAsCode.Model.Questions.Types.MultipleChoice;
using CourseAsCode.Model.Questions.Types.ShortAnswer;
using CourseAsCode.Model.Questions.Types.TrueFalse;
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
                case QuestionType.TRUEFALSE:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to TrueFalseQuestion");
                    return ConvertYamlQuestionToTrueFalseQuestion(yamlQuestion);
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
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to CoderunnerQuestion");
                    return ConvertYamlQuestionToCodeRunnerQuestion(yamlQuestion);
                case QuestionType.DESCRIPTION:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to DescriptionQuestion");
                    return ConvertYamlQuestionToDescriptionQuestion(yamlQuestion);
                case QuestionType.ESSAY:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to EssayQuestion");
                    return ConvertYamlQuestionToEssayQuestion(yamlQuestion);
                case QuestionType.SHORTANSWER:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to ShortAnswerQuestion");
                    return ConvertYamlQuestionToShortAnswerQuestion(yamlQuestion);
                case QuestionType.SIMPLECALCULATED:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to CalculatedSimpleQuestion");
                    return ConvertYamlQuestionToSimpleCalculatedQuestion(yamlQuestion);
                case QuestionType.CALCULATED:
                    _logger.Debug($"Converting question {yamlQuestion.QuestionName} to CalculatedQuestion");
                    return ConvertYamlQuestionToCalculatedQuestion(yamlQuestion);
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
                TestCases = ConvertYamlCodeRunnerTestCasesToCodeRunnerTestCases(yamlQuestion.CodeRunnerTestCases),
            };

            return question;
        }

        private DescriptionQuestion ConvertYamlQuestionToDescriptionQuestion(YamlQuestion yamlQuestion)
        {
            var question = new DescriptionQuestion()
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                Category = yamlQuestion.Category,
                QuestionId = yamlQuestion.QuestionId,
                DefaultMark = yamlQuestion.DefaultMark,
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
                Extra = yamlTestCase.Extra,
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
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints),
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
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints),
            };

            return question;
        }

        private TrueFalseQuestion ConvertYamlQuestionToTrueFalseQuestion(YamlQuestion yamlQuestion)
        {
            var question = new TrueFalseQuestion
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                CorrectAnswer = yamlQuestion.CorrectAnswer,
                ShowStandardInstructions = yamlQuestion.ShowStandardInstructions,
                Category = yamlQuestion.Category,
                QuestionId = yamlQuestion.QuestionId,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                DefaultMark = yamlQuestion.DefaultMark,
                QuestionReady = yamlQuestion.QuestionReady,
                Answers = GetMoodleAnswersFromYamlAnswers(yamlQuestion.Answers),
                HintPenalty = yamlQuestion.HintPenalty,
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints),
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
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints),
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
            if (yamlAnswers == null)
            {
                return [];
            }
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
                AnswerFeedbackFormat = questionAnswer.FeedbackFormat,
            };
        }

        private EssayQuestion ConvertYamlQuestionToEssayQuestion(YamlQuestion yamlQuestion)
        {
            var question = new EssayQuestion
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                Category = yamlQuestion.Category,
                QuestionId = yamlQuestion.QuestionId,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                DefaultMark = yamlQuestion.DefaultMark,
                QuestionReady = yamlQuestion.QuestionReady,
                ResponseFormat = yamlQuestion.ResponseFormat,
                ResponseRequired = yamlQuestion.ResponseRequired,
                ResponseFieldLines = yamlQuestion.ResponseFieldLines,
                MinWordLimit = yamlQuestion.MinWordLimit,
                MaxWordLimit = yamlQuestion.MaxWordLimit,
                AttachmentsAllowed = yamlQuestion.AttachmentsAllowed,
                AttachmentsRequired = yamlQuestion.AttachmentsRequired,
                FileTypesList = yamlQuestion.FileTypesList,
                MaxBytes = yamlQuestion.MaxBytes,
                GraderInfo = yamlQuestion.GraderInfo,
                GraderInfoFormat = yamlQuestion.GraderInfoFormat,
                ResponseTemplate = yamlQuestion.ResponseTemplate,
                ResponseTemplateFormat = yamlQuestion.ResponseTemplateFormat,
            };

            return question;
        }

        private ShortAnswerQuestion ConvertYamlQuestionToShortAnswerQuestion(YamlQuestion yamlQuestion)
        {
            var question = new ShortAnswerQuestion
            {
                QuestionName = yamlQuestion.QuestionName,
                QuestionText = yamlQuestion.QuestionText,
                Category = yamlQuestion.Category,
                QuestionId = yamlQuestion.QuestionId,
                QuestionTextFormat = yamlQuestion.QuestionTextFormat,
                DefaultMark = yamlQuestion.DefaultMark,
                QuestionReady = yamlQuestion.QuestionReady,
                Answers = ConvertYamlAnswersToShortAnswerOptions(yamlQuestion.Answers),
                HintPenalty = yamlQuestion.HintPenalty,
                UseCase = yamlQuestion.UseCase,
                Hints = yamlQuestion.Hints == null ? null : ConvertYamlHintListToMoodleHintList(yamlQuestion.Hints),
            };

            return question;
        }

        private List<ShortAnswerOptions> ConvertYamlAnswersToShortAnswerOptions(List<YamlQuestionAnswer> yamlAnswers)
        {
            if (yamlAnswers == null)
            {
                return new List<ShortAnswerOptions>();
            }

            return yamlAnswers.Select(yamlAnswer => new ShortAnswerOptions
            {
                AnswerText = yamlAnswer.AnswerText,
                AnswerTextFormat = yamlAnswer.AnswerTextFormat,
                AnswerGrade = yamlAnswer.AnswerGrade,     
                AnswerFeedback = yamlAnswer.Feedback,     
                AnswerFeedbackFormat = yamlAnswer.FeedbackFormat 
            }).ToList();
        }

        private SimpleCalculatedQuestion ConvertYamlQuestionToSimpleCalculatedQuestion(YamlQuestion yaml)
        {
            return new SimpleCalculatedQuestion
            {
                QuestionName = yaml.QuestionName,
                QuestionText = yaml.QuestionText,
                QuestionId = yaml.QuestionId,
                Category = yaml.Category,
                DefaultMark = yaml.DefaultMark,
                QuestionReady = yaml.QuestionReady,
                QuestionTextFormat = yaml.QuestionTextFormat,

                NumericalOptions = MapNumericalOptions(yaml.NumericalOptions),
                NumericalUnits = MapNumericalUnits(yaml.NumericalUnits),
                DatasetDefinitions = MapDatasetDefinitions(yaml.DatasetDefinitions),
                CalculatedRecords = MapCalculatedRecords(yaml.CalculatedRecords),

                Answers = ConvertYamlAnswersToCalculatedOptions(yaml.Answers),
            };
        }

        private List<CalculatedHelper.CalculatedAnswer> ConvertYamlAnswersToCalculatedOptions(List<YamlQuestionAnswer> yamlAnswers)
        {
            if (yamlAnswers == null)
            {
                return new List<CalculatedHelper.CalculatedAnswer>();
            }

            return yamlAnswers.Select(yamlAnswer => new CalculatedHelper.CalculatedAnswer
            {
                AnswerText = yamlAnswer.AnswerText,
                AnswerTextFormat = yamlAnswer.AnswerTextFormat,
                Fraction = yamlAnswer.AnswerGrade,
                Feedback = yamlAnswer.Feedback,
                FeedbackFormat = yamlAnswer.FeedbackFormat,
            }).ToList();
        }

        private CalculatedHelper.NumericalOption MapNumericalOptions(YamlNumericalOption? yamlOpt)
        {
            if (yamlOpt == null) return new CalculatedHelper.NumericalOption();

            return new CalculatedHelper.NumericalOption
            {
                ShowUnits = yamlOpt.ShowUnits,
                UnitsLeft = yamlOpt.UnitsLeft,
                UnitGradingType = yamlOpt.UnitGradingType,
                UnitPenalty = yamlOpt.UnitPenalty,
            };
        }

        private List<CalculatedHelper.NumericalUnit> MapNumericalUnits(List<YamlNumericalUnit>? yamlList)
        {
            return yamlList?.Select(u => new CalculatedHelper.NumericalUnit
            {
                UnitName = u.UnitName,
                Multiplier = u.Multiplier,
            }).ToList() ?? new();
        }

        private List<CalculatedHelper.DatasetDefinition> MapDatasetDefinitions(List<YamlDatasetDefinition>? yamlList)
        {
            return yamlList?.Select(d => new CalculatedHelper.DatasetDefinition
            {
                Name = d.Name,
                Category = d.Category,
                Type = d.Type,
                Options = d.Options,
                Items = d.Items.Select(i => new CalculatedHelper.DatasetItem
                {
                    Number = i.Number,
                    Value = i.Value,
                }).ToList(),
            }).ToList() ?? new();
        }

        private List<CalculatedHelper.CalculatedRecord> MapCalculatedRecords(List<YamlCalculatedRecord>? yamlList)
        {
            return yamlList?.Select(r => new CalculatedHelper.CalculatedRecord
            {
                Tolerance = r.Tolerance,
                ToleranceType = r.ToleranceType,
                CorrectAnswerLength = r.CorrectAnswerLength,
                CorrectAnswerFormat = r.CorrectAnswerFormat,
            }).ToList() ?? new();
        }

        private CalculatedHelper.CalculatedOption MapCalculatedOptions(YamlCalculatedOption? yamlOpt)
        {
            if (yamlOpt == null) return new CalculatedHelper.CalculatedOption();

            return new CalculatedHelper.CalculatedOption
            {
                Synchronize = yamlOpt.Synchronize,
                Single = yamlOpt.Single,
                ShuffleAnswers = yamlOpt.ShuffleAnswers,
                CorrectFeedback = yamlOpt.CorrectFeedback,
                CorrectFeedbackFormat = yamlOpt.CorrectFeedbackFormat,
                PartiallyCorrectFeedback = yamlOpt.PartiallyCorrectFeedback,
                PartiallyCorrectFeedbackFormat = yamlOpt.PartiallyCorrectFeedbackFormat,
                IncorrectFeedback = yamlOpt.IncorrectFeedback,
                IncorrectFeedbackFormat = yamlOpt.IncorrectFeedbackFormat,
                AnswerNumbering = yamlOpt.AnswerNumbering,
                ShowNumCorrect = yamlOpt.ShowNumCorrect,
            };
        }

        private CalculatedQuestion ConvertYamlQuestionToCalculatedQuestion(YamlQuestion yaml)
        {
            return new CalculatedQuestion
            {
                QuestionName = yaml.QuestionName,
                QuestionText = yaml.QuestionText,
                QuestionId = yaml.QuestionId,
                Category = yaml.Category,
                DefaultMark = yaml.DefaultMark,
                QuestionReady = yaml.QuestionReady,
                QuestionTextFormat = yaml.QuestionTextFormat,

                NumericalOptions = MapNumericalOptions(yaml.NumericalOptions),
                NumericalUnits = MapNumericalUnits(yaml.NumericalUnits),
                DatasetDefinitions = MapDatasetDefinitions(yaml.DatasetDefinitions),
                CalculatedRecords = MapCalculatedRecords(yaml.CalculatedRecords),
                CalculatedOptions = MapCalculatedOptions(yaml.CalculatedOptions),

                Answers = ConvertYamlAnswersToCalculatedOptions(yaml.Answers),
            };
        }
    }
}
