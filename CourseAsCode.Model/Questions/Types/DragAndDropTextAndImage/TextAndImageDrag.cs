using System.ComponentModel;

namespace CourseAsCode.Model.Questions.Types.DragAndDropTextAndImage
{
    public class TextAndImageDrag
    {
        [Description("Rrefers to the drags, used to map correct answers and images")]
        public int ID { get; set; }

        [Description("Each Drag has a number, starting from 1, then 2, 3, 4 etc")]
        public int Number { get; set; }

        [Description("choices can be grouped into categories to help the learner; choice belonging to a certain category can only be dragged into a drop zone of the same category")]
        public int DragGroup { get; set; }

        [Description("Determines if choices should be one use only or infinite")]
        public bool Infinite { get; set; }

        [Description("Determines the displayed value, in case there is no image")]
        public string? Label { get; set; }

        [Description("Path to the image")]
        public string? ImagePath { get; set; }

        [Description("optional, name for the image")]
        public string? ImageName { get; set; }
    }
}
