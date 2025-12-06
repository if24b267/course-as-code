using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CourseAsCode.Model.Questions.Types.MultipleChoice;

namespace CourseAsCode.Model.Questions.Types.TrueFalse
{
    public class TrueFalseQuestion : Question
    {
        [Description("Set true if 'true' is the correct answer, false if 'false' is the correct answer")]
        public bool CorrectAnswer { get; set; }

        [Description("List of possible answers, must be exactly 2 | The first answer will be the 'true' option, the second the 'false' option")]
        [MinLength(2, ErrorMessage = "True/False questions must have exactly 2 answers.")]
        [MaxLength(2, ErrorMessage = "True/False questions must have exactly 2 answers.")]
        public List<MultiChoiceAnswer> Answers { get; set; } = new();

        [Description("Show Moodle’s standard instructions (e.g. ‘Select one or more:’)")]
        public bool ShowStandardInstructions { get; set; } = true;
    }
}
