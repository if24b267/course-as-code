namespace CourseAsCode.Model.Activities.Quiz
{
	public abstract class QuizQuestion
	{
		public int Id { get; set; }
		// enum?
		public string QuestionType { get; set; }
		public string Title { get; set; }
	}
}