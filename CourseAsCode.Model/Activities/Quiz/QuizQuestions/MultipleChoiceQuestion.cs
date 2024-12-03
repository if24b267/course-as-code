namespace CourseAsCode.Model.Activities.Quiz.QuizQuestions;

public class MultipleChoiceQuestion : QuizQuestion
{
	public List<string> Options { get; set; }
	// todo: couldnt there be multiple true answers - is it a single choice multiple choice or with multiple possible answers
	public string CorrectAnswer { get; set; }
}