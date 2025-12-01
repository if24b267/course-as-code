using CourseAsCode.Model.Grading;
using CourseAsCode.Readers.Interfaces;
using CourseAsCode.Readers.YAML.Model;
using YamlDotNet.Core;

namespace CourseAsCode.Readers.YAML.Readers
{
    public class YamlIGradeCategoriesReader : IGradeCategoriesReader
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IYamlFileReaderHelper _yamlReaderHelper;

        public List<GradeCategory>? Read(string modelObjectPath)
        {
            if (!File.Exists(modelObjectPath))
            {
                _logger.Error($"File {modelObjectPath} does not exist.");
                return null;
            }

            try
            {
                // DeserializeFromContent YAML to object
                var yamlGradeCategories = _yamlReaderHelper.DeserializeFromFile<YamlGradeCategories>(modelObjectPath);

                // Validate the URL format
                var validationResults = _yamlReaderHelper.ValidateObject(yamlGradeCategories);
                if (validationResults.Any())
                {
                    var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                    _logger.Warn($"Validation warnings for URL '{modelObjectPath}': {errors}");
                }

                var moodleGradeCategories = ConvertYamlGradeCategoriesToMoodleGradeCategories(yamlGradeCategories);
                return moodleGradeCategories;
            }
            catch (YamlException ex)
            {
                _logger.Error(ex, $"Error deserializing URL from '{modelObjectPath}': {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unexpected error reading URL from '{modelObjectPath}': {ex.Message}");
                return null;
            }
        }

        private List<GradeCategory> ConvertYamlGradeCategoriesToMoodleGradeCategories(YamlGradeCategories yamlGradeCategories)
        {
            List<GradeCategory> moodleGradeCategories = [];
            foreach (var yamlGradeCategory in yamlGradeCategories.GradeCategories)
            {
                GradeCategory? parent =
                    moodleGradeCategories.FirstOrDefault(g => g.GradeCategoryId == yamlGradeCategory.ParentGradeCategoryId);
                var moodleGradeCategory = new GradeCategory()
                {
                    GradeCategoryId = yamlGradeCategory.GradeCategoryId,
                    AggregationType = yamlGradeCategory.AggregrationType,
                    Parent = parent,
                    DropLowestGrades = yamlGradeCategory.DropLowestGrades,
                    GradeName = yamlGradeCategory.GradeName,
                    MaxGrade = yamlGradeCategory.MaxGrade,
                    GradeToPass = yamlGradeCategory.GradeToPass,
                    GradingWeight = yamlGradeCategory.GradingWeight,
                    GradeHidden = yamlGradeCategory.GradeHidden,
                    GradingLocked = yamlGradeCategory.GradingLocked
                };
                moodleGradeCategories.Add(moodleGradeCategory);
            }
            return moodleGradeCategories;
        }
    }
}
