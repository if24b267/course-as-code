using System.ComponentModel;

namespace CourseAsCode.Model.Questions.Types.ShortAnswer
{
    public class ShortAnswerQuestion : Question
    {
        [Description("Adjusts the case sensitivity, 0 = not sensitive, 1 = sensitive")]
        public bool UseCase { get; set; } = false;

        [Description("List of all answers")]
        public List<ShortAnswerOptions> Answers { get; set; } = new();
    }
}
