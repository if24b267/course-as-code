using CourseAsCode.Model.Enums;
using CourseAsCode.Model.Questions;
using CourseAsCode.Model.Questions.Helper;
using CourseAsCode.Model.Questions.Types;
using CourseAsCode.Model.Questions.Types.DragAndDropText;
using CourseAsCode.Model.Questions.Types.Dropdown;
using CourseAsCode.Readers.Enums;
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

            var questionList = fileReaderHelper.DeserializeFromFile<YamlQuestionList>(modelObjectPath);

            var finalQuestionList = new List<Question>();

            foreach (YamlQuestion yamlQuestion in questionList.Questions)
            {
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
                default:
                    _logger.Error($"Question type {yamlQuestion.QuestionType} is not supported.");
                    break;
            }
            return null;
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
