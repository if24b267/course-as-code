using CourseAsCode.Model.Activities.Resources;
using CourseAsCode.Model.Questions;
using CourseAsCode.Readers.Helper;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlQuizReader : IQuizReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlFileReaderHelper;

        public YamlQuizReader(IYamlFileReaderHelper yamlFileReaderHelper)
        {
            _yamlFileReaderHelper = yamlFileReaderHelper;
        }

        public Quiz? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            var yamlQuiz = _yamlFileReaderHelper.DeserializeFromFile<YamlQuiz>(modelObjectPath);

            var moodleQuiz = ConvertYamlQuizToMoodleQuiz(yamlQuiz, modelObjectPath);

            var grading = ValueKeeper.GradingInfos.FirstOrDefault(gi => gi.PathToActivity == modelObjectPath);

            if (grading != null)
            {
                moodleQuiz = AddGradingToMoodleQuiz(moodleQuiz, grading);
            }

            return moodleQuiz;
        }

        private Quiz ConvertYamlQuizToMoodleQuiz(YamlQuiz yamlQuiz, string modelObjectPath)
        {
            List<Question> questions = GetQuestionsFromValueKeeperByRefs(yamlQuiz.Questions, modelObjectPath);
            return new Quiz()
            {
                ActivityId = ValueKeeper.CurrentActivityId,
                Name = yamlQuiz.Name,
                Description = yamlQuiz.Description,
                OpenTime = yamlQuiz.OpeningTime,
                CloseTime = yamlQuiz.ClosingTime,
                TimeLimit = yamlQuiz.TimeLimit,
                TimeExpiryHandling = yamlQuiz.TimeExpiryHandling,
                AllowedAttempts = yamlQuiz.AllowedAttempts,
                GradeToPass = yamlQuiz.GradeToPass,
                BuildOnPreviousAttempts = yamlQuiz.BuildOnPreviousAttempts,
                GradingMethod = yamlQuiz.GradingMethod,
                QuestionBehaviour = yamlQuiz.QuestionBehaviour,
                ShuffleQuestions = yamlQuiz.ShuffleQuestions,
                AllowRedosDuringAttempt = yamlQuiz.AllowRedosDuringAttempt,
                Password = yamlQuiz.Password,
                Questions = questions,

                ModuleId = ValueKeeper.CurrentModuleId,
                ModuleName = "quiz",
            };
        }

        private Quiz AddGradingToMoodleQuiz(Quiz moodleQuiz, YamlGradingInfo gradingInfo)
        {
            var gradeCategory =
                ValueKeeper.GradeCategories.FirstOrDefault(gc => gc.GradeCategoryId == gradingInfo.GradeCategoryId);

            if (gradeCategory == null)
            {
                return moodleQuiz;
            }

            moodleQuiz.GradingLocked = gradingInfo.GradingLocked;
            moodleQuiz.GradeName = gradingInfo.GradeName;
            moodleQuiz.GradeMultiplicator = gradingInfo.GradeMultiplicator;
            moodleQuiz.GradingWeight = gradingInfo.GradingWeight;
            moodleQuiz.GradeToPass = gradingInfo.GradeToPass;
            moodleQuiz.GradeHiddenUntil = gradingInfo.GradeHiddenUntil;
            moodleQuiz.GradeOffset = gradingInfo.GradeOffset;
            moodleQuiz.GradeHidden = gradingInfo.GradeHidden;
            moodleQuiz.GradeCategory = gradeCategory;

            return moodleQuiz;
        }

        private List<Question> GetQuestionsFromValueKeeperByRefs(List<YamlQuestionReference>? refs, string modelObjectPath)
        {
            if (refs == null) return new List<Question>();

            var quizDir = Path.GetDirectoryName(modelObjectPath);
            if (quizDir == null) return new List<Question>();

            var resolvedQuestions = new List<Question>();
            foreach (var r in refs)
            {
                var targetPath = Path.GetFullPath(Path.Combine(quizDir, r.Ref + ".yaml"));

                var match = ValueKeeper.Questions.FirstOrDefault(vq => 
                    vq.SourcePath != null && vq.SourcePath.Equals(targetPath, StringComparison.OrdinalIgnoreCase));
                
                if (match != null)
                {
                    resolvedQuestions.Add(match);
                }
                else
                {
                    _logger.Warn($"Could not find question matching reference path: {targetPath}");
                }
            }
            return resolvedQuestions;
        }
    }
}
