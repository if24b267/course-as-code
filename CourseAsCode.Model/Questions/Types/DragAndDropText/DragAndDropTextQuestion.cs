namespace CourseAsCode.Model.Questions.Types.DragAndDropText
{
    public class DragAndDropTextQuestion : Question
    {
        public required List<TextChoice> Answers { get; init; } = new();
    }
}
