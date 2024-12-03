namespace CourseAsCode.Model.Activities.Quiz;

public class Quiz
{
	// TODO: shouldn't we convert all dates into proper dateTimes?
	public string OpenDate { get; set; }
	public string CloseDate { get; set; }
	public int TimeLimit { get; set; }
	public int Grade { get; set; }
	// TODO: what more does this need?
	public List<QuizQuestion> Questions { get; set; }
}