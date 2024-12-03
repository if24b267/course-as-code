namespace CourseAsCode.Model.Activities.Quiz.QuizQuestions;

public class CodeRunnerQuestion
{
	public string Code { get; set; }
	public string Language { get; set; }
	public List<CodeRunnerTestCase> TestCases { get; set; }
}