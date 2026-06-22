using CourseAsCode.Model.Defaults.Activities.Resources;
using CourseAsCode.Model.Enums.Quizzes;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlQuiz : YamlActivity
    {
        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }
        public int TimeLimit { get; set; } = QuizDefaults.DEFAULT_TIME_LIMIT;
        public TimeExpiryHandle TimeExpiryHandling { get; set; } = QuizDefaults.DEFAULT_TIME_EXPIRY_HANDLING;
        public int AllowedAttempts { get; set; } = QuizDefaults.DEFAULT_ALLOWED_ATTEMPTS;
        public bool AllowRedosDuringAttempt { get; set; } = QuizDefaults.DEFAULT_ALLOW_REDOS_DURING_ATTEMPT;
        public bool BuildOnPreviousAttempts { get; set; } = QuizDefaults.DEFAULT_BUILD_ON_PREVIOUS_ATTEMPTS;
        public GradingMethod GradingMethod { get; set; } = QuizDefaults.DEFAULT_GRADING_METHOD;
        public float GradeToPass { get; set; } = QuizDefaults.DEFAULT_GRADE_TO_PASS;
        public QuestionBehaviour QuestionBehaviour { get; set; } = QuizDefaults.DEFAULT_QUESTION_BEHAVIOUR;
        public bool ShuffleQuestions { get; set; } = QuizDefaults.DEFAULT_SHUFFLE_QUESTIONS;
        public string? Password { get; set; }
        public List<YamlQuestionReference>? Questions { get; set; }
    }

    public class YamlQuestionReference
    {
        public required string Ref { get; set; }
    }
}
