using System.ComponentModel.DataAnnotations;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlSubQuestion
    {
        [Required]
        public string SubQuestionText { get; set; }
        public FormattingFormat SubQuestionTextFormat { get; set; } = FormattingFormat.PlainText;
        public string CorrectAnswer { get; set; }
        public List<string>? WrongAnswers { get; set; }


    }
}
