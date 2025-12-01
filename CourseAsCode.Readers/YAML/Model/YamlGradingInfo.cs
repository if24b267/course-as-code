using CourseAsCode.Model.Defaults.Activities.Resources;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlGradingInfo
    {
        public int GradeCategoryId { get; set; }
        public bool GradingLocked { get; set; } = QuizDefaults.DEFAULT_LOCKED_GRADE;
        public bool BlindMarking { get; set; } = AssignmentDefaults.DEFAULT_BLIND_MARKING;
        public string? GradeName { get; set; } = QuizDefaults.DEFAULT_GRADE_NAME;
        public float? GradeMultiplicator { get; set; } = QuizDefaults.DEFAULT_GRADE_MULTIPLICATOR;
        public float? GradingWeight { get; set; } = QuizDefaults.DEFAULT_GRADE_WEIGHT;
        public float? GradeToPass { get; set; } = QuizDefaults.DEFAULT_GRADE_TO_PASS;
        public DateTime GradeHiddenUntil { get; set; } = DateTime.Now;
        public float? GradeOffset { get; set; } = AssignmentDefaults.DEFAULT_GRADE_OFFSET;
        public bool HideGrader { get; set; } = AssignmentDefaults.DEFAULT_HIDE_GRADER;
        public string PathToActivity { get; set; }
    }
}
