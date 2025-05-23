using CourseAsCode.Model.Enums;

namespace CourseAsCode.Readers.YAML.Model
{
    public class YamlDragDropAnswerText
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public FormattingFormat? TextFormat { get; set; } = FormattingFormat.PlainText;
        public string? Category { get; set; }
    }
}
