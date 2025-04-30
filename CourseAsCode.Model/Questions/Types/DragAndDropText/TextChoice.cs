using System.ComponentModel;
using CourseAsCode.Model.Enums;

namespace CourseAsCode.Model.Questions.Types.DragAndDropText
{
    public class TextChoice
    {
        [Description("uses to refer to the (correct) text choice in the question text")]
        public required int ID { get; set; }

        [Description("text that is available for the learner to drag and drop")]
        public required string Value { get; set; }

        [Description("format in which Value is provided")]
        public FormattingFormat ValueFormat { get; set; }

        [Description("choices can be grouped into categories to help the learner; choice belonging to a certain category can only be dragged into a drop zone of the same category")]
        public string? Category { get; set; } = string.Empty;
    }
}
